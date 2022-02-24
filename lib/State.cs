using System;
using System.Collections.Generic;
using System.Linq;

namespace bot;

public class State
{
    public State(HashSet<Idea> notStarted, HashSet<Project> inProgress, HashSet<Person> freePeople, Problem problem, List<Project> projects, int time)
    {
        this.NotStarted = notStarted;
        this.InProgress = inProgress;
        this.FreePeople = freePeople;
        Problem = problem;
        Projects = projects;
        Time = time;
    }

    public State(Problem problem)
    {
        Problem = problem;
        Time = 0;
        Projects = new List<Project>();
        NotStarted = problem.Ideas.ToHashSet();
        InProgress = new HashSet<Project>();
        FreePeople = problem.People.ToHashSet();
    }

    public readonly HashSet<Idea> NotStarted;
    public readonly HashSet<Project> InProgress;
    public readonly HashSet<Person> FreePeople;
    
    public IEnumerable<Project> GetPossibleProjectsToStart()
    {
        return NotStarted.Select(TryStartProject).Where(p => p!=null);
    }

    private Project TryStartProject(Idea idea)
    {
        var team = new Person[idea.Roles.Length];
        for (int iRole = 0; iRole < idea.Roles.Length; iRole++)
        {
            var ideaRole = idea.Roles[iRole];
            var person = FreePeople.Where(p => !team.Contains(p) && p.HasSkill(ideaRole)).MinBy(p => p.Skills.Sum(s => s.Level));
            if (person == null)
                return null;
            team[iRole] = person;
        }
        return new Project(idea, team, Time);
    }

    public bool IsFinished()
    {
        if (NotStarted.Count == 0 && InProgress.Count == 0) return true;
        if (InProgress.Count > 0) return false;
        // InProgress.Count == 0 && NotStarted.Count > 0 && не можем ничего запустить (из-за скиллов) :(
        return !GetPossibleProjectsToStart().Any();
    }

    public void StartProject(Project project)
    {
        Projects.Add(project);
        InProgress.Add(project);
        NotStarted.Remove(project.Idea);
        foreach (var member in project.Members)
            FreePeople.Remove(member);
    }
    
    public void WaitNextProjectFinish()
    {
        if (InProgress.Count == 0)
            throw new InvalidOperationException();
        var projectToFinish = InProgress.MinBy(p => p.StartDay + p.Idea.Duration);
        Time = projectToFinish.StartDay + projectToFinish.Idea.Duration;
        var newFreePeople = projectToFinish.Members.Select((m, i) => LevelUp(m, projectToFinish.Idea.Roles[i]));
        foreach (var person in newFreePeople)
            FreePeople.Add(person);
        InProgress.Remove(projectToFinish);
    }

    private Person LevelUp(Person person, Skill role)
    {
        var (roleName, level) = role;
        var index = person.Skills.IndexOf(s => s.Name == roleName);
        if (person.Skills[index].Level == level)
        {
            var newSkills = person.Skills.ToArray();
            newSkills[index] = new Skill(roleName, level + 1);
            return new Person(person.Name, newSkills);
        }
        return person;
    }


    public State Clone()
    {
        return new State(NotStarted.ToHashSet(), InProgress.ToHashSet(), FreePeople.ToHashSet(), 
            Problem, Projects.ToList(), Time);
    }

    public readonly Problem Problem;
    public readonly List<Project> Projects;
    public int Time;
}

public class Project
{
    public Project(Idea idea, Person[] members, int startDay)
    {
        Idea = idea;
        Members = members;
        StartDay = startDay;
    }

    public override string ToString()
    {
        return $"{Idea.Name} {StartDay} {Members.StrJoin(", ")}";
    }

    public readonly Idea Idea;
    public readonly Person[] Members;
    public readonly int StartDay;

    public int GetPoints()
    {
        if (Idea.Duration + StartDay > Idea.BestBefore)
            return Math.Max(0, Idea.Score - (Idea.Duration + StartDay - Idea.BestBefore));
        return Idea.Score;
    }
}