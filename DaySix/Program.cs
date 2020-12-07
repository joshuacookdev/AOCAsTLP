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
    List<string> lines = BuildStringList(filepath);
    long sumOfAllGroupYes = 0;
    int lineCount = 0;
    while(true) // until end of file
    {
        HashSet<char> questionsAnsweredYes = new();
        while (true) // until end of group
        {
            if (lineCount == lines.Count()) break;
            string line = lines[lineCount++];
            if (string.IsNullOrWhiteSpace(line)) break;
            foreach(char c in line)
            {
                questionsAnsweredYes.Add(c);
            }
        }
        sumOfAllGroupYes += questionsAnsweredYes.Count();
        if (lineCount == lines.Count()) break;
    }
    return sumOfAllGroupYes;
}

long ChallengeTwo(string filepath)
{
    return 0;
}

static List<string> BuildStringList(string fp) => File.ReadAllLines(fp).ToList();



