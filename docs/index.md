---
layout: default
---


### Day 25 - [[Combo Breaker]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 25 - Combo Breaker.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 25 - Combo Breaker.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var cardPk = 9717666;
var doorPk = 20089533;

long Transform(long value, long subjectNumber) => ((value * subjectNumber ) % 20201227);
long TransformLoop(long subjectNumber, long loops) { long value=1; for (long l=0; l<loops; l++) value = Transform(value, subjectNumber); return value; }
long FindLoopCount(long publicKey) {long l = 1, v=1; while ((v = Transform(v, 7)) != publicKey) l++; return l; }

var cardLoops = FindLoopCount(cardPk);
var doorLoops = FindLoopCount(doorPk);

TransformLoop(doorPk, cardLoops).Dump();
TransformLoop(cardPk, doorLoops).Dump();
```


### Day 24 - [[Lobby Layout]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 24 - Lobby Layout.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 24 - Lobby Layout.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
(int,int) Move ((int x,int y) position, string direction)
{
	switch (direction)
	{
		case "w": return ( position.x-1, position.y);
		case "sw": return (position.x-Math.Abs((position.y+1)%2), position.y-1);
		case "ne": return (position.x+Math.Abs(position.y%2), position.y+1);
		case "e": return (position.x+1, position.y);
		case "nw": return (position.x-Math.Abs(((position.y+1)%2)), position.y+1);
		case "se": return (position.x+Math.Abs((position.y)%2), position.y-1);
	}
	return (0,0);
}

(int,int) Follow((int x, int y) pos, string directions)
{
	var ptr = 0;
	while (ptr < directions.Length){
		var l = directions[ptr] == 's' || directions[ptr] == 'n' ? 2 : 1;
		pos = Move(pos,directions.Substring(ptr,l))	;
		ptr+=l;
	}
	return pos;
}

var tiles = new Dictionary<(int,int), bool>();

foreach(var i in input){
	var t = Follow((0,0),i);
	if (tiles.ContainsKey(t)){
		tiles[t] = !tiles[t];
	}
	else{
		tiles[t]=true;
	}
}

var part1=tiles.Count(t => t.Value);
part1.Dump();


IEnumerable<(int, int)> GetAdjacents((int, int) tile)
{
	yield return Follow(tile, "w");
	yield return Follow(tile, "nw");
	yield return Follow(tile, "ne");
	yield return Follow(tile, "e");
	yield return Follow(tile, "se");
	yield return Follow(tile, "sw");
}
int CountAdjacentBlackTiles((int,int) tile) => GetAdjacents(tile).Count(t => tiles.TryGetValue(t, out var state) && state);

for (int day=0; day<100; day++){
	var blackTiles = tiles.Where(t => t.Value == true).Select(t => t.Key).ToList();
	var adjacentWhiteTiles = blackTiles.SelectMany(GetAdjacents).Distinct().Except(blackTiles).ToList();
	tiles = blackTiles.Select(t => new { t, n = CountAdjacentBlackTiles(t) }).Where(t => t.n > 0 && t.n <= 2).Select(t => t.t).Concat(adjacentWhiteTiles.Where(t => CountAdjacentBlackTiles(t) == 2)).ToDictionary(t => t, t=> true);
}

var part2 = tiles.Count;
part2.Dump();
```


### Day 23 - [[Crab Cups]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 23 - Crab Cups.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 23 - Crab Cups.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var cups = Enumerable.Range(0,input.Length).ToDictionary(e => input[e], e=> input[(e+1)%input.Length]);
var currentPosition = cups.First().Key;

IEnumerable<int> Play(int moves){	
	for (int move = 0; move < moves; move++)
	{
		var pickedUp = new [] { RemoveAfter(currentPosition), RemoveAfter(currentPosition), RemoveAfter(currentPosition), };
		var dest = currentPosition - 1;
		if (dest < 1) dest = cups.Count;
		while (pickedUp.Contains(dest))
		{
			dest--;
			if (dest < 1) dest = cups.Count;
		}
		InsertAfter(dest, pickedUp[2]);
		InsertAfter(dest, pickedUp[1]);
		InsertAfter(dest, pickedUp[0]);
		currentPosition = cups[currentPosition];
	}
	return GetCups(1);
}

IEnumerable<int> GetCups(int start)
{
	var pos = start;
	for (int i = 0; i < cups.Count; i++)
	{
		yield return pos;
		pos = cups[pos];
	}
}

void InsertAfter(int which, int val){
	var nextCup = cups[which];
	cups[which]=val;
	cups[val] = nextCup;
}
int RemoveAfter(int which){
	var nextCup = cups[which];
	var followingCup = cups[nextCup];
	cups[which] = followingCup;
	return nextCup;
}

