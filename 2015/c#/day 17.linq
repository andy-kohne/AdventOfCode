<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var containers = 
			File.ReadAllLines(Path.Combine(Path.GetDirectoryName (Util.CurrentQueryPath),"..","day17.txt"))
				.Select (f => int.Parse(f)).OrderByDescending(f => f)
				.ToArray();
	
	var counter = 0;
	var max = 1 << containers.Length;
	var c = new List<int>();

	while (++counter < max)
	{
		var y = GetItemsTotaling(containers, counter, 150);
		if (y > 0)
			c.Add(y);
	}
	
	// part 1
	c.Count().Dump();
	
	// part 2
	c.Where(m => m == c.Min(mm => mm)).Count().Dump();
}

int GetItemsTotaling(int[] source, int bits, int total)
{
	var count = 0;
	var volume = 0;
	for (int i = source.Length - 1; i >=0 ; i--)
		if ((bits & (1 << i)) > 0)
		{
			volume += source[i];
			if (volume > total)
				break;
			count++;
		}
	return volume == total ? count : -1;
}