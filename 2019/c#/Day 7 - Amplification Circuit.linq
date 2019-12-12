<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day7.txt")).Split(',').Select(int.Parse).ToArray();

bool IntCodeComputer(int[] mem, IEnumerable<int> inputStream, out IList<int> outputStream)
{
	outputStream = new List<int>();
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
			case 3: if (!inp.MoveNext()) return false; mem[mem[ptr + 1]] = inp.Current; ptr += 2; break;
			case 4: outputStream.Add(getParm(1)); ptr += 2; break;
			case 5: ptr = getParm(1) != 0 ? getParm(2) : ptr + 3; break;
			case 6: ptr = getParm(1) == 0 ? getParm(2) : ptr + 3; break;
			case 7: mem[mem[ptr + 3]] = getParm(1) < getParm(2) ? 1 : 0; ptr += 4; break;
			case 8: mem[mem[ptr + 3]] = getParm(1) == getParm(2) ? 1 : 0; ptr += 4; break;
		}
	}
	return true;
}

IEnumerable<IEnumerable<T>> Permutations<T>(IEnumerable<T> list, int length = 0)
{
	if (length == 0) length = list.Count();
	if (length == 1) return list.Select(t => new T[] { t });

	return Permutations(list, length - 1)
		.SelectMany(t => list.Where(e => !t.Contains(e)),
			(t1, t2) => t1.Concat(new T[] { t2 }));
}

var part1 = Permutations(Enumerable.Range(0, 5)).Max(phases =>
{
	var inp = 0;
	foreach (var phase in phases)
	{
		IntCodeComputer(input.ToArray(), new List<int> { phase, inp }, out var output);
		inp = output.Last();
	}
	return inp;
});
part1.Dump();


var part2 = Permutations(Enumerable.Range(5, 5)).Select(o => o.ToList()).Select(phases =>
{
	var aIn = new List<int> { phases[0], 0 };
	var bIn = new List<int> { phases[1] };
	var cIn = new List<int> { phases[2] };
	var dIn = new List<int> { phases[3] };
	var eIn = new List<int> { phases[4] };
	var done = false;
	IList<int> output = null;
	while (!done)
	{
		IntCodeComputer(input.ToArray(), aIn, out output);
		bIn.Add(output.Last());
		IntCodeComputer(input.ToArray(), bIn, out output);
		cIn.Add(output.Last());
		IntCodeComputer(input.ToArray(), cIn, out output);
		dIn.Add(output.Last());
		IntCodeComputer(input.ToArray(), dIn, out output);
		eIn.Add(output.Last());
		done = IntCodeComputer(input.ToArray(), eIn, out output);
		aIn.Add(output.Last());
	}
	return output.Last();
}).Max();
part2.Dump();