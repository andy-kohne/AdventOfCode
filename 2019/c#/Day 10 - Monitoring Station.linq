<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day10.txt"));

int gcd(int a, int b) => b == 0 ? a : gcd(b, a % b);
(int, int) reduce((int, int) i) => gcd(i.Item1, i.Item2) == 0 ? (1, 0) : (i.Item1 / gcd(i.Item1, i.Item2), i.Item2 / gcd(i.Item1, i.Item2));
(int, int) getslope((int x, int y) p1, (int x, int y) p2) => reduce((p2.x - p1.x, p2.y - p1.y));

bool isVisible((int x, int y) p1, (int x, int y) p2, HashSet<(int x, int y)> map)
{
	if (p1 == p2) return false;
	var slope = getslope(p1, p2);

	for (int x = Math.Min(p1.x, p2.x); x <= Math.Max(p1.x, p2.x); x++)
	{
		for (int y = Math.Min(p1.y, p2.y); y <= Math.Max(p1.y, p2.y); y++)
		{
			if ((x, y) != p1 && (x, y) != p2 && slope == getslope(p1, (x, y)) && map.Contains((x,y))) return false;
		}
	}
	return true;
}

var asteroids = Enumerable.Range(0, input.Length).SelectMany(row => Enumerable.Range(0, input[row].Length).Where(col => input[row][col] == '#').Select(col => (col, row))).ToHashSet();
var monitoringstation = asteroids.Select(asteroid => new { asteroid, count = asteroids.Count(aa => isVisible(asteroid, aa, asteroids)) }).OrderByDescending(a => a.count).First();

var part1 = monitoringstation.count;
part1.Dump();


double todegrees(double radians) => (360 + radians * (180 / Math.PI)) % 360;
double angle((int x, int y) a, (int x, int y) b) => todegrees(Math.Atan2(a.y - b.y, a.x - b.x) - Math.Atan2(1,0));
double distance((int x, int y) b, (int x, int y) a) => Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2));

var asteroidVectors = asteroids
			.Where(a => a != monitoringstation.asteroid)
			.Select(a => new { asteroid = a, vector = (angle: angle(monitoringstation.asteroid, a), dist: distance(a, monitoringstation.asteroid))})
			.GroupBy(a => a.vector.angle)
			.OrderBy(a => a.Key)
			.Select(a => a.OrderBy(v => v.vector.dist).ToList()).ToList();

var destroyed = new List<(int x, int y)>();

while (asteroidVectors.Any(o => o.Any())){
	foreach (var o in asteroidVectors)
	{
		if (o.Any())
		{
			destroyed.Add(o.First().asteroid);
			o.RemoveAt(0);
		}
	}
}

var part2 = destroyed[199].x * 100 + destroyed[199].y;
part2.Dump();