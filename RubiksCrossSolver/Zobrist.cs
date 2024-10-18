namespace RubiksCrossSolver;

/// <summary> Формирование Zobrist-ключей хэша для позиций </summary>
/// <remarks> Смотри https://en.wikipedia.org/wiki/Zobrist_hashing </remarks>
public class Zobrist
{
    private byte _fiedlsAmount; // 48
    private byte _statesAmount; // 6        
    private int[][] _hashTable;
    public Zobrist(byte fieldsAmount, byte statesAmount)
    {
        _hashTable = new int[fieldsAmount][];
        _fiedlsAmount = fieldsAmount;
        _statesAmount = statesAmount;
        for (int i = 0; i < fieldsAmount; i++)
        {
            _hashTable[i] = new int[statesAmount];
            for (int j = 0; j < statesAmount; j++)
            {
                _hashTable[i][j] = GetRandomBitString();
            }
        }
    }    

    public int Hash(byte[] state)
    {
        if (state.Length != _fiedlsAmount)
            throw new ArgumentException("Неверный размер поля", nameof(state));

        int result = 0;
        for (int i = 0; i < _fiedlsAmount; i++)
        {
            byte j = (byte)(state[i] - 1);
            result ^= _hashTable[i][j];
        }

        return result;
    }

    private int GetRandomBitString()
    {
        const int _multiplicator = 14143;
        var rnd = new Random();
        var s = rnd.Next();
        s ^= s >> 11;
        s ^= s >> 13;
        s ^= s >> 17;
        s ^= s >> 23;
        return s * _multiplicator;
    }
}
