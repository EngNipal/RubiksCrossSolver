using RubiksCrossSolver.SolveDto;
using System.Numerics;

namespace RubiksCrossSolver;

internal sealed class Solver
{
    private readonly int _maxDepth;

    public Solver(int maxDepth)
    {
        _maxDepth = maxDepth;
    }

    public CrossSolvesAggregator GetSolves(Colour[] initialState)
    {
        var initial = new Position(0, initialState, []);
        const int additionalDepth = 4;
        CrossSolvesAggregator solves = GetSolvesOnDepth(initial, additionalDepth, fillDeep: true);
        List<Position> deepPositions = solves.VerifiedPositions.Where(x => x.Depth == additionalDepth).ToList();
        foreach (Position deepPosition in deepPositions)
        {
            CrossSolvesAggregator innerSolves =
                GetSolvesOnDepth(deepPosition, _maxDepth - deepPosition.Depth, fillDeep: false);
            if (innerSolves.HasAnySolves())
            {
                MergeSolves(innerSolves, solves);
            }
        }

        return solves;
    }

    private CrossSolvesAggregator GetSolvesOnDepth(Position initial, int additionalDepth, bool fillDeep)
    {
        var depth = 0;
        var solves = new CrossSolvesAggregator();
        Turn[] turns = Enum.GetValues<Turn>();
        var positions = new Dictionary<BigInteger, Position> { { initial.Hash, initial } };

        while (depth < additionalDepth)
        {
            var whiteStop = solves.Cross[Colour.White].Count > 0;
            var orangeStop = solves.Cross[Colour.Orange].Count > 0;
            var greenStop = solves.Cross[Colour.Green].Count > 0;
            var redStop = solves.Cross[Colour.Red].Count > 0;
            var blueStop = solves.Cross[Colour.Blue].Count > 0;
            var yellowStop = solves.Cross[Colour.Yellow].Count > 0;
            var whitePairStop = solves.CrossPair[Colour.White].Count > 0;
            var orangePairStop = solves.CrossPair[Colour.Orange].Count > 0;
            var greenPairStop = solves.CrossPair[Colour.Green].Count > 0;
            var redPairStop = solves.CrossPair[Colour.Red].Count > 0;
            var bluePairStop = solves.CrossPair[Colour.Blue].Count > 0;
            var yellowPairStop = solves.CrossPair[Colour.Yellow].Count > 0;
            List<Position> deepPositions = positions.Values.Where(x => x.Depth == depth + initial.Depth).ToList();
            foreach (Position position in deepPositions)
            {
                foreach (Turn turn in turns)
                {
                    Position newPosition = CreatePosition(position, turn, out Colour[] state);
                    if (!positions.TryAdd(newPosition.Hash, newPosition))
                    {
                        continue;
                    }

                    var whiteCross = WhiteCrossIsSolved(state);
                    var orangeCross = OrangeCrossIsSolved(state);
                    var greenCross = GreenCrossIsSolved(state);
                    var redCross = RedCrossIsSolved(state);
                    var blueCross = BlueCrossIsSolved(state);
                    var yellowCross = YellowCrossIsSolved(state);
                    if (!whiteStop && whiteCross)
                    {
                        solves.Cross[Colour.White].AddSolve(newPosition);
                    }

                    if (!orangeStop && orangeCross)
                    {
                        solves.Cross[Colour.Orange].AddSolve(newPosition);
                    }

                    if (!greenStop && greenCross)
                    {
                        solves.Cross[Colour.Green].AddSolve(newPosition);
                    }

                    if (!redStop && redCross)
                    {
                        solves.Cross[Colour.Red].AddSolve(newPosition);
                    }

                    if (!blueStop && blueCross)
                    {
                        solves.Cross[Colour.Blue].AddSolve(newPosition);
                    }

                    if (!yellowStop && yellowCross)
                    {
                        solves.Cross[Colour.Yellow].AddSolve(newPosition);
                    }

                    if (!whitePairStop && whiteCross && WhitePairIsSolved(state))
                    {
                        solves.CrossPair[Colour.White].AddSolve(newPosition);
                    }

                    if (!orangePairStop && orangeCross && OrangePairIsSolved(state))
                    {
                        solves.CrossPair[Colour.Orange].AddSolve(newPosition);
                    }

                    if (!greenPairStop && greenCross && GreenPairIsSolved(state))
                    {
                        solves.CrossPair[Colour.Green].AddSolve(newPosition);
                    }

                    if (!redPairStop && redCross && RedPairIsSolved(state))
                    {
                        solves.CrossPair[Colour.Red].AddSolve(newPosition);
                    }

                    if (!bluePairStop && blueCross && BluePairIsSolved(state))
                    {
                        solves.CrossPair[Colour.Blue].AddSolve(newPosition);
                    }

                    if (!yellowPairStop && yellowCross && YellowPairIsSolved(state))
                    {
                        solves.CrossPair[Colour.Yellow].AddSolve(newPosition);
                    }
                }
            }

            depth++;
        }

        if (fillDeep)
        {
            solves.VerifiedPositions = positions.Values.ToList();
        }

        return solves;
    }

