using RubiksCrossSolver;
using Xunit;

namespace RubiksCubeTests;

public class TurnTests
{
    [Fact]
    public void MakeTurnScrambleShouldBeCorrectState()
    {
        // Arrange
        var scramble0 = new[] { Turn.D2, Turn.U2, Turn.R2, Turn.U2, Turn.F, Turn.D2, Turn.B, Turn.U2, Turn.F2, Turn.R2,
            Turn.B2, Turn.Up, Turn.F, Turn.L, Turn.D2, Turn.B, Turn.L, Turn.Bp, Turn.D, Turn.Rp, Turn.Up };
        var expectedState0 = new uint[] { 4, 5, 2, 2, 1, 6, 4, 4, /**/ 5, 6, 2, 2, 6, 1, 5, 4, /**/ 5, 6, 1, 5, 6, 6, 5, 5, /**/
            3, 3, 3, 3, 3, 1, 1, 6, /**/ 1, 1, 6, 2, 1, 2, 4, 4, /**/ 3, 2, 2, 4, 4, 5, 3, 3 };

        var scramble1 = new[] { Turn.R, Turn.U, Turn.F };
        var expectedState1 = new uint[] { 1, 1, 1, 1, 1, 2, 2, 6, /**/ 3, 3, 6, 2, 6, 2, 2, 5, /**/ 3, 3, 4, 3, 4, 6, 6, 4, /**/
            3, 5, 5, 3, 4, 3, 4, 4, /**/ 2, 2, 2, 1, 5, 1, 5, 5, /**/ 4, 4, 1, 6, 5, 6, 6, 5 };

        var scramble2 = new[] { Turn.L, Turn.D, Turn.B };
        var expectedState2 = new uint[] { 4, 4, 3, 5, 1, 5, 1, 1, /**/ 1, 2, 2, 1, 2, 5, 5, 6, /**/ 1, 3, 3, 1, 3, 2, 2, 2, /**/
            4, 4, 6, 4, 6, 1, 3, 6, /**/ 4, 5, 5, 4, 5, 4, 6, 6, /**/ 3, 3, 3, 6, 6, 2, 2, 5 };

        var scramble3 = new[] { Turn.Rp, Turn.Up, Turn.Fp };
        var expectedState3 = new uint[] { 5, 5, 5, 1, 1, 3, 4, 4, /**/ 6, 5, 1, 2, 1, 2, 2, 1, /**/ 2, 1, 1, 2, 3, 2, 3, 3, /**/
            3, 3, 1, 6, 4, 6, 4, 4, /**/ 4, 4, 4, 6, 5, 6, 5, 5, /**/ 5, 2, 2, 6, 3, 6, 6, 3 };

        var scramble4 = new[] { Turn.Lp, Turn.Dp, Turn.Bp };
        var expectedState4 = new uint[] { 6, 2, 2, 3, 1, 3, 1, 1, /**/ 5, 2, 2, 5, 2, 5, 3, 3, /**/ 6, 3, 3, 6, 3, 4, 4, 4, /**/
            4, 4, 3, 4, 1, 5, 5, 1, /**/ 1, 1, 2, 5, 2, 5, 5, 2, /**/ 6, 6, 6, 6, 6, 1, 4, 4 };

        var scramble5 = new[] { Turn.R2, Turn.U2, Turn.F2 };
        var expectedState5 = new uint[] { 6, 1, 1, 6, 1, 1, 6, 6, /**/ 4, 4, 4, 2, 4, 2, 2, 2, /**/ 5, 3, 3, 5, 3, 5, 5, 3, /**/
            2, 2, 2, 2, 4, 4, 4, 4, /**/ 3, 3, 5, 3, 5, 3, 5, 5, /**/ 1, 1, 6, 6, 1, 6, 6, 1 };

        var scramble6 = new[] { Turn.L2, Turn.D2, Turn.B2 };
        var expectedState6 = new uint[] { 1, 6, 6, 6, 1, 6, 1, 1, /**/ 2, 2, 2, 4, 2, 4, 4, 4, /**/ 5, 3, 3, 5, 3, 5, 5, 3, /**/
            4, 4, 4, 4, 2, 2, 2, 2, /**/ 3, 3, 5, 3, 5, 3, 5, 5, /**/ 6, 6, 1, 6, 1, 1, 1, 6 };

        var scramble7 = new[] {Turn.L2, Turn.F2, Turn.D2, Turn.Bp };
        var expectedState7 = new uint[] { 2, 2, 2, 6, 1, 6, 6, 1, /**/ 6, 2, 4, 1, 4, 1, 4, 4, /**/ 3, 3, 5, 3, 5, 5, 5, 3, /**/
            2, 4, 6, 2, 1, 2, 2, 1, /**/ 3, 3, 5, 5, 3, 5, 5, 3, /**/ 6, 6, 1, 6, 1, 4, 4, 4 };


        // Act
        var rubiksCube0 = new RubiksCube(scramble0);
        var rubiksCube1 = new RubiksCube(scramble1);
        var rubiksCube2 = new RubiksCube(scramble2);
        var rubiksCube3 = new RubiksCube(scramble3);
        var rubiksCube4 = new RubiksCube(scramble4);
        var rubiksCube5 = new RubiksCube(scramble5);
        var rubiksCube6 = new RubiksCube(scramble6);
        var rubiksCube7 = new RubiksCube(scramble7);

        var resultState0 = rubiksCube0.GetCurrentState();
        var resultState1 = rubiksCube1.GetCurrentState();
        var resultState2 = rubiksCube2.GetCurrentState();
        var resultState3 = rubiksCube3.GetCurrentState();
        var resultState4 = rubiksCube4.GetCurrentState();
        var resultState5 = rubiksCube5.GetCurrentState();
        var resultState6 = rubiksCube6.GetCurrentState();
        var resultState7 = rubiksCube7.GetCurrentState();

        // Assert
        Assert.Equal(expectedState0, resultState0);
        Assert.Equal(expectedState1, resultState1);
        Assert.Equal(expectedState2, resultState2);
        Assert.Equal(expectedState3, resultState3);
        Assert.Equal(expectedState4, resultState4);
        Assert.Equal(expectedState5, resultState5);
        Assert.Equal(expectedState6, resultState6);
        Assert.Equal(expectedState7, resultState7);
    }

    [Fact]
    public void MakeStringScrambleShouldReturnCorrectState()
    {
        // Arrange
        string scramble = "L2, B, F2, D2, Bp, R2, F2, D2, L2, U2, Fp, R, Fp, D, U2, Rp, Fp, Up, F, R2";
        var expectedState = new uint[] { 6, 1, 2, 4, 3, 3, 2, 2, /**/ 2, 6, 4, 4, 4, 6, 3, 5, /**/ 1, 3, 1, 3, 1, 4, 2, 3, /**/
            5, 1, 5, 5, 5, 2, 1, 4, /**/ 6, 4, 3, 6, 5, 5, 2, 4, /**/ 1, 6, 1, 6, 2, 3, 5, 6 };

        // Act
        var rubiksCube = new RubiksCube(scramble);
        var resultState = rubiksCube.GetCurrentState();

        // Assert
        Assert.Equal(expectedState, resultState);
    }
}
