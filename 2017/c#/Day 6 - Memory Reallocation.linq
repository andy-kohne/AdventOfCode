<Query Kind="Statements">
  <Namespace>System.Drawing</Namespace>
</Query>

var input = "10	3	15	10	5	15	5	15	9	2	5	8	5	2	3	6".Split('\t').Select(i => int.Parse(i)).ToArray();

// part 1
var allocations = new List<string>();
var stamp = input.Aggregate("", (a,s) => a+','+s);
while(!allocations.Contains(stamp)) {
	allocations.Add(stamp);
	var pos = input.Select((i,p) => new { i, p}).First(i => i.i == input.Max()).p;
	var m = input[pos];
	input[pos] = 0;
	while (m > 0){
		pos++;
		if (pos >= input.Length)
			pos = 0;
		input[pos]++;
		m--;
	}
	stamp = input.Aggregate("", (a,s) => a+','+s);
}
var part1 = allocations.Count();
part1.Dump();

// part 2
var part2 = allocations.Count() - allocations.IndexOf(stamp);
part2.Dump();