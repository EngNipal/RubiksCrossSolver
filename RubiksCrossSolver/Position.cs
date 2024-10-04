namespace RubiksCrossSolver
{
    public class Position(int depth, uint[] state)
    {
        public int Depth { get; private set; } = depth;
        public ulong Hash { get; private set; }
        public uint[] State { get; private set; } = state;
        public List<Turn> Turns { get; private set; }

        public void SetHash(Zobrist zobrist)
        {
            Hash = zobrist.Hash(State);
        }

        public Turn? GetAntiturn()
        {
            return Turns?.Last() switch
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
                null => null,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}