var part1 = string.Join("",Play(100).Skip(1));
part1.Dump();

input = input.Concat(Enumerable.Range(input.Length+1, 1000000 - input.Length)).ToArray();
cups = Enumerable.Range(0,input.Length).ToDictionary(e => input[e], e=> input[(e+1)%input.Length]);
currentPosition = cups.First().Key;

var part2 = Play(10000000).Skip(1).Take(2).Select(i => (long)i).Aggregate ((x, y) => x * y);
part2.Dump();
```


### Day 22 - [[Crab Combat]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 22 - Crab Combat.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 22 - Crab Combat.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
IEnumerable<IEnumerable<T>> Split<T>(IEnumerator<T> e, Func<T, bool> splitFunc)
{
	while (e.MoveNext()) { var ret = new List<T>(); while (!splitFunc(e.Current)) { ret.Add(e.Current); e.MoveNext();} yield return ret; }
}
int ScoreHand(List<int> hand) => hand.Select((c,i)=> (c,i)).OrderByDescending(h => h.i).Select((h,i)=> h.c*(i+1)).Sum();

var hands = Split(input.GetEnumerator(), string.IsNullOrEmpty).Select(i => i.Skip(1).Select(int.Parse).ToList()).ToList();
while (hands.All(h => h.Any()))
{
	var nextCards = hands.Select(h => h.First()).OrderByDescending(h => h).ToArray();
	var winningHand = hands.Single(h => h[0] == nextCards[0]);
	hands.ForEach(h => h.RemoveAt(0));
	winningHand.AddRange(nextCards);
}

var part1 = ScoreHand(hands.Single(h => h.Any()));
part1.Dump();


hands = Split(input.GetEnumerator(), string.IsNullOrEmpty).Select(i => i.Skip(1).Select(int.Parse).ToList()).ToList();
int RecursiveCombat(List<int> p1, List<int> p2)
{
	var infiniteGamePrevention = new HashSet<string>();
	while (p1.Any() && p2.Any())
	{
		var key = string.Join(":", string.Join(",", p1),  string.Join(",", p2));
		if (infiniteGamePrevention.Contains(key)) break;
		infiniteGamePrevention.Add(key);
		
		var p1c = p1[0];
		var p2c = p2[0];
		
		p1.RemoveAt(0);
		p2.RemoveAt(0);
		
		var winner = p1c <= p1.Count && p2c <= p2.Count
			? RecursiveCombat (p1.Select(p => p).Take(p1c).ToList(), p2.Select(p => p).Take(p2c).ToList())
			: p1c > p2c ? 1 : 2;

		if (winner == 1)
		{
			p1.Add(p1c);
			p1.Add(p2c);
		}
		else
		{
			p2.Add(p2c);
			p2.Add(p1c);
		}		
	}
	return p1.Any() ? 1 : 2;
}

var win = RecursiveCombat(hands[0], hands[1]);

var part2 = ScoreHand(win == 1 ? hands[0] : hands[1]);
part2.Dump();
```


### Day 21 - [[Allergen Assessment]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 21 - Allergen Assessment.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 21 - Allergen Assessment.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var re = new Regex(@"(.+) \(contains (.+)\)");

var foods = input.Select(i => re.Match(i)).Select(i => (i.Groups[1].Value.Split(' ').ToArray(), i.Groups[2].Value.Split(new[] { ", "}, StringSplitOptions.None).ToArray())).ToArray();
var allergens = foods.SelectMany(f => f.Item2.Select(a => (a,food:f.Item1))).GroupBy(a=> a.Item1).Select(g => (g.Key, g.SelectMany(i => i.food).Where(f => g.All(o => o.food.Contains(f))).Distinct().ToArray())).ToArray();
var safeIngredients = foods.SelectMany(f => f.Item1).Except(allergens.SelectMany(a => a.Item2)).ToArray();
var part1 = foods.Sum(f => f.Item1.Count(fi => safeIngredients.Contains(fi)));
part1.Dump();

var identified = new Dictionary<string,string>();
while (allergens.Any(a => !identified.ContainsValue(a.Key)))
{
	foreach (var a in allergens.Where(a => !identified.ContainsKey(a.Key))){
		var extra = a.Item2.Except(identified.Keys);
		if (extra.Count() == 1){
			identified[extra.Single()]=a.Key;			
		}
		
	}
}

