---
layout: default
---
# 2019



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