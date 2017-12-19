---
layout: default
---

# 2017

### Day 17 - [[Spinlock]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day 17 - Spinlock.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2017/c%23/Day 17 - Spinlock.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var buffer = new[] { 0};
var pos = 0;

for (int i = 1; i <=2017; i++)
{
	pos = (pos + input ) % buffer.Length;
	Array.Resize(ref buffer, buffer.Length + 1);
	Array.Copy(buffer, pos, buffer, pos +1, buffer.Length -pos -1);
	pos++;
	buffer[pos] = i;
}
var part1 = buffer[pos+1];
part1.Dump();


pos =0;
var part2 = 0;
for (int i = 1; i <= 50_000_000; i++)
{
	pos = (pos + input) % i;
	if (pos == 0) part2 = i;
	pos++;
}
part2.Dump();
```


### Day 16 - [[Permutation Promenade]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day 16 - Permutation Promenade.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2017/c%23/Day 16 - Permutation Promenade.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var dancemoves = input.Select(o => new { Move = o[0], Parms = o.Substring(1).Split('/') })
					  .Select(o => new
					  {
						  Move = o.Move,
						  X = o.Move == 'p' ? 0 : int.Parse(o.Parms[0]),
						  Y = o.Move == 'x' ? int.Parse(o.Parms[1]) : 0,
						  A = o.Parms[0][0],
						  B = o.Parms.Length < 2 ? 'x' : o.Parms[1][0]
					  }).ToArray();

string spin(string progs, int x) => progs.Substring(progs.Length - x) + progs.Substring(0, progs.Length - x);
string exchange(string progs, int x, int y) { var p = progs.ToCharArray(); var temp = p[x]; p[x] = p[y]; p[y] = temp; return new string(p); }
string partner(string progs, char a, char b) => progs.Replace(a, 'x').Replace(b, a).Replace('x', b);
string dance(string progs)
{
	foreach (var i in dancemoves)
	{
		switch (i.Move)
		{
			case 's': progs = spin(progs, i.X); break;
			case 'x': progs = exchange(progs, i.X, i.Y); break;
			case 'p': progs = partner(progs, i.A, i.B); break;
		}
	}
	return progs;
}


var part1 = dance("abcedfghijklmnop");
part1.Dump();

var iterationsTilRepeat = 1;
var part2 = dance("abcedfghijklmnop");
while (part2 != "abcedfghijklmnop")
{
	part2 = dance(part2);
	iterationsTilRepeat++;
}
for (int i = 0; i < 1000000000 % iterationsTilRepeat; i++)
{
	part2 = dance(part2);
}
part2.Dump();
```


### Day 15 - [[Dueling Generators]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day 15 - Dueling Generators.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2017/c%23/Day 15 - Dueling Generators.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
uint generate(uint last, int factor) => (uint)((ulong)(last * factor) % 2147483647);
bool isMatch(uint? a, uint? b) => ((a << 48) >> 48) == ((b << 48) >> 48);

Func<uint?, uint> GenA = last => generate(last ?? 516, 16807);
Func<uint?, uint> GenB = last => generate(last ?? 190, 48271);

int countMatches(Func<uint?, uint> a, Func<uint?, uint> b, int iterations)
{
	uint? lastA = null, lastB = null;
	int count = 0;
	for (var i = 0; i < iterations; i++)
	{
		lastA = a(lastA);
		lastB = b(lastB);
		if (isMatch(lastA, lastB))
			count++;
	}
	return count;
};

var part1 = countMatches(GenA, GenB, 40000000);
part1.Dump();

uint constrainedGenerate(Func<uint?, uint> gen, uint? last, int f)
{
	do
	{
		last = gen(last);
	} while ((last % f != 0));
	return last.Value;
};

