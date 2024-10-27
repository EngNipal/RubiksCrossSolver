namespace RubiksCrossSolver.SolveDto
{
    internal class CrossSolvesAggregator
    {
        public CrossSolvesAggregator()
        {
            Colour[] colours = Enum.GetValues<Colour>();
            Cross = new Dictionary<Colour, CrossSolveDto>(colours.Length);
            CrossPair = new Dictionary<Colour, CrossPairSolveDto>(colours.Length);
            foreach (Colour colour in colours)
            {
                Cross.Add(colour, new CrossSolveDto(colour));
                CrossPair.Add(colour, new CrossPairSolveDto(colour));
            }

            VerifiedPositions = [];
        }
        
        /// <summary> Решения для креста </summary>
        public Dictionary<Colour, CrossSolveDto> Cross { get; private set; }
        
        /// <summary> Решения для креста с парами </summary>
        public Dictionary<Colour, CrossPairSolveDto> CrossPair { get; private set; }

        /// <summary> Список позиций, проверенных в ходе решения </summary>
        public List<Position> VerifiedPositions { get; set; }

        /// <summary> Признак наличия каких-либо решений </summary>
        /// <returns> true - решения есть, false - решений нет </returns>
        public bool HasAnySolves()
        {
            return HasAnyCrossSolves() || HasAnyCrossPairSolves();
        }

        /// <summary> Признак наличия решений для креста </summary>
        /// <returns> true - решения есть, false - решений нет </returns>
        public bool HasAnyCrossSolves() => Cross.Any(x => x.Value.Count > 0);
        
        /// <summary> Признак наличия решений для креста с парой </summary>
        /// <returns> true - решения есть, false - решений нет </returns>
        public bool HasAnyCrossPairSolves() => CrossPair.Any(x => x.Value.Count > 0);
    }
}