var part2 = string.Join(",",identified.OrderBy(t => t.Value).Select(t => t.Key));
part2.Dump();
```


### Day 20 - [[Jurassic Jigsaw]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 20 - Jurassic Jigsaw.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 20 - Jurassic Jigsaw.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
void Main()
{
	var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day20.txt")).ToList();
	
	var tiles = ReadTiles(input.GetEnumerator()).ToArray();
	
	var corners = tiles.Where(t => tiles.Where(o => o.Id != t.Id).Count(o => t.CanTouch(o)) == 2).ToArray();
	var part1 = corners.Select(t => t.Id).Aggregate ((x, y) => x * y);
	part1.Dump();

	var size = (int)Math.Sqrt(tiles.Count());
	var layout = new Tile[size,size];
	
	for(int row=0; row<size; row++){
		for(int col=0; col<size; col++){
			if (row == 0 && col == 0)
			{
				var c = corners.First();
				c.Flip();
				if (c.SpinAndFlipUntil(() => tiles.Where(o => o.Id != c.Id).Count(o => o.PossibleEdges.Contains(c.BottomEdge)) == 1 &&
					tiles.Where(o => o.Id != c.Id).Count(o => o.PossibleEdges.Contains(c.RightEdge)) == 1))
					layout[row, col] = c;
			}else if (row ==0) {
				var existing = layout[0,col-1];
				foreach( var candidate in  tiles.Where(t => t.Id != existing.Id).Where(o => o.PossibleEdges.Contains(existing.RightEdge))){
					if (candidate.SpinAndFlipUntil (() => existing.RightEdge == candidate.LeftEdge))
						layout[row,col]=candidate;
						break;
					}				
			}else{
				var existing = layout[row - 1,col];
				foreach (var candidate in tiles.Where(t => t.Id != existing.Id).Where(o => o.PossibleEdges.Contains(existing.BottomEdge)))				
					if (candidate.SpinAndFlipUntil(() => existing.BottomEdge == candidate.TopEdge))
					{
						layout[row, col] = candidate;
						break;
					}				
			}
		}		
	}
	
	//assemble the image
	var w = tiles.First().Bits.GetLength(1) - 2;
	var h = tiles.First().Bits.GetLength(0)-2;	
	var image = new char[layout.GetLength(0)*h,layout.GetLength(1)*w ];
	for(int row=0; row<size; row++){
		for(int col=0; col<size; col++){
			Helpers.Copy2DArray(ref image,col*w,row*h,layout[row,col].Bits,1,1,w,h);
		}		
	}

	var seaMonster = new[] { "                  # ", "#    ##    ##    ###", " #  #  #  #  #  #   "}.Select(o => o.ToCharArray()).ToArray().To2DArray();

	bool FindSeaMonster(int row, int col) {
		if (row+seaMonster.GetLength(0) > image.GetLength(0) || col+seaMonster.GetLength(1) > image.GetLength(1))
			return false;
		for (int r = 0; r<seaMonster.GetLength(0); r++)
		{
			for (int c = 0; c < seaMonster.GetLength(1); c++)
			{
				if (seaMonster[r, c] == '#' && image[row + r, col + c] != '#')
					return false;
			}
		}
		return true;
	}
	
	var monsters = Enumerable.Range(0, image.GetLength(0)).Sum(row => Enumerable.Range(0, image.GetLength(1)).Count(col => FindSeaMonster(row,col)));
	var ctr = 0;
	while (monsters == 0){
		image = image.RotateArray();
		if (++ctr == 4) image= image.FlipArray(); 
		monsters = Enumerable.Range(0, image.GetLength(0)).Sum(row => Enumerable.Range(0, image.GetLength(1)).Count(col => FindSeaMonster(row,col)));
	}
	
	var part2 = image.ToEnumerable().Count(i => i=='#') - (monsters * seaMonster.ToEnumerable().Count(i => i=='#'));
	part2.Dump();
}

IEnumerable<Tile> ReadTiles(IEnumerator<string> e)
{
	while (e.MoveNext())
	{
		var tileid = int.Parse(((string)e.Current).Substring(5, 4));
		var lines = new List<string>();
		e.MoveNext();
		while (!string.IsNullOrEmpty(e.Current))
		{
			lines.Add(e.Current);
			e.MoveNext();
		}
		yield return new Tile(tileid, lines.Select(l => l.ToCharArray().ToArray()).ToArray().To2DArray() );
	}
}

public class Tile
{
	public Tile(long id, char[,] bits)
	{
		Id = id;
		Bits = bits;
	}
	
	public long Id { get; private set; }
	public char[,] Bits { get; private set; }

	private List<string> _possibleEdges;
	public List<string> PossibleEdges
	{
		get
		{
			if (_possibleEdges == null)
			{
				_possibleEdges = new List<string> { TopEdge, BottomEdge, LeftEdge, RightEdge, new string(TopEdge.Reverse().ToArray()), new string(BottomEdge.Reverse().ToArray()), new string(LeftEdge.Reverse().ToArray()), new string(RightEdge.Reverse().ToArray()) };
			}
			return _possibleEdges;
		}
	}
	
	public void Rotate() { Bits = Bits.RotateArray(); }
	public void Flip() { Bits = Bits.FlipArray() ; }

	public string TopEdge => new string(Enumerable.Range(0, Bits.GetLength(1)).Select(col => Bits[0, col]).ToArray());
	public string BottomEdge => new string(Enumerable.Range(0, Bits.GetLength(1)).Select(col => Bits[ Bits.GetLength(0)-1, col]).ToArray());
	public string LeftEdge => new string(Enumerable.Range(0, Bits.GetLength(0)).Select(row => Bits[row, 0]).ToArray());
	public string RightEdge => new string(Enumerable.Range(0, Bits.GetLength(0)).Select(row => Bits[row, Bits.GetLength(1)-1]).ToArray());
	
	public bool CanTouch(Tile other) => PossibleEdges.Any(e => other.PossibleEdges.Contains(e));
	public bool SpinAndFlipUntil(Func<bool> condition)
	{
		for (int opt = 0; opt < 9; opt++)
		{
			if (condition())
				return true;

			Rotate();
			if (opt == 4) Flip();
		}
		return false;
	}
}

public static class Helpers
{
	public static T[,] RotateArray<T>(this T[,] src)
	{
		int oRows = src.GetLength(0);
		int oCols = src.GetLength(1);

		int nRows = oCols;
		int nCols = oRows;
		
		T[,] ret = new T[nRows, nCols];

		int oldRow;
		for (int newRow = 0; newRow < nRows; newRow++)
		{
			oldRow = oRows -1;
			for (int newCol = 0; newCol < nCols; newCol++)			
				ret[newRow, newCol] = src[oldRow--, newRow];	
		}

		return ret;
	}

	public static T[,] FlipArray<T>(this T[,] src)
	{
		int rows = src.GetLength(0);
		int cols = src.GetLength(1);

		T[,] ret = new T[rows, cols];

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				ret[i, j] = src[i, cols - j - 1];
			}
		}

		return ret;
	}

	public static T[,] To2DArray<T>(this IList<IList<T>> src)
	{
		int max = src.Select(l => l).Max(l => l.Count());
		var ret = new T[src.Count, max];

		for (int i = 0; i < src.Count; i++)
		{
			for (int j = 0; j < src[i].Count(); j++)
			{
				ret[i, j] = src[i][j];
			}
		}
		return ret;
	}

	public static void Copy2DArray<T>(ref T[,] dest, int destCol, int destRow, T[,] source, int? sourceCol = null, int? sourceRow = null, int? cols = null, int? rows = null)
	{
		// no error checking...
		var rowsToCopy = rows ?? source.GetLength(0) - sourceRow ?? source.GetLength(0);
		var colsToCopy = cols ?? source.GetLength(1) - sourceCol ?? source.GetLength(1);

		for (int r = 0; r < rowsToCopy; r++)
		{
			for (int c = 0; c < colsToCopy; c++)
			{
				dest[destRow + r, destCol + c] = source[sourceRow.GetValueOrDefault() + r, sourceCol.GetValueOrDefault() + c];

			}
		}
	}
	
	public static IEnumerable<T> ToEnumerable<T>(this T[,] src)
	{
		for (int r=0; r<src.GetLength(0);r++)
		for (int c=0; c<src.GetLength(1);c++)
			yield return src[r,c];
	}
}
```


