<Query Kind="Program">
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

void Main()
{
	var input =
		File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day24.txt"))
		.Select(int.Parse)
		.ToList();

	var part1 = GetPackageConfiguration(input, 3);
	part1.Dump();

	var part2 = GetPackageConfiguration(input, 4);
	part2.Dump();

}

public long CalculateQuantumeEntanglement(IEnumerable<int> packages) => packages.Aggregate(1L, (x, y) => x * y);

public long GetPackageConfiguration(List<int> packages, int groups)
{
	var groupWeight = packages.Sum() / groups;
	for (var packagesInGroup = 0; packagesInGroup < packages.Count; packagesInGroup++)
	{
		var possibleGroups = FindPackageGroups(packages, packagesInGroup, groupWeight);
		if (possibleGroups.Any())
			return possibleGroups.Min(CalculateQuantumeEntanglement);
	}
	return default;
}

public IEnumerable<IImmutableList<int>> FindPackageGroups(List<int> packages, int packagesToInclude, int remainingWeight)
{
	if (remainingWeight == 0)
	{
		yield return ImmutableList.Create<int>();
		yield break;
	}

	if (packagesToInclude < 0 || remainingWeight < 0 || packages.Count == 0)
		yield break;


	if (packages[0] <= remainingWeight)
		foreach (var group in FindPackageGroups(packages.Skip(1).ToList(), packagesToInclude - 1, remainingWeight - packages[0]))
		{
			yield return group.Add(packages[0]);
		}

	foreach (var group in FindPackageGroups(packages.Skip(1).ToList(), packagesToInclude, remainingWeight))
	{
		yield return group;
	}
}