using System;
using System.Collections.Generic;
using System.Linq;

namespace bot;

public class State
{
    public State(Problem problem, List<Project> projects = null)
    {
        Problem = problem;
        Projects = projects ?? new List<Project>();
    }

    public IEnumerable<Project> GetPossibleProjectsToStart()
    {
        throw new NotImplementedException();
    }

    public State Clone()
    {
        return new State(Problem, Projects.ToList());
    }

    public readonly Problem Problem;
    public readonly List<Project> Projects;
}

public class Project
{
    public Project(Idea idea, Person[] members, int startDay)
    {
        Idea = idea;
        Members = members;
        StartDay = startDay;
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