using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


long ChallengeOne(string filepath)
{
    var data = BuildData(filepath);

    return data.Count();
}

long ChallengeTwo(string filepath)
{
    var data = BuildData(filepath).ToHashSet();

    return data.Count();
}


List<double> BuildData(string fp) => File.ReadAllLines(fp).Select(double.Parse).ToList();


Console.WriteLine(ChallengeOne("Sample.txt"));
Console.WriteLine(ChallengeOne("Input.txt"));
Console.WriteLine(ChallengeTwo("Sample.txt"));
Console.WriteLine(ChallengeTwo("Input.txt"));