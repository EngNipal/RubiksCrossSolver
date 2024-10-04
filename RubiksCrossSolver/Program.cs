namespace RubiksCrossSolver
{
    internal class Program
    {
        static void Main(string[] args)
        {            
            var scramble = new Turn[] { Turn.R };
            var rubiksCube = new RubiksCube(scramble);
            uint[] initialState = rubiksCube.GetCurrentState();
            List<Position> solves = GetSolves(initialState);
            PrintSolution(solves);
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
            var zobrist = new Zobrist(48, 6);
            var initial = new Position(depth, initialState);
            initial.SetHash(zobrist);

            var positions = new Dictionary<ulong, Position>
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
                        Position newPosition = CreatePosition(zobrist, depth, position.State, turn);
                        if (positions.TryAdd(newPosition.Hash, newPosition))
                        {
                            newPosition.AddTurn(turn);
                            if (CrossIsSolved(newPosition.State))
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

        private static Position CreatePosition(Zobrist zobrist, int depth, uint[] state, Turn turn)
        {
            uint[] newState = RubiksCube.GetStateByTurn(state, turn);
            var newPosition = new Position(depth + 1, newState);
            newPosition.SetHash(zobrist);
            return newPosition;
        }

        private static bool CrossIsSolved(uint[] state)
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
}
