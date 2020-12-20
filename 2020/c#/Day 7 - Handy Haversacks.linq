<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day7.txt"));
var rules = input.Select(i => Regex.Match(i, @"^(.+) bags contain (no other bags|(([ ]*\d ([^,]+) bag[^,]*[,]?)*)).$")).ToDictionary(m => m.Groups[1].Value, m => m.Groups[3].Value.Split(',').Where(i => !string.IsNullOrEmpty(i)).Select(i => Regex.Match(i, @"(\d+) (.+) bag")).Select(i => (count: int.Parse(i.Groups[1].Value),bag: i.Groups[2].Value)).ToArray());

bool CanContain(string bag, string inner) =>  rules[bag].Any(i => i.bag == inner || CanContain(i.bag, inner));
var part1 = rules.Count(r => CanContain(r.Key, "shiny gold"));
part1.Dump();


int Contains(string bag) => rules[bag].Sum(v => v.count + (v.count * Contains(v.bag)));
var part2 = Contains("shiny gold");
part2.Dump();

