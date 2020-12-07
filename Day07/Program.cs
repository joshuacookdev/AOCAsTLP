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
    return BuildStringList(filepath).Count();
}

long ChallengeTwo(string filepath)
{
    return BuildStringList(filepath).Count();
}

static List<string> BuildStringList(string fp) => File.ReadAllLines(fp).ToList();



