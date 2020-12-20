<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day2.txt"));

var re = new Regex(@"^(\d+)-(\d+) (.): (.+)$");
var passwords = input.Select(i => re.Match(i)).Select(i => (min: int.Parse(i.Groups[1].Value) , max: int.Parse(i.Groups[2].Value), c: i.Groups[3].Value.Single(), pw: i.Groups[4].Value) ).ToList();

var part1 = passwords.Select(item => (item.min, item.max, count: item.pw.Count(p => p == item.c))).Count(item => item.count >= item.min && item.count <= item.max);
part1.Dump();

var part2 = passwords.Count(item => item.pw[item.min-1]==item.c ^ item.pw[item.max-1]==item.c);
part2.Dump();
