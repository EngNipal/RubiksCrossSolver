namespace RubiksCrossSolver;

internal class Program
{
    static void Main(string[] args)
    {
        //string scramble = "L2, B, F2, D2, Bp, R2, F2, D2, L2, U2, Fp, R, Fp, D, U2, Rp, Fp, Up, F, R2";
        //string scramble = "L2, F2, D, R2, U, F2, U2, B, Lp, Rp, B2, Dp, Fp, U2, Fp, L2";
        string scramble = "F2, L, U2, F2, L2, D2, U2, B2, L, B, F, U, R, F, Up, Rp, B, D2, L2, U";
        var rubiksCube = new RubiksCube(scramble);
        uint[] initialState = rubiksCube.GetCurrentState();
        List<Position> solves = GetSolves(initialState);
        PrintSolution(solves);
        Console.ReadLine();
    }

    private static void PrintSolution(List<Position> solved)
    {
        foreach (var position in solved)
        {
            var solution = string.Join(", ", position.Turns);
            Console.WriteLine(solution);
        }
    }

    private static List<Position> GetSolves(uint[] initialState)
    {
        int depth = 0;
        var initial = new Position(depth, initialState, []);
        initial.SetHash();

        var positions = new Dictionary<string, Position>
        {
            { initial.Hash, initial }
        };

        var solves = new List<Position>();
        while (depth < 9 && solves.Count == 0)
        {
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
                        if (WhiteCrossIsSolved(newPosition.State))
                        {
                            solves.Add(newPosition);
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
}
