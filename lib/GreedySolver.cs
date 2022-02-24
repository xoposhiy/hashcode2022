using System.Collections.Generic;

namespace bot;

public class GreedySolver : AbstractGreedySolver<State, Project>
{
    public GreedySolver(IEstimator<State> estimator) : base(estimator)
    {
        
    }

    protected override State ApplyMove(State problem, Project move)
    {
        var copy = problem.Clone();
        copy.Projects.Add(move);
        return copy;
    }

    protected override IEnumerable<Project> GetMoves(State problem)
    {
        return problem.GetPossibleProjectsToStart();
    }
}