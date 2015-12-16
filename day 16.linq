<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var auntSues = File.ReadAllLines(Path.Combine(Path.GetDirectoryName (Util.CurrentQueryPath),"day16.txt")).Select (f => ParseSue(f));

	// part 1
	auntSues.Where(a => a.Children == null || a.Children == 3)
			.Where(a => a.Cats == null || a.Cats == 7)
			.Where(a => a.Samoyeds == null || a.Samoyeds == 2)
			.Where(a => a.Pomeranians == null || a.Pomeranians == 3)
			.Where(a => a.Akitas == null || a.Akitas == 0)
			.Where(a => a.Vizslas == null || a.Vizslas == 0)
			.Where(a => a.Goldfish == null || a.Goldfish == 5)
			.Where(a => a.Trees == null || a.Trees == 3)
			.Where(a => a.Cars == null || a.Cars == 2)
			.Where(a => a.Perfumes == null || a.Perfumes == 1)
			.Single().Id.Dump();
	
	// part 2
	auntSues.Where(a => a.Children == null || a.Children == 3)
			.Where(a => a.Cats == null || a.Cats > 7)
			.Where(a => a.Samoyeds == null || a.Samoyeds == 2)
			.Where(a => a.Pomeranians == null || a.Pomeranians < 3)
			.Where(a => a.Akitas == null || a.Akitas == 0)
			.Where(a => a.Vizslas == null || a.Vizslas == 0)
			.Where(a => a.Goldfish == null || a.Goldfish < 5)
			.Where(a => a.Trees == null || a.Trees > 3)
			.Where(a => a.Cars == null || a.Cars == 2)
			.Where(a => a.Perfumes == null || a.Perfumes == 1)
			.Single().Id.Dump();
}

public AuntSue ParseSue(string input)
{
	var m = Regex.Match(input, @"^Sue (\d+)\: (([a-z]+)\: (\d+)(, )?)+$");
	var sue = new AuntSue { Id = int.Parse(m.Groups[1].Value) };
	
	for( var i = 0; i < m.Groups[2].Captures.Count; i++)
	{
		switch(m.Groups[3].Captures[i].Value)
		{
			case "children": sue.Children=int.Parse(m.Groups[4].Captures[i].Value); break;
			case "cats": sue.Cats=int.Parse(m.Groups[4].Captures[i].Value); break;
			case "samoyeds": sue.Samoyeds=int.Parse(m.Groups[4].Captures[i].Value); break;
			case "pomeranians": sue.Pomeranians=int.Parse(m.Groups[4].Captures[i].Value); break;
			case "akitas": sue.Akitas=int.Parse(m.Groups[4].Captures[i].Value); break;
			case "vizslas": sue.Vizslas=int.Parse(m.Groups[4].Captures[i].Value); break;
			case "goldfish": sue.Goldfish=int.Parse(m.Groups[4].Captures[i].Value); break;
			case "trees": sue.Trees=int.Parse(m.Groups[4].Captures[i].Value); break;
			case "cars": sue.Cars=int.Parse(m.Groups[4].Captures[i].Value); break;
			case "perfumes": sue.Perfumes=int.Parse(m.Groups[4].Captures[i].Value); break;
		}
	}
	return sue;
}

public class AuntSue
{
	public int Id { get; set; }
	public int? Children { get ;set; }
	public int? Cats { get ;set; }
	public int? Samoyeds { get; set; }
	public int? Pomeranians { get ;set; }
	public int? Akitas { get ;set; }
	public int? Vizslas { get ;set; }
	public int? Goldfish { get ;set; }
	public int? Trees { get ;set; }
	public int? Cars { get; set; }
	public int? Perfumes { get; set; }
}