### Day 19 - [[Monster Messages]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 19 - Monster Messages.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 19 - Monster Messages.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var rules = input.TakeWhile(i => !string.IsNullOrEmpty(i)).Select(i => i.Split(':')).ToDictionary(i => int.Parse(i[0]), i => i[1]);
var messages = input.Skip(rules.Count()).Skip(1).ToList();

var compiled = rules.Where(r => r.Value.Trim().StartsWith("\"")).ToDictionary(r => r.Key, r => new[] { r.Value.Trim().Substring(1, 1) });
rules = rules.Where(r => !compiled.ContainsKey(r.Key)).ToDictionary(r => r.Key, r => r.Value);
while (rules.Any())
{
	foreach (var rule in rules)
	{

		var deps = rule.Value.Split(" |".ToCharArray()).Where(r => !string.IsNullOrEmpty(r)).Select(int.Parse);
		if (deps.All(d => compiled.ContainsKey(d)))
		{
			var options = new List<string>();
			foreach (var segment in rule.Value.Split('|'))
			{
				var dep = segment.Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).Select(d => compiled[d]).ToArray();
				if (dep.Length > 1)
					options.AddRange(from l in dep[0]
									 from r in dep[1]
									 let o =l+r 
									 select o);
				else options.AddRange(dep[0]);
			}
			compiled[rule.Key] = options.ToArray();
		}

	}
	rules = rules.Where(r => !compiled.ContainsKey(r.Key)).ToDictionary(r => r.Key, r => r.Value);
}

