<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Namespace>System.Drawing</Namespace>
</Query>

var instructions = File.ReadAllLines(Path.Combine(Path.GetDirectoryName (Util.CurrentQueryPath),"day6.txt"));

var grid = new Boolean[1000, 1000];
var intensity = new int[1000, 1000];

foreach (var i in instructions)
{
	var coords = Regex.Match(i, @"(\d+),(\d+)");
	var tl = new Point(int.Parse(coords.Groups[1].Value), int.Parse(coords.Groups[2].Value));
	coords = coords.NextMatch();
	var br = new Point(int.Parse(coords.Groups[1].Value), int.Parse(coords.Groups[2].Value));
	
		for (var x = tl.X; x <= br.X; x++)
		{
			for (var y = tl.Y; y <= br.Y; y++)
			{
				if (i.Contains("toggle"))
				{
					grid[x, y] = !grid[x, y];
					intensity[x, y] += 2;
				}
				else
				{
					var val = i.Contains("on");
					grid[x, y] = val;
					if (val)
						intensity[x ,y]++;
					else
						if (intensity[x, y] > 0)
							intensity[x, y]--;
				}
			}
		}

}

int counter = 0;
int brightness = 0;
for (var x = 0; x < 1000; x++)
{
	for (var y = 0; y < 1000; y++)
	{
		brightness += intensity[x, y];
		if (grid[x,y])
			counter++;
	}
}

counter.Dump();
brightness.Dump();