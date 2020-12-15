using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

Console.WriteLine(ChallengeOne("Sample.txt"));
Console.WriteLine(ChallengeOne("Input.txt"));
Console.WriteLine(ChallengeTwo("Sample.txt"));
Console.WriteLine(ChallengeTwo("Input.txt"));

long ChallengeOne(string filepath)
{
    BusSchedule schedule = BuildDataOne(filepath);
    long ArrivalTime = schedule.time;
    long CurrentTime = ArrivalTime;
    while (true)
    {
        foreach (Bus bus in schedule.buses)
        {
            if (CurrentTime % bus.number == 0)
                return bus.number * (CurrentTime-ArrivalTime);
        }
        CurrentTime++;
    }
}

long ChallengeTwo(string filepath)
{
    List<Bus> buses = BuildDataTwo(filepath);
    long time = 0; // no start time is described here, starting at T0
    long increment = buses[0].number; // start with first bus in data

    //iterate over other buses in data
    foreach(Bus bus in buses.GetRange(1,buses.Count()-1))
    {
        long newTime = bus.number;
        while (true)
        {
            time += increment;
            if ((time + bus.tMinus) % newTime == 0)
            {
                increment *= newTime;
                break;
            }
        }
    }

    return time;
}

BusSchedule BuildDataOne(string fp)
{
    string[] data = File.ReadAllLines(fp);
    int time = int.Parse(data[0]);
    return new BusSchedule(time, GetBuses(data[1]));
}

List<Bus> BuildDataTwo(string fp) => GetBuses(File.ReadAllLines(fp)[1]);

List<Bus> GetBuses(string input)
{
    string[] data = input.Split(',');
    List<Bus> buses = new();
    for (int i = 0; i < data.Length; i++)
    {
        if (data[i] != "x")
        {
            buses.Add(new(long.Parse(data[i]), (long)i));
        }
    }
    return buses;
}

public record BusSchedule(long time, List<Bus> buses) { }

public record Bus(long number, long tMinus) { }
