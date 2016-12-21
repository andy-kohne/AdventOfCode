<Query Kind="Statements">
  <Output>DataGrids</Output>
</Query>

var strings = File.ReadAllLines(Path.Combine(Path.GetDirectoryName (Util.CurrentQueryPath),"..","day8.txt"));


// part 1
var totalLength = strings.Sum(s => s.Length);
var unescaped =  strings.Select(s => s.Substring(1, s.Length-2))
						.Select(s => Regex.Replace(s, "\\\\\\\\", @"\"))
						.Select(s => Regex.Replace(s, "\\\\\"", "\""))
						.Select(s => Regex.Replace(s, "\\\\[x]([0-9a-f]{2})", "X"));
var memLength = unescaped.Sum(s => s.Length);
(totalLength - memLength).Dump();



// part 2
var escaped = strings.Select(u => Regex.Replace(u, "\\\\", "\\\\"))
					   .Select(u => Regex.Replace(u, "\"","\\\""));
(escaped.Sum(s => s.Length + 2) - totalLength).Dump();