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
    Regex expression = new Regex(@"(\d*)-(\d*)\s(\w):\s(\w*)");
    int a = 0;
    List<string> data = BuildStringList(filepath);
    data.ForEach(s =>
    {
        ParseFeed(expression.Match(s), out int lower, out int upper, out char letter, out string pass);
        int count = pass.Count(s => s==letter);
        if(count >= lower & count <= upper) a++;
    });

    return a;
}

static int ChallengeTwo(string filepath)
{
    Regex expression = new Regex(@"(\d*)-(\d*)\s(\w):\s(\w*)");
    int a = 0;
    List<string> data = BuildStringList(filepath);
    data.ForEach(s =>
    {
        ParseFeed(expression.Match(s), out int first, out int last, out char letter, out string pass);
        if (pass.ElementAt(first-1) == letter ^ pass.ElementAt(last-1) == letter) a++;
    });
    return a;
}

static void ParseFeed(Match match, out int first, out int last, out char letter, out string pass)
{
    first = int.Parse(match.Groups[1].Value);
    last = int.Parse(match.Groups[2].Value);
    letter = match.Groups[3].Value.First();
    pass = match.Groups[4].Value;
}

static List<string> BuildStringList(string fp) => File.ReadAllLines(fp).ToList();
