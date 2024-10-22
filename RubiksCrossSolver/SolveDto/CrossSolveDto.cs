namespace RubiksCrossSolver.SolveDto
{
    internal class CrossSolveDto
    {
        public CrossSolveDto(Colour colour)
        {
            Colour = colour;
            Solves = [];
        }

        public Colour Colour { get; private set; }
        public List<Position> Solves { get; private set; }
        public int Count => Solves.Count;
    }
}
