<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Namespace>System.Drawing</Namespace>
</Query>

var keypad = new int[,] { { 1,4,7 }, {2,5,8 }, {3,6, 9} };
var pos = new Point { X = 1, Y = 1 };
var instructions = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day2.txt"));

string code = "";

foreach (var instruction in instructions)
{
	for (int ctr = 0; ctr < instruction.Length; ctr++)
	{
		switch (instruction[ctr])
		{
			case 'U': pos.Y = pos.Y > 0 ? pos.Y - 1 : 0; break;
			case 'D': pos.Y = pos.Y < 2 ? pos.Y + 1 : 2; break;
			case 'L': pos.X = pos.X > 0 ? pos.X - 1 : 0; break;
			case 'R': pos.X = pos.X < 2 ? pos.X + 1 : 2; break;
		}
	}
	code += keypad[pos.X,pos.Y];
}
Console.WriteLine($"Part 1: {code}");


var newKeypad = new char[,] { { ' ', ' ', ' ', ' ', ' ', ' ', ' '},
							  { ' ', ' ', ' ', '5', ' ', ' ', ' '},
							  { ' ', ' ', '2', '6', 'A', ' ', ' '}, 
							  { ' ', '1', '3', '7', 'B', 'D', ' '}, 
							  { ' ', ' ', '4', '8', 'C', ' ', ' '},
							  { ' ', ' ', ' ', '9', ' ', ' ', ' '},
							  { ' ', ' ', ' ', ' ', ' ', ' ', ' '}};

pos = new Point { X = 1, Y = 3};
code = "";

foreach (var instruction in instructions)
{
	for (int ctr = 0; ctr < instruction.Length; ctr++)
	{
		var p = new Point { X = pos.X, Y = pos.Y};
		
		switch (instruction[ctr])
		{
			case 'U': p.Y--; break;
			case 'D': p.Y++; break;
			case 'L': p.X--; break;
			case 'R': p.X++; break;
		}
		
		if (newKeypad[p.X,p.Y] == ' ')
			continue;
			
		pos = p;
	}
	code += newKeypad[pos.X, pos.Y];
}
Console.WriteLine($"Part 2: {code}");