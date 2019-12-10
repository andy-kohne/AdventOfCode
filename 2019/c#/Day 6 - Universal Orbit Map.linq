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

var d = input.ToDictionary(i => i.Split(')')[1], i => i.Split(')')[0]);
var part1 = d.Sum(i => getOrbits(d, i.Key).Count());
part1.Dump();

var you = getOrbits(d, "YOU").ToList();
var santa = getOrbits(d, "SAN").ToList();
var common = you.Intersect(santa).First();

var part2 = you.IndexOf(common) + santa.IndexOf(common);
part2.Dump();
