<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day11.txt")).Select(o => o.ToCharArray()).ToArray();
var current = input;

bool isValid(int y, int x) => y >= 0 && y < current.Length && x >= 0 && x < input[y].Length;
bool isOccupied(int y, int x) => isValid(y, x) && current[y][x] == '#';
int adjacentOccupied(int y, int x)
{
	var count = 0;
	for (int col = x - 1; col <= x + 1; col++)
		for (int row = y - 1; row <= y + 1; row++)
			if ((x != col || y != row) && isOccupied(row, col))
				count++;
	return count;
}
char[][] stepPart1() => current.Select((r, ri) => r.Select((c, ci) => c == 'L' ? adjacentOccupied(ri, ci) == 0 ? '#' : c : c == '#' ? adjacentOccupied(ri, ci) >= 4 ? 'L' : c : c).ToArray()).ToArray();

var nextStep = stepPart1();
while (!current.Select(p => new string(p)).SequenceEqual(nextStep.Select(s => new string(s))))
{
	current = nextStep;
	nextStep = stepPart1();
}

var part1 = current.Sum(r => r.Count(c => c == '#'));
part1.Dump();


bool visible(int y, int x, int dy, int dx)
{
	while (isValid(y+=dy, x+=dx) && current[y][x] != '.')
		 return isOccupied(y, x);
	return false;
}
int visibleOccupied(int y, int x)
{
	var count = 0;
	if (visible(y, x, 1, 0)) count++;
	if (visible(y, x, 1, 1)) count++;
	if (visible(y, x, 0, 1)) count++;
	if (visible(y, x, -1, 1)) count++;
	if (visible(y, x, -1, 0)) count++;
	if (visible(y, x, -1, -1)) count++;
	if (visible(y, x, 0, -1)) count++;
	if (visible(y, x, 1, -1)) count++;
	return count;
}
char[][] stepPart2() => current.Select((r, ri) => r.Select((c, ci) => c == 'L' ? visibleOccupied(ri, ci) == 0 ? '#' : c : c == '#' ? visibleOccupied(ri, ci) >= 5 ? 'L' : c : c).ToArray()).ToArray();


current = input;
nextStep = stepPart2();
while (!current.Select(p => new string(p)).SequenceEqual(nextStep.Select(s => new string(s))))
{
	current = nextStep;
	nextStep = stepPart2();
}
var part2 = current.Sum(r => r.Count(c => c == '#'));
part2.Dump();
