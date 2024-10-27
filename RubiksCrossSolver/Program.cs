using System.Diagnostics;
using RubiksCrossSolver.SolveDto;

namespace RubiksCrossSolver;

internal class Program
{
    private static void Main()
    {
        var sw = Stopwatch.StartNew();
        //string scramble = "L2, B, F2, D2, Bp, R2, F2, D2, L2, U2, Fp, R, Fp, D, U2, Rp, Fp, Up, F, R2";
        //string scramble = "L2, F2, D, R2, U, F2, U2, B, Lp, Rp, B2, Dp, Fp, U2, Fp, L2";
        //string scramble = "F2, L, U2, F2, L2, D2, U2, B2, L, B, F, U, R, F, Up, Rp, B, D2, L2, U";
        //string scramble = "Rp, F2, U2, L, F2, R, U, Rp, B, Up, Dp, L2, B, D2, F2, Bp, D2, B2, U2";
        //string scramble = "D2, L2, F2, L2, R2, F2, Up, R2, Up, Lp, Fp, R2, D, Up, Bp, R, D, U, F, Lp";
        var scramble = "Dp Lp U2 Rp U2 L2 B2 L B2 F2 L D L2 D2 Bp U2 Fp R Dp";
        Console.WriteLine("Скрамбл: " + scramble);
        var cube = new RubiksCube(scramble);
        Colour[] initialState = cube.GetColourState();
        Solver solver = new(maxDepth: 6);
        CrossSolvesAggregator solves = solver.GetSolves(initialState);
        sw.Stop();
        Console.WriteLine(sw.Elapsed);
        PrintSolution(solves);
    }

    private static void PrintSolution(CrossSolvesAggregator solves)
    {
        foreach (KeyValuePair<Colour, CrossSolveDto> solve in solves.Cross)
        {
            Console.WriteLine($"Решения для {ColorName(solve.Key)} креста:");
            foreach (Position position in solve.Value.GetSolves())
            {
                var solution = string.Join(", ", position.GetTurns());
                Console.WriteLine(solution);
            }
        }

        foreach (KeyValuePair<Colour, CrossPairSolveDto> solve in solves.CrossPair)
        {
            Console.WriteLine($"Решения для {ColorName(solve.Key)} креста с парой:");
            foreach (var solution in solve.Value.GetSolves().Select(position => string.Join(", ", position.GetTurns())))
            {
                Console.WriteLine(solution);
            }
        }
    }

    private static string ColorName(Colour colour)
    {
        var res = colour switch
        {
            Colour.White => "белого",
            Colour.Orange => "оранжевого",
            Colour.Green => "зелёного",
            Colour.Red => "красного",
            Colour.Blue => "синего",
            Colour.Yellow => "жёлтого",
            _ => throw new ArgumentOutOfRangeException(nameof(colour)),
        };
        return res;
    }
}
