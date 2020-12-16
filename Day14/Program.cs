using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

Console.WriteLine(ChallengeOne("Sample.txt"));
Console.WriteLine(ChallengeOne("Input.txt"));
Console.WriteLine(ChallengeTwo("Sample1.txt"));
Console.WriteLine(ChallengeTwo("Input.txt"));

long ChallengeOne(string filepath)
{
    List<string> input = ParseInput(filepath);
    Memory mem = new();
    ValueMask mask = new(0,0);

    foreach (string line in input)
    {
        if (IsMaskUpdate(line))
            mask = ValueMask.Parse(line);
        else
            mem.MaskedValueUpdate(line, mask);
    }

    return mem.Sum;
}

long ChallengeTwo(string filepath)
{
    List<string> input = ParseInput(filepath);
    Memory mem = new();
    AddressMask mask = new(new(),new(),0);

    foreach (string line in input)
    {
        if (IsMaskUpdate(line))
            mask = AddressMask.Parse(line);
        else
            mem.MaskedAddressUpdate(line, mask);
    }

    return mem.Sum;
}

bool IsMaskUpdate(string line) => line.StartsWith("mask");

List<string> ParseInput(string fp) => File.ReadAllLines(fp).ToList();

public record ValueMask(long And, long Or)
{
    public static ValueMask Parse(string line)
    {
        long or = 0;
        long and = 0;
        long current = 1;

        foreach(char bit in line[7..].Reverse())
        {
            switch (bit)
            {
                case 'X':
                    and |= current;
                    break;
                case '1':
                    or |= current;
                    break;
            }
            current <<= 1;
        }
        return new(and, or);
    }
}

public record AddressMask(List<long> FloatingBits, List<List<bool>> FloatingInstructions, long Or)
{
    public static AddressMask Parse(string line)
    {
        List<long> floatingBits = new();
        List<List<bool>> floatingInstructions = new();
        long or = 0;
        long current = 1;

        foreach(char bit in line[7..].Reverse())
        {
            if (bit == 'X')
            {
                if (floatingInstructions.Count == 0)
                {
                    floatingInstructions.Add(new List<bool> { false });
                    floatingInstructions.Add(new List<bool> { true });
                }
                else
                {
                    List<List<bool>> newInstructions = new();
                    foreach (List<bool> floatingInstruction in floatingInstructions)
                    {
                        newInstructions.Add(floatingInstruction.Append(false).ToList());
                        newInstructions.Add(floatingInstruction.Append(true).ToList());
                    }
                    floatingInstructions = newInstructions;
                }

                floatingBits.Add(current);
            }
            else if (bit == '1')
                or |= current;

            current <<= 1;
        }
        return new(floatingBits, floatingInstructions, or);
    }
}

/// <summary>
/// Wrapper class for parsing and consuming dictionary
/// </summary>
public class Memory
{
    Dictionary<long, long> _dictionary = new();
    
    Regex regex = new("^mem\\[(\\d+)\\] = (\\d+)$");

    public void MaskedValueUpdate(string line, ValueMask mask)
    {
        Match match = regex.Match(line.Trim());

        long address = long.Parse(match.Groups[1].Value);
        long value = long.Parse(match.Groups[2].Value);
        value = (value & mask.And) | mask.Or;

        if (!_dictionary.TryAdd(address, value))
            _dictionary[address] = value;
    }

    public bool MaskedAddressUpdate(string line, AddressMask mask)
    {
        Match match = regex.Match(line.Trim());

        long address = long.Parse(match.Groups[1].Value);
        long value = long.Parse(match.Groups[2].Value);

        foreach (List<bool> floatingInstruction in mask.FloatingInstructions)
        {
            long or = 0L;
            long and = ~0;

            for (int i = 0; i < floatingInstruction.Count; ++i)
            {
                long bit = mask.FloatingBits[i];
                if (floatingInstruction[i])
                    or |= bit;
                else
                    and &= ~bit;
            }

            long newAddress = (address & and) | or | mask.Or;

            if (!_dictionary.TryAdd(newAddress, value))
                _dictionary[newAddress] = value;
        }

        return true;
    }

    public long Sum => _dictionary.Values.Sum();
}

