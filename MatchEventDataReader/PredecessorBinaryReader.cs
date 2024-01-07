using System.Text;
using MatchEventDataReader.Enums;
using MatchEventDataReader.Models;

namespace MatchEventDataReader;

public class PredecessorBinaryReader : BinaryReader
{
    private const int SkipArray = 13;
    private const int SkipStruct = 14;
    private const int SkipString = 11;
    private const int SkipInteger = 11;
    private const int SkipName = 12;

    public PredecessorBinaryReader(Stream input) : base(input)
    {
    }

    public PredecessorBinaryReader(Stream input, Encoding encoding) : base(input, encoding)
    {
    }

    public PredecessorBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
    {
    }

    public string ReadStr()
    {
        var length = ReadUInt32();
        if (length == 0) return "";
        var stringBytes = ReadBytes((int)length - 1);
        return Encoding.UTF8.GetString(stringBytes);
    }

    public string ReadGameId()
    {
        ReadUInt32();
        return ReadStr();
    }

    
    public List<User> ReadUsers()
    {
        var users = new List<User>();
        Read();
        ReadStr();
        SkipProperty(Property.Array);
        SkipProperty(Property.Struct);
        var userReaders = Split("UserId"u8.ToArray());
        userReaders.RemoveAt(0);
        foreach (var userReader in userReaders)
        {
            var user = new User();
            userReader.BaseStream.Seek(30, SeekOrigin.Begin);
            user.UserId = Encoding.UTF8.GetString(userReader.ReadBytes(32));
            userReader.Skip(39);
            user.UserName = userReader.ReadStr();
            userReader.Skip(40);
            user.HeroName = userReader.ReadStr();
            userReader.Skip(35);
            user.Team = userReader.ReadUInt32() > 0 ? Team.Dusk : Team.Dawn;
            userReader.Skip(35);
            user.Kills = (int)userReader.ReadUInt32();
            userReader.Skip(36);
            user.Deaths = (int)userReader.ReadUInt32();
            userReader.Skip(37);
            user.Assists = (int)userReader.ReadUInt32();
            users.Add(user);
        }

        return users;
    }

    public (DateTime, DateTime?) ReadTimeMatch()
    {
        var result = Split("DateTime"u8.ToArray());

        result.RemoveAt(0);

        result[0].Skip(18);
        var start = DateTime.FromBinary(result[0].ReadInt64());

        result[1].Skip(18);
        DateTime? stop = DateTime.FromBinary(result[1].ReadInt64());

        if (stop < start || stop > start.AddHours(3))
        {
            stop = null;
        }

        return (start, stop);
    }

    public void SkipProperty(Property property)
    {
        var skipLength = property switch
        {
            Property.Array => SkipArray,
            Property.Struct => SkipStruct,
            Property.String => SkipString,
            Property.Integer => SkipInteger,
            Property.Name => SkipName,
            _ => throw new ArgumentException($"Unknown property type: {property}")
        };
        Skip(skipLength);
    }

    public void Skip(int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        BaseStream.Seek(offset, SeekOrigin.Current);
    }

    public List<PredecessorBinaryReader> Split(byte[] delimiter)
    {
        BaseStream.Seek(0, SeekOrigin.Begin);
        var data = ReadBytes((int) BaseStream.Length);

        var result = new List<PredecessorBinaryReader>();
        var start = 0;
        for (var i = 0; i < data.Length - delimiter.Length + 1; i++)
        {
            if (!IsDelimiter(data, i, delimiter)) continue;
            result.Add(CreateReader(data, start, i));
            start = i + delimiter.Length;
            i += delimiter.Length - 1;
        }
        result.Add(CreateReader(data, start, data.Length));
        return result;
    }

    private static bool IsDelimiter(IReadOnlyList<byte> data, int index, IEnumerable<byte> delimiter)
    {
        return !delimiter.Where((t, i) => data[index + i] != t).Any();
    }

    private static PredecessorBinaryReader CreateReader(byte[] data, int start, int end)
    {
        var length = end - start;
        var memoryStream = new MemoryStream(data, start, length);
        return new PredecessorBinaryReader(memoryStream);
    }
}