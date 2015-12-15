<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var reindeer = 
			File.ReadAllLines(Path.Combine(Path.GetDirectoryName (Util.CurrentQueryPath),"day14.txt"))
				.Select(s => 
				{
					var m = Regex.Match(s, @"^([^ ]+) .* (\d+) .* (\d+) .* (\d+)" );
					return new Reindeer
					{
						Name = m.Groups[1].Value,
						Rate = int.Parse(m.Groups[2].Value),
						Fly = int.Parse(m.Groups[3].Value),
						Rest = int.Parse(m.Groups[4].Value)
					};
				});

	
	// part 1
	reindeer.Select(r => FlyFor(r, 2503)).OrderByDescending(t => t).First().Dump();
	
	
	// part 2
	var scores = reindeer.ToDictionary (r => r.Name, r => 0);
	for (var t = 1; t < 2503; t++)
	{
		var leaders = reindeer.Select (r => new { r.Name, Distance = FlyFor(r, t) })
							  .GroupBy (r => r.Distance)
							  .OrderByDescending (r => r.Key)
							  .First()
							  .Select(r => r.Name);
		foreach (var l in leaders)
			scores[l]++;
	}
	scores.OrderByDescending (s => s.Value).First().Value.Dump();
}

public class Reindeer
{
	public string Name { get ;set; }
	public int Rate { get ;set; }
	public int Fly { get ;set; }
	public int Rest { get ;set; }
}

public int FlyFor(Reindeer d, int seconds)
{
	var distance = 0;
	var elapsed = 0;
	
	while (elapsed < seconds)
	{
		if (elapsed + d.Fly < seconds)
		{
			elapsed += d.Fly;
			distance += d.Rate * d.Fly;
		}
		else
		{
			distance += d.Rate * (seconds - elapsed);
			elapsed = seconds;
		}
		elapsed += d.Rest;
	}
	
	return distance;
}