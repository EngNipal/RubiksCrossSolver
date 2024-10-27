using System.Numerics;

namespace RubiksCrossSolver.SolveDto
{
    internal class CrossSolveDto
    {
        private Dictionary<BigInteger, Position> Solves { get; set; }

        public CrossSolveDto(Colour colour)
        {
            Colour = colour;
            Solves = new Dictionary<BigInteger, Position>();
        }

        public Colour Colour { get; private set; }

        public int Count => Solves.Count;

        public void AddSolve(Position position)
        {
            Solves.TryAdd(position.Hash, position);
        }

        public void AddSolveRange(IEnumerable<Position> positions)
        {
            foreach (Position position in positions)
            {
                AddSolve(position);
            }
        }

        public IReadOnlyCollection<Position> GetSolves() => Solves.Values;
    }
}