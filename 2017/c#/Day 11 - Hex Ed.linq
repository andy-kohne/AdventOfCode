<Query Kind="Statements">
  <Namespace>System.Drawing</Namespace>
</Query>

var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day11.txt")).Split(',');

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
