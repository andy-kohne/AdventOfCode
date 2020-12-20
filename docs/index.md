---
layout: default
---


### Day 10 - [[Adapter Array]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 10 - Adapter Array.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 10 - Adapter Array.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var jolts = new[] { 0, input.Max()+3 }.Concat(input).OrderBy(o => o).ToArray();

var diffs = jolts.Zip(jolts.Skip(1), (a,b) =>  b-a).GroupBy(j => j).ToDictionary(g => g.Key, g => g.Count());
var part1 = diffs[1] * diffs[3];
part1.Dump();

var connections = new Dictionary<int, long> { { 0, 1 }};
foreach (var jolt in jolts.Skip(1))
{
	connections[jolt] = (connections.TryGetValue(jolt - 1, out var one) ? one : 0) +
						(connections.TryGetValue(jolt - 2, out var two) ? two : 0) +
						(connections.TryGetValue(jolt - 3, out var three) ? three : 0);
}
var part2 = connections[jolts.Max()];
part2.Dump();
```


### Day 9 - [[Encoding Error]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 9 - Encoding Error.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 9 - Encoding Error.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
IEnumerable<(T, T)> Pairs<T>(IList<T> items)
{
	for (var outer = 0; outer < items.Count() - 1; outer++)
		for (var inner = outer + 1; inner < items.Count(); inner++)
			if (!items[outer].Equals(items[inner]))
				yield return (items[outer], items[inner]);
}

bool isValid(int pos) => Pairs(input.Skip(pos-25).Take(25).ToList()).Any(p => p.Item1 + p.Item2 == input[pos]);
var part1 = Enumerable.Range(25, input.Length-25).Where(i => !isValid(i)).Select(i => input[i]).First();

long[] findWeakness(int pos){
	var numbers = new List<long>();
	while (numbers.Sum() < part1)
		numbers.Add(input[pos+numbers.Count()]);
	return numbers.Sum() == part1 ? numbers.ToArray() : null;
}

var w = Enumerable.Range(0, input.Length).Select(findWeakness).First(o => o != null);
var part2 = w.Max() + w.Min();
part2.Dump();
```


### Day 8 - [[Handheld Halting]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 8 - Handheld Halting.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 8 - Handheld Halting.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
(bool, int) Run(List<List<string>> code)
{
	var executed = new Dictionary<int, int>();

	var ptr = 0;
	var acc = 0;
	while (ptr < code.Count)
	{
		if (executed.ContainsKey(ptr))
			return (false, acc);
		
		executed[ptr] = 1;
		var cmd = code[ptr];
		switch (cmd[0])
		{
			case "acc":
				acc += int.Parse(cmd[1]);
				ptr++;
				break;
			case "nop":
				ptr++;
				break;
			case "jmp":
				ptr += int.Parse(cmd[1]);
				break;
		}
	}
	return (true, acc);
}

(_, var part1) = Run(input);
part1.Dump();

for (int i = 0; i < input.Count(); i++){
	var altered = input.Select(o => o.Select(ii => ii).ToList()).ToList();
	if (altered[i][0] == "nop"){
		altered[i][0] = "jmp";
	}
	else if (altered[i][0] == "jmp"){
		altered[i][0] = "nop";
	}
	else{
		continue;
	}
	(var result, var part2) = Run(altered);
	if (result == true){
		part2.Dump();
	}
}
```


### Day 7 - [[Handy Haversacks]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 7 - Handy Haversacks.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 7 - Handy Haversacks.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var rules = input.Select(i => Regex.Match(i, @"^(.+) bags contain (no other bags|(([ ]*\d ([^,]+) bag[^,]*[,]?)*)).$")).ToDictionary(m => m.Groups[1].Value, m => m.Groups[3].Value.Split(',').Where(i => !string.IsNullOrEmpty(i)).Select(i => Regex.Match(i, @"(\d+) (.+) bag")).Select(i => (count: int.Parse(i.Groups[1].Value),bag: i.Groups[2].Value)).ToArray());

bool CanContain(string bag, string inner) =>  rules[bag].Any(i => i.bag == inner || CanContain(i.bag, inner));
var part1 = rules.Count(r => CanContain(r.Key, "shiny gold"));
part1.Dump();


int Contains(string bag) => rules[bag].Sum(v => v.count + (v.count * Contains(v.bag)));
var part2 = Contains("shiny gold");
part2.Dump();
```


### Day 6 - [[Custom Customs]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 6 - Custom Customs.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 6 - Custom Customs.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
IEnumerable<List<string>> GetGroups(string[] raw)
{
	var e = raw.GetEnumerator();
	var s = new List<string>();
	while (e.MoveNext())
	{
		var line = (string)e.Current;
		if (string.IsNullOrEmpty(line))
		{
			yield return s;
			s = new List<string>();
			continue;
		}
		s.Add(line);
	}
	yield return s;
}

var groups = GetGroups(input).ToList();

var part1 = groups.Sum(g => g.Aggregate((x, y) => x + y).ToCharArray().Distinct().Count());
part1.Dump();

