using System.Drawing;

namespace RubiksCrossSolver
{
    public sealed class RubiksCube
    {
        private const int _elements = 8;
        private const int _colorAmount = 6;
        public RubiksCube()
        {
            for (int i = 0; i < _elements; i++)
            {
                State[i] = (uint)Colour.White;
                State[i + _elements] = (uint)Colour.Orange;
                State[i + _elements * 2] = (uint)Colour.Green;
                State[i + _elements * 3] = (uint)Colour.Red;
                State[i + _elements * 4] = (uint)Colour.Blue;
                State[i + _elements * 5] = (uint)Colour.Yellow;
            }
        }

        public RubiksCube(Turns[] scramble) : this()
        {
            foreach (var turn in scramble)
            {
                State = GetStateByTurn(State, turn);
            }
        }

        public RubiksCube(ulong[] state)
        {
            state.CopyTo(State, 0);
        }

        public uint[] State { get; private set; } = new uint[_colorAmount * _elements];

        public uint[] GetCurrentState()
        {
            var state = new uint[_colorAmount * _elements];
            State.CopyTo(state, 0);
            return state;
        }

        public Color[][] GetColoredState()
        {
            var result = new Color[_colorAmount][];
            result[0] = GetSideByColor(KnownColor.White);
            result[1] = GetSideByColor(KnownColor.Orange);
            result[2] = GetSideByColor(KnownColor.Green);
            result[3] = GetSideByColor(KnownColor.Red);
            result[4] = GetSideByColor(KnownColor.Blue);
            result[5] = GetSideByColor(KnownColor.Yellow);
            return result;
        }

        public Color[] GetSideByColor(KnownColor color)
        {
            var result = new Color[_elements];
            int offset = GetOffset(color);
            for (int i = 0; i < _elements; i++)
            {
                var kc = ConvertToKnownColor((Colour)State[i + offset]);
                result[i] = Color.FromKnownColor(kc);
            }

            return result;
        }

        private static KnownColor ConvertToKnownColor(Colour colour)
        {
            return colour switch
            {
                Colour.White => KnownColor.White,
                Colour.Orange => KnownColor.Orange,
                Colour.Green => KnownColor.Green,
                Colour.Red => KnownColor.Red,
                Colour.Blue => KnownColor.Blue,
                Colour.Yellow => KnownColor.Yellow,
                _ => throw new ArgumentException("Передан неподдерживаемый цвет", nameof(colour))
            };
        }

        // TODO: Написать версию метода, которая принимает перехват кубика или их комбинацию + Turn,
        // переделывает Turn нужным образом, а затем вызывает основной MakeTurn с правильным Turn-ом.
        public static uint[] GetStateByTurn(uint[] state, Turns turn)
        {
            // Белый              Оранжевый              Зелёный                    Красный                 Синий                    Жёлтый
            // 0 1 2 3 4 5 6 7   8 9 10 11 12 13 14 15   16 17 18 19 20 21 22 23   24 25 26 27 28 29 30 31  32 33 34 35 36 37 38 39  40 41 42 43 44 45 46 47
            var res = new uint[state.Length]; 
            state.CopyTo(res, 0);
            uint buffer;
            switch (turn)
            {
                case Turns.R:
                    {
                        // Углы красной грани
                        buffer = res[24];
                        res[24] = res[29];
                        res[29] = res[31];
                        res[31] = res[26];
                        res[26] = buffer;

                        // Рёбра красной грани
                        buffer = res[25];
                        res[25] = res[27];
                        res[27] = res[30];
                        res[30] = res[28];
                        res[28] = buffer;

                        // Углы верхние
                        buffer = res[2];
                        res[2] = res[18];
                        res[18] = res[42];
                        res[42] = res[37];
                        res[37] = buffer;

                        // Углы нижние
                        buffer = res[7];
                        res[7] = res[23];
                        res[23] = res[47];
                        res[47] = res[32];
                        res[32] = buffer;

                        // Рёбра
                        buffer = res[4];
                        res[4] = res[20];
                        res[20] = res[44];
                        res[44] = res[35];
                        res[35] = buffer;
                    }
                    break;
                case Turns.Rp:
                    {
                        // Углы красной грани 24-29-31-26-24
                        buffer = res[24];
                        res[24] = res[26];
                        res[26] = res[31];
                        res[31] = res[24];
                        res[24] = buffer;

                        // Рёбра красной грани 25-27-30-28-25
                        buffer = res[25];
                        res[25] = res[28];
                        res[28] = res[30];
                        res[30] = res[27];
                        res[27] = buffer;

                        // Углы верхние 2-18-42-37-2
                        buffer = res[2];
                        res[2] = res[37];
                        res[37] = res[42];
                        res[42] = res[18];
                        res[18] = buffer;

                        // Углы нижние 7-23-47-32-7
                        buffer = res[7];
                        res[7] = res[32];
                        res[32] = res[47];
                        res[47] = res[23];
                        res[23] = buffer;

                        // Рёбра 4-20-44-35-4
                        buffer = res[4];
                        res[4] = res[35];
                        res[35] = res[44];
                        res[44] = res[20];
                        res[20] = buffer;
                    }
                    break;
                case Turns.R2:
                    {

                    }
                    break;
                case Turns.U:
                    {

                    }
                    break;
                case Turns.Up:
                    {

                    }
                    break;
                case Turns.U2:
                    {

                    }
                    break;
                case Turns.F:
                    {

                    }
                    break;
                case Turns.Fp:
                    {

                    }
                    break;
                case Turns.F2:
                    {

                    }
                    break;
                case Turns.L:
                    {

                    }
                    break;
                case Turns.Lp:
                    {

                    }
                    break;
                case Turns.L2:
                    {

                    }
                    break;
                case Turns.B:
                    {

                    }
                    break;
                case Turns.Bp:
                    {

                    }
                    break;
                case Turns.B2:
                    {

                    }
                    break;
                case Turns.D:
                    {

                    }
                    break;
                case Turns.Dp:
                    {

                    }
                    break;
                case Turns.D2:
                    {

                    }
                    break;
                default: break;
            }
            return res;
        }

        private static int GetOffset(KnownColor color)
        {
            return color switch
            {
                KnownColor.White => 0,
                KnownColor.Orange => _elements,
                KnownColor.Green => _elements * 2,
                KnownColor.Red => _elements * 3,
                KnownColor.Blue => _elements * 4,
                KnownColor.Yellow => _elements * 5,
                _ => throw new ArgumentException("Передан неподдерживаемый цвет", nameof(color)),
            };
        }
    }
}
