using System;
using System.Collections.Generic;
using System.Linq;

namespace bot;

public class State
{
    public State(Problem problem, List<Project> projects)
    {
        Problem = problem;
        Projects = projects;
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
}