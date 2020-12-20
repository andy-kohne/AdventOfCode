<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day9.txt")).Select(long.Parse).ToArray();

IEnumerable<(T, T)> Pairs<T>(IList<T> items)
{
	for (var outer = 0; outer < items.Count() - 1; outer++)
		for (var inner = outer + 1; inner < items.Count(); inner++)
			if (!items[outer].Equals(items[inner]))
				yield return (items[outer], items[inner]);
}

bool isValid(int pos) => Pairs(input.Skip(pos-25).Take(25).ToList()).Any(p => p.Item1 + p.Item2 == input[pos]);
var part1 = Enumerable.Range(25, input.Length-25).Where(i => !isValid(i)).Select(i => input[i]).First();

long[] findWeakness(int pos){
	var numbers = new List<long>();
	while (numbers.Sum() < part1)
		numbers.Add(input[pos+numbers.Count()]);
	return numbers.Sum() == part1 ? numbers.ToArray() : null;
}

var w = Enumerable.Range(0, input.Length).Select(findWeakness).First(o => o != null);
var part2 = w.Max() + w.Min();
part2.Dump();
