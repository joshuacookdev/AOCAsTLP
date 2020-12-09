using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


long ChallengeOne(string filepath, int preamble)
{
    var data = BuildData(filepath);

    for(int i = preamble; i<data.Count();i++)
    {
        if (!PriorHasSum(data,i, preamble))
            return data[i];
    }

    return 0;
}

long ChallengeTwo(string filepath, int preamble)
{
    var data = BuildData(filepath).ToArray();
    long magicNumber = ChallengeOne(filepath, preamble);

    for (int i = 0; i < data.Count(); ++i)
    {
        var sum = data[i];
        for (int j = i + 1; j < preamble + i; j++)
        {
            sum += data[j];
            if (sum == magicNumber)
            {
                return data[i..j].Min() + data[i..j].Max();
            }
        }
    }

    return 0;
}

bool PriorHasSum(List<long> data,int pos, int count)
{
    List<long> sample = new(data.GetRange(pos - count, count));
    if (sample.Any(num => sample.Contains(data[pos] - num)))
        return true;

    return false;
}


List<long> BuildData(string fp) => File.ReadAllLines(fp).Select(long.Parse).ToList();

Console.WriteLine(ChallengeOne("Sample.txt", 5));
Console.WriteLine(ChallengeOne("Input.txt", 25));
Console.WriteLine(ChallengeTwo("Sample.txt", 5));
Console.WriteLine(ChallengeTwo("Input.txt", 25));