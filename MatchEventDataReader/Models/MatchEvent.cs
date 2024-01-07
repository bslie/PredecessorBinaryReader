namespace MatchEventDataReader.Models;

public class MatchEvent
{
    public string? MatchId { get; set; }
    public List<User>? Users { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? StopTime { get; set; }
    public bool IsLive { get; set; }
}

public class User
{
    public string? UserId { get; set; }
    public string? UserName { get; set;}
    public string? HeroName { get; set; }
    public Team Team { get; set; }
    public int? Kills { get; set; }
    public int? Deaths { get; set; }
    public int? Assists { get; set; }
}

public enum Team : byte
{
    Dawn = 0,
    Dusk = 1,
}