    private void MergeSolves(CrossSolvesAggregator source, CrossSolvesAggregator destination)
    {
        if (!source.HasAnyCrossSolves() && !source.HasAnyCrossPairSolves()) return;
        foreach (KeyValuePair<Colour, CrossSolveDto> pair in source.Cross.Where(x => x.Value.Count > 0))
        {
            destination.Cross[pair.Key].AddSolveRange(pair.Value.GetSolves());
        }

        foreach (KeyValuePair<Colour, CrossPairSolveDto> pair in source.CrossPair.Where(x => x.Value.Count > 0))
        {
            destination.CrossPair[pair.Key].AddSolveRange(pair.Value.GetSolves());
        }
    }

    #region Static

    private static Position CreatePosition(Position position, Turn turn, out Colour[] state)
    {
        state = RubiksCube.GetStateByHash(position.Hash, turn);
        var newPosition = new Position(position.Depth + 1, state, position.GetTurns());
        newPosition.AddTurn(turn);
        return newPosition;
    }

    private static bool WhiteCrossIsSolved(Colour[] state)
    {
        return state[1] == Colour.White &&
               state[3] == Colour.White &&
               state[4] == Colour.White &&
               state[6] == Colour.White &&
               state[9] == Colour.Orange &&
               state[17] == Colour.Green &&
               state[25] == Colour.Red &&
               state[33] == Colour.Blue;
    }

    private static bool OrangeCrossIsSolved(Colour[] state)
    {
        return state[9] == Colour.Orange &&
               state[11] == Colour.Orange &&
               state[12] == Colour.Orange &&
               state[14] == Colour.Orange &&
               state[3] == Colour.White &&
               state[19] == Colour.Green &&
               state[43] == Colour.Yellow &&
               state[36] == Colour.Blue;
    }

    private static bool GreenCrossIsSolved(Colour[] state)
    {
        return state[17] == Colour.Green &&
               state[19] == Colour.Green &&
               state[20] == Colour.Green &&
               state[22] == Colour.Green &&
               state[6] == Colour.White &&
               state[27] == Colour.Red &&
               state[41] == Colour.Yellow &&
               state[12] == Colour.Orange;
    }

    private static bool RedCrossIsSolved(Colour[] state)
    {
        return state[25] == Colour.Red &&
               state[27] == Colour.Red &&
               state[28] == Colour.Red &&
               state[30] == Colour.Red &&
               state[4] == Colour.White &&
               state[35] == Colour.Blue &&
               state[44] == Colour.Yellow &&
               state[20] == Colour.Green;
    }

    private static bool BlueCrossIsSolved(Colour[] state)
    {
        return state[33] == Colour.Blue &&
               state[35] == Colour.Blue &&
               state[36] == Colour.Blue &&
               state[38] == Colour.Blue &&
               state[1] == Colour.White &&
               state[11] == Colour.Orange &&
               state[46] == Colour.Yellow &&
               state[28] == Colour.Red;
    }

    private static bool YellowCrossIsSolved(Colour[] state)
    {
        return state[41] == Colour.Yellow &&
               state[43] == Colour.Yellow &&
               state[44] == Colour.Yellow &&
               state[46] == Colour.Yellow &&
               state[14] == Colour.Orange &&
               state[22] == Colour.Green &&
               state[30] == Colour.Red &&
               state[38] == Colour.Blue;
    }

    private static bool WhitePairIsSolved(Colour[] state)
    {
        return // оранжево-синяя пара
            state[0] == Colour.White &&
            state[8] == Colour.Orange &&
            state[11] == Colour.Orange &&
            state[34] == Colour.Blue &&
            state[36] == Colour.Blue ||
            // сине-красная пара
            state[2] == Colour.White &&
            state[26] == Colour.Red &&
            state[28] == Colour.Red &&
            state[32] == Colour.Blue &&
            state[35] == Colour.Blue ||
            // оранжево-зелёная пара
            state[5] == Colour.White &&
            state[10] == Colour.Orange &&
            state[12] == Colour.Orange &&
            state[16] == Colour.Green &&
            state[19] == Colour.Green ||
            // зелёно-красная пара
            state[7] == Colour.White &&
            state[18] == Colour.Green &&
            state[20] == Colour.Green &&
            state[24] == Colour.Red &&
            state[27] == Colour.Red;
    }

