using System.Drawing;

namespace RubiksCrossSolver
{
    public sealed class RubiksCube
    {
        /// <summary> Количество элементов на одной грани. </summary>
        /// <remarks> Центр фиксирован, не перемещается и не учитывается. </remarks>
        private const int _elements = 8;

        /// <summary> Количество цветов или граней </summary>
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

        public RubiksCube(Turn[] scramble) : this()
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
        // переделывает Turn нужным образом, а затем вызывает основной GetStateByTurn с правильным Turn-ом.
        public static uint[] GetStateByTurn(uint[] state, Turn turn)
        {
            // Белый              Оранжевый              Зелёный                    Красный                 Синий                    Жёлтый
            // 0 1 2 3 4 5 6 7   8 9 10 11 12 13 14 15   16 17 18 19 20 21 22 23   24 25 26 27 28 29 30 31  32 33 34 35 36 37 38 39  40 41 42 43 44 45 46 47
            var res = new uint[state.Length]; 
            state.CopyTo(res, 0);
            uint buffer;
            switch (turn)
            {
                case Turn.R:
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
                case Turn.Rp:
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
                case Turn.R2:
                    {
                        // Углы красной грани 24-31 и 26-29
                        buffer = res[24];
                        res[24] = res[31];
                        res[31] = buffer;
                        buffer = res[26];
                        res[26] = res[29];
                        res[29] = buffer;

                        // Рёбра красной грани 25-30 и 27-28
                        buffer = res[25];
                        res[25] = res[30];
                        res[30] = buffer;
                        buffer = res[27];
                        res[27] = res[28];
                        res[28] = buffer;

                        // Углы верхние 2-42 и 18-37
                        buffer = res[2];
                        res[2] = res[42];
                        res[42] = buffer;
                        buffer = res[18];
                        res[18] = res[37];
                        res[37] = buffer;

                        // Углы нижние 7-47 и 23-32
                        buffer = res[7];
                        res[7] = res[47];
                        res[47] = buffer;
                        buffer = res[23];
                        res[23] = res[32];
                        res[32] = buffer;

                        // Рёбра 4-44 и 20-35
                        buffer = res[4];
                        res[4] = res[44];
                        res[44] = buffer;
                        buffer = res[20];
                        res[20] = res[35];
                        res[35] = buffer;
                    }
                    break;
                case Turn.U:
                    {
                        // Углы белой грани 0-2-7-5-0
                        buffer = res[0];
                        res[0] = res[5];
                        res[5] = res[7];
                        res[7] = res[2];
                        res[2] = buffer;

                        // Рёбра белой грани 1-4-6-3-1
                        buffer = res[1];
                        res[1] = res[3];
                        res[3] = res[6];
                        res[6] = res[4];
                        res[4] = buffer;

                        // Углы левые 16-8-32-24-16
                        buffer = res[16];
                        res[16] = res[24];
                        res[24] = res[32];
                        res[32] = res[8];
                        res[8] = buffer;

                        // Углы правые 18-10-34-26-18
                        buffer = res[18];
                        res[18] = res[26];
                        res[26] = res[34];
                        res[34] = res[10];
                        res[10] = buffer;

                        // Рёбра 17-9-33-25-17
                        buffer = res[17];
                        res[17] = res[25];
                        res[25] = res[33];
                        res[33] = res[9];
                        res[9] = buffer;
                    }
                    break;
                case Turn.Up:
                    {
                        // Углы белой грани 0-5-7-2-0
                        buffer = res[0];
                        res[0] = res[2];
                        res[2] = res[7];
                        res[7] = res[5];
                        res[5] = buffer;

                        // Рёбра белой грани 1-3-6-4-1
                        buffer = res[1];
                        res[1] = res[4];
                        res[4] = res[6];
                        res[6] = res[3];
                        res[3] = buffer;

                        // Углы левые 16-24-32-8-16
                        buffer = res[16];
                        res[16] = res[24];
                        res[24] = res[32];
                        res[32] = res[8];
                        res[8] = buffer;

                        // Углы правые 18-26-34-10-18
                        buffer = res[18];
                        res[18] = res[26];
                        res[26] = res[34];
                        res[34] = res[10];
                        res[10] = buffer;

                        // Рёбра 17-25-33-9-17
                        buffer = res[17];
                        res[17] = res[25];
                        res[25] = res[33];
                        res[33] = res[9];
                        res[9] = buffer;
                    }
                    break;
                case Turn.U2:
                    {
                        // Углы белой грани 0-7 и 2-5
                        buffer = res[0];
                        res[0] = res[7];
                        res[7] = buffer;
                        buffer = res[2];
                        res[2] = res[5];
                        res[5] = buffer;

                        // Рёбра белой грани 1-6 и 3-4
                        buffer = res[1];
                        res[1] = res[6];
                        res[6] = buffer;
                        buffer = res[3];
                        res[3] = res[4];
                        res[4] = buffer;

                        // Углы левые 16-32 и 8-24
                        buffer = res[16];
                        res[16] = res[32];
                        res[32] = buffer;
                        buffer = res[8];
                        res[8] = res[24];
                        res[24] = buffer;

                        // Углы правые 18-34 и 10-26
                        buffer = res[18];
                        res[18] = res[34];
                        res[34] = buffer;
                        buffer = res[10];
                        res[10] = res[26];
                        res[26] = buffer;

                        // Рёбра 17-33 и 9-25
                        buffer = res[17];
                        res[17] = res[33];
                        res[33] = buffer;
                        buffer = res[9];
                        res[9] = res[25];
                        res[25] = buffer;
                    }
                    break;
                case Turn.F:
                    {
                        // Углы зелёной грани 16-18-21-23-16
                        buffer = res[16];
                        res[16] = res[23];
                        res[23] = res[21];
                        res[21] = res[18];
                        res[18] = buffer;

                        // Рёбра зелёной грани 17-19-20-22-17
                        buffer = res[17];
                        res[17] = res[22];
                        res[22] = res[20];
                        res[20] = res[19];
                        res[19] = buffer;

                        // Углы левые 5-24-42-15-5
                        buffer = res[5];
                        res[5] = res[15];
                        res[15] = res[42];
                        res[42] = res[24];
                        res[24] = buffer;

                        // Углы правые 7-29-40-10-7
                        buffer = res[7];
                        res[7] = res[10];
                        res[10] = res[40];
                        res[40] = res[29];
                        res[29] = buffer;

                        // Рёбра 6-27-41-12-6
                        buffer = res[6];
                        res[6] = res[27];
                        res[27] = res[41];
                        res[41] = res[12];
                        res[12] = buffer;
                    }
                    break;
                case Turn.Fp:
                    {
                        // Углы зелёной грани 16-23-21-18-16
                        buffer = res[16];
                        res[16] = res[18];
                        res[18] = res[21];
                        res[21] = res[23];
                        res[23] = buffer;

                        // Рёбра зелёной грани 17-22-20-19-17
                        buffer = res[17];
                        res[17] = res[19];
                        res[19] = res[20];
                        res[20] = res[22];
                        res[22] = buffer;

                        // Углы левые 5-15-42-24-5
                        buffer = res[5];
                        res[5] = res[24];
                        res[24] = res[42];
                        res[42] = res[15];
                        res[15] = buffer;

                        // Углы правые 7-10-40-29-7
                        buffer = res[7];
                        res[7] = res[29];
                        res[29] = res[40];
                        res[40] = res[10];
                        res[10] = buffer;

                        // Рёбра 6-27-41-12-6
                        buffer = res[6];
                        res[6] = res[27];
                        res[27] = res[41];
                        res[41] = res[12];
                        res[12] = buffer;
                    }
                    break;
                case Turn.F2:
                    {
                        // Углы зелёной грани 16-23 и 18-21
                        buffer = res[16];
                        res[16] = res[23];
                        res[23] = buffer;
                        buffer = res[18];
                        res[18] = res[21];
                        res[21] = buffer;

                        // Рёбра зелёной грани 17-22 и 19-20
                        buffer = res[17];
                        res[17] = res[22];
                        res[22] = buffer;
                        buffer = res[19];
                        res[19] = res[20];
                        res[20] = buffer;

                        // Углы левые 5-42 и 15-24
                        buffer = res[5];
                        res[5] = res[42];
                        res[42] = buffer;
                        buffer = res[15];
                        res[15] = res[24];
                        res[24] = buffer;

                        // Углы правые 7-40 и 10-29
                        buffer = res[7];
                        res[7] = res[40];
                        res[40] = buffer;
                        buffer = res[10];
                        res[10] = res[29];
                        res[29] = buffer;

                        // Рёбра 6-41 и 12-27
                        buffer = res[6];
                        res[6] = res[41];
                        res[41] = buffer;
                        buffer = res[12];
                        res[12] = res[27];
                        res[27] = buffer;
                    }
                    break;
                case Turn.L:
                    {
                        // Углы оранжевой грани 8-10-15-13-8
                        buffer = res[8];
                        res[8] = res[13];
                        res[13] = res[15];
                        res[15] = res[10];
                        res[10] = buffer;

                        // Рёбра красной грани 9-12-14-11-9
                        buffer = res[9];
                        res[9] = res[11];
                        res[11] = res[14];
                        res[14] = res[12];
                        res[12] = buffer;

                        // Углы верхние 0-16-40-39-0
                        buffer = res[0];
                        res[0] = res[39];
                        res[39] = res[40];
                        res[40] = res[16];
                        res[16] = buffer;

                        // Углы нижние 5-21-45-34-5
                        buffer = res[5];
                        res[5] = res[34];
                        res[34] = res[45];
                        res[45] = res[21];
                        res[21] = buffer;

                        // Рёбра 3-19-43-36-3
                        buffer = res[3];
                        res[3] = res[36];
                        res[36] = res[43];
                        res[43] = res[19];
                        res[19] = buffer;
                    }
                    break;
                case Turn.Lp:
                    {
                        // Углы оранжевой грани 8-13-15-10-8
                        buffer = res[8];
                        res[8] = res[10];
                        res[10] = res[15];
                        res[15] = res[13];
                        res[13] = buffer;

                        // Рёбра оранжевой грани 9-11-14-12-9
                        buffer = res[9];
                        res[9] = res[12];
                        res[12] = res[14];
                        res[14] = res[11];
                        res[11] = buffer;

                        // Углы верхние 0-39-40-16-0
                        buffer = res[0];
                        res[0] = res[16];
                        res[16] = res[40];
                        res[40] = res[39];
                        res[39] = buffer;

                        // Углы нижние 5-34-45-21-5
                        buffer = res[5];
                        res[5] = res[21];
                        res[21] = res[45];
                        res[45] = res[34];
                        res[34] = buffer;

                        // Рёбра 3-36-43-19-3
                        buffer = res[3];
                        res[3] = res[19];
                        res[19] = res[43];
                        res[43] = res[36];
                        res[36] = buffer;
                    }
                    break;
                case Turn.L2:
                    {
                        // Углы оранжевой грани 8-15 10-13
                        buffer = res[8];
                        res[8] = res[15];
                        res[15] = buffer;
                        buffer = res[10];
                        res[10] = res[13];
                        res[13] = buffer;

                        // Рёбра оранжевой грани 9-14 11-12
                        buffer = res[9];
                        res[9] = res[14];
                        res[14] = buffer;
                        buffer = res[11];
                        res[11] = res[12];
                        res[12] = buffer;

                        // Углы верхние 0-40 16-39
                        buffer = res[0];
                        res[0] = res[40];
                        res[40] = buffer;
                        buffer = res[16];
                        res[16] = res[39];
                        res[39] = buffer;

                        // Углы нижние 5-45 21-34
                        buffer = res[5];
                        res[5] = res[45];
                        res[45] = buffer;
                        buffer = res[21];
                        res[21] = res[34];
                        res[34] = buffer;

                        // Рёбра 3-43 19-36
                        buffer = res[3];
                        res[3] = res[43];
                        res[43] = buffer;
                        buffer = res[19];
                        res[19] = res[36];
                        res[36] = buffer;
                    }
                    break;
                // Белый              Оранжевый              Зелёный                    Красный                 Синий                    Жёлтый
                // 0 1 2 3 4 5 6 7   8 9 10 11 12 13 14 15   16 17 18 19 20 21 22 23   24 25 26 27 28 29 30 31  32 33 34 35 36 37 38 39  40 41 42 43 44 45 46 47
                case Turn.B:
                    {
                        // Углы синей грани 32-34-39-37-32
                        buffer = res[32];
                        res[32] = res[37];
                        res[37] = res[39];
                        res[39] = res[34];
                        res[34] = buffer;

                        // Рёбра синей грани 33-36-38-35-33
                        buffer = res[33];
                        res[33] = res[35];
                        res[35] = res[38];
                        res[38] = res[36];
                        res[36] = buffer;

                        // Углы первый набор 2-8-45-31-2
                        buffer = res[2];
                        res[2] = res[31];
                        res[31] = res[45];
                        res[45] = res[8];
                        res[8] = buffer;

                        // Углы второй набор 0-13-47-26-0 
                        buffer = res[0];
                        res[0] = res[26];
                        res[26] = res[47];
                        res[47] = res[13];
                        res[13] = buffer;

                        // Рёбра 1-11-46-28-1
                        buffer = res[1];
                        res[1] = res[28];
                        res[28] = res[46];
                        res[46] = res[11];
                        res[11] = buffer;
                    }
                    break;
                case Turn.Bp:
                    {
                        // Углы синей грани 32-37-39-34-32
                        buffer = res[32];
                        res[32] = res[34];
                        res[34] = res[39];
                        res[39] = res[37];
                        res[37] = buffer;

                        // Рёбра синей грани 33-35-38-36-33
                        buffer = res[33];
                        res[33] = res[36];
                        res[36] = res[38];
                        res[38] = res[35];
                        res[35] = buffer;

                        // Углы первый набор 2-31-45-8-2
                        buffer = res[2];
                        res[2] = res[8];
                        res[8] = res[45];
                        res[45] = res[31];
                        res[31] = buffer;

                        // Углы второй набор 0-26-47-13-0 
                        buffer = res[0];
                        res[0] = res[13];
                        res[13] = res[47];
                        res[47] = res[26];
                        res[26] = buffer;

                        // Рёбра 1-28-46-11-1
                        buffer = res[1];
                        res[1] = res[11];
                        res[11] = res[46];
                        res[46] = res[28];
                        res[28] = buffer;
                    }
                    break;
                case Turn.B2:
                    {
                        // Углы синей грани 32-39 34-37
                        buffer = res[32];
                        res[32] = res[39];
                        res[39] = buffer;
                        buffer = res[34];
                        res[34] = res[37];
                        res[37] = buffer;

                        // Рёбра синей грани 33-38 35-36
                        buffer = res[33];
                        res[33] = res[38];
                        res[38] = buffer;
                        buffer = res[35];
                        res[35] = res[36];
                        res[36] = buffer;

                        // Углы первый набор 2-45 8-31
                        buffer = res[2];
                        res[2] = res[45];
                        res[45] = buffer;
                        buffer = res[8];
                        res[8] = res[31];
                        res[31] = buffer;

                        // Углы второй набор 0-47 13-26 
                        buffer = res[0];
                        res[0] = res[47];
                        res[47] = buffer;
                        buffer = res[13];
                        res[13] = res[26];
                        res[26] = buffer;

                        // Рёбра 1-46 11-28
                        buffer = res[1];
                        res[1] = res[46];
                        res[46] = buffer;
                        buffer = res[11];
                        res[11] = res[28];
                        res[28] = buffer;
                    }
                    break;
                case Turn.D:
                    {
                        // Углы жёлтой грани 40-42-47-45-40
                        buffer = res[40];
                        res[40] = res[45];
                        res[45] = res[47];
                        res[47] = res[42];
                        res[42] = buffer;

                        // Рёбра жёлтой грани 41-44-46-43-41
                        buffer = res[41];
                        res[41] = res[43];
                        res[43] = res[46];
                        res[46] = res[44];
                        res[44] = buffer;

                        // Углы первый набор 21-29-37-13-21
                        buffer = res[21];
                        res[21] = res[13];
                        res[13] = res[37];
                        res[37] = res[29];
                        res[29] = buffer;

                        // Углы второй набор 23-31-39-15-23
                        buffer = res[23];
                        res[23] = res[15];
                        res[15] = res[39];
                        res[39] = res[31];
                        res[31] = buffer;

                        // Рёбра 22-30-38-14-22
                        buffer = res[22];
                        res[22] = res[14];
                        res[14] = res[38];
                        res[38] = res[30];
                        res[30] = buffer;
                    }
                    break;
                case Turn.Dp:
                    {
                        // Углы жёлтой грани 40-45-47-42-40
                        buffer = res[40];
                        res[40] = res[42];
                        res[42] = res[47];
                        res[47] = res[45];
                        res[45] = buffer;

                        // Рёбра жёлтой грани 41-43-46-44-41
                        buffer = res[41];
                        res[41] = res[44];
                        res[44] = res[46];
                        res[46] = res[43];
                        res[43] = buffer;

                        // Углы первый набор 21-13-37-29-21
                        buffer = res[21];
                        res[21] = res[29];
                        res[29] = res[37];
                        res[37] = res[13];
                        res[13] = buffer;

                        // Углы второй набор 23-15-39-31-23
                        buffer = res[23];
                        res[23] = res[31];
                        res[31] = res[39];
                        res[39] = res[15];
                        res[15] = buffer;

                        // Рёбра 22-14-38-30-22
                        buffer = res[22];
                        res[22] = res[30];
                        res[30] = res[38];
                        res[38] = res[14];
                        res[14] = buffer;
                    }
                    break;
                case Turn.D2:
                    {
                        // Углы жёлтой грани 40-47 42-45
                        buffer = res[40];
                        res[40] = res[47];
                        res[47] = buffer;
                        buffer = res[42];
                        res[42] = res[45];
                        res[45] = buffer;

                        // Рёбра жёлтой грани 41-46 43-44
                        buffer = res[41];
                        res[41] = res[46];
                        res[46] = buffer;
                        buffer = res[43];
                        res[43] = res[44];
                        res[44] = buffer;

                        // Углы первый набор 21-37 13-29
                        buffer = res[21];
                        res[21] = res[37];
                        res[37] = buffer;
                        buffer = res[13];
                        res[13] = res[29];
                        res[29] = buffer;

                        // Углы второй набор 23-39 15-31
                        buffer = res[23];
                        res[23] = res[39];
                        res[39] = buffer;
                        buffer = res[15];
                        res[15] = res[31];
                        res[31] = buffer;

                        // Рёбра 22-38 14-30
                        buffer = res[22];
                        res[22] = res[38];
                        res[38] = buffer;
                        buffer = res[14];
                        res[14] = res[30];
                        res[30] = buffer;
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
