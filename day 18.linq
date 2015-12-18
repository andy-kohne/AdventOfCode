<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var gridOrigin = new bool [100,100];
	var input =	File.ReadAllLines(Path.Combine(Path.GetDirectoryName (Util.CurrentQueryPath),"day18.txt")).ToArray();
	
	for(var x = 0; x<input.Length; x++)
		for (var y = 0; y<input[x].Length; y++)
			gridOrigin[x,y] = input[x][y] == '#';


	// part 1
	var grid = gridOrigin;
	for (var steps = 0; steps<100; steps++)
		grid=Step(grid);
	Total(grid).Dump();


	// part 2
	grid = gridOrigin;
	Corners(grid);
	for (var steps = 0; steps<100; steps++)
	{
		grid=Step(grid);
		Corners(grid);
	}
	Total(grid).Dump();
}

void Corners(bool[,] grid)
{
	grid[0,0] = true;
	grid[grid.GetLength(0)-1,0] = true;
	grid[0, grid.GetLength(1)-1] = true;
	grid[grid.GetLength(0)-1, grid.GetLength(1)-1] = true;
}

int Total(bool[,] grid)
{
	var count = 0;
	for(var x = 0; x<grid.GetLength(0); x++)
		for (var y = 0; y<grid.GetLength(1); y++)
			if (grid[x,y])
				count++;
	return count;
}

bool[,] Step(bool[,] current)
{
	var ret = new bool[current.GetLength(0),current.GetLength(1)];
	for(var x=0; x<current.GetLength(0); x++)
		for (var y=0; y<current.GetLength(1); y++)
			ret[x,y] = current[x,y] 
				? (Count(current,x,y) == 2 || Count(current,x,y) == 3)
				: Count(current,x,y) == 3;
	return ret;
}

int Count(bool[,] current, int x, int y)
{
	var maxX = current.GetLength(0)-1;
	var maxY = current.GetLength(1)-1;
	var ret=0;
	for(var xx = (x==0?0:x-1); xx<=(x==maxX?maxX:x+1);xx++)
		for(var yy = (y==0?0:y-1); yy<=(y==maxY?maxY:y+1);yy++)
			if (!(x == xx && y == yy) && current[xx,yy])
				ret++;
	return ret;
}
