## What's that?
This is actually a simple library for reading a binary data derived from the [Predecessor game](https://www.predecessorgame.com/). 

> The data is obtained for information purposes and is not intended for
> commercial interest.

The data is obtained from the endpoint: 

     https://backend.production.omeda-aws.com/api/event/{sessionId}_Frontend

The data is in a format: 
```C#
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
```
If the match has status `isLive == true`, then KDA and timer data can be updated

> I will not show an example of use for ethical reasons. 
> The endpoint is protected by an authorization token.
