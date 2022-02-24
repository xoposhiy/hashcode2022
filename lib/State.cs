using System;
using System.Collections.Generic;

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
        throw new NotImplementedException();
    }

    public Problem Problem;
    public List<Project> Projects;
}

public class Project
{
    public Project(Idea idea, Person[] members, int startDay)
    {
        Idea = idea;
        Members = members;
        StartDay = startDay;
    }

    public Idea Idea;
    public Person[] Members;
    public int StartDay;
}