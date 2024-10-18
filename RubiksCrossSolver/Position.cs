using System.Numerics;
using System.Text;

namespace RubiksCrossSolver;

public class Position
{
    public Position(int depth, Colour[] state, List<Turn> turns)
    {
        Depth = depth;
        var bytedState = state.Select(x => (byte)x).ToArray();
        Hash = BigInteger.Parse(string.Join(string.Empty, bytedState));
        foreach (var turn in turns)
        {
            Turns.Add(turn);
        }
    }
    public int Depth { get; private set; }
    public BigInteger Hash { get; private set; } = BigInteger.Zero;
    public List<Turn> Turns { get; private set; } = [];

    public void AddTurn(Turn turn)
    {
        Turns.Add(turn);
    }

    public Turn? GetAntiturn()
    {
        if (Turns.Count == 0) return null;

        return Turns.Last() switch
        {
            Turn.R => Turn.Rp,
            Turn.Rp => Turn.R,
            Turn.R2 => Turn.R2,
            Turn.U => Turn.Up,
            Turn.Up => Turn.U,
            Turn.U2 => Turn.U2,
            Turn.F => Turn.Fp,
            Turn.Fp => Turn.F,
            Turn.F2 => Turn.F2,
            Turn.L => Turn.Lp,
            Turn.Lp => Turn.L,
            Turn.L2 => Turn.L2,
            Turn.D => Turn.Dp,
            Turn.Dp => Turn.D,
            Turn.D2 => Turn.D2,
            Turn.B => Turn.Bp,
            Turn.Bp => Turn.B,
            Turn.B2 => Turn.B2,
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

    public override string ToString()
    {
        const string separator = ", ";
        var sb = new StringBuilder();
        foreach (var t in Turns)
        {
            sb.Append(t);
            sb.Append(separator);
        }

        return sb.ToString().Trim(separator.ToCharArray());
    }
}
