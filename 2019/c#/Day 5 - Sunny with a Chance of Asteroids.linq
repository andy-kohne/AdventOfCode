<Query Kind="Statements" />

var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day5.txt")).Split(',').Select(int.Parse).ToArray();

IEnumerable<int> IntCodeComputer(int[] mem, IEnumerable<int> inputStream)
{
	var ptr = 0;
	var inp = inputStream.GetEnumerator();
	(int[] parmModes, int opcode) op;

	(int[] parmModes, int opcode) readOpCode(int opCode) => (new[] { (opCode / 100) % 10, (opCode / 1000) % 10, (opCode / 10000) % 10 }, opCode % 100);
	int getParmVal(int mode, int value) => mode == 0 ? mem[value] : value;
	int getParm(int parm) => getParmVal(op.parmModes[parm - 1], mem[ptr + parm]);

	while (mem[ptr] != 99)
	{
		op = readOpCode(mem[ptr]);
		switch (op.opcode)
		{
			case 1: mem[mem[ptr + 3]] = getParm(1) + getParm(2); ptr += 4; break;
			case 2: mem[mem[ptr + 3]] = getParm(1) * getParm(2); ptr += 4; break;
			case 3: inp.MoveNext();	mem[mem[ptr + 1]] = inp.Current; ptr += 2; break;
			case 4: yield return getParm(1);ptr += 2; break;
			case 5: ptr = getParm(1) != 0 ? getParm(2) : ptr + 3; break;
			case 6: ptr = getParm(1) == 0 ? getParm(2) : ptr + 3; break;
			case 7: mem[mem[ptr + 3]] = getParm(1) < getParm(2) ? 1 : 0; ptr += 4; break;
			case 8: mem[mem[ptr + 3]] = getParm(1) == getParm(2) ? 1 : 0;ptr += 4; break;
		}
	}
}


var part1 = IntCodeComputer(input.ToArray(), new[] { 1 }).Last();
part1.Dump();

var part2 = IntCodeComputer(input.ToArray(), new[] { 5 }).Last();
part2.Dump();