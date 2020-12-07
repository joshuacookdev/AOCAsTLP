using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

Console.WriteLine(ChallengeOne("Sample.txt"));
Console.WriteLine(ChallengeOne("Input.txt"));
Console.WriteLine(ChallengeTwo("Sample.txt"));
Console.WriteLine(ChallengeTwo("Input.txt"));


static int ChallengeOne(string filepath)
{
    List<string> data = BuildStringList(filepath);
    return GetSpaceCounts(data,3,1);
}

static long ChallengeTwo(string filepath)
{
    List<string> data = BuildStringList(filepath);
    HashSet<(int,int)> input = new(){ (1,1), (3,1), (5,1), (7,1), (1,2) };
    long product = 1;
    foreach ((int,int) set in input)
    {
        product *= GetSpaceCounts(data, set.Item1, set.Item2);
    }
    
    return product;
}

static int GetSpaceCounts(List<string> data, int right, int down)
{
    int treeCount = 0, colCount = 0;

    for (int i = 0; i < data.Count(); i += down)
    {
        if (data[i].ElementAt(colCount % (data.First().Length)) == '#')
            treeCount++;
        colCount += right;
    }
    return treeCount;

}

static List<string> BuildStringList(string fp) => File.ReadAllLines(fp).ToList();