var part1 = messages.Count(m => compiled[0].Contains(m));
part1.Dump();


int part2 = 0;
foreach (var message in messages)
{
	// rule 8
	var segments = compiled[42].Where(o => message.IndexOf(o) > -1).ToArray();
	var rule8 = new List<string>(segments);
	string[] possibles = segments;
	while (possibles.Any() && possibles.First().Length < message.Length)
	{
		possibles = (from s1 in segments
					 from s2 in possibles
					 let s = s1 + s2
					 where message.IndexOf(s) > -1
					 select s)
						.ToArray();
		rule8.AddRange(possibles);
	}

	// rule 11	
	var left = compiled[42].Where(o => message.IndexOf(o) > -1).ToArray();
	var right = compiled[31].Where(o => message.IndexOf(o) > -1).ToArray();
	possibles = (from l in left from r in right let o =l+r where message.IndexOf(o) > -1 select o).ToArray();
	var rule11 = new List<string>(possibles);
	while (possibles.Any() && possibles.First().Length < message.Length){
		
		possibles = (from l in left
					 from p in possibles
					 from r in right
					 let s = l + p + r
					 where message.IndexOf(s) > -1
					 select s)
						.ToArray();
					rule11.AddRange(possibles);
	}

	// rule 0: 8 11
	if ((from a in rule8 from b in rule11 let c=a+b where c.Length == message.Length select c).Any(c => message == c))
		part2++;
}
part2.Dump();
```


### Day 18 - [[Operation Order]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 18 - Operation Order.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 18 - Operation Order.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var reGroup = new Regex(@"(\(([^\(\)]+)\))");
var reOp = new Regex(@" *(\d+) *([+*]) *(\d+) *(.*)");
var reOpAdd = new Regex(@" *((\d+) *\+ *(\d+)) *");

string evalLeftToRight(string src) 
{
	var g = reGroup.Match(src);
	while (g.Success) {
		src = src.Substring(0,g.Groups[1].Index) + evalLeftToRight(g.Groups[2].Value)+ src.Substring(g.Groups[1].Index + g.Groups[1].Length);
		g = reGroup.Match(src);
	}
	var op = reOp.Match(src); 
	while(op.Success) {
		var o1 = long.Parse(op.Groups[1].Value);
		var o2 = long.Parse(op.Groups[3].Value);
		var val = op.Groups[2].Value == "+" ? o1 + o2 : o1 * o2;
		src = $"{val}{op.Groups[4].Value}";
		op = reOp.Match(src);
	}
	return src; 
}

var part1 = input.Select(evalLeftToRight).Select(long.Parse).Sum();
part1.Dump();

string evalAdditionFirst(string src)
{
	var g = reGroup.Match(src);
	while (g.Success)
	{
		src = src.Substring(0, g.Groups[1].Index) + evalAdditionFirst(g.Groups[2].Value) + src.Substring(g.Groups[1].Index + g.Groups[1].Length);
		g = reGroup.Match(src);
	}


	var opAdd = reOpAdd.Match(src);
	while (opAdd.Success)
	{
		var o1 = long.Parse(opAdd.Groups[2].Value);
		var o2 = long.Parse(opAdd.Groups[3].Value);
		var val = o1 + o2;
		src = src.Substring(0, opAdd.Groups[1].Index) + val.ToString() + src.Substring(opAdd.Groups[1].Index + opAdd.Groups[1].Length);
		opAdd = reOpAdd.Match(src);
	}

	var op = reOp.Match(src);
	while (op.Success)
	{
		var o1 = long.Parse(op.Groups[1].Value);
		var o2 = long.Parse(op.Groups[3].Value);
		var val = op.Groups[2].Value == "+" ? o1 + o2 : o1 * o2;
		src = $"{val}{op.Groups[4].Value}";
		op = reOp.Match(src);
	}
	return src;
}

var part2 = input.Select(evalAdditionFirst).Select(long.Parse).Sum();
part2.Dump();
```


