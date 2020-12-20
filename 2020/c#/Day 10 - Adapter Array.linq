<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day10.txt")).Select(int.Parse).ToArray();

var jolts = new[] { 0, input.Max()+3 }.Concat(input).OrderBy(o => o).ToArray();

var diffs = jolts.Zip(jolts.Skip(1), (a,b) =>  b-a).GroupBy(j => j).ToDictionary(g => g.Key, g => g.Count());
var part1 = diffs[1] * diffs[3];
part1.Dump();

var connections = new Dictionary<int, long> { { 0, 1 }};
foreach (var jolt in jolts.Skip(1))
{
	connections[jolt] = (connections.TryGetValue(jolt - 1, out var one) ? one : 0) +
						(connections.TryGetValue(jolt - 2, out var two) ? two : 0) +
						(connections.TryGetValue(jolt - 3, out var three) ? three : 0);
}
var part2 = connections[jolts.Max()];
part2.Dump();