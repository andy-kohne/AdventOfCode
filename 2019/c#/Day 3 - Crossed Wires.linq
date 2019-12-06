<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day3.txt")).ToArray();

IEnumerable<((int X, int Y) position, int steps)> getPoints(string path)
{
	var steps = path.Split(',').Select(p => (dir: p[0], dist: int.Parse(p.Substring(1))));
	int x = 0, y = 0, c = 0; 
	foreach (var step in steps)
	{
		switch (step.dir)
		{
			case 'R': for (var i = 0; i < step.dist; i++) yield return ((x++, y), c++); break;
			case 'L': for (var i = 0; i < step.dist; i++) yield return ((x--, y), c++); break;
			case 'U': for (var i = 0; i < step.dist; i++) yield return ((x, y++), c++); break;
			case 'D': for (var i = 0; i < step.dist; i++) yield return ((x, y--), c++); break;
		}
	}
}

var path1 = getPoints(input[0]);
var path2 = getPoints(input[1]);

var intersect = path1.GroupBy(p => p.position).Join(path2.GroupBy(p => p.position), 
													p => p.Key, 
													p => p.Key, 
													(o, i) => new { o.Key, path1 = o.Min(s => s.steps), path2 = i.Min(s => s.steps) })
														.Skip(1).ToList();

var part1 = intersect.Min(p => Math.Abs(p.Key.X) + Math.Abs(p.Key.Y));
part1.Dump();

var part2 = intersect.Min(i => i.path1 + i.path2);
part2.Dump();