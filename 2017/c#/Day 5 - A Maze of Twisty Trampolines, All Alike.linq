<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day4.txt"));

var part1 = input.Select(i => i.Split().GroupBy(w => w)).Where(i => i.All(w => w.Count() == 1)).Count();
part1.Dump();


var part2 = input.Select(i => i.Split().Select(w => new string(w.OrderBy(c => c).ToArray())).GroupBy(w => w))
				 .Where(o => o.All(w => w.Count() == 1)).Count();
part2.Dump();