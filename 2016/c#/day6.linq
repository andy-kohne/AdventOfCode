<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day6.txt"));

var decoded = new string(Enumerable.Range(0, 8)
								   .Select(e => input.Select(i => i[e])
								   					 .GroupBy(i => i)
													 .OrderByDescending(i => i.Count())
													 .First()
													 .Key)
								   .ToArray());

Console.WriteLine($"Part 1: {decoded}");


decoded = new string(Enumerable.Range(0, 8)
							   .Select(e => input.Select(i => i[e])
												 .GroupBy(i => i)
												 .OrderBy(i => i.Count())
												 .First()
												 .Key)
							   .ToArray());
								   
Console.WriteLine($"Part 2: {decoded}");
