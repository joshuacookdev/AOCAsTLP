using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

Console.WriteLine(ChallengeOne("Sample.txt"));
Console.WriteLine(ChallengeOne("Input.txt"));
Console.WriteLine(ChallengeTwo("Sample.txt"));
Console.WriteLine(ChallengeTwo("Input.txt"));

double ChallengeOne(string filepath)
{
    List<Command> commands = BuildData(filepath);
    Ship ship = new(new Position(0, 0), new Direction(0));

    commands.ForEach(command =>
    {
        ship = ship.Follow(command);
    });

    return Math.Abs(ship.position.N) + Math.Abs(ship.position.E);
}

double ChallengeTwo(string filepath)
{
    List<Command> commands = BuildData(filepath);
    Position waypoint = new(1, 10);
    TrackingShip ship = new(new(0, 0), waypoint);

    commands.ForEach(command =>
    {
        ship = ship.Follow(command);
    });

    return Math.Abs(ship.position.N) + Math.Abs(ship.position.E);
}

List<Command> BuildData(string fp)
{
    List<Command> data = new();
    File.ReadAllLines(fp).ToList().ForEach(line => data.Add(Command.Parse(line)));
    return data;
}

public record Command(char command, int change)
{
    public static Command Parse(string s) => new Command(s[0], int.Parse(s[1..]));
}

public record Direction(int Degrees)
{
    public Direction Rotate(int deg) => new((Degrees + deg + 360) % 360);
    public char ToChar()
    {
        return ((Degrees + 360) % 360) switch
        {
            0 => 'E',
            90 => 'S',
            180 => 'W',
            270 => 'N',
            _ => throw new Exception("You shouldn't be here.")
        };
    }
}


public record Ship(Position position, Direction direction)
{
    public Ship Follow(Command command)
    {
        return command switch
        {
            ('R', int change) => this with { direction = direction.Rotate(change) }, 
            ('L', int change) => this with { direction = direction.Rotate(-change) },
            ('F', int change) => this with { position = position.Move(new Command(direction.ToChar(), change))},
            _ =>  this with { position = position.Move(command) }
        };
    }
}

public record TrackingShip(Position position, Position waypoint)
{
    public TrackingShip Follow(Command command)
    {
        return command switch
        {
            ('R', int change) => this with { waypoint = waypoint.Rotate(command) },
            ('L', int change) => this with { waypoint = waypoint.Rotate(new('L',-change)) },
            ('F', int change) => this with { position = position.Move(waypoint,change) },
            _ => this with {  waypoint = waypoint.Move(command) }
        };
    }
}

public record Position(int N, int E)
{
    public Position Move(Command command)
    {
        return command.command switch
        {
            'N' => this with { N = N + command.change },
            'E' => this with { E = E + command.change },
            'S' => this with { N = N - command.change },
            'W' => this with { E = E - command.change },
            _ => throw new Exception("You shouldn't be here.")
        };
    }

    public Position Move(Position wp, int change) => this with
    {
        N = N + change * wp.N,
        E = E + change * wp.E
    };

    public Position Rotate(Command command)
    {
        return ((command.change + 360) % 360) switch
        {
            0 => this,
            90 => this with { N = -E, E = N },
            180 => this with { N = -N, E = -E },
            270 => this with { N = E, E = -N },
            _ => throw new Exception("You shouldn't be here.")
        };
    }
}