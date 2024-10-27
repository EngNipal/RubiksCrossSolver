using System.Numerics;
using System.Text;

namespace RubiksCrossSolver;

public class Position
{
    private List<Turn> Turns { get; set; } = [];
    public Position(int depth, Colour[] state, IReadOnlyCollection<Turn> turns)
    {
        Depth = depth;
        var bytedState = state.Select(x => (byte)x).ToArray();
        Hash = BigInteger.Parse(string.Join(string.Empty, bytedState));
        foreach (Turn turn in turns)
        {
            Turns.Add(turn);
        }
    }
    public int Depth { get; private set; }
    public BigInteger Hash { get; private set; }
    public IReadOnlyCollection<Turn> GetTurns() => Turns;

    public void AddTurn(Turn turn)
    {
        Turns.Add(turn);
    }

    public override string ToString()
    {
        const string separator = ", ";
        var sb = new StringBuilder();
        foreach (Turn t in Turns)
        {
            sb.Append(t);
            sb.Append(separator);
        }

        return sb.ToString().Trim(separator.ToCharArray());
    }
}
