<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day6.txt")).ToArray();

IEnumerable<string> getOrbits(Dictionary<string, string> map, string key)
{
	while (key != "COM")
	{
		yield return map[key];
		key = map[key];
	}
}

var orbits = input.Select(i => i.Split(')')).ToDictionary(i => i[1], i => i[0]);
var part1 = orbits.Sum(i => getOrbits(orbits, i.Key).Count());
part1.Dump();

var you = getOrbits(orbits, "YOU").ToList();
var santa = getOrbits(orbits, "SAN").ToList();
var common = you.Intersect(santa).First();

var part2 = you.IndexOf(common) + santa.IndexOf(common);
part2.Dump();