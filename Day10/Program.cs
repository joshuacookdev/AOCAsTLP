using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


double ChallengeOne(string filepath)
{
    var data = BuildData(filepath);
    var joltDeltas = GetJoltDeltasInFullChain(data);
    return joltDeltas.Count(i => i == 1) * joltDeltas.Count(i => i == 3);
}

List<double> GetJoltDeltasInFullChain(List<double> data)
{
    List<double> results = new();
    for (int i = 0; i < data.Count() - 1; i++)
    {
        results.Add(data[i + 1] - data[i]);
    }
    return results;
}


double ChallengeTwo(string filepath)
{
    var data = BuildData(filepath).ToHashSet();
    Dictionary<double, double> results = new();
    return CountConfigurations(data, results, 0);
}

double CountConfigurations(HashSet<double> data, Dictionary<double,double> results, double current)
{
    double configurations = 0;

    if (current == data.Max()) return 1;
    else if (results.TryGetValue(current, out double result)) return result;
    
    for (int i = 1;i<=3;i++)
    {
        if (data.Contains(current + i))
            configurations += CountConfigurations(data, results, current + i);
    }

    results[current] = configurations;
    return configurations;
}


List<double> BuildData(string fp)
{
    List<double> data = File.ReadAllLines(fp).Select(double.Parse).ToList();
    data = data.Prepend(0).ToList();
    data = data.Append(data.Max() + 3).ToList();
    data.Sort();
    return data;
}

Console.WriteLine(ChallengeOne("Sample.txt"));
Console.WriteLine(ChallengeOne("Input.txt"));
Console.WriteLine(ChallengeTwo("Sample.txt"));
Console.WriteLine(ChallengeTwo("Input.txt"));