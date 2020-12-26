<Query Kind="Statements">
  <Namespace>System.Collections.Generic</Namespace>
</Query>

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day17.txt")).ToList();

var pocketSpace = input.SelectMany((s, li) => s.Select((c, ci) => ((ci, li, 0), c))).Where(i => i.c =='#').Select(i => i.Item1).ToHashSet();

HashSet<(int, int, int)> Cycle(HashSet<(int x, int y, int z)> src)
{
	int CountNeighbors((int x, int y, int z) p) => src.Count(s => Math.Abs(p.x - s.x) < 2 && Math.Abs(p.y - s.y) < 2 && Math.Abs(p.z - s.z) < 2 );

	var ret = new HashSet<(int, int, int)>();
	for (int dx = src.Select(k => k.x).Min() - 1; dx <= src.Select(k => k.x).Max() + 1; dx++)
		for (int dy = src.Select(k => k.y).Min() - 1; dy <= src.Select(k => k.y).Max() + 1; dy++)
			for (int dz = src.Select(k => k.z).Min() - 1; dz <= src.Select(k => k.z).Max() + 1; dz++)
			{
				var k = (dx, dy, dz);
				var n = CountNeighbors(k);
				if (n == 3 || (src.TryGetValue(k, out var _) && n == 4))
					ret.Add(k);
			}
	return ret;
}

for (int cycle = 0; cycle < 6; cycle++)
{
	pocketSpace = Cycle(pocketSpace);
}
var part1 = pocketSpace.Count();
part1.Dump();


var hyperCubePocketSpace = input.SelectMany((s, li) => s.Select((c, ci) => ((x: ci, y: li, z: 0, w: 0), c))).Where(i => i.c =='#').Select(i => i.Item1).ToHashSet();
HashSet<(int, int, int, int)> Cycle2(HashSet<(int x, int y, int z, int w)> src)
{
	int CountNeighbors((int x, int y, int z, int w) p) => src.Count(s => s.x>=p.x-1 && s.x<=p.x+1 && s.y>=p.y-1 && s.y<=p.y+1 && s.z>=p.z-1 && s.z<=p.z+1 && s.w>=p.w-1 && s.w<=p.w+1);
																
	var wmax = src.Select(k => k.w).Max() + 1;
	var wmin = src.Select(k => k.w).Min() - 1;
	var zmax = src.Select(k => k.z).Max() + 1;
	var zmin = src.Select(k => k.z).Min() - 1;
	var ymax = src.Select(k => k.y).Max() + 1;
	var ymin = src.Select(k => k.y).Min() - 1;
	var xmax = src.Select(k => k.x).Max() + 1;
	var xmin = src.Select(k => k.x).Min() - 1;
	var ret = new HashSet<(int, int, int, int)>();
	for (int dx = xmin; dx <= xmax; dx++)
		for (int dy = ymin; dy <= ymax; dy++)
			for (int dz = zmin; dz <= zmax; dz++)
				for (int dw = wmin; dw <= wmax; dw++)
				{
					var k = (dx, dy, dz, dw);
					var n = CountNeighbors(k);
					if (n == 3 || (src.TryGetValue(k, out var _) && n == 4))
						ret.Add(k);
				}
	return ret;
}
for (int cycle = 0; cycle < 6; cycle++)
{
	hyperCubePocketSpace = Cycle2(hyperCubePocketSpace);
}
var part2 = hyperCubePocketSpace.Count();
part2.Dump();
