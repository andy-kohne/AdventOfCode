<Query Kind="Statements" />

var estimate = 1001798;
var input = "19,x,x,x,x,x,x,x,x,41,x,x,x,x,x,x,x,x,x,859,x,x,x,x,x,x,x,23,x,x,x,x,13,x,x,x,17,x,x,x,x,x,x,x,x,x,x,x,29,x,373,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,37";

var buses = input.Split(',').Select((b,i) => (b,i)).Where(o => o.b != "x").Select(o => (bus:int.Parse(o.b),index:o.i)).ToArray();

var nextDeparture = buses.Select(b => b.bus).Select(b => (b, ((estimate / b) + 1) * b)).OrderBy(b => b.Item2).First();
var part1 = (nextDeparture.Item2-estimate)*nextDeparture.b;
part1.Dump();

long timestamp = buses.First().bus;
long interval = buses.First().bus;
foreach (var b in buses.Skip(1))
{
	while ((timestamp+b.index)%b.bus != 0) timestamp += interval;
	interval *= b.bus;
}
var part2 = timestamp;
part2.Dump();
