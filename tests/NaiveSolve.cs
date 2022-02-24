using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using NUnit.Framework;

namespace bot;

[TestFixture]
public class NaiveSolve
{
    [TestCase("a_an_example")]
    [TestCase("b_better_start_small")]
    [TestCase("c_collaboration")]
    [TestCase("d_dense_schedule")]
    [TestCase("e_exceptional_skills")]
    [TestCase("f_find_great_mentors")]
    public void Test(string name)
    {
        var problem = new ProblemReader().ReadByName(name);
        var state = new State(problem);
        while (!state.IsFinished())
        {
            var move = state.GetPossibleProjectsToStart().FirstOrDefault();
            if (move == null)
                state.WaitNextProjectFinish();
            else
                state.StartProject(move);
        }

        Console.WriteLine(new StupidEstimator().GetScore(state));
    }
}