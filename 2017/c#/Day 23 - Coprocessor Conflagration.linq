<Query Kind="Statements">
  <Namespace>System.Drawing</Namespace>
</Query>

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day23.txt"));

var instructions = input.Select(i => i.Split()).Select(i => new { op = i[0], reg = i[1], parm = i.Length > 2 ? i[2] : "" }).ToArray();
var registers = Enumerable.Range(0,8).ToDictionary(e => ((char)('a'+e)).ToString(), e=> 0L);
long get(Dictionary<string, long> regs, string x) => long.TryParse(x, out long y) ? y : regs[x];

var mulCtr = 0;
bool doInst(Dictionary<string, long> regs, ref long ins)
{
	if (ins < -1 || ins >= instructions.Length) return false;
	var reg = instructions[ins].reg;
	switch (instructions[ins].op)
	{
		case "set": regs[reg] = get(regs, instructions[ins].parm); break;
		case "sub": regs[reg] -= get(regs, instructions[ins].parm); break;
		case "mul": regs[reg] *= get(regs, instructions[ins].parm); mulCtr++; break;
		case "jnz": if (get(regs, reg) != 0) { ins += long.Parse(instructions[ins].parm); ins--; } break;
	}
	ins++;
	return true;
}

var pos = 0L;
while (doInst(registers, ref pos))
{ }
mulCtr.Dump();



// part 2
var b = 67;
var c = b;
var a = 1;
int h = 0;

if (a != 0)
{
	b = (b * 100) + 100000;
	c = b + 17000;
}

do
{
	for (var d = 2; d < b; d++)
	{
		if (b % d == 0)
		{
			h++;
			break;
		}
	}

	b += 17;
} while (b - c != 17);
h.Dump();