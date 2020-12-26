<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day12.txt")).Select(l => (l[0], int.Parse(l.Substring(1)))).ToList();

var x = 0;
var y = 0;
var h = 90;

foreach (var step in input)
{
	switch (step.Item1)
	{
		case 'S': y -= step.Item2; break;
		case 'N': y += step.Item2; break;
		case 'W': x -= step.Item2; break;
		case 'E': x += step.Item2; break;
		case 'L': h += (360 - step.Item2); h %= 360; break;
		case 'R': h += step.Item2; h %= 360; break;
		case 'F':
			switch (h)
			{
				case 90: x += step.Item2; break;
				case 270: x -= step.Item2; break;
				case 0: y += step.Item2; break;
				case 180: y -= step.Item2; break;
			}
			break;
	}
}

var part1 = Math.Abs(x) + Math.Abs(y);
part1.Dump();


var shipx = 0;
var shipy = 0;
var wpx = 10;
var wpy = 1;

foreach (var step in input)
{
	switch (step.Item1)
	{
		case 'S': wpy -= step.Item2; break;
		case 'N': wpy += step.Item2; break;
		case 'W': wpx -= step.Item2; break;
		case 'E': wpx += step.Item2; break;
		case 'L':
		case 'R':
			var rot = step.Item1 == 'L' ? 360 - step.Item2 : step.Item2;
			int temp;
			switch (rot)
			{
				case 90: temp = wpx; wpx = wpy; wpy = -temp; break;
				case 180: wpx = -wpx; wpy = -wpy; break;
				case 270: temp = wpx; wpx = -wpy; wpy = temp; break;
			}
			break;
		case 'F': shipx += (step.Item2 * wpx); shipy += (step.Item2 * wpy); break;
	}
}

var part2 = Math.Abs(shipx) + Math.Abs(shipy);
part2.Dump();