var part2 = groups.Sum(g => g.SelectMany(a => a.ToCharArray()).GroupBy(a => a).Count(ga => ga.Count() == g.Count()));
part2.Dump();
```


### Day 5 - [[Binary Boarding]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 5 - Binary Boarding.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 5 - Binary Boarding.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
int GetNumber(string id, char lower)
{
	var range = (0, (int)Math.Pow(2, id.Length) - 1);
	for (int i = 0; i < id.Length; i++)
		range = GetHalf(range, id[i] == lower);
	return range.Item1;
}
(int min, int max) GetHalf((int min, int max) r, bool lower) => lower ? (r.min, r.max - (r.max - r.min + 1) / 2) : (r.min + (r.max - r.min + 1) / 2, r.max);
int SeatId(string id) => GetNumber(id.Substring(0,7),'F') * 8 + GetNumber(id.Substring(7,3), 'L');

var part1 = input.Max(i => SeatId(i));
part1.Dump();

var seats = input.Select(SeatId).OrderBy(i => i).ToArray();
var part2 = seats.Zip(seats.Skip(1), (a, b) => b == a + 2 ? a + 1 : 0).Max();
part2.Dump();
```


### Day 4 - [[Passport Processing]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 4 - Passport Processing.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 4 - Passport Processing.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
IEnumerable<Dictionary<string, string>> Parse(string[] raw) {
	var e = raw.GetEnumerator();
	var dict = new Dictionary<string,string>();
	while(e.MoveNext()){
		var line = (string)e.Current;
		if (string.IsNullOrEmpty(line))
		{
			yield return dict;
			dict = new Dictionary<string, string>();
			continue;
		}
		var items = line.Split(' ');
		foreach (var item in items)
		{
			var parts = item.Split(':');
			dict.Add(parts[0], parts[1]);
		}
	}
	yield return dict;
}
var passports = Parse(input);


var required = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
bool isValid(Dictionary<string,string> passport) => required.All(r => passport.ContainsKey(r));
var part1 = passports.Count(isValid);
part1.Dump();


var fields = new Dictionary<string, Func<string, bool>> {
	{ "byr", i => { var n = int.Parse(i); return n >= 1920 && n <= 2002; }},
	{ "iyr", i => { var n = int.Parse(i); return n >= 2010 && n <= 2020; }},
	{ "eyr", i => { var n = int.Parse(i); return n >= 2020 && n <= 2030; }},
	{ "hgt", i => { var m = Regex.Match(i, @"^(\d+)(cm|in)$"); return m.Success && int.TryParse(m.Groups[1].Value, out int n) && (m.Groups[2].Value == "cm" ? n >= 150 && n <= 193 : n>=59 && n <=76 ); }},
	{ "hcl", i => { return Regex.IsMatch(i, @"^#[0-9a-f]{6}$"); }},
	{ "ecl", i => { return i == "amb" || i == "blu" || i == "brn" || i == "gry" || i == "grn" || i == "hzl" || i == "oth"; }},
	{ "pid", i => { return Regex.IsMatch(i, @"^\d{9}$"); }},
};
bool isValidPartTwo (Dictionary<string,string> passport) => fields.All(f => passport.ContainsKey(f.Key) && f.Value(passport[f.Key]));
var part2 = passports.Count(isValidPartTwo);
part2.Dump();
```


### Day 3 - [[Toboggan Trajectory]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 3 - Toboggan Trajectory.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 3 - Toboggan Trajectory.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
bool isTree(int row, int col) => input[row][col % input[row].Length] == '#';

var part1 = Enumerable.Range(0, input.Length).Count(i => isTree(i, i*3));
part1.Dump();

var part2 = (long)Enumerable.Range(0, input.Length).Count(i => isTree(i, i))
		  * Enumerable.Range(0, input.Length).Count(i => isTree(i, i * 3))
		  * Enumerable.Range(0, input.Length).Count(i => isTree(i, i * 5))
		  * Enumerable.Range(0, input.Length).Count(i => isTree(i, i * 7))
		  * Enumerable.Range(0, input.Length / 2).Count(i => isTree(i * 2, i));
part2.Dump();
```


### Day 2 - [[Password Philosophy]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 2 - Password Philosophy.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 2 - Password Philosophy.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var re = new Regex(@"^(\d+)-(\d+) (.): (.+)$");
var passwords = input.Select(i => re.Match(i)).Select(i => (min: int.Parse(i.Groups[1].Value) , max: int.Parse(i.Groups[2].Value), c: i.Groups[3].Value.Single(), pw: i.Groups[4].Value) ).ToList();

var part1 = passwords.Select(item => (item.min, item.max, count: item.pw.Count(p => p == item.c))).Count(item => item.count >= item.min && item.count <= item.max);
part1.Dump();

var part2 = passwords.Count(item => item.pw[item.min-1]==item.c ^ item.pw[item.max-1]==item.c);
part2.Dump();
```


### Day 1 - [[Report Repair]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 1 - Report Repair.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 1 - Report Repair.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
IEnumerable<(T, T)> Pairs<T>(IList<T> items) {
	for (var outer = 0; outer < input.Count() -1 ; outer++)
		for (var inner = outer + 1; inner < input.Count(); inner++)
			yield return (items[outer], items[inner]);
}

var pair = Pairs(input).First(p => p.Item1 + p.Item2 == 2020);
var part1 = pair.Item1 * pair.Item2;
part1.Dump();


IEnumerable<(T, T, T)> Triples<T>(IList<T> items)
{
	for (var x = 0; x < input.Count() - 2; x++)
		for (var y = x + 1; y < input.Count() - 1; y++)
			for (var z = y + 1; z < input.Count(); z++)
				yield return (items[x], items[y], items[z]);
}

var triple = Triples(input).First(p => p.Item1 + p.Item2 + p.Item3 == 2020);
var part2 = triple.Item1 * triple.Item2 * triple.Item3;
part2.Dump();
```