    private static bool OrangePairIsSolved(Colour[] state)
    {
        return // бело-синяя пара
            state[8] == Colour.Orange &&
            state[0] == Colour.White &&
            state[1] == Colour.White &&
            state[33] == Colour.Blue &&
            state[34] == Colour.Blue ||
            // бело-зелёная пара
            state[10] == Colour.Orange &&
            state[5] == Colour.White &&
            state[6] == Colour.White &&
            state[16] == Colour.Green &&
            state[17] == Colour.Green ||
            // сине-жёлтая пара
            state[13] == Colour.Orange &&
            state[38] == Colour.Blue &&
            state[39] == Colour.Blue &&
            state[45] == Colour.Yellow &&
            state[46] == Colour.Yellow ||
            // зелёно-жёлтая пара
            state[15] == Colour.Orange &&
            state[19] == Colour.Green &&
            state[21] == Colour.Green &&
            state[40] == Colour.Yellow &&
            state[41] == Colour.Yellow;
    }

    private static bool GreenPairIsSolved(Colour[] state)
    {
        return // бело-оранжевая пара
            state[16] == Colour.Green &&
            state[3] == Colour.White &&
            state[5] == Colour.White &&
            state[9] == Colour.Orange &&
            state[10] == Colour.Orange ||
            // бело-красная пара
            state[18] == Colour.Green &&
            state[4] == Colour.White &&
            state[7] == Colour.White &&
            state[24] == Colour.Red &&
            state[25] == Colour.Red ||
            // оранжево-жёлтая пара
            state[21] == Colour.Green &&
            state[14] == Colour.Orange &&
            state[15] == Colour.Orange &&
            state[40] == Colour.Yellow &&
            state[43] == Colour.Yellow ||
            // красно-жёлтая пара
            state[23] == Colour.Green &&
            state[29] == Colour.Red &&
            state[30] == Colour.Red &&
            state[42] == Colour.Yellow &&
            state[44] == Colour.Yellow;
    }

    private static bool RedPairIsSolved(Colour[] state)
    {
        return // бело-зелёная пара
            state[24] == Colour.Red &&
            state[6] == Colour.White &&
            state[7] == Colour.White &&
            state[17] == Colour.Green &&
            state[18] == Colour.Green ||
            // бело-синяя пара
            state[26] == Colour.Red &&
            state[1] == Colour.White &&
            state[2] == Colour.White &&
            state[32] == Colour.Blue &&
            state[33] == Colour.Blue ||
            // зелёно-жёлтая пара
            state[29] == Colour.Red &&
            state[22] == Colour.Green &&
            state[23] == Colour.Green &&
            state[41] == Colour.Yellow &&
            state[42] == Colour.Yellow ||
            // сине-жёлтая пара
            state[31] == Colour.Red &&
            state[37] == Colour.Blue &&
            state[38] == Colour.Blue &&
            state[46] == Colour.Yellow &&
            state[47] == Colour.Yellow;
    }

    private static bool BluePairIsSolved(Colour[] state)
    {
        return // бело-красная пара
            state[32] == Colour.Blue &&
            state[2] == Colour.White &&
            state[4] == Colour.White &&
            state[25] == Colour.Red &&
            state[26] == Colour.Red ||
            // бело-оранжевая пара
            state[34] == Colour.Blue &&
            state[0] == Colour.White &&
            state[3] == Colour.White &&
            state[8] == Colour.Orange &&
            state[9] == Colour.Orange ||
            // красно-жёлтая пара
            state[37] == Colour.Blue &&
            state[30] == Colour.Red &&
            state[31] == Colour.Red &&
            state[44] == Colour.Yellow &&
            state[47] == Colour.Yellow ||
            // оранжево-жёлтая пара
            state[39] == Colour.Blue &&
            state[13] == Colour.Orange &&
            state[14] == Colour.Orange &&
            state[43] == Colour.Yellow &&
            state[45] == Colour.Yellow;
    }

    private static bool YellowPairIsSolved(Colour[] state)
    {
        return // оранжево-зелёная пара
            state[40] == Colour.Yellow &&
            state[12] == Colour.Orange &&
            state[15] == Colour.Orange &&
            state[19] == Colour.Green &&
            state[21] == Colour.Green ||
            // оранжево-синяя пара
            state[42] == Colour.Yellow &&
            state[11] == Colour.Orange &&
            state[13] == Colour.Orange &&
            state[36] == Colour.Blue &&
            state[39] == Colour.Blue ||
            // зелёно-красная пара
            state[45] == Colour.Yellow &&
            state[20] == Colour.Green &&
            state[23] == Colour.Green &&
            state[27] == Colour.Red &&
            state[29] == Colour.Red ||
            // сине-красная пара
            state[47] == Colour.Yellow &&
            state[28] == Colour.Red &&
            state[31] == Colour.Red &&
            state[35] == Colour.Blue &&
            state[37] == Colour.Blue;
    }

    #endregion
}