var p2 = countMatches(last => constrainedGenerate(GenA, last, 4), last => constrainedGenerate(GenB, last, 8), 5000000);
p2.Dump();
```


### Day 14 - [[Disk Defragmentation]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day 14 - Disk Defragmentation.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2017/c%23/Day 14 - Disk Defragmentation.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
byte[] knotHash(string str, int cycles = 64)
{
	var key = Encoding.ASCII.GetBytes(str).Concat(new byte [] {17, 31, 73, 47, 23}).ToArray();
	var clist = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();

	int pos = 0, skip=0;
	for (int c = 0; c < cycles; c++)
	{
		foreach (var i in key)
		{
			for (int s = 0; s < i / 2; s++)
			{
				var a = (pos + s) % clist.Length;
				var b = (pos + i - s - 1) % clist.Length;
				var t = clist[a];
				clist[a] = clist[b];
				clist[b] = t;
			}
			pos += (i + skip);
			pos = pos % clist.Length;
			skip++;
		}
	}
	return Enumerable.Range(0, 16).Select(r => clist.Skip(r * 16).Take(16).Aggregate(0, (s, a) => s ^ a)).Select(i => (byte)i).ToArray();
}
int[] toBinary(byte[] bytes) => bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')).Aggregate((s, a) => s + a).Select(c => c - '0').ToArray();



// construct the grid
var grid = new int[128][];
for (int row = 0; row < 128; row++)
	grid[row] = toBinary(knotHash($"{input}-{row}"));

// part 1
var part1 = grid.Sum(row => row.Count(b => b == 1));
part1.Dump();

// part 2
bool isSet(int x, int y) => x >= 0 && x < 128 && y >= 0 && y < 128 && grid[y][x] == 1;
bool findRegion(out int x, out int y)
{
	for (x = 0; x < 128; x++)
		for (y = 0; y < 128; y++)
			if (grid[y][x] == 1)
				return true;
	y = 0;
	return false;
}
void eatRegion(int x, int y)
{
	if (!isSet(x, y)) return;
	grid[y][x] = 0;
	eatRegion(x - 1, y);
	eatRegion(x + 1, y);
	eatRegion(x, y - 1);
	eatRegion(x, y + 1);
}

var part2 = 0;
while (findRegion(out int x, out int y))
{
	eatRegion(x, y);
	part2++;
}
part2.Dump();
```


### Day 13 - [[Packet Scanners]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day 13 - Packet Scanners.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2017/c%23/Day 13 - Packet Scanners.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
bool isCaught(int layer, int delay) => input.TryGetValue(layer, out int depth) && (layer + delay) % (((depth - 2) * 2) + 2) == 0;

// part 1
var part1 = input.Keys.Where(k => isCaught(k, 0)).Sum(k => k * input[k]);
part1.Dump();

// part 2
var part2 = 0;
while (input.Keys.Any(k => isCaught(k, part2)))
	part2++;	
part2.Dump();
```


### Day 12 - [[Digital Plumber]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day 12 - Digital Plumber.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2017/c%23/Day 12 - Digital Plumber.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var progs = input.Select(i => i.Split(new[] { "<->" }, StringSplitOptions.None)).ToDictionary(i => int.Parse(i[0]), i => i[1].Split(',').Select(o => int.Parse(o)).ToArray());

void findGroup(int prog, List<int> group)
{
	if (group.Contains(prog))
		return;
	group.Add(prog);
	foreach (var p in progs[prog])
		findGroup(p, group);
}

var part1 = new List<int>();
findGroup(0, part1);
part1.Count().Dump();

var part2 = new List<List<int>> { part1 };
foreach (var prog in progs.Keys)
{
	if (part2.Any(p => p.Contains(prog)))
		continue;
	var g = new List<int>();
	findGroup(prog, g);
	part2.Add(g);
}
part2.Count().Dump();
```


### Day 11 - [[Hex Ed]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day 11 - Hex Ed.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2017/c%23/Day 11 - Hex Ed.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
int x = 0, y = 0, z = 0;
int current = 0;
int farthest = 0;

for (int i = 0; i < input.Length; i++)
{
	switch (input[i])
	{
		case "n": y++; z--; break;
		case "nw": y++; x--; break;
		case "ne": x++; z--; break;
		case "s": y--; z++; break;
		case "sw": z++; x--; break;
		case "se": x++; y--; break;
	}
	current = (Math.Abs(x) + Math.Abs(y) + Math.Abs(z)) / 2;
	if (current > farthest)
		farthest = current;
}

