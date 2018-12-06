<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Namespace>System.Configuration</Namespace>
</Query>

void Main()
{
	var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day3.txt"));

	var re = new Regex(@"(\D+(\d+)){5}");
	var numbers = input.Select(i => re.Match(i).Groups[2].Captures.Cast<Capture>().Select(c => int.Parse(c.Value)).ToArray());
	var squares = numbers.Select(n => new Square(n)).ToArray();

	var part1 = Enumerable.Range(1, 1000).Sum(x => Enumerable.Range(1, 1000).Count(y => squares.Count(s => s.Contains(x, y)) > 1));

//	var part1 = Enumerable.Range(0, 1000).Select(e => new { x = e, sq = squares.Where(s => e > s.Left && e <= s.Right).ToArray() })
//		.Sum(e => Enumerable.Range(0, 1000).Count(y => e.sq.Count(s => s.Contains(e.x, y)) > 1)).Dump();

	var part2 = squares.Single(s => squares.Count(o => o.Overlaps(s)) == 1).Id;
	part2.Dump();

//	int part1 = 0;
//	for (int x = 0; x < 1000; x++)
//	{
//		var sq = squares.Where(o => x > o.Left && x <= o.Right).ToArray();
//		for (int y = 0; y < 1000; y++)
//		{
//			int count = 0;
//			foreach (var s in sq)
//			{
//				if (s.Contains(x, y))
//				{
//					count++;
//					if (count > 1)
//					{
//						part1++;
//						break;
//					}
//
//				}
//			}
//		}
//	}
	part1.Dump();

}

public class Square
{
	public Square(params int[] values)
	{
		Id = values[0];
		Left = values[1];
		Top = values[2];
		Bottom = Top + values[4];
		Right = Left + values[3];
	}
	public int Id { get; set; }
	public int Left { get; set; }
	public int Top { get; set; }
	public int Bottom { get; set; }
	public int Right { get; set; }
	public bool Contains(int x, int y) => x > Left && x <= Right && y > Top && y <= Bottom;
	public bool Overlaps(Square other) =>
			(
			(Left >= other.Left && Left <= other.Right) ||
			(other.Left >= Left && other.Left <= Right) ||
			(Right >= other.Left && Right <= other.Right) ||
			(other.Right >= Left && other.Right <= Right)
			) &&
			(
			(Top >= other.Top && Top <= other.Bottom) ||
			(other.Top >= Top && other.Top <= Bottom) ||
			(Bottom >= other.Top && Bottom <= other.Bottom) ||
			(other.Bottom >= Top && other.Bottom <= Bottom)
			);
}

// Define other methods and classes here
