---
layout: default
---
# 2019



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