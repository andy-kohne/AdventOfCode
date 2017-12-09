<Query Kind="Statements">
  <Namespace>System.Drawing</Namespace>
</Query>

var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day9.txt"));

var level = 0;
var garbage = false;
var part1 = 0;
var part2 = 0;

for (int i = 0; i < input.Length; i++)
{
	if (input[i] == '!')
		i++;
	else if (!garbage && input[i] == '{')
		part1 += ++level;
	else if (!garbage && input[i] == '}')
		level -= 1;
	else if (!garbage && input[i] == '<')
		garbage = true;
	else if (garbage && input[i] == '>')
		garbage = false;
	else if (garbage)
		part2++;
}

part1.Dump();
part2.Dump();