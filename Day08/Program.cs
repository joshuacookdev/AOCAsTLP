using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


int ChallengeOne(string filepath)
{
    bool completed = GetRepeatedCommand(BuildRules(filepath), out int accumulator);
    return accumulator;
}

long ChallengeTwo(string filepath)
{
    List<(string command, int change)> rules = BuildRules(filepath);

    foreach (var rule in rules)
    {
        if(rule.command != "acc")
        {
            var newList = BuildListWithConvertedCommand(rules, rules.IndexOf(rule));
            if (GetRepeatedCommand(newList, out int acc))
                return acc;
        }
    };

    return 0;
}


/// <summary>
/// Brute force, I know. I don't like it either.
/// </summary>
List<(string command, int change)> BuildListWithConvertedCommand(List<(string command, int change)> rules, int position)
{
    List<(string command, int change)> changedList = new();
    
    for(int i = 0; i < rules.Count(); i++)
    {
        if (i == position)
            changedList.Add((CommandConverter(rules[i].command), rules[i].change));
        else
            changedList.Add(rules[i]);
    }

    return changedList;
}

string CommandConverter(string input)
{
    return input switch
    {
        "jmp" => "nop",
        "nop" => "jmp",
        _ => input
    };
}

static bool GetRepeatedCommand(List<(string command, int change)> rules, out int accumulator)
{
    bool[] nodes = new bool[rules.Count()];
    int position = 0;
    accumulator = 0;
    while (nodes[position] != true)
    {

        nodes[position] = true;
        switch (rules[position].command)
        {
            case "nop":
                position++;
                break;
            case "acc":
                accumulator += rules[position].change;
                position++;
                break;
            case "jmp":
                position += rules[position].change;
                break;
        }
        if (position == nodes.Count())
            return true;
    }
    return false;
}

static List<(string command, int change)> BuildRules(string fp)
{
    string[] feed = File.ReadAllLines(fp);
    List<(string,int)> ruleset = new();
    foreach(string line in feed)
    {
        string command = line.Substring(0, 3);
        int change = int.Parse(line.Substring(line.IndexOf(" ")));
        ruleset.Add((command, change));
    }
    return ruleset;
}


Console.WriteLine(ChallengeOne("Sample.txt") == 5);
Console.WriteLine(ChallengeOne("Input.txt"));
Console.WriteLine(ChallengeTwo("Sample.txt") == 8);
Console.WriteLine(ChallengeTwo("Input.txt"));