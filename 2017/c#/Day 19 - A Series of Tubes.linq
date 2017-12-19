<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day19.txt"));

var x = input[0].IndexOf("|");
var y = 0;
var lastx = x;
var lasty = -1;

var part1 = "";
var part2 = 0;

while (input[y][x] != ' ')
{
	var dx = x - lastx;
	var dy = y - lasty;

	lastx = x;
	lasty = y;

	if (input[y][x] == '+')
	{
		if (dx == 0)
			x += input[y][x - 1] == ' ' ? 1 : -1;
		else
			y += input[y - 1][x] == ' ' ? 1 : -1;
	}
	else
	{
		if (input[y][x] != '-' && input[y][x] != '|')
			part1 += input[y][x];
		x += dx;
		y += dy;
	}
	part2++;
}

part1.Dump();
part2.Dump();