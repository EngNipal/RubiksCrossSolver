namespace RubiksCrossSolver;

/// <summary> Формирование Zobrist-ключей хэша для позиций </summary>
/// <remarks> Смотри https://en.wikipedia.org/wiki/Zobrist_hashing </remarks>
public class Zobrist
{
    private const ulong _defaultRandomString = 7080895258217026699;
    private int _fiedlsAmount; // 48
    private int _statesAmount; // 6        
    private ulong[][] _hashTable;
    public Zobrist(int fieldsAmount, int statesAmount)
    {
        _hashTable = new ulong[fieldsAmount][];
        _fiedlsAmount = fieldsAmount;
        _statesAmount = statesAmount;
        for (int i = 0; i < fieldsAmount; i++)
        {
            _hashTable[i] = new ulong[statesAmount];
            for (int j = 0; j < statesAmount; j++)
            {
                _hashTable[i][j] = GetRandomBitString();
            }
        }
    }

    public ulong Seed { get; private set; } = _defaultRandomString;

    public ulong Hash(uint[] state)
    {
        if (state.Length != _fiedlsAmount)
            throw new ArgumentException("Неверный размер поля", nameof(state));

        ulong result = 0;
        for (int i = 0; i < _fiedlsAmount; i++)
        {
            ulong j = state[i] - 1;
            result ^= _hashTable[i][j];
        }

        return result;
    }

    private ulong GetRandomBitString()
    {
        const ulong _multiplicator = 91283962591;
        var s = Seed;
        s ^= s >> 11;
        s ^= s >> 13;
        s ^= s >> 17;
        Seed = s;
        return s * _multiplicator;
    }
}
