<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day8.txt"));

int width = 25, height = 6;
var layersize = width * height;

var chunks = Enumerable.Range(0, input.Length / layersize).Select(i => input.Substring(i * layersize, layersize));
var layers = chunks.Select(c => new { Raw = c, Counts = c.ToCharArray().GroupBy(l => l).ToDictionary(g => g.Key, g => g.Count())}).ToList();

var part1 = layers.OrderBy(l => l.Counts['0']).Select(l => l.Counts['1'] * l.Counts['2']).First();
part1.Dump();

var combined = new string(Enumerable.Range(0, layersize).Select(p => layers.First(l => l.Raw[p] != '2').Raw[p]).ToArray());
combined = combined.Replace('0', '_').Replace('1','8');  // help with image readability
var part2 = Enumerable.Range(0,height).Select(i => combined.Substring(i * width, width)).ToArray();
part2.Dump();