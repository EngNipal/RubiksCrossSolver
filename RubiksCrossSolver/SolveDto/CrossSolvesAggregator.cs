namespace RubiksCrossSolver.SolveDto
{
    internal class CrossSolvesAggregator
    {
        public CrossSolvesAggregator(ICollection<Colour> colours)
        {
            Cross = new Dictionary<Colour, CrossSolveDto>(colours.Count);
            CrossPair = new Dictionary<Colour, CrossPairSolveDto>(colours.Count);
            foreach (var colour in colours)
            {
                Cross.Add(colour, new CrossSolveDto(colour));
                CrossPair.Add(colour, new CrossPairSolveDto(colour));
            }
        }
        public Dictionary<Colour, CrossSolveDto> Cross { get; private set; }
        public Dictionary<Colour, CrossPairSolveDto> CrossPair { get; private set; }
    }
}
