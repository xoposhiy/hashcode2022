using System.Linq;

namespace bot;

public class StupidEstimator : IEstimator<State>
{
    public double GetScore(State state)
    {
        return state.Projects.Sum(p => p.GetPoints());
    }
}