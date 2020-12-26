<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day19.txt")).ToList();

var rules = input.TakeWhile(i => !string.IsNullOrEmpty(i)).Select(i => i.Split(':')).ToDictionary(i => int.Parse(i[0]), i => i[1]);
var messages = input.Skip(rules.Count()).Skip(1).ToList();

var compiled = rules.Where(r => r.Value.Trim().StartsWith("\"")).ToDictionary(r => r.Key, r => new[] { r.Value.Trim().Substring(1, 1) });
rules = rules.Where(r => !compiled.ContainsKey(r.Key)).ToDictionary(r => r.Key, r => r.Value);
while (rules.Any())
{
	foreach (var rule in rules)
	{

		var deps = rule.Value.Split(" |".ToCharArray()).Where(r => !string.IsNullOrEmpty(r)).Select(int.Parse);
		if (deps.All(d => compiled.ContainsKey(d)))
		{
			var options = new List<string>();
			foreach (var segment in rule.Value.Split('|'))
			{
				var dep = segment.Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).Select(d => compiled[d]).ToArray();
				if (dep.Length > 1)
					options.AddRange(from l in dep[0]
									 from r in dep[1]
									 let o =l+r 
									 select o);
				else options.AddRange(dep[0]);
			}
			compiled[rule.Key] = options.ToArray();
		}

	}
	rules = rules.Where(r => !compiled.ContainsKey(r.Key)).ToDictionary(r => r.Key, r => r.Value);
}

var part1 = messages.Count(m => compiled[0].Contains(m));
part1.Dump();


int part2 = 0;
foreach (var message in messages)
{
	// rule 8
	var segments = compiled[42].Where(o => message.IndexOf(o) > -1).ToArray();
	var rule8 = new List<string>(segments);
	string[] possibles = segments;
	while (possibles.Any() && possibles.First().Length < message.Length)
	{
		possibles = (from s1 in segments
					 from s2 in possibles
					 let s = s1 + s2
					 where message.IndexOf(s) > -1
					 select s)
						.ToArray();
		rule8.AddRange(possibles);
	}

	// rule 11	
	var left = compiled[42].Where(o => message.IndexOf(o) > -1).ToArray();
	var right = compiled[31].Where(o => message.IndexOf(o) > -1).ToArray();
	possibles = (from l in left from r in right let o =l+r where message.IndexOf(o) > -1 select o).ToArray();
	var rule11 = new List<string>(possibles);
	while (possibles.Any() && possibles.First().Length < message.Length){
		
		possibles = (from l in left
					 from p in possibles
					 from r in right
					 let s = l + p + r
					 where message.IndexOf(s) > -1
					 select s)
						.ToArray();
					rule11.AddRange(possibles);
	}

	// rule 0: 8 11
	if ((from a in rule8 from b in rule11 let c=a+b where c.Length == message.Length select c).Any(c => message == c))
		part2++;
}
part2.Dump();
