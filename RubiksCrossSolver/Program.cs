namespace RubiksCrossSolver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var zobrist = new Zobrist(48, 6);
            var scramble = new Turns[] {Turns.R };
            var rubiksCube = new RubiksCube(scramble);

            uint[] initialState = rubiksCube.GetCurrentState();
            var initial = new Position(0, initialState);
            initial.SetHash(zobrist);

            var positions = new Dictionary<ulong, Position>();            
            positions.Add(initial.Hash, initial);
        }
    }
}
