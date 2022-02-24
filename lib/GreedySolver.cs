using System.Collections.Generic;
using System.Linq;

namespace bot;

public class GreedySolver : AbstractGreedySolver<State, Project>
{
    public GreedySolver(IEstimator<State> estimator) : base(estimator)
    {
        
    }

    protected override State ApplyMove(State problem, Project move)
    {
        if (move == null)
        {
            problem.WaitNextProjectFinish();
        }
        else
        {
            problem.StartProject(move);
        }
        
        return problem;
    }

    protected override IEnumerable<Project> GetMoves(State problem)
    {
        var moves = problem.GetPossibleProjectsToStart().ToList();
        return moves.Count == 0 ? new() { null } : moves;
    }
}