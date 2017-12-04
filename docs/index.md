---
layout: default
---

# 2017

### Day 3 - [[Spiral Memory]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day%203%20-%20Spiral%20Memory.linq)

```csharp
// part 1
Func<int, Point> FindPos = i =>
 {
	 var gs = (int)(Math.Ceiling(Math.Sqrt(i)));
	 var offset = (int)(Math.Floor((double)gs / 2));
	 if (gs % 2 == 0) gs++;
	 var br = gs * gs;
	 var bl = br - (gs - 1);
	 var tl = bl - (gs - 1);
	 var tr = tl - (gs - 1);
	 if (i >= bl)
		 return new Point { X = i - br + offset, Y = -offset };
	 if (i >= tl)
		 return new Point { X = -offset, Y = tl - i + offset };
	 if (i >= tr)
		 return new Point { X = tr - i + offset, Y = offset };
	 return new Point { X = offset, Y = i - tr + offset };
 };

var position = FindPos(input);
var part1 = Math.Abs(position.X) + Math.Abs(position.Y);
```

```csharp
// part 2
var grid = new Dictionary<Point, int>();
Func<int, int, int> GetValue = (x, y) => grid.TryGetValue(new Point(x, y), out int r) ? r : 0;
grid.Add(new Point(0, 0), 1);

while (grid.Values.Max() < input)
{
	var pos = FindPos(grid.Count() + 1);
	var val = GetValue(pos.X - 1, pos.Y - 1) +
			  GetValue(pos.X, pos.Y - 1) +
			  GetValue(pos.X + 1, pos.Y - 1) +
			  GetValue(pos.X - 1, pos.Y + 1) +
			  GetValue(pos.X, pos.Y + 1) +
			  GetValue(pos.X + 1, pos.Y + 1) +
			  GetValue(pos.X - 1, pos.Y) +
			  GetValue(pos.X + 1, pos.Y);
	grid.Add(pos, val);
};
var part2 = grid.Values.Max();
```


### Day 2 - [[Corruption Checksum]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day%202%20-%20Corruption%20Checksum.linq)

```csharp
var part1 = lines.Select(l => l.Max() - l.Min()).Sum();

Func<int[], int> EvenlyDivide = src =>
{
Func<int[], int> EvenlyDivide = src =>
{
	for (int o = 0; o < src.Length; o++)
		for (int i = 0; i < src.Length; i++)
			if (i != o && src[o] % src[i] == 0)
				return src[o] / src[i];
	return 0;
};
var part2 = lines.Select(l => EvenlyDivisible(l.ToArray())).Sum();
```


### Day 1 - [[Inverse Captcha]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day%201%20-%20Inverse%20Captcha.linq)

```csharp
var part1 = input.Zip(input.Skip(1).Concat(input), (a, b) => a == b ? a - '0' : 0).Sum();
var part2 = input.Zip(input.Skip(input.Length / 2).Concat(input), (a, b) => a == b ? a - '0' : 0).Sum();
```

or, without duplicating the input...

```csharp
var part1 = input.Where((c, i) => c == input[(i + 1) % input.Length]).Sum(c => c - '0');
var part2 = input.Where((c, i) => c == input[(i + input.Length/2) % input.Length]).Sum(c => c - '0');
```