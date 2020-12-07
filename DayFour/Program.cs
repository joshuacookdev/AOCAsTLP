using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

static string[] BuildStringList(string fp) => File.ReadAllLines(fp);

bool IntInRange(int value, int lb, int up) => value >= lb && value <= up;

bool BYRCheck(string value) =>
	int.TryParse(value, out var byr)
	&& IntInRange(byr, 1920, 2002);

bool IYRCheck(string value) =>
	int.TryParse(value, out var iyr)
	&& IntInRange(iyr, 2010, 2020);

bool EYRCheck(string value) =>
	int.TryParse(value, out var eyr)
	&& IntInRange(eyr, 2020, 2030);

bool HGTCheck(string value)
{
	return value.Last() switch
	{
		'm' => IntInRange(int.Parse(value[0..^2]),150,193),
		'n' => IntInRange(int.Parse(value[0..^2]), 59, 76),
		_ => false
	};
}

Regex hcl = new("^#[0-9a-f]{6}$");
bool HCLCheck(string value) => hcl.Match(value).Success;

HashSet<string> ecl = new() { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
bool ECLCheck(string value) => ecl.Contains(value);

Regex pid = new("^[0-9]{9}$");
bool PIDCheck(string value) => pid.Match(value).Success;

Dictionary<string, Func<string, bool>> requiredFields = new()
{
	{ "byr", BYRCheck },
	{ "iyr", IYRCheck },
	{ "eyr", EYRCheck },
	{ "hgt", HGTCheck },
	{ "hcl", HCLCheck },
	{ "ecl", ECLCheck },
	{ "pid", PIDCheck },
};

Console.WriteLine(ChallengeOne("Sample.txt"));
Console.WriteLine(ChallengeOne("Input.txt"));
Console.WriteLine(ChallengeTwo("Sample.txt"));
Console.WriteLine(ChallengeTwo("Input.txt"));

int ChallengeOne(string filepath, bool evaluate = false)
{
	int a = 0;
	string[] lines = BuildStringList(filepath);
	int lineCount = 0;

	while (true)
	{
		var passportFields = new Dictionary<string, string>();

		while (true)
		{
			string line = lines[lineCount++];
			if (string.IsNullOrWhiteSpace(line)) break;

			string[] fieldsOnLine = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
			foreach (string field in fieldsOnLine)
			{
				string[] values = field.Split(":");
				passportFields.Add(values[0], values[1]);
			}

			if (lineCount == lines.Length) break;
		}

		if (requiredFields.Keys.All(i => passportFields.Keys.Contains(i)))
		{
			if(!evaluate) a++;
            else
			{
				if (requiredFields.All(i => i.Value(passportFields[i.Key])))
					a++;
            }
		}

		if (lineCount == lines.Length) break;
	}

	return a;
}

int ChallengeTwo(string filepath)
{
	return ChallengeOne(filepath, true);
}