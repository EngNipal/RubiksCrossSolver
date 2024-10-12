using RubiksCrossSolver;
using Xunit;

namespace RubiksCubeTests
{
    public class TurnTests
    {
        [Fact]
        public void MakeScramble_ShouldBeCorrectState()
        {
            // Arrange
            var scramble = new[] { Turn.D2, Turn.U2, Turn.R2, Turn.U2, Turn.F, Turn.D2, Turn.B, Turn.U2, Turn.F2, Turn.R2,
                Turn.B2, Turn.Up, Turn.F, Turn.L, Turn.D2, Turn.B, Turn.L, Turn.Bp, Turn.D, Turn.Rp, Turn.Up };
            var expectedState = new uint[] { 4, 5, 2, 2, 1, 6, 4, 4, 5, 6, 2, 2, 6, 1, 5, 4, 5, 6, 1, 5, 6, 6, 5, 5,
                3, 3, 3, 3, 3, 1, 1, 6, 1, 1, 6, 2, 1, 2, 4, 4, 3, 2, 2, 4, 4, 5, 3, 3 };

            // Act
            var rubiksCube = new RubiksCube(scramble);
            var resultState = rubiksCube.GetCurrentState();

            // Assert
            Assert.Equal(expectedState, resultState);
        }
    }
}
