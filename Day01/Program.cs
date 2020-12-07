using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

Console.WriteLine(ChallengeOne("Sample.txt"));
Console.WriteLine(ChallengeOne("Input.txt"));
Console.WriteLine(ChallengeTwo("Sample.txt"));
Console.WriteLine(ChallengeTwo("Input.txt"));


static int ChallengeOne(string filepath)
{
    int a;
    List<int> data = BuildIntList(filepath);
    a = data.Where(i => data.Contains(2020 - i)).First();
    return (2020 - a) * a;

}

static int ChallengeTwo(string filepath)
{
    int a = 0, b = 0;
    List<int> data = BuildIntList(filepath);
    data.Sort();
    a = data.Where(i => data.Any(j => data.Contains(2020 - i - j))).First();
    b = data.Where(j => data.Contains(2020 - a - j)).First();
    return (2020 - (a + b)) * a * b;
}

static List<int> BuildIntList(string fp) => File.ReadAllLines(fp).ToList().Select(int.Parse).ToList();



