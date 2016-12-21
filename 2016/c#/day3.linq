<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Namespace>System.Drawing</Namespace>
</Query>

var designdocument =  File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day3.txt"))
							.Select(l => Regex.Replace(l, " +", " ").Trim())
							.Select(l => l.Split(' ').Select(x => int.Parse(x)))
							.ToArray();
var triplets = designdocument.Select(d => d.OrderBy(x => x).ToArray());
var triangles = triplets.Where(t => t[0] + t[1] > t[2]);

Console.WriteLine($"Part 1: {triangles.Count()} out of {triplets.Count()}");


var verticaltriplets = new List<int[]>();
for (var ctr = 0; ctr < designdocument.Count(); ctr += 3)
	for (var ctr2 = 0; ctr2 < 3; ctr2++)
		verticaltriplets.Add(new[] { designdocument[ctr].Skip(ctr2).First(), 
									 designdocument[ctr+1].Skip(ctr2).First(), 
									 designdocument[ctr+2].Skip(ctr2).First()});

var verticaltriangles = verticaltriplets.Select(t => t.OrderBy(x => x).ToArray())
										.Where(t => t[0] + t[1] > t[2]);
Console.WriteLine($"Part 2: {verticaltriangles.Count()} out of {verticaltriplets.Count()}");
