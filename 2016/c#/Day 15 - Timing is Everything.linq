<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var discs = new[] {
		new Disc { Positions = 13, Initial = 10 },
		new Disc { Positions = 17, Initial = 15 },
		new Disc { Positions = 19, Initial = 17 },
		new Disc { Positions = 7, Initial = 1 },
		new Disc { Positions = 5, Initial = 0 },
		new Disc { Positions = 3, Initial = 1 }
	};

	var part1 = FindTime(discs);
	part1.Dump();

	var part2 = FindTime(discs.Concat(new [] { new Disc { Positions = 11, Initial = 0}}).ToArray());
	part2.Dump();
}

int FindTime(Disc[] discs)
{
	var time = 0;
	while (!discs.Select((d, i) => d.SlotOpen(time, i + 1)).All(d => d))
	{
		time++;
	}
	return time;
}

class Disc
{
	public int Positions { get; set; }
	public int Initial { get; set; }
	public bool SlotOpen(int offset, int depth) => (Initial + offset + depth) % Positions == 0;
}