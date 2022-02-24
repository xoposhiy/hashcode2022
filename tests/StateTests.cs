using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        Console.WriteLine(problem.Ideas.Max(idea => idea.Duration));
        Console.WriteLine(problem.Ideas.Length);
        var skillsCount = problem.People.Select(e => e.Skills.Length).OrderByDescending(e => e).First();

        var projectRolesCount = problem.Ideas.Select(e => e.Roles.Length).OrderByDescending(e => e).First();
        Console.WriteLine(projectRolesCount);
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
            var singleMoveSolution = greedySolver.GetSolutions(state, int.MaxValue).ToList();
            if (singleMoveSolution.Count == 0)
            {
                break;
            }

            var move = singleMoveSolution.First().Move;

            if (move == null)
            {
                state.WaitNextProjectFinish();
            }
            else
            {
                state.StartProject(move);
            }
        }

        var score = stupidEstimator.GetScore(state);
        Console.WriteLine(score);
        //Console.WriteLine(state);
        File.WriteAllText($"../../../../solutions/{name}-{score}.out.txt", state.ToString());

    }
    
    [TestCase("a_an_example")]
    [TestCase("b_better_start_small")]
    [TestCase("c_collaboration")]
    [TestCase("d_dense_schedule")]
    [TestCase("e_exceptional_skills")]
    [TestCase("f_find_great_mentors")]
    public void NaiveSolveWriter(string name)
    {
        var problem = new ProblemReader().ReadByName(name);
        var state = new State(problem);
        var sw = new Stopwatch();
        sw.Start();
        while (!state.IsFinished())
        {
            var move = state.GetPossibleProjectsToStart().FirstOrDefault();
            if (move == null)
                state.WaitNextProjectFinish();
            else
                state.StartProject(move);
            if (sw.ElapsedMilliseconds > 20000)
                break;
        }

        var score = new StupidEstimator().GetScore(state);
        File.WriteAllText($"../../../../solutions/{name}-{score}.out.txt", state.ToString());
    }
}