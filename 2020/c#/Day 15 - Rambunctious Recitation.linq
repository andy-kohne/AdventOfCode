<Query Kind="Statements" />

var input = "19,20,14,0,9,1".Split(',').Select(int.Parse).ToArray();

var spoken = new Dictionary<int,(int,int)>();
int lastSpoken = 0, part1 = 0;
for (var turn = 0; turn < 30000000; turn++)
{
	if (turn == 2020)
		part1 = lastSpoken;

	void speak(int n) { if (!spoken.ContainsKey(n)) spoken[n]=(-1,turn); else spoken[n]=(spoken[n].Item2, turn); lastSpoken = n; }

	if (turn < input.Length)
		speak(input[turn]);
	else
		if (spoken.TryGetValue(lastSpoken, out var ns) && ns.Item1 > -1 )
			speak(ns.Item2-ns.Item1);
		else
			speak(0);									
}
var part2 = lastSpoken;

part1.Dump();
part2.Dump();
