<Query Kind="Statements" />

var input = 356;

var buffer = new[] { 0};
var pos = 0;

for (int i = 1; i <=2017; i++)
{
	pos = (pos + input ) % buffer.Length;
	Array.Resize(ref buffer, buffer.Length + 1);
	Array.Copy(buffer, pos, buffer, pos +1, buffer.Length -pos -1);
	pos++;
	buffer[pos] = i;
}
var part1 = buffer[pos+1];
part1.Dump();


pos =0;
var part2 = 0;
for (int i = 1; i <= 50_000_000; i++)
{
	pos = (pos + input) % i;
	if (pos == 0) part2 = i;
	pos++;
}
part2.Dump();
