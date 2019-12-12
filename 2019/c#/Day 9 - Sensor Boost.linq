<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day9.txt")).Split(',').Select(int.Parse).ToArray();

bool IntCodeComputer(int[] program, IEnumerable<int> inputStream, out IList<long> outputStream)
{
	var mem = program.Select((p, i) => new { p, i = (long)(i) }).ToDictionary(o => (long)o.i, o => (long)o.p);

	outputStream = new List<long>();
	long ptr = 0;
	long relativebase = 0;
	var inp = inputStream.GetEnumerator();
	(int[] parmModes, int opcode) op;

	(int[] parmModes, int opcode) readOpCode(int opCode) => (new[] { (opCode / 100) % 10, (opCode / 1000) % 10, (opCode / 10000) % 10 }, opCode % 100);
	long getParmVal(int mode, long value) => mode == 0 ? mem[value] : mode == 2 ? mem[relativebase + value] : value;
	long getParm(long parm) => getParmVal(op.parmModes[parm - 1], mem[ptr + parm]);
	void setParmVal(int mode, long val, long value)  { if (mode == 2) mem[relativebase + val]=value; else mem[val]=value; }
	void setParm(long parm, long val) => setParmVal(op.parmModes[parm-1], mem[ptr+parm], val);

	while (mem[ptr] != 99)
	{
		op = readOpCode((int)mem[ptr]);
		switch (op.opcode)
		{
			case 1: setParm(3, getParm(1) + getParm(2)); ptr += 4; break;
			case 2: setParm(3, getParm(1) * getParm(2)); ptr += 4; break;
			case 3: if (!inp.MoveNext()) return false; setParm(1, inp.Current); ptr += 2; break;
			case 4: outputStream.Add(getParm(1)); ptr += 2; break;
			case 5: ptr = getParm(1) != 0 ? getParm(2) : ptr + 3; break;
			case 6: ptr = getParm(1) == 0 ? getParm(2) : ptr + 3; break;
			case 7: setParm(3, getParm(1) < getParm(2) ? 1 : 0); ptr += 4; break;
			case 8: setParm(3, getParm(1) == getParm(2) ? 1 : 0); ptr += 4; break;
			case 9: relativebase += getParm(1); ptr += 2; break;
		}
	}
	return true;
}

IntCodeComputer(input.ToArray(), new [] { 1}, out var p1);
var part1 = p1.Last();
part1.Dump();

IntCodeComputer(input.ToArray(), new [] { 2}, out var p2);
var part2 = p2.Last();
part2.Dump();

