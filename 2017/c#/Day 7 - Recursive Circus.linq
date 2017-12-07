<Query Kind="Program">
  <Namespace>System.Drawing</Namespace>
</Query>

void Main()
{
	var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day7.txt"));
	var re = new Regex(@"^([^ ]+) \((\d+)\)( -> (([^, ]+(, )?)+))?");
	var programs = input.Select(i => re.Match(i))
						.ToDictionary(m => m.Groups[1].Value, m => new Program
						{
							Weight = int.Parse(m.Groups[2].Value),
							Holding = m.Groups[4].Value.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToArray()
						});

	// part1
	var part1 = programs.Single(p => !programs.Any(pr => pr.Value.Holding.Contains(p.Key))).Key;
	part1.Dump();

	// part 2
	var unbalanced = programs.Values
			.Where(p => p.Holding.Select(h => programs[h].TotalWeight(programs)).Distinct().Count() > 1)
			.OrderBy(p => p.TotalWeight(programs))
			.First()
			.Holding.Select(h => programs[h]);
	var stacks = unbalanced.GroupBy(u => u.TotalWeight(programs)).OrderBy(u => u.Count()).Select(u => u.First()).ToArray();
	var part2 = stacks[0].Weight + (stacks[1].TotalWeight(programs) - stacks[0].TotalWeight(programs));
	part2.Dump();
}


class Program
{
	public int Weight { get; set; }
	public string[] Holding { get; set; }

	public int TotalWeight(Dictionary<string, Program> programs) => Weight + Holding.Sum(h => programs[h].TotalWeight(programs));
}