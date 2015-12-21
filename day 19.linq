<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var input = 
			File.ReadAllLines(Path.Combine(Path.GetDirectoryName (Util.CurrentQueryPath),"day19.txt"));
			
	var replacements = 
			input.Where(i => i.Contains("=>"))
				 .Select(i => i.Split(new[] { " => "},StringSplitOptions.None))
				 .Select(i => new ParticleReplacement { Particle = i[0], Replacement = i[1]  })
				 .ToList();
	
	var molecule = 
			input.Where(i => !i.Contains("=>") && !string.IsNullOrEmpty(i))
				 .FirstOrDefault();
			

	// part 1
	var p = new List<string>();
	foreach (Match m in Regex.Matches(molecule, "("+ string.Join("|", replacements.Select(r => r.Particle).Distinct())+")"))
	{
		var leading = molecule.Substring(0, m.Groups[1].Index);
		var trailing = molecule.Substring(m.Groups[1].Index + m.Groups[1].Length);
		foreach(var s in replacements.Where(r=>r.Particle == m.Groups[1].Value).Select(r=>r.Replacement))
			p.Add(leading + s + trailing);
	}
	p.Distinct().ToList().Count().Dump();
	
	
	// part 2
	var temp = molecule;
	var steps = 0;
	while (temp != "e")
	{
		var w = replacements.OrderByDescending (r => r.Replacement.Length).First(r => temp.Contains(r.Replacement));
		steps += Regex.Matches(temp,w.Replacement).Count;
		temp = Regex.Replace(temp,w.Replacement, w.Particle);
	}	
	steps.Dump();
	
}

public class ParticleReplacement 
{
	public string Particle  { get; set; }
	public string Replacement { get; set; }
}

