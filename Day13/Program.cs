using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


double ChallengeOne(string filepath)
{
    var data = BuildData(filepath);
    int ArrivalTime = data.time;
    int CurrentTime = ArrivalTime;
    while (true)
    {
        foreach (int bus in data.buses)
        {
            if (CurrentTime % bus == 0)
                return bus * (CurrentTime-ArrivalTime);
        }
        CurrentTime++;
    }
}

double ChallengeTwo(string filepath)
{
    var data = BuildData(filepath);
    return data.buses.Count();
}

(int time, List<int> buses) BuildData(string fp)
{
    string[] data = File.ReadAllLines(fp);
    int time = int.Parse(data[0]);
    List<int> buses = data[1].Split(',').Where(i => i != "x").Select(int.Parse).ToList();
    return (time, buses);
}

Console.WriteLine(ChallengeOne("Sample.txt"));
Console.WriteLine(ChallengeOne("Input.txt"));
Console.WriteLine(ChallengeTwo("Sample.txt"));
Console.WriteLine(ChallengeTwo("Input.txt"));