### Day 17 - [[Conway Cubes]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 17 - Conway Cubes.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 17 - Conway Cubes.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var pocketSpace = input.SelectMany((s, li) => s.Select((c, ci) => ((ci, li, 0), c))).Where(i => i.c =='#').Select(i => i.Item1).ToHashSet();

HashSet<(int, int, int)> Cycle(HashSet<(int x, int y, int z)> src)
{
	int CountNeighbors((int x, int y, int z) p) => src.Count(s => Math.Abs(p.x - s.x) < 2 && Math.Abs(p.y - s.y) < 2 && Math.Abs(p.z - s.z) < 2 );

	var ret = new HashSet<(int, int, int)>();
	for (int dx = src.Select(k => k.x).Min() - 1; dx <= src.Select(k => k.x).Max() + 1; dx++)
		for (int dy = src.Select(k => k.y).Min() - 1; dy <= src.Select(k => k.y).Max() + 1; dy++)
			for (int dz = src.Select(k => k.z).Min() - 1; dz <= src.Select(k => k.z).Max() + 1; dz++)
			{
				var k = (dx, dy, dz);
				var n = CountNeighbors(k);
				if (n == 3 || (src.TryGetValue(k, out var _) && n == 4))
					ret.Add(k);
			}
	return ret;
}

for (int cycle = 0; cycle < 6; cycle++)
{
	pocketSpace = Cycle(pocketSpace);
}
var part1 = pocketSpace.Count();
part1.Dump();


var hyperCubePocketSpace = input.SelectMany((s, li) => s.Select((c, ci) => ((x: ci, y: li, z: 0, w: 0), c))).Where(i => i.c =='#').Select(i => i.Item1).ToHashSet();
HashSet<(int, int, int, int)> Cycle2(HashSet<(int x, int y, int z, int w)> src)
{
	int CountNeighbors((int x, int y, int z, int w) p) => src.Count(s => s.x>=p.x-1 && s.x<=p.x+1 && s.y>=p.y-1 && s.y<=p.y+1 && s.z>=p.z-1 && s.z<=p.z+1 && s.w>=p.w-1 && s.w<=p.w+1);
																
	var wmax = src.Select(k => k.w).Max() + 1;
	var wmin = src.Select(k => k.w).Min() - 1;
	var zmax = src.Select(k => k.z).Max() + 1;
	var zmin = src.Select(k => k.z).Min() - 1;
	var ymax = src.Select(k => k.y).Max() + 1;
	var ymin = src.Select(k => k.y).Min() - 1;
	var xmax = src.Select(k => k.x).Max() + 1;
	var xmin = src.Select(k => k.x).Min() - 1;
	var ret = new HashSet<(int, int, int, int)>();
	for (int dx = xmin; dx <= xmax; dx++)
		for (int dy = ymin; dy <= ymax; dy++)
			for (int dz = zmin; dz <= zmax; dz++)
				for (int dw = wmin; dw <= wmax; dw++)
				{
					var k = (dx, dy, dz, dw);
					var n = CountNeighbors(k);
					if (n == 3 || (src.TryGetValue(k, out var _) && n == 4))
						ret.Add(k);
				}
	return ret;
}
for (int cycle = 0; cycle < 6; cycle++)
{
	hyperCubePocketSpace = Cycle2(hyperCubePocketSpace);
}
var part2 = hyperCubePocketSpace.Count();
part2.Dump();
```


### Day 16 - [[Ticket Translation]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 16 - Ticket Translation.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 16 - Ticket Translation.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var fields = input.TakeWhile(i => !string.IsNullOrEmpty(i)).Select(i => Regex.Match(i, @"(.+): (\d+)-(\d+) or (\d+)-(\d+)")).ToDictionary(m => m.Groups[1].Value, m => new List<(int, int)> { (int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value)), (int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value)), });
var myTicket = input.Skip(input.IndexOf("your ticket:")).Skip(1).Select(i => i.Split(',').Select(int.Parse).ToArray()).First();
var nearbyTickets = input.Skip(input.IndexOf("nearby tickets:")).Skip(1).Select(i => i.Split(',').Select(int.Parse).ToArray()).ToArray();

int scanningErrorRate(int[] ticket) => ticket.Where(t => fields.SelectMany(f => f.Value).All(f => !(t >= f.Item1 && t <= f.Item2))).Sum();
var part1 = nearbyTickets.Sum(scanningErrorRate);
part1.Dump();

var validTickets = nearbyTickets.Where(t => scanningErrorRate(t) == 0);
var maps = fields.Select(f => (f.Key, Enumerable.Range(0, myTicket.Length).Where(slot => validTickets.Select(vt => vt[slot]).All(value => f.Value.Any(range => range.Item1 <= value && range.Item2 >= value))).ToList())).ToDictionary(f => f.Key, f => f.Item2); ;
while (maps.Any(m => m.Value.Count() > 1))
{
	var excluded = maps.Where(m => m.Value.Count() == 1).SelectMany(m => m.Value);
	var keys = maps.Where(m => m.Value.Count() > 1).Select(m => m.Key).ToList();
	foreach (var map in keys)
		maps[map] = maps[map].Except(excluded).ToList();
}
var part2 = maps.Where(m => m.Key.StartsWith("departure")).Select(m => (long)myTicket[m.Value[0]]).Aggregate((x, y) => x * y);
part2.Dump();
```


