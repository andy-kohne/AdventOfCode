<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day13.txt")).Select(i => i.Split(':')).ToDictionary(i => int.Parse(i[0]), i => int.Parse(i[1]));

bool isCaught(int layer, int delay) => input.TryGetValue(layer, out int depth) && (layer + delay) % (((depth - 2) * 2) + 2) == 0;

// part 1
var part1 = input.Keys.Where(k => isCaught(k, 0)).Sum(k => k * input[k]);
part1.Dump();

// part 2
var part2 = 0;
while (input.Keys.Any(k => isCaught(k, part2)))
	part2++;	
part2.Dump();
