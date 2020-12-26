<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day22.txt")).ToList();

IEnumerable<IEnumerable<T>> Split<T>(IEnumerator<T> e, Func<T, bool> splitFunc)
{
	while (e.MoveNext()) { var ret = new List<T>(); while (!splitFunc(e.Current)) { ret.Add(e.Current); e.MoveNext();} yield return ret; }
}
int ScoreHand(List<int> hand) => hand.Select((c,i)=> (c,i)).OrderByDescending(h => h.i).Select((h,i)=> h.c*(i+1)).Sum();

var hands = Split(input.GetEnumerator(), string.IsNullOrEmpty).Select(i => i.Skip(1).Select(int.Parse).ToList()).ToList();
while (hands.All(h => h.Any()))
{
	var nextCards = hands.Select(h => h.First()).OrderByDescending(h => h).ToArray();
	var winningHand = hands.Single(h => h[0] == nextCards[0]);
	hands.ForEach(h => h.RemoveAt(0));
	winningHand.AddRange(nextCards);
}

var part1 = ScoreHand(hands.Single(h => h.Any()));
part1.Dump();


hands = Split(input.GetEnumerator(), string.IsNullOrEmpty).Select(i => i.Skip(1).Select(int.Parse).ToList()).ToList();
int RecursiveCombat(List<int> p1, List<int> p2)
{
	var infiniteGamePrevention = new HashSet<string>();
	while (p1.Any() && p2.Any())
	{
		var key = string.Join(":", string.Join(",", p1),  string.Join(",", p2));
		if (infiniteGamePrevention.Contains(key)) break;
		infiniteGamePrevention.Add(key);
		
		var p1c = p1[0];
		var p2c = p2[0];
		
		p1.RemoveAt(0);
		p2.RemoveAt(0);
		
		var winner = p1c <= p1.Count && p2c <= p2.Count
			? RecursiveCombat (p1.Select(p => p).Take(p1c).ToList(), p2.Select(p => p).Take(p2c).ToList())
			: p1c > p2c ? 1 : 2;

		if (winner == 1)
		{
			p1.Add(p1c);
			p1.Add(p2c);
		}
		else
		{
			p2.Add(p2c);
			p2.Add(p1c);
		}		
	}
	return p1.Any() ? 1 : 2;
}

var win = RecursiveCombat(hands[0], hands[1]);

var part2 = ScoreHand(win == 1 ? hands[0] : hands[1]);
part2.Dump();