### Day 15 - [[Rambunctious Recitation]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 15 - Rambunctious Recitation.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 15 - Rambunctious Recitation.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var spoken = new Dictionary<int,(int,int)>();
int lastSpoken = 0, part1 = 0;
for (var turn = 0; turn < 30000000; turn++)
{
	if (turn == 2020)
		part1 = lastSpoken;

	void speak(int n) { if (!spoken.ContainsKey(n)) spoken[n]=(-1,turn); else spoken[n]=(spoken[n].Item2, turn); lastSpoken = n; }

	if (turn < input.Length)
		speak(input[turn]);
	else
		if (spoken.TryGetValue(lastSpoken, out var ns) && ns.Item1 > -1 )
			speak(ns.Item2-ns.Item1);
		else
			speak(0);									
}
var part2 = lastSpoken;

part1.Dump();
part2.Dump();
```


### Day 14 - [[Docking Data]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 14 - Docking Data.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 14 - Docking Data.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var memory = new Dictionary<int,long>();
var mask = string.Empty;

long ApplyMask(long val) => Convert.ToInt64(new string(mask.ToCharArray().Zip(Convert.ToString(val,2).PadLeft(mask.Length,'0').ToCharArray(), (m,v) => m == 'X' ? v : m).ToArray()),2);

foreach (var i in input)
{
	if (i.StartsWith("mask"))
		mask = i.Substring(7);
	else
	{
		var m = Regex.Match(i, @"\[(\d+)\].*?=.*?(\d+)");
		var address = int.Parse(m.Groups[1].Value);
		var val = long.Parse(m.Groups[2].Value);
		memory[address]=ApplyMask(val);
	}
}
var part1 = memory.Sum(m => m.Value);
part1.Dump();


var mem2 = new Dictionary<long, long>();
IEnumerable<long> GetMaskedAddresses(long a) 
{
	var xcount = mask.ToCharArray().Count(o => o =='X');
	var addr = 	Convert.ToString(a,2).PadLeft(mask.Length,'0').ToCharArray();
	for (int i = 0; i < Math.Pow(2,xcount); i++){
		var xbits = new System.Collections.Generic.Queue<char>(Convert.ToString(i,2).PadLeft(xcount,'0').ToCharArray());
		yield return Convert.ToInt64(new string(mask.ToCharArray().Zip(addr, (mb,ab) => mb=='0' ? ab : mb=='1' ? mb : xbits.Dequeue()).ToArray()),2);
	}
}
foreach (var i in input)
{
	if (i.StartsWith("mask"))
		mask = i.Substring(7);
	else
	{
		var m = Regex.Match(i, @"\[(\d+)\].*?=.*?(\d+)");
		var address = long.Parse(m.Groups[1].Value);
		var val = long.Parse(m.Groups[2].Value);
		foreach(var a in GetMaskedAddresses(address))
			mem2[a] = val;
	}
}
var part2 = mem2.Sum(m => m.Value);
part2.Dump();
```


### Day 13 - [[Shuttle Search]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 13 - Shuttle Search.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 13 - Shuttle Search.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var buses = input.Split(',').Select((b,i) => (b,i)).Where(o => o.b != "x").Select(o => (bus:int.Parse(o.b),index:o.i)).ToArray();

var nextDeparture = buses.Select(b => b.bus).Select(b => (b, ((estimate / b) + 1) * b)).OrderBy(b => b.Item2).First();
var part1 = (nextDeparture.Item2-estimate)*nextDeparture.b;
part1.Dump();

