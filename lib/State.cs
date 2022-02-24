using System;
using System.Collections.Generic;
using System.Linq;

namespace bot;

public class State
{
    public State(HashSet<Idea> notStarted, HashSet<Project> inProgress, List<Person> freePeople, Problem problem, List<Project> projects, int time)
    {
        this.notStarted = notStarted;
        this.inProgress = inProgress;
        this.freePeople = freePeople;
        Problem = problem;
        Projects = projects;
        Time = time;
    }

    public State(Problem problem)
    {
        Problem = problem;
        Time = 0;
        Projects = new List<Project>();
        notStarted = problem.Ideas.ToHashSet();
        inProgress = new HashSet<Project>();
        freePeople = problem.People.ToList();
    }

    private readonly HashSet<Idea> notStarted;
    private readonly HashSet<Project> inProgress;
    private readonly List<Person> freePeople;
    
    public IEnumerable<Project> GetPossibleProjectsToStart()
    {
        return notStarted.Select(TryStartProject).Where(p => p!=null);
    }

    private Project TryStartProject(Idea idea)
    {
        var team = new Person[idea.Roles.Length];
        for (int iRole = 0; iRole < idea.Roles.Length; iRole++)
        {
            var ideaRole = idea.Roles[iRole];
            var person = freePeople.Where(p => !team.Contains(p) && p.HasSkill(ideaRole)).MinBy(p => p.Skills.Sum(s => s.Level));
            if (person == null)
                return null;
            team[iRole] = person;
        }
        return new Project(idea, team, Time);
    }

    public void StartProject(Idea idea, Person[] members)
    {
        var project = new Project(idea, members, Time);
        inProgress.Add(project);
    }

    public void WaitNextProjectFinish()
    {
        if (inProgress.Count == 0)
            throw new InvalidOperationException();
        var projectToFinish = inProgress.MinBy(p => p.StartDay + p.Idea.Duration);
        Time = projectToFinish.StartDay + projectToFinish.Idea.Duration;
        freePeople.AddRange(projectToFinish.Members.Select((m, i) => LevelUp(m, projectToFinish.Idea.Roles[i])));
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
        return new State(notStarted.ToHashSet(), inProgress.ToHashSet(), freePeople.ToList(), 
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