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
        if (moves.Count == 0 && problem.InProgress.Count == 0)
        {
            return Enumerable.Empty<Project>();
        }
        return moves.Count == 0 ? new() { null } : moves;
    }
}