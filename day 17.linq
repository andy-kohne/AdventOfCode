<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var containers = 
			File.ReadAllLines(Path.Combine(Path.GetDirectoryName (Util.CurrentQueryPath),"day17.txt")).Select (f => int.Parse(f)).ToArray();

	var counter = 0u;
	var max = Math.Pow(2, containers.Length);
	var matches = new List<IEnumerable<int>>();

	while (++counter < max)
	{
		var x = GetItems(containers, counter);
		if (x.Sum() == 150)
			matches.Add(x);
	}
	
	// part 1
	matches.Count().Dump();
	
	// part 2
	matches.Where(m => m.Count() == matches.Min(mm => mm.Count())).Count().Dump();
}

IEnumerable<int> GetItems(int[] source, uint bits)
{
	var l = new List<int>();
	for (int i =0; i < source.Length; i++)
		if ((bits & (uint)Math.Pow(2,i)) > 0)
			l.Add(source[i]);
	return l;
}