current.Dump();
farthest.Dump();
```


### Day 10 - [[Knot Hash]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day 10 - Knot Hash.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2017/c%23/Day 10 - Knot Hash.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
byte[] knotHash(byte[] key, int cycles)
{
	var clist = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();

	var pos = 0;
	var skip = 0;

	for (int c = 0; c < cycles; c++)
	{
		foreach (var i in key)
		{
			for (int s = 0; s < i / 2; s++)
			{
				var a = (pos + s) % clist.Length;
				var b = (pos + i - s - 1) % clist.Length;
				var t = clist[a];
				clist[a] = clist[b];
				clist[b] = t;
			}
			pos += (i + skip);
			pos = pos % clist.Length;

			skip++;
		}
	}
	return clist;
}

var hash = knotHash(input, 1);
var part1 = hash[0] * hash[1];
part1.Dump();

var source = "34,88,2,222,254,93,150,0,199,255,39,32,137,136,1,167";
var sparseHash = knotHash(source.Select(c => (byte)c).Concat(new byte[] { 17, 31, 73, 47, 23 }).ToArray(), 64);
var dense = Enumerable.Range(0, 16).Select(r => sparseHash.Skip(r * 16).Take(16).Aggregate(0, (s, a) => s ^ a)).Select(i => (byte)i).ToArray();
var part2 = BitConverter.ToString(dense).Replace("-", "").ToLower();
part2.Dump();
```


### Day 9 - [[Stream Processing]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day 9 - Stream Processing.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2017/c%23/Day 9 - Stream Processing.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var level = 0;
var garbage = false;
var part1 = 0;
var part2 = 0;

for (int i = 0; i < input.Length; i++)
{
	if (input[i] == '!')
		i++;
	else if (!garbage && input[i] == '{')
		part1 += ++level;
	else if (!garbage && input[i] == '}')
		level -= 1;
	else if (!garbage && input[i] == '<')
		garbage = true;
	else if (garbage && input[i] == '>')
		garbage = false;
	else if (garbage)
		part2++;
}

part1.Dump();
part2.Dump();
```


### Day 8 - [[I Heard You Like Registers]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day 8 - I Heard You Like Registers.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2017/c%23/Day 8 - I Heard You Like Registers.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var instructions = input.Select(s => s.Split(' ')).Select(s => new { 
				Register = s[0], 
				Direction = s[1], 
				Amount = int.Parse(s[2]), 
				CondReg = s[4], 
				CondOp = s[5], 
				CondVal = int.Parse(s[6]) });
var regs = instructions.Select(i => i.Register).Distinct().ToDictionary(i => i, i => 0);

var compareFuncs = new Dictionary<string, Func<int, int, bool>>
{
	{ "==", (a, b) => a == b },
	{ "!=", (a, b) => a != b },
	{ ">", (a, b) => a > b },
	{ "<", (a, b) => a < b },
	{ ">=", (a, b) => a >= b},
	{ "<=", (a, b) => a <= b}
};

var part2 = 0;

foreach (var i in instructions)
{
	if (compareFuncs[i.CondOp](regs[i.CondReg], i.CondVal))
	{
		regs[i.Register] += i.Direction == "inc" ? i.Amount : -i.Amount;
		if (regs[i.Register] > part2) part2 = regs[i.Register];
	}
}

var part1 = regs.Max(r => r.Value);
part1.Dump();
part2.Dump();
```


