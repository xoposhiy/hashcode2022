using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace bot;

[TestFixture]
public class StateTests
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
        Console.WriteLine(problem);
    }


    [Test]
    public void GetMoves()
    {
        var problem = new ProblemReader().ReadByName("a_an_example");
        var state = new State(problem);
        var actual = state.GetPossibleProjectsToStart().ToList();
        Console.WriteLine(actual.StrJoin("\n"));
    }

    [TestCase("a_an_example")]
    [TestCase("b_better_start_small")]
    [TestCase("c_collaboration")]
    [TestCase("d_dense_schedule")]
    [TestCase("e_exceptional_skills")]
    [TestCase("f_find_great_mentors")]
    public void StupidGreedyWithStupidEstimator(string name)
    {
        var stupidEstimator = new StupidEstimator();
        var problem = new ProblemReader().ReadByName(name);
        var state = new State(problem);
        
        var greedySolver = new GreedySolver(stupidEstimator);

        while (true)
        {
            var singleMoveSolution = greedySolver.GetSolutions(state, int.MaxValue).FirstOrDefault();
            if (singleMoveSolution == default) break;
            state.Projects.Add(singleMoveSolution.Move);
        }

        Console.WriteLine(stupidEstimator.GetScore(state));
        Console.WriteLine(state);
    }
}