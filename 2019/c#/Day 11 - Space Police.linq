<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day11.txt")).Split(',').Select(long.Parse).ToArray();

	var comp = new IntCodeComputer(input.ToArray());
	var robot = new Robot(0, comp);
	var part1 = robot.Panels.Count();
	part1.Dump();

	comp = new IntCodeComputer(input.ToArray());
	robot = new Robot(1, comp);
	var minx = robot.Panels.Min(o => o.Key.Item1);
	var miny = robot.Panels.Min(o => o.Key.Item2);

	var panels = robot.Panels.ToDictionary(p => (p.Key.Item1 + Math.Abs(minx), p.Key.Item2 + Math.Abs(miny)), p => p.Value);
	var drawing = Enumerable.Range(0, panels.Max(p => p.Key.Item2) + 1).Select(y =>
	 new string(Enumerable.Range(0, panels.Max(p => p.Key.Item1) + 1).Select(x => panels.TryGetValue((x, y), out var p) ? (p == 1 ? '8' : '_') : '_').ToArray())).ToList();

	for (int i = drawing.Count - 1; i >= 0; i--)
	{
		drawing[i].Dump();
	}
}

class Robot
{
	public readonly Dictionary<(int, int), int> Panels = new Dictionary<(int, int), int>();
	public Robot(int startingColor, IntCodeComputer comp)
	{

		int x = 0, y = 0, heading = 0;

		Panels[(x, y)] = startingColor;

		var done = false;
		while (!done)
		{
			comp.Input.Enqueue(Panels.ContainsKey((x, y)) ? Panels[(x, y)] : 0);
			done = comp.GetNext(out var color);
			if (!done)
			{
				Panels[(x, y)] = (int)color;
				done = comp.GetNext(out var turn);
				if (!done)
				{
					if (turn == 0)
					{
						heading = (heading + 270) % 360;
					}
					else
					{
						heading = (heading + 90) % 360;
					}
					switch (heading)
					{
						case 0: y++; break;
						case 90: x++; break;
						case 180: y--; break;
						case 270: x--; break;
					}
				}
			}

		}
	}

}
class IntCodeComputer
{
	private readonly Dictionary<long, long> _mem;
	private long ptr = 0;
	private long relativebase = 0;
	public IntCodeComputer(long[] program)
	{
		_mem = program.Select((p, i) => new { p, i = (long)(i) }).ToDictionary(o => (long)o.i, o => (long)o.p);
	}
	public readonly Queue<int> Input = new Queue<int>();

	public bool GetNext(out long? output)
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
				case 3: if (Input.Count() == 0) return false; setParm(1, Input.Dequeue()); ptr += 2; break;
				case 4: output = getParm(1); ptr += 2; return false; break;
				case 5: ptr = getParm(1) != 0 ? getParm(2) : ptr + 3; break;
				case 6: ptr = getParm(1) == 0 ? getParm(2) : ptr + 3; break;
				case 7: setParm(3, getParm(1) < getParm(2) ? 1 : 0); ptr += 4; break;
				case 8: setParm(3, getParm(1) == getParm(2) ? 1 : 0); ptr += 4; break;
				case 9: relativebase += getParm(1); ptr += 2; break;
			}
		}
		return true;
	}
}

