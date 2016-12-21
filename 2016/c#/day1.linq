<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var instructions = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day1.txt")).SelectMany(l => l.Split(new string[] { "," }, StringSplitOptions.None).Select(o => o.Trim()));

	var pos = new Vector { X = 0, Y = 0, Heading = 0};
	var locations = new List<string>();
	
	foreach (var i in instructions)
		pos = Move(pos, i);

	Console.WriteLine($"Part 1: {Math.Abs(pos.X) + Math.Abs(pos.Y)}");

	var grid = new int[1000, 1000];
	pos = new Vector { X = 500, Y = 500, Heading = 0 };
	int? visited = null;
	
	foreach (var i in instructions)
	{
		var pos2 = Move(pos,i);

		if (pos.Y == pos2.Y)
		{
			for (var x = pos.X; x != pos2.X; x += pos.X < pos2.X ? 1 : -1)
			{
				if (grid[x, pos.Y] == 1)
				{
					visited = Math.Abs(x-500) + Math.Abs(pos.Y - 500);
					break;
				}
				else
					grid[x, pos.Y] = 1;
			}
		}
		else
		{
			for (var y = pos.Y; y != pos2.Y; y += pos.Y < pos2.Y ? 1 : -1)
			{
				if (grid[pos.X, y] == 1)
				{
					visited = Math.Abs(pos.X - 500) + Math.Abs(y - 500);
					break;
				}
				else
					grid[pos.X, y] = 1;
			}
			
		}
		pos = pos2;
		if (visited != null) break;
	}
	Console.WriteLine($"Part 2: {visited}");

}

public class Vector
{
	public int X { get; set; }
	public int Y { get; set; }
	public int Heading { get; set; }
}

Vector Move(Vector start, string instruction)
{
	var ret = new Vector
	{
		X = start.X,
		Y = start.Y,
		Heading = start.Heading
	};
	
	ret.Heading += instruction[0] == 'L' ? -90 : 90;
	if (ret.Heading < 0) ret.Heading += 360;
	if (ret.Heading >= 360) ret.Heading -= 360;

	var steps = Convert.ToInt32(instruction.Substring(1));

	switch (ret.Heading)
	{
		case 0:   ret.Y += steps; break;
		case 90:  ret.X += steps; break;
		case 180: ret.Y -= steps; break;
		case 270: ret.X -= steps; break;
	}

	return ret;
}
