using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

Console.WriteLine(ChallengeOne("Sample.txt"));
Console.WriteLine(ChallengeOne("Input.txt"));
Console.WriteLine(ChallengeTwo("Input.txt"));


long ChallengeOne(string filepath)
{
    return GetSeatIds(BuildStringList(filepath)).Max();
}

long ChallengeTwo(string filepath)
{
    var seatIds = GetSeatIds(BuildStringList(filepath));
    return seatIds.First(seat => seatIds.Contains(seat + 2) && !seatIds.Contains(seat + 1)) + 1;
}

List<long> GetSeatIds(List<string> boardingPasses)
{
    List<long> seatIds = new();
    boardingPasses.ForEach(seat =>
    {
        int row = CalculatePosition(seat.Substring(0, 7), 'F', 'B', 128);
        int seatNumber = CalculatePosition(seat.Substring(7, 3), 'L', 'R', 8);
        seatIds.Add(row * 8 + seatNumber);
    });
    return seatIds;
}

int CalculatePosition(string input, char up, char down, int rangeSize)
{
    int low = 0;
    int high = rangeSize;
    foreach (char c in input)
    {
        if (c == up)
            high = (high + low) / 2;
        else
            low = (low + high) / 2;
    };
    return low;
}

static List<string> BuildStringList(string fp) => File.ReadAllLines(fp).ToList();



