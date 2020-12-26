<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day14.txt")).ToList();

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