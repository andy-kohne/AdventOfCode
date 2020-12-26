<Query Kind="Program" />

void Main()
{
	var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day20.txt")).ToList();
	
	var tiles = ReadTiles(input.GetEnumerator()).ToArray();
	
	var corners = tiles.Where(t => tiles.Where(o => o.Id != t.Id).Count(o => t.CanTouch(o)) == 2).ToArray();
	var part1 = corners.Select(t => t.Id).Aggregate ((x, y) => x * y);
	part1.Dump();

	var size = (int)Math.Sqrt(tiles.Count());
	var layout = new Tile[size,size];
	
	for(int row=0; row<size; row++){
		for(int col=0; col<size; col++){
			if (row == 0 && col == 0)
			{
				var c = corners.First();
				c.Flip();
				if (c.SpinAndFlipUntil(() => tiles.Where(o => o.Id != c.Id).Count(o => o.PossibleEdges.Contains(c.BottomEdge)) == 1 &&
					tiles.Where(o => o.Id != c.Id).Count(o => o.PossibleEdges.Contains(c.RightEdge)) == 1))
					layout[row, col] = c;
			}else if (row ==0) {
				var existing = layout[0,col-1];
				foreach( var candidate in  tiles.Where(t => t.Id != existing.Id).Where(o => o.PossibleEdges.Contains(existing.RightEdge))){
					if (candidate.SpinAndFlipUntil (() => existing.RightEdge == candidate.LeftEdge))
						layout[row,col]=candidate;
						break;
					}				
			}else{
				var existing = layout[row - 1,col];
				foreach (var candidate in tiles.Where(t => t.Id != existing.Id).Where(o => o.PossibleEdges.Contains(existing.BottomEdge)))				
					if (candidate.SpinAndFlipUntil(() => existing.BottomEdge == candidate.TopEdge))
					{
						layout[row, col] = candidate;
						break;
					}				
			}
		}		
	}
	
	//assemble the image
	var w = tiles.First().Bits.GetLength(1) - 2;
	var h = tiles.First().Bits.GetLength(0)-2;	
	var image = new char[layout.GetLength(0)*h,layout.GetLength(1)*w ];
	for(int row=0; row<size; row++){
		for(int col=0; col<size; col++){
			Helpers.Copy2DArray(ref image,col*w,row*h,layout[row,col].Bits,1,1,w,h);
		}		
	}

	var seaMonster = new[] { "                  # ", "#    ##    ##    ###", " #  #  #  #  #  #   "}.Select(o => o.ToCharArray()).ToArray().To2DArray();

	bool FindSeaMonster(int row, int col) {
		if (row+seaMonster.GetLength(0) > image.GetLength(0) || col+seaMonster.GetLength(1) > image.GetLength(1))
			return false;
		for (int r = 0; r<seaMonster.GetLength(0); r++)
		{
			for (int c = 0; c < seaMonster.GetLength(1); c++)
			{
				if (seaMonster[r, c] == '#' && image[row + r, col + c] != '#')
					return false;
			}
		}
		return true;
	}
	
	var monsters = Enumerable.Range(0, image.GetLength(0)).Sum(row => Enumerable.Range(0, image.GetLength(1)).Count(col => FindSeaMonster(row,col)));
	var ctr = 0;
	while (monsters == 0){
		image = image.RotateArray();
		if (++ctr == 4) image= image.FlipArray(); 
		monsters = Enumerable.Range(0, image.GetLength(0)).Sum(row => Enumerable.Range(0, image.GetLength(1)).Count(col => FindSeaMonster(row,col)));
	}
	
	var part2 = image.ToEnumerable().Count(i => i=='#') - (monsters * seaMonster.ToEnumerable().Count(i => i=='#'));
	part2.Dump();
}

IEnumerable<Tile> ReadTiles(IEnumerator<string> e)
{
	while (e.MoveNext())
	{
		var tileid = int.Parse(((string)e.Current).Substring(5, 4));
		var lines = new List<string>();
		e.MoveNext();
		while (!string.IsNullOrEmpty(e.Current))
		{
			lines.Add(e.Current);
			e.MoveNext();
		}
		yield return new Tile(tileid, lines.Select(l => l.ToCharArray().ToArray()).ToArray().To2DArray() );
	}
}

