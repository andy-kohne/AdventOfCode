<Query Kind="Statements" />

var input = "315679824".ToCharArray().Select(c => int.Parse($"{c}")).ToArray();

var cups = Enumerable.Range(0,input.Length).ToDictionary(e => input[e], e=> input[(e+1)%input.Length]);
var currentPosition = cups.First().Key;

IEnumerable<int> Play(int moves){	
	for (int move = 0; move < moves; move++)
	{
		var pickedUp = new [] { RemoveAfter(currentPosition), RemoveAfter(currentPosition), RemoveAfter(currentPosition), };
		var dest = currentPosition - 1;
		if (dest < 1) dest = cups.Count;
		while (pickedUp.Contains(dest))
		{
			dest--;
			if (dest < 1) dest = cups.Count;
		}
		InsertAfter(dest, pickedUp[2]);
		InsertAfter(dest, pickedUp[1]);
		InsertAfter(dest, pickedUp[0]);
		currentPosition = cups[currentPosition];
	}
	return GetCups(1);
}

IEnumerable<int> GetCups(int start)
{
	var pos = start;
	for (int i = 0; i < cups.Count; i++)
	{
		yield return pos;
		pos = cups[pos];
	}
}

void InsertAfter(int which, int val){
	var nextCup = cups[which];
	cups[which]=val;
	cups[val] = nextCup;
}
int RemoveAfter(int which){
	var nextCup = cups[which];
	var followingCup = cups[nextCup];
	cups[which] = followingCup;
	return nextCup;
}

var part1 = string.Join("",Play(100).Skip(1));
part1.Dump();

input = input.Concat(Enumerable.Range(input.Length+1, 1000000 - input.Length)).ToArray();
cups = Enumerable.Range(0,input.Length).ToDictionary(e => input[e], e=> input[(e+1)%input.Length]);
currentPosition = cups.First().Key;

var part2 = Play(10000000).Skip(1).Take(2).Select(i => (long)i).Aggregate ((x, y) => x * y);
part2.Dump();
