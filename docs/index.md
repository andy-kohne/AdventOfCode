---
layout: default
---
# 2018



### Day 1 - [[Inverse Captcha]](https://github.com/andy-kohne/AdventOfCode/blob/master/2018/c%23/Day 1 - Chronal Calibration.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2018/c%23/Day 1 - Chronal Calibration.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var part1 = input.Sum();


var frequencies = new List<int>();
var frequency = 0;
var part2 = 0;
while (part2 == 0){
	foreach (var change in input){
		frequency += change;
		if (frequencies.Contains(frequency)){
			part2 = frequency;
			break;
		}
		frequencies.Add(frequency);
	}
}
```