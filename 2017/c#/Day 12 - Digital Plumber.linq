<Query Kind="Statements">
  <Namespace>System.Drawing</Namespace>
</Query>

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day12.txt"));

var progs = input.Select(i => i.Split(new[] { "<->" }, StringSplitOptions.None)).ToDictionary(i => int.Parse(i[0]), i => i[1].Split(',').Select(o => int.Parse(o)).ToArray());

void findGroup(int prog, List<int> group)
{
	if (group.Contains(prog))
		return;
	group.Add(prog);
	foreach (var p in progs[prog])
		findGroup(p, group);
}

var part1 = new List<int>();
findGroup(0, part1);
part1.Count().Dump();

var part2 = new List<List<int>> { part1 };
foreach (var prog in progs.Keys)
{
	if (part2.Any(p => p.Contains(prog)))
		continue;
	var g = new List<int>();
	findGroup(prog, g);
	part2.Add(g);
}
part2.Count().Dump();
