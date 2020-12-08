using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

Regex containerRegex = new("^(.*) bags contain (.*).$");
Regex containsRegex = new("(\\d+) (.*) bag");

Console.WriteLine(ChallengeOne("Sample.txt"));
Console.WriteLine(ChallengeOne("Input.txt"));
Console.WriteLine(ChallengeTwo("Sample.txt"));
Console.WriteLine(ChallengeTwo("Input.txt"));


long ChallengeOne(string filepath)
{
    Dictionary<string, Dictionary<string, int>> rules = BuildRuleSet(BuildStringList(filepath));

    HashSet<string> answers = new(); 
    GetContainersThatHold("shiny gold", answers, rules);
    return answers.Count();
}

long ChallengeTwo(string filepath)
{
    Dictionary<string, Dictionary<string, int>> rules = BuildRuleSet(BuildStringList(filepath));
    return rules.Count();
}

Dictionary<string, Dictionary<string, int>> BuildRuleSet(List<string> input)
{
    Dictionary<string, Dictionary<string, int>> rules = new();
    input.ForEach(line =>
    {
        Match container = containerRegex.Match(line);
        string containerColor = container.Groups[1].Value;
        string contained = container.Groups[2].Value;

        Dictionary<string, int> BagContents = new();
        if(contained != "no other bags")
        {
            foreach(string bag in contained.Split(','))
            {
                Match bagInfo = containsRegex.Match(bag);

                int count = int.Parse(bagInfo.Groups[1].Value);
                string color = bagInfo.Groups[2].Value;

                BagContents.Add(color, count);
            }
        }
        rules.Add(containerColor, BagContents);
    });

    return rules;
}

void GetContainersThatHold(string color, HashSet<string> containers, Dictionary<string,Dictionary<string,int>> d)
{
    foreach(var container in d)
    {
        if(container.Value.ContainsKey(color))
        {
            containers.Add(container.Key);
            if (d.TryGetValue(container.Key, out var keyValuePairs))
                GetContainersThatHold(container.Key, containers, d);
        }
    }
}


static List<string> BuildStringList(string fp) => 
    File.ReadAllLines(fp).Select(line => line.Trim()).ToList();