long timestamp = buses.First().bus;
long interval = buses.First().bus;
foreach (var b in buses.Skip(1))
{
	while ((timestamp+b.index)%b.bus != 0) timestamp += interval;
	interval *= b.bus;
}
var part2 = timestamp;
part2.Dump();
```


### Day 12 - [[Rain Risk]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 12 - Rain Risk.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 12 - Rain Risk.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var x = 0;
var y = 0;
var h = 90;

foreach (var step in input)
{
	switch (step.Item1)
	{
		case 'S': y -= step.Item2; break;
		case 'N': y += step.Item2; break;
		case 'W': x -= step.Item2; break;
		case 'E': x += step.Item2; break;
		case 'L': h += (360 - step.Item2); h %= 360; break;
		case 'R': h += step.Item2; h %= 360; break;
		case 'F':
			switch (h)
			{
				case 90: x += step.Item2; break;
				case 270: x -= step.Item2; break;
				case 0: y += step.Item2; break;
				case 180: y -= step.Item2; break;
			}
			break;
	}
}

var part1 = Math.Abs(x) + Math.Abs(y);
part1.Dump();


var shipx = 0;
var shipy = 0;
var wpx = 10;
var wpy = 1;

foreach (var step in input)
{
	switch (step.Item1)
	{
		case 'S': wpy -= step.Item2; break;
		case 'N': wpy += step.Item2; break;
		case 'W': wpx -= step.Item2; break;
		case 'E': wpx += step.Item2; break;
		case 'L':
		case 'R':
			var rot = step.Item1 == 'L' ? 360 - step.Item2 : step.Item2;
			int temp;
			switch (rot)
			{
				case 90: temp = wpx; wpx = wpy; wpy = -temp; break;
				case 180: wpx = -wpx; wpy = -wpy; break;
				case 270: temp = wpx; wpx = -wpy; wpy = temp; break;
			}
			break;
		case 'F': shipx += (step.Item2 * wpx); shipy += (step.Item2 * wpy); break;
	}
}

var part2 = Math.Abs(shipx) + Math.Abs(shipy);
part2.Dump();
```


### Day 11 - [[Seating System]](https://github.com/andy-kohne/AdventOfCode/blob/master/2020/c%23/Day 11 - Seating System.linq) <a class="linqpad" href="https://raw.githubusercontent.com/andy-kohne/AdventOfCode/master/2020/c%23/Day 11 - Seating System.linq"  title="Download LinqPad script" download><img src="LINQPad.png" alt=""/></a>

```csharp
var current = input;

bool isValid(int y, int x) => y >= 0 && y < current.Length && x >= 0 && x < input[y].Length;
bool isOccupied(int y, int x) => isValid(y, x) && current[y][x] == '#';
int adjacentOccupied(int y, int x)
{
	var count = 0;
	for (int col = x - 1; col <= x + 1; col++)
		for (int row = y - 1; row <= y + 1; row++)
			if ((x != col || y != row) && isOccupied(row, col))
				count++;
	return count;
}
char[][] stepPart1() => current.Select((r, ri) => r.Select((c, ci) => c == 'L' ? adjacentOccupied(ri, ci) == 0 ? '#' : c : c == '#' ? adjacentOccupied(ri, ci) >= 4 ? 'L' : c : c).ToArray()).ToArray();

var nextStep = stepPart1();
while (!current.Select(p => new string(p)).SequenceEqual(nextStep.Select(s => new string(s))))
{
	current = nextStep;
	nextStep = stepPart1();
}

var part1 = current.Sum(r => r.Count(c => c == '#'));
part1.Dump();


bool visible(int y, int x, int dy, int dx)
{
	while (isValid(y+=dy, x+=dx) && current[y][x] != '.')
		 return isOccupied(y, x);
	return false;
}
int visibleOccupied(int y, int x)
{
	var count = 0;
	if (visible(y, x, 1, 0)) count++;
	if (visible(y, x, 1, 1)) count++;
	if (visible(y, x, 0, 1)) count++;
	if (visible(y, x, -1, 1)) count++;
	if (visible(y, x, -1, 0)) count++;
	if (visible(y, x, -1, -1)) count++;
	if (visible(y, x, 0, -1)) count++;
	if (visible(y, x, 1, -1)) count++;
	return count;
}
char[][] stepPart2() => current.Select((r, ri) => r.Select((c, ci) => c == 'L' ? visibleOccupied(ri, ci) == 0 ? '#' : c : c == '#' ? visibleOccupied(ri, ci) >= 5 ? 'L' : c : c).ToArray()).ToArray();


current = input;
nextStep = stepPart2();
while (!current.Select(p => new string(p)).SequenceEqual(nextStep.Select(s => new string(s))))
{
	current = nextStep;
	nextStep = stepPart2();
}
var part2 = current.Sum(r => r.Count(c => c == '#'));
part2.Dump();
```


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