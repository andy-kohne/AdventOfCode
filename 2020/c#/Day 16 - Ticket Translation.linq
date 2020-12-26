<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day16.txt")).ToList();

var fields = input.TakeWhile(i => !string.IsNullOrEmpty(i)).Select(i => Regex.Match(i, @"(.+): (\d+)-(\d+) or (\d+)-(\d+)")).ToDictionary(m => m.Groups[1].Value, m => new List<(int, int)> { (int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value)), (int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value)), });
var myTicket = input.Skip(input.IndexOf("your ticket:")).Skip(1).Select(i => i.Split(',').Select(int.Parse).ToArray()).First();
var nearbyTickets = input.Skip(input.IndexOf("nearby tickets:")).Skip(1).Select(i => i.Split(',').Select(int.Parse).ToArray()).ToArray();

int scanningErrorRate(int[] ticket) => ticket.Where(t => fields.SelectMany(f => f.Value).All(f => !(t >= f.Item1 && t <= f.Item2))).Sum();
var part1 = nearbyTickets.Sum(scanningErrorRate);
part1.Dump();

var validTickets = nearbyTickets.Where(t => scanningErrorRate(t) == 0);
var maps = fields.Select(f => (f.Key, Enumerable.Range(0, myTicket.Length).Where(slot => validTickets.Select(vt => vt[slot]).All(value => f.Value.Any(range => range.Item1 <= value && range.Item2 >= value))).ToList())).ToDictionary(f => f.Key, f => f.Item2); ;
while (maps.Any(m => m.Value.Count() > 1))
{
	var excluded = maps.Where(m => m.Value.Count() == 1).SelectMany(m => m.Value);
	var keys = maps.Where(m => m.Value.Count() > 1).Select(m => m.Key).ToList();
	foreach (var map in keys)
		maps[map] = maps[map].Except(excluded).ToList();
}
var part2 = maps.Where(m => m.Key.StartsWith("departure")).Select(m => (long)myTicket[m.Value[0]]).Aggregate((x, y) => x * y);
part2.Dump();