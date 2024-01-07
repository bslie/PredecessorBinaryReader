// See https://aka.ms/new-console-template for more information

using System.Text;
using MatchEventDataReader;
using MatchEventDataReader.Models;

Console.WriteLine("Hello, World!");

var path =
    @"C:\Users\Lie\Downloads\response.txt";

var m = new MatchEventDataReader.MatchEventDataReader();

var result = m.Read(path);

Console.WriteLine(result.MatchId);
Console.WriteLine(result.StartTime.ToString());
Console.WriteLine(result.StopTime.ToString());

Console.WriteLine($"Match is Live? {result.IsLive}");

foreach (var user in result.Users)
{
    Console.WriteLine(user.UserName);
}


Console.Read();
