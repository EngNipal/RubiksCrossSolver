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

    public List<Position>[] GetSolves(Colour[] initialState)
    {
        int depth = 0;
        var initial = new Position(depth, initialState, []);
        var whiteSolves = new List<Position>();
        var whiteWithPair = new List<Position>();
        var orangeSolves = new List<Position>();
        var orangeWithPair = new List<Position>();
        var greenSolves = new List<Position>();
        var greenWithPair = new List<Position>();
        var redSolves = new List<Position>();
        var redWithPair = new List<Position>();
        var blueSolves = new List<Position>();
        var blueWithPair = new List<Position>();
        var yellowSolves = new List<Position>();
        var yellowWithPair = new List<Position>();
        Dictionary<BigInteger, Position> positions = new() { { initial.Hash, initial } };
        var solves = new[] { whiteSolves, orangeSolves, greenSolves, redSolves, blueSolves, yellowSolves,
            whiteWithPair, orangeWithPair, greenWithPair, redWithPair, blueWithPair, yellowWithPair };
        while (depth < _maxDepth)
        {
            bool whiteStop = whiteSolves.Count > 0;
            bool orangeStop = orangeSolves.Count > 0;
            bool greenStop = greenSolves.Count > 0;
            bool redStop = redSolves.Count > 0;
            bool blueStop = blueSolves.Count > 0;
            bool yellowStop = yellowSolves.Count > 0;
            bool whitePairStop = whiteWithPair.Count > 0;
            bool orangePairStop = orangeWithPair.Count > 0;
            bool greenPairStop = greenWithPair.Count > 0;
            bool redPairStop = redWithPair.Count > 0;
            bool bluePairStop = blueWithPair.Count > 0;
            bool yellowPairStop = yellowWithPair.Count > 0;
            
            var deepPositions = positions.Values.Where(x => x.Depth == depth).ToList();
            foreach (var position in deepPositions)
            {
                var antiturn = position.GetAntiturn();
                foreach (Turn turn in Enum.GetValues(typeof(Turn)))
                {
                    if (depth > 0 && turn == antiturn) continue;
                    Position newPosition = CreatePosition(position, turn, out Colour[] state);
                    if (!positions.TryAdd(newPosition.Hash, newPosition))
                    {
                        continue;
                    }

                    bool whiteCross = WhiteCrossIsSolved(state);
                    bool orangeCross = OrangeCrossIsSolved(state);
                    bool greenCross = GreenCrossIsSolved(state);
                    bool redCross = RedCrossIsSolved(state);
                    bool blueCross = BlueCrossIsSolved(state);
                    bool yellowCross = YellowCrossIsSolved(state);
                    if (!whiteStop && whiteCross)
                    {
                        whiteSolves.Add(newPosition);
                    }
                    if (!orangeStop && orangeCross)
                    {
                        orangeSolves.Add(newPosition);
                    }
                    if (!greenStop && greenCross)
                    {
                        greenSolves.Add(newPosition);
                    }
                    if (!redStop && redCross)
                    {
                        redSolves.Add(newPosition);
                    }
                    if (!blueStop && blueCross)
                    {
                        blueSolves.Add(newPosition);
                    }
                    if (!yellowStop && yellowCross)
                    {
                        yellowSolves.Add(newPosition);
                    }
                    // ---
                    if (!whitePairStop && whiteCross && WhitePairIsSolved(state))
                    {
                        whiteWithPair.Add(newPosition);
                    }
                    if (!orangePairStop && orangeCross && OrangePairIsSolved(state))
                    {
                        orangeWithPair.Add(newPosition);
                    }
                    if (!greenPairStop && greenCross && GreenPairIsSolved(state))
                    {
                        greenWithPair.Add(newPosition);
                    }
                    if (!redPairStop && redCross && RedPairIsSolved(state))
                    {
                        redWithPair.Add(newPosition);
                    }
                    if (!bluePairStop && blueCross && BluePairIsSolved(state))
                    {
                        blueWithPair.Add(newPosition);
                    }
                    if (!yellowPairStop && yellowCross && YellowPairIsSolved(state))
                    {
                        yellowWithPair.Add(newPosition);
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
