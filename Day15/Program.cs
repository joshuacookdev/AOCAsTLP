using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


List<List<long>> Inputs = new()
{
    new List<long> { 0, 3, 6 },
    new List<long> { 1, 3, 2 },
    new List<long> { 2, 1, 3 },
    new List<long> { 1, 2, 3 },
    new List<long> { 2, 3, 1 },
    new List<long> { 3, 2, 1 },
    new List<long> { 3, 1, 2 },
    new List<long> { 1, 17, 0, 10, 18, 11, 6 },
};

Challenge(Inputs, 2020);
Challenge(Inputs, 30000000);

void Challenge(List<List<long>> inputs, long stop)
{
    foreach(List<long> input in inputs)
    {
        Console.WriteLine(FindNumberAtStop(input,stop));
    }
}

long FindNumberAtStop(List<long> input, long stop)
{
    Stopwatch sw = new();
    sw.Start();
    Dictionary<long, long> existing = new();
    for(int i = 0; i < input.Count-1; i++)
    {
        if (!existing.TryAdd(input[i], i))
            existing[input[i]] = i;
    }

    if (stop < input.Count) return 0;

    for(int i = input.Count; i <= stop-1; i++)
    {
        long last = input[i-1];

        if(existing.TryGetValue(last,out long lastIndex))
        {
            input.Add((i - 1) - lastIndex);
        }
        else
        {
            input.Add(0);
        }

        if (!existing.TryAdd(last, i - 1))
            existing[last] = i - 1;
    }
    sw.Stop();
    Console.WriteLine();
    Console.WriteLine(sw.ElapsedMilliseconds + "ms");
    return input.Last();
}

