---
layout: default
---
# 2018



### Day 2 - [[Inverse Captcha]](https://github.com/andy-kohne/AdventOfCode/blob/master/2018/c%23/Day 2 - Inventory Management System.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2018/c%23/Day 2 - Inventory Management System.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var counts = input.Select(i => i.GroupBy(c => c).Select(c => new { character = c, count = c.Count() }).ToList());
var two = counts.Count(count => count.Any(c => c.count == 2));
var three = counts.Count(count => count.Any(c => c.count == 3));
var part1 = two * three;


bool isMatch(string a, string b)
{
	var differnces = 0;
	for (int i = 0; i < a.Length; i++)
	{
		if (a[i] != b[i])
		{
			differnces++;
			if (differnces > 1)
				return false;
		}
	}
	return differnces == 1;
}

var part2 = input.SelectMany(i => input.Select(i2 => new { i, i2 }).Where(o => o.i != o.i2 && isMatch(o.i, o.i2)))
				 .Select(o => o.i.Zip(o.i2, (f,s) => f == s ? f : ' ').Where(c => c != ' ').ToArray())
				 .Select(c => new string(c))
				 .First();
```

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