### Day 7 - [[Recursive Circus]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day 7 - Recursive Circus.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2017/c%23/Day 7 - Recursive Circus.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
void Main()
{
	var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day7.txt"));
	var re = new Regex(@"^([^ ]+) \((\d+)\)( -> (([^, ]+(, )?)+))?");
	var programs = input.Select(i => re.Match(i))
						.ToDictionary(m => m.Groups[1].Value, m => new Program
						{
							Weight = int.Parse(m.Groups[2].Value),
							Holding = m.Groups[4].Value.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToArray()
						});

	// part1
	var part1 = programs.Single(p => !programs.Any(pr => pr.Value.Holding.Contains(p.Key))).Key;
	part1.Dump();

	// part 2
	var unbalanced = programs.Values
			.Where(p => p.Holding.Select(h => programs[h].TotalWeight(programs)).Distinct().Count() > 1)
			.OrderBy(p => p.TotalWeight(programs))
			.First()
			.Holding.Select(h => programs[h]);
	var stacks = unbalanced.GroupBy(u => u.TotalWeight(programs)).OrderBy(u => u.Count()).Select(u => u.First()).ToArray();
	var part2 = stacks[0].Weight + (stacks[1].TotalWeight(programs) - stacks[0].TotalWeight(programs));
	part2.Dump();
}

class Program
{
	public int Weight { get; set; }
	public string[] Holding { get; set; }

	public int TotalWeight(Dictionary<string, Program> programs) => Weight + Holding.Sum(h => programs[h].TotalWeight(programs));
}
```


### Day 6 - [[Memory Reallocation]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day 6 - Memory Reallocation.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2017/c%23/Day 6 - Memory Reallocation.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
// part 1
var allocations = new List<string>();
var stamp = input.Aggregate("", (a,s) => a+','+s);
while(!allocations.Contains(stamp)) {
	allocations.Add(stamp);
	var pos = input.Select((i,p) => new { i, p}).First(i => i.i == input.Max()).p;
	var m = input[pos];
	input[pos] = 0;
	while (m > 0){
		pos++;
		if (pos >= input.Length)
			pos = 0;
		input[pos]++;
		m--;
	}
	stamp = input.Aggregate("", (a,s) => a+','+s);
}
var part1 = allocations.Count();
part1.Dump();

// part 2
var part2 = allocations.Count() - allocations.IndexOf(stamp);
part2.Dump();
```


### Day 5 - [[A Maze of Twisty Trampolines, All Alike]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day 5 - A Maze of Twisty Trampolines, All Alike.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2017/c%23/Day 5 - A Maze of Twisty Trampolines, All Alike.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var jumps = input.ToArray();
var pos = 0;
var part1 = 0;
while (pos < jumps.Length)
{
	part1++;
	var offset = jumps[pos]++;
	pos += offset;
}

jumps = input.ToArray();
pos = 0;
var part2 = 0;
while (pos < jumps.Length)
{
	part2++;
	var offset = jumps[pos];
	if (offset >= 3)
		jumps[pos]--;
	else
		jumps[pos]++;
	pos += offset;
}
```


### Day 4 - [[High-Entropy Passphrases]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day 4 - High-Entropy Passphrases.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2017/c%23/Day 4 - High-Entropy Passphrases.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var part1 = input.Select(i => i.Split().GroupBy(x => x)).Where(o => o.All(oo => oo.Count() == 1)).Count();

var part2 = input.Select(i => i.Split().Select(w => new string(w.OrderBy(c => c).ToArray())).GroupBy(w => w))
				 .Where(o => o.All(w => w.Count() == 1)).Count();
```


### Day 3 - [[Spiral Memory]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day 3 - Spiral Memory.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2017/c%23/Day 3 - Spiral Memory.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

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


### Day 2 - [[Corruption Checksum]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day 2 - Corruption Checksum.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2017/c%23/Day 2 - Corruption Checksum.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

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


### Day 1 - [[Inverse Captcha]](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day 1 - Inverse Captcha.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2017/c%23/Day 1 - Inverse Captcha.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var part1 = input.Zip(input.Skip(1).Concat(input), (a, b) => a == b ? a - '0' : 0).Sum();
var part2 = input.Zip(input.Skip(input.Length / 2).Concat(input), (a, b) => a == b ? a - '0' : 0).Sum();
```

or, without duplicating the input...

```csharp
var part1 = input.Where((c, i) => c == input[(i + 1) % input.Length]).Sum(c => c - '0');
var part2 = input.Where((c, i) => c == input[(i + input.Length/2) % input.Length]).Sum(c => c - '0');
```