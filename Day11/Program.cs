using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


long Day11(string filepath, Func<List<List<bool?>>, int, int, bool?> evaluation)
{
    var data = BuildData(filepath);
    List<List<bool?>> seatMap = BuildSeatMap(data);
    int pre = 0, post = 1;
    while (pre != post)
    {
        pre = seatMap.Sum(l => l.Count(i => i == true));
        seatMap = ProcessSeats(seatMap, evaluation);
        //PrintSeatMap(seatMap);
        post = seatMap.Sum(l => l.Count(i => i == true));
    }
    return seatMap.Sum(l => l.Count(i => i == true));
}

List<List<bool?>> BuildSeatMap(List<string> data)
{
    List<List<bool?>> results = new();
    List<bool?> buffer = new bool?[data.First().Count() + 2].ToList();
    results.Add(buffer);
    data.ForEach(l =>
    {
        List<bool?> line = new();
        line.Add(null);
        foreach (char c in l)
        {
            line.Add(c == 'L' ? false : null);
        }
        line.Add(null);
        results.Add(line);
    });
    results.Add(buffer);
    return results;
}

void PrintSeatMap(List<List<bool?>> map)
{
    map.ForEach(l =>
    {
        l.ForEach(v =>
        {
            if (v is null) Console.Write(".");
            else if (v == true) Console.Write("#");
            else Console.Write("L");
        });
        Console.WriteLine();
    });
}


List<List<bool?>> ProcessSeats(List<List<bool?>> oldMap, Func<List<List<bool?>>, int, int, bool?> evaluation)
{
    List<List<bool?>> newMap = new();

    for (int row = 0; row < oldMap.Count(); row++)
    {
        List<bool?> newRow = new();
        for (int col = 0; col < oldMap.First().Count(); col++)
        {
            newRow.Add(evaluation(oldMap, row, col));
        }
        newMap.Add(newRow);
    }

    return newMap;
}

bool? EvaluateSeatOne(List<List<bool?>> oldMap, int row, int col)
{
    if (oldMap[row][col] is null) return null;
    if (!(bool)oldMap[row][col]) // seat is empty
        return SumSurroundingOne(oldMap, row, col) == 0 ? true : false;
    else
        return SumSurroundingOne(oldMap, row, col) < 4 ? true : false;
}


int SumSurroundingOne(List<List<bool?>> oldMap, int row, int col)
{
    int takenSeats = 0;
    for (int i = -1; i <= +1; i++)
    {
        for (int j = -1; j <= +1; j++)
        {
            if ((i, j) == (0, 0)) continue;
            var seat = oldMap[row + i][col + j];
            if (seat is null)
            {
                continue;
            }
            takenSeats += (bool)seat ? 1 : 0;
        }
    }
    return takenSeats;
}

bool? EvaluateSeatTwo(List<List<bool?>> oldMap, int row, int col)
{
    int xMax = oldMap.First().Count() - 1;
    int yMax = oldMap.Count() - 1;

    if (oldMap[row][col] is null) return null;
    if (!(bool)oldMap[row][col]) // seat is empty
        return SumSurroundingTwo(oldMap, row, col, xMax, yMax) == 0 ? true : false;
    else
        return SumSurroundingTwo(oldMap, row, col, xMax, yMax) < 5 ? true : false;
}

// 8 directions (and scales) to search for seats in Part 2
List<(int x, int y)> directionalAddends = new()
{
    (0, 1),
    (1, 1),
    (1, 0),
    (1, -1),
    (0, -1),
    (-1, -1),
    (-1, 0),
    (-1, 1)
};

int SumSurroundingTwo(List<List<bool?>> oldMap, int row, int col, int xMax, int yMax)
{
    
    int takenSeats = 0;
    
    // Get Closest seat in each of 8 directions
    foreach(var dAddend in directionalAddends)
    {
        int x = col, y = row;
        do
        {
            x += dAddend.x;
            y += dAddend.y;
            if (x < 0 || x > xMax || y < 0 || y > yMax)
            {
                (x, y) = (0, 0);
                break;
            }
        }
        while (oldMap[y][x] == null);

        // using 0,0 as a "I intentionally broke this" case, since I put a border of nulls around the chairs
        // only evaluating if I know it made it out of the while loop organically.
        if((x, y) != (0, 0))
        {
            bool? seat = oldMap[y][x];
            takenSeats += (bool)seat ? 1 : 0;
        }
    }
    return takenSeats;
}




List<string> BuildData(string fp) => File.ReadAllLines(fp).ToList();


Console.WriteLine(Day11("Sample.txt", EvaluateSeatOne));
Console.WriteLine(Day11("Input.txt", EvaluateSeatOne));
Console.WriteLine(Day11("Sample.txt", EvaluateSeatTwo));
Console.WriteLine(Day11("Input.txt", EvaluateSeatTwo));