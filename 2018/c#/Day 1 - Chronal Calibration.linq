<Query Kind="Statements">
  <Reference>&lt;RuntimeDirectory&gt;\System.configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Namespace>System.Configuration</Namespace>
</Query>

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day1.txt")).Select(int.Parse).ToList();

var part1 = input.Sum();
part1.Dump();


var frequencies = new List<int>();
var frequency = 0;
var part2 = 0;
while (part2 == 0){
	foreach (var change in input){
		frequency += change;
		if (frequencies.Contains(frequency)){
			part2 = frequency;
			break;
		}
		frequencies.Add(frequency);
	}
}
part2.Dump();