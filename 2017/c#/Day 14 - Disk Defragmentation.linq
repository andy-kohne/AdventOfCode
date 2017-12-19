<Query Kind="Statements" />

var input = "vbqugkhl";

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