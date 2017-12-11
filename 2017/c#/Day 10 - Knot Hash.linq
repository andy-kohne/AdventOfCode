<Query Kind="Statements">
  <Namespace>System.Drawing</Namespace>
</Query>

var input = new byte[] { 34, 88, 2, 222, 254, 93, 150, 0, 199, 255, 39, 32, 137, 136, 1, 167 };

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
