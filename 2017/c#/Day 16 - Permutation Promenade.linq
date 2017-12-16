<Query Kind="Statements" />

var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day16.txt")).Split(',');

var dancemoves = input.Select(o => new { Move = o[0], Parms = o.Substring(1).Split('/') })
					  .Select(o => new
					  {
						  Move = o.Move,
						  X = o.Move == 'p' ? 0 : int.Parse(o.Parms[0]),
						  Y = o.Move == 'x' ? int.Parse(o.Parms[1]) : 0,
						  A = o.Parms[0][0],
						  B = o.Parms.Length < 2 ? 'x' : o.Parms[1][0]
					  }).ToArray();

string spin(string progs, int x) => progs.Substring(progs.Length - x) + progs.Substring(0, progs.Length - x);
string exchange(string progs, int x, int y) { var p = progs.ToCharArray(); var temp = p[x]; p[x] = p[y]; p[y] = temp; return new string(p); }
string partner(string progs, char a, char b) => progs.Replace(a, 'x').Replace(b, a).Replace('x', b);
string dance(string progs)
{
	foreach (var i in dancemoves)
	{
		switch (i.Move)
		{
			case 's': progs = spin(progs, i.X); break;
			case 'x': progs = exchange(progs, i.X, i.Y); break;
			case 'p': progs = partner(progs, i.A, i.B); break;
		}
	}
	return progs;
}


var part1 = dance("abcedfghijklmnop");
part1.Dump();

var iterationsTilRepeat = 1;
var part2 = dance("abcedfghijklmnop");
while (part2 != "abcedfghijklmnop")
{
	part2 = dance(part2);
	iterationsTilRepeat++;
}
for (int i = 0; i < 1000000000 % iterationsTilRepeat; i++)
{
	part2 = dance(part2);
}
part2.Dump();
