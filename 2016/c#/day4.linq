<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Namespace>System.Drawing</Namespace>
</Query>

var re = new Regex(@"^(?<name>.+)-(?<sector>\d+)\[(?<checksum>[a-z]+)\]$");
var listofrooms = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day4.txt"))
							.Select(l => 
							{
								var p = re.Match(l);
								return new  
								{
									Name = p.Groups["name"].Value,
									SectorId = int.Parse(p.Groups["sector"].Value),
									Checksum = p.Groups["checksum"].Value
								};
							});

var validrooms = listofrooms.Where(r => r.Checksum == new string(r.Name.ToCharArray()
			.Where(c => c >= 'a' && c <= 'z')
			.GroupBy(c => c)
			.OrderByDescending(c => c.Count())
			.ThenBy(c => c.Key)
			.Take(5)
			.Select(c => c.Key)
			.ToArray()));

Console.WriteLine($"Part 1: {validrooms.Sum(r => r.SectorId)}");


Func<string, int, string> decrypt = (name, sector) => 
{
	var shift = sector % 26;
	var chars = name.ToCharArray();
	for (var ctr = 0; ctr < chars.Length; ctr++)
	{
		if (chars[ctr] == '-')
			chars[ctr] = ' ';
		else
		{
			var nc = (int)chars[ctr] + shift;
			if (nc > (int)'z') nc -= 26;
			chars[ctr] = (char)nc;
		}
	}
	return new string(chars);
};

var decrypted = validrooms.Select(r => new { Name = decrypt(r.Name, r.SectorId), Sector = r.SectorId });
var northpole = decrypted.Where(d => d.Name.Contains("north")).Single();
Console.WriteLine($"Part 2: {northpole.Name}, {northpole.Sector}");
