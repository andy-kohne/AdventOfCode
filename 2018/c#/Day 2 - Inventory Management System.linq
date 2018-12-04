<Query Kind="Statements">
  <Reference>&lt;RuntimeDirectory&gt;\System.configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Namespace>System.Configuration</Namespace>
</Query>

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day2.txt"));

var counts = input.Select(i => i.GroupBy(c => c).Select(c => new { character = c, count = c.Count() }).ToList());
var two = counts.Count(count => count.Any(c => c.count == 2));
var three = counts.Count(count => count.Any(c => c.count == 3));
var part1 = two * three;
part1.Dump();

bool isMatch(string a, string b)
{
	var differnces = 0;
	for (int i = 0; i < a.Length; i++)
	{
		if (a[i] != b[i])
		{
			differnces++;
			if (differnces > 1)
				return false;
		}
	}
	return differnces == 1;
}

var part2 = input.SelectMany(i => input.Select(i2 => new { i, i2 }).Where(o => o.i != o.i2 && isMatch(o.i, o.i2)))
				 .Select(o => o.i.Zip(o.i2, (f,s) => f == s ? f : ' ').Where(c => c != ' ').ToArray())
				 .Select(c => new string(c))
				 .First();
part2.Dump();