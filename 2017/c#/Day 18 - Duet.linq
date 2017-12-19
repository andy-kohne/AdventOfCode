<Query Kind="Statements">
  <Namespace>System.Drawing</Namespace>
</Query>

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day18.txt"));

var instructions = input.Select(i => i.Split()).Select(i => new { op = i[0], reg = i[1], parm = i.Length > 2 ? i[2] : "" }).ToArray();
var registers = instructions.Select(i => i.reg).Where(i => !int.TryParse(i, out int ii)).Distinct().ToDictionary(i => i, i => 0L);
long get(Dictionary<string, long> regs, string x) => long.TryParse(x, out long y) ? y : regs[x];

bool doInst(Dictionary<string, long> regs, ref long ins, Func<long?> rcv, Action<long> snd, out long? ret)
{
	ret = null;
	if (ins < -1 || ins >= instructions.Length) return false;
	var reg = instructions[ins].reg;
	switch (instructions[ins].op)
	{
		case "snd": snd(get(regs, reg)); break;
		case "set": regs[reg] = get(regs, instructions[ins].parm); break;
		case "add": regs[reg] += get(regs, instructions[ins].parm); break;
		case "mul": regs[reg] *= get(regs, instructions[ins].parm); break;
		case "mod": regs[reg] %= get(regs, instructions[ins].parm); break;
		case "rcv": ret = rcv(); if (ret == null) return false; regs[reg] = ret.Value; break;
		case "jgz": if (get(regs, reg) > 0) { ins += get(regs, instructions[ins].parm); ins--; } break;
	}
	ins++;
	return true;
}

var pos = 0L;
long? part1 = null, lastSound = null;
while (part1 == null)
{
	if (!doInst(registers, ref pos, () => get(registers, instructions[pos].reg) != 0 ? lastSound : null, sound => lastSound = sound, out part1))
		pos++;
}
part1.Dump();


long pos1 = 0, pos2 = 0;
var q1 = new Queue<long>();
var q2 = new Queue<long>();
var regs1 = registers.ToDictionary(r => r.Key, r => 0L);
var regs2 = registers.ToDictionary(r => r.Key, r => 0L);
regs2["p"] = 1;
var part2 = 0;
while ((doInst(regs1, ref pos1, () => q1.Count > 0 ? (long?)q1.Dequeue() : null, snd => q2.Enqueue(snd), out long? x) |
		doInst(regs2, ref pos2, () => q2.Count > 0 ? (long?)q2.Dequeue() : null, snd => { q1.Enqueue(snd); part2++; }, out long? y)))
{ }
part2.Dump();
