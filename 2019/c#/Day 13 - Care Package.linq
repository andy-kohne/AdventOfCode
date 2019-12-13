<Query Kind="Program" />

void Main()
{
	var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day13.txt")).Split(',').Select(long.Parse).ToArray();

	var comp = new IntCodeComputer(input.ToArray());
	var board = new Dictionary<(long x, long y), int>();
	
	while (!comp.Halted){
		comp.GetNext(out var x);
		comp.GetNext(out var y);
		comp.GetNext(out var t);
		if (!comp.Halted)
			board[((long)x,(long)y)] = (int)t;
	};
	
	var part1 = board.Count(t => t.Value == 2);
	part1.Dump();


	comp = new IntCodeComputer(input.ToArray());
	board = new Dictionary<(long x, long y), int>();
	comp._mem[0] = 2;
	var score = 0;

	do
	{
		comp.GetNext(out var x);
		if (x == null){
			var ball = board.First(ti => ti.Value == 4).Key.x;
			var paddle = board.First(ti => ti.Value == 3).Key.x;
			comp.Input.Enqueue(ball.CompareTo(paddle));
			continue;
		}
		comp.GetNext(out var y);
		comp.GetNext(out var t);
		if (x == -1 && y == 0)
			score = (int)t;		
		else if (!comp.Halted)
			board[((long)x, (long)y)] = (int)t;		
	} while (!comp.Halted);
	
	var part2 = score;
	part2.Dump();
}


class IntCodeComputer
{
	public readonly Dictionary<long, long> _mem;
	private long ptr = 0;
	private long relativebase = 0;
	public IntCodeComputer(long[] program)
	{
		_mem = program.Select((p, i) => new { p, i = (long)(i) }).ToDictionary(o => (long)o.i, o => (long)o.p);
	}
	public readonly Queue<int> Input = new Queue<int>();

	public bool Halted;
	public void GetNext(out long? output)
	{
		(int[] parmModes, int opcode) op;
		(int[] parmModes, int opcode) readOpCode(int opCode) => (new[] { (opCode / 100) % 10, (opCode / 1000) % 10, (opCode / 10000) % 10 }, opCode % 100);
		long readMem(long pos) => _mem.ContainsKey(pos) ? _mem[pos] : 0;
		long getParmVal(int mode, long value) => mode == 0 ? readMem(value) : mode == 2 ? readMem(relativebase + value) : value;
		long getParm(long parm) => getParmVal(op.parmModes[parm - 1], _mem[ptr + parm]);
		void setParmVal(int mode, long val, long value) { if (mode == 2) _mem[relativebase + val] = value; else _mem[val] = value; }
		void setParm(long parm, long val) => setParmVal(op.parmModes[parm - 1], _mem[ptr + parm], val);

		output = default;
		while (_mem[ptr] != 99)
		{
			op = readOpCode((int)_mem[ptr]);
			switch (op.opcode)
			{
				case 1: setParm(3, getParm(1) + getParm(2)); ptr += 4; break;
				case 2: setParm(3, getParm(1) * getParm(2)); ptr += 4; break;
				case 3: if (Input.Count() == 0) return; setParm(1, Input.Dequeue()); ptr += 2; break;
				case 4: output = getParm(1); ptr += 2; return; break;
				case 5: ptr = getParm(1) != 0 ? getParm(2) : ptr + 3; break;
				case 6: ptr = getParm(1) == 0 ? getParm(2) : ptr + 3; break;
				case 7: setParm(3, getParm(1) < getParm(2) ? 1 : 0); ptr += 4; break;
				case 8: setParm(3, getParm(1) == getParm(2) ? 1 : 0); ptr += 4; break;
				case 9: relativebase += getParm(1); ptr += 2; break;
			}
		}
		Halted=true;
	}
}