namespace RubiksCrossSolver
{
    internal enum Rotation
    {
        /// <summary> Зелёный фронт, белый верх </summary>
        None = 0,

        /// <summary> Жёлтый фронт, зелёный верх </summary>
        X = 1,

        /// <summary> Белый фронт, синий верх </summary>
        Xp = 2,

        /// <summary> Синий фронт, желтый верх </summary>
        X2 = 3,

        /// <summary> Красный фронт, белый верх </summary>
        Y = 4,

        /// <summary> Оранжевый фронт, белый верх </summary>
        Yp = 5,

        /// <summary> Синий фронт, белый верх </summary> --- 
        Y2 = 6,
        X2Z2 = 6,

        /// <summary> Зелёный фронт, оранжевый верх </summary>
        Z = 7,

        /// <summary> Зелёный фронт, красный верх </summary>
        Zp = 8,

        /// <summary> Зелёный фронт, жёлтый верх </summary>
        Z2 = 9,
        X2Y2 = 9,

        /// <summary> Красный фронт, зелёный верх </summary>
        XY = 10,

        /// <summary> Оранжевый фронт, зелёный верх </summary>
        XYp = 11,

        /// <summary> Белый фронт, зелёный верх </summary>
        XY2 = 12,
        XpZ2 = 12,

        /// <summary> Жёлтый фронт, оранжевый верх </summary>
        XZ = 13,

        /// <summary> Жёлтый фронт, красный верх </summary>
        XZp = 14,

        /// <summary> Жёлтый фронт, синий верх </summary>
        XZ2 = 15,
        XpY2 = 15,

        /// <summary> Красный фронт, синий верх </summary>
        XpY = 16,

        /// <summary> Оранжевый фронт, синий верх </summary>
        XpYp = 17,

        /// <summary> Белый фронт, оранжевый верх </summary>
        XpZ = 18,

        /// <summary> Белый фронт, красный верх </summary>
        XpZp = 19,

        /// <summary> Красный фронт, жёлтый верх </summary>
        X2Y = 20,

        /// <summary> Оранжевый фронт, жёлтый верх </summary>
        X2Yp = 21,

        /// <summary> Синий фронт, оранжевый верх </summary>
        X2Z = 22,

        /// <summary> Синий фронт, красный верх </summary>
        X2Zp = 23,
    }
}
