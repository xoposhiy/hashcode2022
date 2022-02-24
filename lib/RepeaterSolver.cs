using System.Collections.Generic;
using System.Linq;

namespace bot;

public class RepeaterSolver
{
    public State GetSolutions(State problem, Countdown countdown)
    {
        var stupidEstimator = new StupidEstimator();
        var greedySolver = new GreedySolver(stupidEstimator);
        var bestSolutions = new PriorityQueue<State, double>();
        while (!countdown.IsFinished())
        {
            var state = problem.Clone();
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
            bestSolutions.Enqueue(state, -score);
        }

        return bestSolutions.Dequeue();
    }
}