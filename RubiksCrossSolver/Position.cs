using System.Text;

namespace RubiksCrossSolver;

public class Position(int depth, uint[] state, List<Turn> turns)
{
    public int Depth { get; private set; } = depth;
    public string Hash { get; private set; } = string.Empty;
    public uint[] State { get; private set; } = state;
    public List<Turn> Turns { get; private set; } = [.. turns];

    public void SetHash()
    {
        Hash = string.Join(string.Empty, State);
    }

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

    public bool IsEqualTo(Position other)
    {
        int i = 0;
        while (i < other.State.Length && State[i] == other.State[i])
        {
            i++;
        }

        return i == other.State.Length;
    }
}
