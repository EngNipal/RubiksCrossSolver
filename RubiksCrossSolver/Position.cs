namespace RubiksCrossSolver
{
    public class Position(int depth, uint[] state)
    {
        public int Depth { get; private set; } = depth;
        public ulong Hash { get; private set; }
        public uint[] State { get; private set; } = state;

        public void SetHash(Zobrist zobrist)
        {
            Hash = zobrist.Hash(State);
        }
    }
}
