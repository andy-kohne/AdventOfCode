<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day1.txt")).Select(int.Parse).ToList();

IEnumerable<(T, T)> Pairs<T>(IList<T> items) {
	for (var outer = 0; outer < input.Count() -1 ; outer++)
		for (var inner = outer + 1; inner < input.Count(); inner++)
			yield return (items[outer], items[inner]);
}

var pair = Pairs(input).First(p => p.Item1 + p.Item2 == 2020);
var part1 = pair.Item1 * pair.Item2;
part1.Dump();


IEnumerable<(T, T, T)> Triples<T>(IList<T> items)
{
	for (var x = 0; x < input.Count() - 2; x++)
		for (var y = x + 1; y < input.Count() - 1; y++)
			for (var z = y + 1; z < input.Count(); z++)
				yield return (items[x], items[y], items[z]);
}

var triple = Triples(input).First(p => p.Item1 + p.Item2 + p.Item3 == 2020);
var part2 = triple.Item1 * triple.Item2 * triple.Item3;
part2.Dump();
