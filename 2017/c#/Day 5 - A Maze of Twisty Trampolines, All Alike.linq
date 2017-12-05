<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day5.txt")).Select(l => int.Parse(l));

var jumps = input.ToArray();
var pos = 0;
var part1 = 0;
while (pos < jumps.Length)
{
	part1++;
	var offset = jumps[pos]++;
	pos += offset;
}

part1.Dump();


jumps = input.ToArray();
pos = 0;
var part2 = 0;
while (pos < jumps.Length)
{
	part2++;
	var offset = jumps[pos];
	if (offset >= 3)
		jumps[pos]--;
	else
		jumps[pos]++;
	pos += offset;
}

part2.Dump();