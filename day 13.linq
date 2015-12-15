<Query Kind="Program" />

void Main()
{
	var measurements = File.ReadAllLines(Path.Combine(Path.GetDirectoryName (Util.CurrentQueryPath),"day13.txt")).Select(s => 
	{ 
		var m = Regex.Match(s, @"([^ ]+) would (gain|lose) (\d+) .* ([^ ]+)\."); 
		return new Measurement { Person = m.Groups[1].Value, Adjacent = m.Groups[4].Value, HappinessUnits = int.Parse(m.Groups[3].Value) * (m.Groups[2].Value == "lose" ? -1 : 1) };  
	}).ToList();
	
	
	var possibilities = Permutations(measurements.Select(m => m.Person).Distinct().ToList()).ToList();
	var happiness = possibilities.Select(p => p.ToArray()).Select(p => 
	{
		var score = 0;
		for (var pos = 0; pos < p.Length; pos ++)
		{
			score += measurements.First (m => m.Person == p[pos] && m.Adjacent == p[(pos+1 < p.Length) ? pos + 1 : 0]).HappinessUnits;
			score += measurements.First (m => m.Person == p[pos] && m.Adjacent == p[(pos > 0) ? pos - 1 : p.Length-1]).HappinessUnits;
			
		}
		return score;
	});
	
	happiness.OrderByDescending(s => s).First ().Dump();
}

public class Measurement
{
	public string Person { get; set; }
	public int HappinessUnits { get; set; }
	public string Adjacent { get; set; }
}

IEnumerable<IEnumerable<T>> Permutations<T>(IEnumerable<T> list, int length = 0)
{
	if (length == 0) length = list.Count();
    if (length == 1) return list.Select(t => new T[] { t });

    return Permutations(list, length - 1)
        .SelectMany(t => list.Where(e => !t.Contains(e)),
            (t1, t2) => t1.Concat(new T[] { t2 }));
}
