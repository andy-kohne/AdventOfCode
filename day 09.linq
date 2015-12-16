<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var segments = File.ReadAllLines(Path.Combine(Path.GetDirectoryName (Util.CurrentQueryPath),"day9.txt")).Select(s => { var m = Regex.Match(s, @"([^ ]+) to ([^ ]+) = (\d+)"); return new Segment { PointA = m.Groups[1].Value, PointB = m.Groups[2].Value, Distance = int.Parse(m.Groups[3].Value)};  }).ToList();
	
	var possibilities = Permutations( segments.Select(s => s.PointA).Concat(segments.Select(s => s.PointB)).Distinct());
	var routes = possibilities.Select(p => Calculate(p, segments));
	
	routes.Min().Dump();
	routes.Max().Dump();
}

int Calculate(IEnumerable<string> points, List<Segment> segments)
{
	return points.Zip(points.Skip(1), (f,s) => segments.Single(se => (se.PointA == f && se.PointB == s) || (se.PointB == f && se.PointA == s) ).Distance ).Sum();
}

IEnumerable<IEnumerable<T>> Permutations<T>(IEnumerable<T> list, int length = 0)
{
	if (length == 0) length = list.Count();
    if (length == 1) return list.Select(t => new T[] { t });

    return Permutations(list, length - 1)
        .SelectMany(t => list.Where(e => !t.Contains(e)),
            (t1, t2) => t1.Concat(new T[] { t2 }));
}

// Define other methods and classes here
public class Route
{
	public IEnumerable<string> Points { get ;set; }
	public IEnumerable<Segment> Legs { get ;set; }
	public int Distance { get ;set; }
}

public class Segment
{
	public string PointA { get ;set; }
	public string PointB { get ;set; }
	public int Distance { get ;set; }
}