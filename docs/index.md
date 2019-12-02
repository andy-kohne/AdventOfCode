---
layout: default
---
# 2019



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