<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day18.txt")).ToList();

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
