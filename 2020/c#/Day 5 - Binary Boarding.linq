<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day5.txt"));

int GetNumber(string id, char lower)
{
	var range = (0, (int)Math.Pow(2, id.Length) - 1);
	for (int i = 0; i < id.Length; i++)
		range = GetHalf(range, id[i] == lower);
	return range.Item1;
}
(int min, int max) GetHalf((int min, int max) r, bool lower) => lower ? (r.min, r.max - (r.max - r.min + 1) / 2) : (r.min + (r.max - r.min + 1) / 2, r.max);
int SeatId(string id) => GetNumber(id.Substring(0,7),'F') * 8 + GetNumber(id.Substring(7,3), 'L');

var part1 = input.Max(i => SeatId(i));
part1.Dump();

var seats = input.Select(SeatId).OrderBy(i => i).ToArray();
var part2 = seats.Zip(seats.Skip(1), (a, b) => b == a + 2 ? a + 1 : 0).Max();
part2.Dump();