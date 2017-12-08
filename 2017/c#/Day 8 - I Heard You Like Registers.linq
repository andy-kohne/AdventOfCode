<Query Kind="Statements">
  <Namespace>System.Drawing</Namespace>
</Query>

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day8.txt"));

var instructions = input.Select(s => s.Split(' ')).Select(s => new { Register = s[0], 
																     Direction = s[1], 
																	 Amount = int.Parse(s[2]), 
																	 CondReg = s[4], 
																	 CondOp = s[5], 
																	 CondVal = int.Parse(s[6]) });
var regs = instructions.Select(i => i.Register).Distinct().ToDictionary(i => i, i => 0);

var compareFuncs = new Dictionary<string, Func<int, int, bool>>
{
	{ "==", (a, b) => a == b },
	{ "!=", (a, b) => a != b },
	{ ">", (a, b) => a > b },
	{ "<", (a, b) => a < b },
	{ ">=", (a, b) => a >= b},
	{ "<=", (a, b) => a <= b}
};

var part2 = 0;

foreach (var i in instructions)
{
	if (compareFuncs[i.CondOp](regs[i.CondReg], i.CondVal))
	{
		regs[i.Register] += i.Direction == "inc" ? i.Amount : -i.Amount;
		if (regs[i.Register] > part2) part2 = regs[i.Register];
	}
}

var part1 = regs.Max(r => r.Value);
part1.Dump();
part2.Dump();