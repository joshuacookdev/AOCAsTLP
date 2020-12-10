using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

long ChallengeOne(string filepath, int preamble)
{
    List<long> data = BuildData(filepath);

    for (int i = preamble; i < data.Count(); i++)
    {
        if (!PriorHasSum(data, i, preamble))
        {
            return data[i];
        }
    }

    return 0;
}

long ChallengeTwo(string filepath, int preamble)
{
    long[] data = BuildData(filepath).ToArray();
    long magicNumber = ChallengeOne(filepath, preamble);
    int pos = Array.IndexOf(data, magicNumber);

    // start immediate prior to magicNumber
    for (int j = pos - 1; j >= 0; j--)
    {
        long sum = data[j];
        // Begin summation of elements in reverse order
        for (int i = j - 1; i > 0; i--)
        {
            sum += data[i];
            // as soon as sum is hit
            if (sum > magicNumber)
            {
                i = 0;
                break;
            }
            if (sum == magicNumber)
            {
                return data[i..j].Min() + data[i..j].Max();
            }
        }
    }

    return 0;
}

bool PriorHasSum(List<long> data, int pos, int count)
{
    List<long> sample = new(data.GetRange(pos - count, count));
    if (sample.Any(num => sample.Contains(data[pos] - num)))
    {
        return true;
    }

    return false;
}

List<long> BuildData(string fp) => File.ReadAllLines(fp).Select(long.Parse).ToList();

Console.WriteLine(ChallengeOne("Sample.txt", 5));
Console.WriteLine(ChallengeOne("Input.txt", 25));
Console.WriteLine(ChallengeTwo("Sample.txt", 5));
Console.WriteLine(ChallengeTwo("Input.txt", 25));