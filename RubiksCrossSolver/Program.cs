namespace RubiksCrossSolver;

internal class Program
{
    static void Main(string[] args)
    {
        //string scramble = "L2, B, F2, D2, Bp, R2, F2, D2, L2, U2, Fp, R, Fp, D, U2, Rp, Fp, Up, F, R2";
        //string scramble = "L2, F2, D, R2, U, F2, U2, B, Lp, Rp, B2, Dp, Fp, U2, Fp, L2";
        //string scramble = "F2, L, U2, F2, L2, D2, U2, B2, L, B, F, U, R, F, Up, Rp, B, D2, L2, U";
        //string scramble = "Rp, F2, U2, L, F2, R, U, Rp, B, Up, Dp, L2, B, D2, F2, Bp, D2, B2, U2";
        string scramble = "D2, L2, F2, L2, R2, F2, Up, R2, Up, Lp, Fp, R2, D, Up, Bp, R, D, U, F, Lp";
        Console.WriteLine("Скрамбл: " + scramble);
        var rubiksCube = new RubiksCube(scramble);
        uint[] initialState = rubiksCube.GetCurrentState();
        List<Position>[] solves = GetSolves(initialState);
        PrintSolution(solves);
        Console.ReadLine();
    }

    private static void PrintSolution(List<Position>[] solves)
    {
        for (int i = 0; i < solves.Length; i++)
        {
            string colorName = ColorName(i);
            Console.WriteLine($"Решения для {colorName} креста:");
            foreach (var position in solves[i])
            {
                var solution = string.Join(", ", position.Turns);
                Console.WriteLine(solution);
            }
        }
    }

    private static string ColorName(int i)
    {
        string res = i switch
        {
            0 => "белого",
            1 => "оранжевого",
            2 => "зелёного",
            3 => "красного",
            4 => "синего",
            5 => "жёлтого",
            _ => throw new ArgumentOutOfRangeException(nameof(i)),
        };
        return res;
    }

    private static List<Position>[] GetSolves(uint[] initialState)
    {
        int depth = 0;
        var initial = new Position(depth, initialState, []);
        initial.SetHash();

        var positions = new Dictionary<string, Position>
        {
            { initial.Hash, initial }
        };

        var whiteSolves = new List<Position>();
        var orangeSolves = new List<Position>();
        var greenSolves = new List<Position>();
        var redSolves = new List<Position>();
        var blueSolves = new List<Position>();
        var yellowSolves = new List<Position>();
        var solves = new[] { whiteSolves, orangeSolves, greenSolves, redSolves, blueSolves, yellowSolves };
        while (depth < 7 && solves.Count(x => x.Count == 0) > 2)
        {
            bool whiteStop = whiteSolves.Count > 0;
            bool orangeStop = orangeSolves.Count > 0;
            bool greenStop = greenSolves.Count > 0;
            bool redStop = redSolves.Count > 0;
            bool blueStop = blueSolves.Count > 0;
            bool yellowStop = yellowSolves.Count > 0;
            var deepPositions = positions.Values.Where(x => x.Depth == depth).ToList();
            foreach (var position in deepPositions)
            {
                var antiturn = position.GetAntiturn();
                foreach (Turn turn in Enum.GetValues(typeof(Turn)))
                {
                    if (depth > 0 && turn == antiturn) continue;
                    Position newPosition = CreatePosition(position, turn);
                    if (positions.TryAdd(newPosition.Hash, newPosition))
                    {                        
                        if (!whiteStop && WhiteCrossIsSolved(newPosition.State))
                        {
                            whiteSolves.Add(newPosition);
                        }
                        if (!orangeStop && OrangeCrossIsSolved(newPosition.State))
                        {
                            orangeSolves.Add(newPosition);
                        }
                        if (!greenStop && GreenCrossIsSolved(newPosition.State))
                        {
                            greenSolves.Add(newPosition);
                        }
                        if (!redStop && RedCrossIsSolved(newPosition.State))
                        {
                            redSolves.Add(newPosition);
                        }
                        if (!blueStop && BlueCrossIsSolved(newPosition.State))
                        {
                            blueSolves.Add(newPosition);
                        }
                        if (!yellowStop && YellowCrossIsSolved(newPosition.State))
                        {
                            yellowSolves.Add(newPosition);
                        }
                    }
                }
            }

            depth++;
        }

        return solves;
    }

    private static Position CreatePosition(Position position, Turn turn)
    {
        uint[] newState = RubiksCube.GetStateByTurn(position.State, turn);
        var newPosition = new Position(position.Depth + 1, newState, position.Turns);
        newPosition.SetHash();
        newPosition.AddTurn(turn);
        return newPosition;
    }

    private static bool WhiteCrossIsSolved(uint[] state)
    {
        return state[1] == (uint)Colour.White &&
            state[3] == (uint)Colour.White &&
            state[4] == (uint)Colour.White &&
            state[6] == (uint)Colour.White &&
            state[9] == (uint)Colour.Orange &&
            state[17] == (uint)Colour.Green &&
            state[25] == (uint)Colour.Red &&
            state[33] == (uint)Colour.Blue;
    }

    private static bool OrangeCrossIsSolved(uint[] state)
    {
        return state[9] == (uint)Colour.Orange &&
            state[11] == (uint)Colour.Orange &&
            state[12] == (uint)Colour.Orange &&
            state[14] == (uint)Colour.Orange &&
            state[3] == (uint)Colour.White &&
            state[19] == (uint)Colour.Green &&
            state[43] == (uint)Colour.Yellow &&
            state[36] == (uint)Colour.Blue;
    }

    private static bool GreenCrossIsSolved(uint[] state)
    {
        return state[17] == (uint)Colour.Green &&
            state[19] == (uint)Colour.Green &&
            state[20] == (uint)Colour.Green &&
            state[22] == (uint)Colour.Green &&
            state[6] == (uint)Colour.White &&
            state[27] == (uint)Colour.Red &&
            state[41] == (uint)Colour.Yellow &&
            state[12] == (uint)Colour.Orange;
    }

    private static bool RedCrossIsSolved(uint[] state)
    {
        return state[25] == (uint)Colour.Red &&
            state[27] == (uint)Colour.Red &&
            state[28] == (uint)Colour.Red &&
            state[30] == (uint)Colour.Red &&
            state[4] == (uint)Colour.White &&
            state[35] == (uint)Colour.Blue &&
            state[44] == (uint)Colour.Yellow &&
            state[20] == (uint)Colour.Green;
    }

    private static bool BlueCrossIsSolved(uint[] state)
    {
        return state[33] == (uint)Colour.Blue &&
            state[35] == (uint)Colour.Blue &&
            state[36] == (uint)Colour.Blue &&
            state[38] == (uint)Colour.Blue &&
            state[1] == (uint)Colour.White &&
            state[11] == (uint)Colour.Orange &&
            state[46] == (uint)Colour.Yellow &&
            state[28] == (uint)Colour.Red;
    }

    private static bool YellowCrossIsSolved(uint[] state)
    {
        return state[41] == (uint)Colour.Yellow &&
            state[43] == (uint)Colour.Yellow &&
            state[44] == (uint)Colour.Yellow &&
            state[46] == (uint)Colour.Yellow &&
            state[14] == (uint)Colour.Orange &&
            state[22] == (uint)Colour.Green &&
            state[30] == (uint)Colour.Red &&
            state[38] == (uint)Colour.Blue;
    }
}
