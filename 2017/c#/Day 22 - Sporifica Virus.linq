<Query Kind="Statements">
  <Namespace>System.Drawing</Namespace>
</Query>

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day22.txt"));

var curY = (input.Length / 2) + 1;
var curX = (input[0].Length / 2) + 1;
var hdg = 0;

// part 1
var grid = new Dictionary<Point, Boolean>();
for (var y = 0; y < input.Length; y++)
	for (var x = 0; x < input[y].Length; x++)
		grid.Add(new Point(x + 1, input.Length - y), input[y][x] == '#');

var part1 = 0;
for (var burst = 0; burst < 10000; burst++)
{
	var pos = new Point(curX, curY);

	if (!grid.ContainsKey(pos))
		grid.Add(pos, false);

	if (grid[pos])
		hdg = (hdg + 90) % 360;
	else
	{
		hdg = (hdg + 360 - 90) % 360;
		part1++;
	}
	grid[pos] = !grid[pos];
	switch (hdg)
	{
		case 90: curX++; break;
		case 270: curX--; break;
		case 0: curY++; break;
		case 180: curY--; break;
	}
}
part1.Dump();


// part 2
hdg = 0;
curY = (input.Length / 2) + 1;
curX = (input[0].Length / 2) + 1;

var grid2 = new Dictionary<Point, char>();
for (var y = 0; y < input.Length; y++)
	for (var x = 0; x < input[y].Length; x++)
		grid2.Add(new Point(x + 1, input.Length - y), input[y][x] == '#' ? 'I' : 'C');

var part2 = 0;
for (var burst = 0; burst < 10000000; burst++)
{
	var pos = new Point(curX, curY);

	if (!grid2.ContainsKey(pos))
		grid2.Add(pos, 'C');

	switch (grid2[pos])
	{
		case 'C': grid2[pos] = 'W'; hdg = (hdg + 360 - 90) % 360; break;
		case 'I': grid2[pos] = 'F'; hdg = (hdg + 90) % 360; break;
		case 'F': grid2[pos] = 'C'; hdg = (hdg + 180) % 360; break;
		case 'W': grid2[pos] = 'I'; part2++; break;
	}
	switch (hdg)
	{
		case 90: curX++; break;
		case 270: curX--; break;
		case 0: curY++; break;
		case 180: curY--; break;
	}
}
part2.Dump();
