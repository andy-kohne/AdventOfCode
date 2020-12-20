<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day6.txt"));

IEnumerable<List<string>> GetGroups(string[] raw)
{
	var e = raw.GetEnumerator();
	var s = new List<string>();
	while (e.MoveNext())
	{
		var line = (string)e.Current;
		if (string.IsNullOrEmpty(line))
		{
			yield return s;
			s = new List<string>();
			continue;
		}
		s.Add(line);
	}
	yield return s;
}

var groups = GetGroups(input).ToList();

var part1 = groups.Sum(g => g.Aggregate((x, y) => x + y).ToCharArray().Distinct().Count());
part1.Dump();

var part2 = groups.Sum(g => g.SelectMany(a => a.ToCharArray()).GroupBy(a => a).Count(ga => ga.Count() == g.Count()));
part2.Dump();
