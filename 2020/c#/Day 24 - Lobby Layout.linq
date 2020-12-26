<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day24.txt")).ToList();

(int,int) Move ((int x,int y) position, string direction)
{
	switch (direction)
	{
		case "w": return ( position.x-1, position.y);
		case "sw": return (position.x-Math.Abs((position.y+1)%2), position.y-1);
		case "ne": return (position.x+Math.Abs(position.y%2), position.y+1);
		case "e": return (position.x+1, position.y);
		case "nw": return (position.x-Math.Abs(((position.y+1)%2)), position.y+1);
		case "se": return (position.x+Math.Abs((position.y)%2), position.y-1);
	}
	return (0,0);
}

(int,int) Follow((int x, int y) pos, string directions)
{
	var ptr = 0;
	while (ptr < directions.Length){
		var l = directions[ptr] == 's' || directions[ptr] == 'n' ? 2 : 1;
		pos = Move(pos,directions.Substring(ptr,l))	;
		ptr+=l;
	}
	return pos;
}

var tiles = new Dictionary<(int,int), bool>();

foreach(var i in input){
	var t = Follow((0,0),i);
	if (tiles.ContainsKey(t)){
		tiles[t] = !tiles[t];
	}
	else{
		tiles[t]=true;
	}
}

var part1=tiles.Count(t => t.Value);
part1.Dump();


IEnumerable<(int, int)> GetAdjacents((int, int) tile)
{
	yield return Follow(tile, "w");
	yield return Follow(tile, "nw");
	yield return Follow(tile, "ne");
	yield return Follow(tile, "e");
	yield return Follow(tile, "se");
	yield return Follow(tile, "sw");
}
int CountAdjacentBlackTiles((int,int) tile) => GetAdjacents(tile).Count(t => tiles.TryGetValue(t, out var state) && state);

for (int day=0; day<100; day++){
	var blackTiles = tiles.Where(t => t.Value == true).Select(t => t.Key).ToList();
	var adjacentWhiteTiles = blackTiles.SelectMany(GetAdjacents).Distinct().Except(blackTiles).ToList();
	tiles = blackTiles.Select(t => new { t, n = CountAdjacentBlackTiles(t) }).Where(t => t.n > 0 && t.n <= 2).Select(t => t.t).Concat(adjacentWhiteTiles.Where(t => CountAdjacentBlackTiles(t) == 2)).ToDictionary(t => t, t=> true);
}

var part2 = tiles.Count;
part2.Dump();