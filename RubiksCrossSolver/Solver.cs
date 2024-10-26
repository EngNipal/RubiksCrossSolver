using RubiksCrossSolver.SolveDto;
using System.Collections.Concurrent;
using System.Numerics;

namespace RubiksCrossSolver;

internal sealed class Solver
{
    private readonly int _maxDepth;
    private readonly Zobrist _zobrist;
    public Solver(byte fieldsAmount, byte statesAmount, int maxDepth)
    {
        _zobrist = new Zobrist(fieldsAmount, statesAmount);
        _maxDepth = maxDepth;
    }

    public CrossSolvesAggregator GetSolves(Colour[] initialState)
    {
        var depth = 0;
        var initial = new Position(depth, initialState, []);
        var solves = new CrossSolvesAggregator(Enum.GetValues<Colour>());

        ConcurrentDictionary<BigInteger, Position> positions = new();
        positions.AddOrUpdate(initial.Hash, initial, (BigInteger hash, Position pos) => pos);
        while (depth < _maxDepth)
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
            List<Position> deepPositions = positions.Values.Where(x => x.Depth == depth).ToList();
            var chunkSize = deepPositions.Count / 24 + 1;
            IEnumerable<Position[]> deepPositionsChunked = deepPositions.Chunk(chunkSize);
            Console.WriteLine($"Глубина {depth}");

            foreach(Position[] chunk in deepPositionsChunked)
            {
                foreach (Position position in chunk)
                {
                    //var antiturn = position.GetAntiturn();
                    foreach (Turn turn in Enum.GetValues(typeof(Turn)))
                    {
                        //if (depth > 0 && turn == antiturn) continue;
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
                            solves.Cross[Colour.White].Solves.Add(newPosition);
                        }
                        if (!orangeStop && orangeCross)
                        {
                            solves.Cross[Colour.Orange].Solves.Add(newPosition);
                        }
                        if (!greenStop && greenCross)
                        {
                            solves.Cross[Colour.Green].Solves.Add(newPosition);
                        }
                        if (!redStop && redCross)
                        {
                            solves.Cross[Colour.Red].Solves.Add(newPosition);
                        }
                        if (!blueStop && blueCross)
                        {
                            solves.Cross[Colour.Blue].Solves.Add(newPosition);
                        }
                        if (!yellowStop && yellowCross)
                        {
                            solves.Cross[Colour.Yellow].Solves.Add(newPosition);
                        }
                        // ---
                        if (!whitePairStop && whiteCross && WhitePairIsSolved(state))
                        {
                            solves.CrossPair[Colour.White].Solves.Add(newPosition);
                        }
                        if (!orangePairStop && orangeCross && OrangePairIsSolved(state))
                        {
                            solves.CrossPair[Colour.Orange].Solves.Add(newPosition);
                        }
                        if (!greenPairStop && greenCross && GreenPairIsSolved(state))
                        {
                            solves.CrossPair[Colour.Green].Solves.Add(newPosition);
                        }
                        if (!redPairStop && redCross && RedPairIsSolved(state))
                        {
                            solves.CrossPair[Colour.Red].Solves.Add(newPosition);
                        }
                        if (!bluePairStop && blueCross && BluePairIsSolved(state))
                        {
                            solves.CrossPair[Colour.Blue].Solves.Add(newPosition);
                        }
                        if (!yellowPairStop && yellowCross && YellowPairIsSolved(state))
                        {
                            solves.CrossPair[Colour.Yellow].Solves.Add(newPosition);
                        }
                    }
                }                
            }

            depth++;
        }

        return solves;
    }

    private static Position CreatePosition(Position position, Turn turn, out Colour[] state)
    {
        state = RubiksCube.GetStateByHash(position.Hash, turn);
        var newPosition = new Position(position.Depth + 1, state, position.Turns);
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
}
