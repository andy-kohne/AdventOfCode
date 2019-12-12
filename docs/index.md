---
layout: default
---
# 2019



### Day 9 - [[Sensor Boost]](https://github.com/andy-kohne/AdventOfCode/blob/master/2019/c%23/Day 9 - Sensor Boost.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2019/c%23/Day 9 - Sensor Boost.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
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
```


### Day 8 - [[Space Image Format]](https://github.com/andy-kohne/AdventOfCode/blob/master/2019/c%23/Day 8 - Space Image Format.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2019/c%23/Day 8 - Space Image Format.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
int width = 25, height = 6;
var layersize = width * height;

var chunks = Enumerable.Range(0, input.Length / layersize).Select(i => input.Substring(i * layersize, layersize));
var layers = chunks.Select(c => new { Raw = c, Counts = c.ToCharArray().GroupBy(l => l).ToDictionary(g => g.Key, g => g.Count())}).ToList();

var part1 = layers.OrderBy(l => l.Counts['0']).Select(l => l.Counts['1'] * l.Counts['2']).First();
part1.Dump();

var combined = new string(Enumerable.Range(0, layersize).Select(p => layers.First(l => l.Raw[p] != '2').Raw[p]).ToArray());
combined = combined.Replace('0', '_').Replace('1','8');  // help with image readability
var part2 = Enumerable.Range(0,height).Select(i => combined.Substring(i * width, width)).ToArray();
part2.Dump();
```


### Day 7 - [[Amplification Circuit]](https://github.com/andy-kohne/AdventOfCode/blob/master/2019/c%23/Day 7 - Amplification Circuit.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2019/c%23/Day 7 - Amplification Circuit.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
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
```


### Day 6 - [[Universal Orbit Map]](https://github.com/andy-kohne/AdventOfCode/blob/master/2019/c%23/Day 6 - Universal Orbit Map.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2019/c%23/Day 6 - Universal Orbit Map.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
IEnumerable<string> getOrbits(Dictionary<string, string> map, string key)
{
	while (key != "COM")
	{
		yield return map[key];
		key = map[key];
	}
}

var orbits = input.Select(i => i.Split(')')).ToDictionary(i => i[1], i => i[0]);
var part1 = orbits.Sum(i => getOrbits(orbits, i.Key).Count());
part1.Dump();

var you = getOrbits(orbits, "YOU").ToList();
var santa = getOrbits(orbits, "SAN").ToList();
var common = you.Intersect(santa).First();

var part2 = you.IndexOf(common) + santa.IndexOf(common);
part2.Dump();
```


### Day 5 - [[Sunny with a Chance of Asteroids]](https://github.com/andy-kohne/AdventOfCode/blob/master/2019/c%23/Day 5 - Sunny with a Chance of Asteroids.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2019/c%23/Day 5 - Sunny with a Chance of Asteroids.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
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
```


### Day 4 - [[Secure Container]](https://github.com/andy-kohne/AdventOfCode/blob/master/2019/c%23/Day 4 - Secure Container.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2019/c%23/Day 4 - Secure Container.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
bool HasDoubleDigit(string pw) => pw[0] == pw[1] || pw[1] == pw[2] || pw[2] == pw[3] || pw[3] ==pw[4] || pw[4] == pw[5];
bool IsAscending(string pw) => pw[0] <= pw[1] && pw[1] <= pw[2] && pw[2] <= pw[3] && pw[3] <= pw[4] && pw[4] <= pw[5];
bool HasNotTripledDoubleDigit(string pw) => (pw[0] == pw[1] && pw[2] != pw[1]) ||
											(pw[0] != pw[1] && pw[1] == pw[2] && pw[2] != pw[3]) ||
											(pw[1] != pw[2] && pw[2] == pw[3] && pw[3] != pw[4]) ||
											(pw[2] != pw[3] && pw[3] == pw[4] && pw[4] != pw[5]) ||
											(pw[3] != pw[4] && pw[4] == pw[5]);

int part1 = 0, part2 = 0;
for (var pw = 254032; pw <= 789860; pw++){
	var pws = pw.ToString();
	if (HasDoubleDigit(pws) && IsAscending(pws))
	{
		part1++;
		if (HasNotTripledDoubleDigit(pws))
			part2++;
	}
	
}

part1.Dump();
part2.Dump();
```


### Day 3 - [[Crossed Wires]](https://github.com/andy-kohne/AdventOfCode/blob/master/2019/c%23/Day 3 - Crossed Wires.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2019/c%23/Day 3 - Crossed Wires.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
IEnumerable<((int X, int Y) position, int steps)> getPoints(string path)
{
	var steps = path.Split(',').Select(p => (dir: p[0], dist: int.Parse(p.Substring(1))));
	int x = 0, y = 0, c = 0; 
	foreach (var step in steps)
	{
		switch (step.dir)
		{
			case 'R': for (var i = 0; i < step.dist; i++) yield return ((x++, y), c++); break;
			case 'L': for (var i = 0; i < step.dist; i++) yield return ((x--, y), c++); break;
			case 'U': for (var i = 0; i < step.dist; i++) yield return ((x, y++), c++); break;
			case 'D': for (var i = 0; i < step.dist; i++) yield return ((x, y--), c++); break;
		}
	}
}

var path1 = getPoints(input[0]);
var path2 = getPoints(input[1]);

var intersect = path1.GroupBy(p => p.position).Join(path2.GroupBy(p => p.position), 
													p => p.Key, 
													p => p.Key, 
													(o, i) => new { o.Key, path1 = o.Min(s => s.steps), path2 = i.Min(s => s.steps) })
														.Skip(1).ToList();

var part1 = intersect.Min(p => Math.Abs(p.Key.X) + Math.Abs(p.Key.Y));
part1.Dump();

var part2 = intersect.Min(i => i.path1 + i.path2);
part2.Dump();
```


### Day 2 - [[1202 Program Alarm]](https://github.com/andy-kohne/AdventOfCode/blob/master/2019/c%23/Day 2 - 1202 Program Alarm.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2019/c%23/Day 2 - 1202 Program Alarm.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
void IntCodeComputer(int[] instructions) {
	var ptr = 0;
	while (instructions[ptr] != 99)
	{
		var regA = instructions[instructions[ptr + 1]];
		var regB = instructions[instructions[ptr + 2]];
		instructions[instructions[ptr + 3]] =
			instructions[ptr] == 1
				? regA + regB
				: regA * regB;
		ptr += 4;
	}	
}

var part1 = input.Select(i => i).ToArray();
part1[1] = 12;
part1[2] = 2;
IntCodeComputer(part1);
part1[0].Dump();

for (var noun = 0; noun < 100; noun++){
	for (var verb = 0; verb < 100; verb++)
	{
		var part2 = input.Select(i => i).ToArray();
		part2[1] = noun;
		part2[2] = verb;
		IntCodeComputer(part2);
		if (part2[0] == 19690720)
		{
			(100*noun+verb).Dump();
			return;
		}
	}
}
```


### Day 1 - [[The Tyranny of the Rocket Equation]](https://github.com/andy-kohne/AdventOfCode/blob/master/2019/c%23/Day 1 - The Tyranny of the Rocket Equation.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2019/c%23/Day 1 - The Tyranny of the Rocket Equation.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var part1 = input.Select(mass => (mass/3)-2).Sum();
part1.Dump();

int additionalFuel(int mass) {
	var f = (mass/3)-2;
	return f > 0 ? f + additionalFuel(f) : 0;
}
var part2 = input.Select(additionalFuel).Sum();
part2.Dump();
```