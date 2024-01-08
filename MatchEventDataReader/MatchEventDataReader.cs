using MatchEventDataReader.Models;

namespace MatchEventDataReader;

public class MatchEventReader
{
    public MatchEvent Read(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        using var reader = new PredecessorBinaryReader(stream);
        var matchEvent = new MatchEvent
        {
            MatchId = reader.ReadGameId(),
            Users = reader.ReadUsers()
        };

        var times = reader.ReadTimeMatch();
        matchEvent.StartTime = times.Item1;
        matchEvent.StopTime = times.Item2;
        matchEvent.IsLive = times.Item2 == null;

        return matchEvent;
    }

    public MatchEvent Read(Stream stream)
    {
        using var reader = new PredecessorBinaryReader(stream);
        var matchEvent = new MatchEvent
        {
            MatchId = reader.ReadGameId(),
            Users = reader.ReadUsers()
        };

        var times = reader.ReadTimeMatch();
        matchEvent.StartTime = times.Item1;
        matchEvent.StopTime = times.Item2;
        matchEvent.IsLive = times.Item2 == null;

        return matchEvent;
    }
}