public class Tile
{
	public Tile(long id, char[,] bits)
	{
		Id = id;
		Bits = bits;
	}
	
	public long Id { get; private set; }
	public char[,] Bits { get; private set; }

	private List<string> _possibleEdges;
	public List<string> PossibleEdges
	{
		get
		{
			if (_possibleEdges == null)
			{
				_possibleEdges = new List<string> { TopEdge, BottomEdge, LeftEdge, RightEdge, new string(TopEdge.Reverse().ToArray()), new string(BottomEdge.Reverse().ToArray()), new string(LeftEdge.Reverse().ToArray()), new string(RightEdge.Reverse().ToArray()) };
			}
			return _possibleEdges;
		}
	}
	
	public void Rotate() { Bits = Bits.RotateArray(); }
	public void Flip() { Bits = Bits.FlipArray() ; }

	public string TopEdge => new string(Enumerable.Range(0, Bits.GetLength(1)).Select(col => Bits[0, col]).ToArray());
	public string BottomEdge => new string(Enumerable.Range(0, Bits.GetLength(1)).Select(col => Bits[ Bits.GetLength(0)-1, col]).ToArray());
	public string LeftEdge => new string(Enumerable.Range(0, Bits.GetLength(0)).Select(row => Bits[row, 0]).ToArray());
	public string RightEdge => new string(Enumerable.Range(0, Bits.GetLength(0)).Select(row => Bits[row, Bits.GetLength(1)-1]).ToArray());
	
	public bool CanTouch(Tile other) => PossibleEdges.Any(e => other.PossibleEdges.Contains(e));
	public bool SpinAndFlipUntil(Func<bool> condition)
	{
		for (int opt = 0; opt < 9; opt++)
		{
			if (condition())
				return true;

			Rotate();
			if (opt == 4) Flip();
		}
		return false;
	}
}

public static class Helpers
{
	public static T[,] RotateArray<T>(this T[,] src)
	{
		int oRows = src.GetLength(0);
		int oCols = src.GetLength(1);

		int nRows = oCols;
		int nCols = oRows;
		
		T[,] ret = new T[nRows, nCols];

		int oldRow;
		for (int newRow = 0; newRow < nRows; newRow++)
		{
			oldRow = oRows -1;
			for (int newCol = 0; newCol < nCols; newCol++)			
				ret[newRow, newCol] = src[oldRow--, newRow];	
		}

		return ret;
	}

	public static T[,] FlipArray<T>(this T[,] src)
	{
		int rows = src.GetLength(0);
		int cols = src.GetLength(1);

		T[,] ret = new T[rows, cols];

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				ret[i, j] = src[i, cols - j - 1];
			}
		}

		return ret;
	}

	public static T[,] To2DArray<T>(this IList<IList<T>> src)
	{
		int max = src.Select(l => l).Max(l => l.Count());
		var ret = new T[src.Count, max];

		for (int i = 0; i < src.Count; i++)
		{
			for (int j = 0; j < src[i].Count(); j++)
			{
				ret[i, j] = src[i][j];
			}
		}
		return ret;
	}

	public static void Copy2DArray<T>(ref T[,] dest, int destCol, int destRow, T[,] source, int? sourceCol = null, int? sourceRow = null, int? cols = null, int? rows = null)
	{
		// no error checking...
		var rowsToCopy = rows ?? source.GetLength(0) - sourceRow ?? source.GetLength(0);
		var colsToCopy = cols ?? source.GetLength(1) - sourceCol ?? source.GetLength(1);

		for (int r = 0; r < rowsToCopy; r++)
		{
			for (int c = 0; c < colsToCopy; c++)
			{
				dest[destRow + r, destCol + c] = source[sourceRow.GetValueOrDefault() + r, sourceCol.GetValueOrDefault() + c];

			}
		}
	}
	
	public static IEnumerable<T> ToEnumerable<T>(this T[,] src)
	{
		for (int r=0; r<src.GetLength(0);r++)
		for (int c=0; c<src.GetLength(1);c++)
			yield return src[r,c];
	}
}
