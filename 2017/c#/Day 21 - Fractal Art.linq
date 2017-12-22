<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day21.txt"));

var rules = input.Select(i => i.Split(new[] { " => " }, StringSplitOptions.None))
	.Select(i => new { Input = i[0].Split('/'), Output = i[1].Split('/') })
	.SelectMany(r => keys(r.Input).Select(ru => new { pattern = ru, r.Output }))
	.ToDictionary(r => r.pattern, r => r.Output);

string[][] patterns(string[] block) => new[]
{
				block, rotate(block), rotate(rotate(block)), rotate(rotate(rotate(block))), flip(block),
				flip(rotate(block)), flip(rotate(rotate(block))), flip(rotate(rotate(rotate(block))))
			};
string[] keys(string[] block) => patterns(block).Select(c => c.Aggregate((s, a) => s + a)).Distinct().ToArray();

var image = new[] { ".#.", "..#", "###" };

string[] rotate(string[] b)
{
	return b.Length == 2
		? new[] { new string(new[] { b[1][0], b[0][0] }), new string(new[] { b[1][1], b[0][1] }) }
		: new[] { new string(new[] { b[2][0], b[1][0], b[0][0] }), new string(new[] { b[2][1], b[1][1], b[0][1] }), new string(new[] { b[2][2], b[1][2], b[0][2] }) };
}
string[] flip(string[] b) => b.Reverse().ToArray();

IEnumerable<string> getBlock(string[] source, int rw, int c, int s) => Enumerable.Range(0, s).Select(r => source[rw * s + r].Substring(c * s, s));
string[] grow(string[] source)
{
	var size = source.Length;
	var blocksize = size % 2 == 0 ? 2 : 3;
	var blocks = size / blocksize;
	var output = new string[(blocks) * (blocksize + 1)];

	for (var row = 0; row < blocks; row++)
		for (var col = 0; col < blocks; col++)
		{
			var b = rules[getBlock(source,row,col,blocksize).Aggregate((sd, a) => sd + a)];
			for (var i = 0; i < b.Length; i++)
			{
				output[row * b.Length + i] += b[i];
			}
		}
	return output;
}

for (int iterations = 0; iterations < 5; iterations++)
	image = grow(image);

var part1 = image.Sum(i => i.ToCharArray().Count(c => c == '#'));
part1.Dump();

for (int iterations = 5; iterations < 18; iterations++)
	image = grow(image);

var part2 = image.Sum(i => i.ToCharArray().Count(c => c == '#'));
part2.Dump();

