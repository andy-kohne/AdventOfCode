<Query Kind="Statements" />

var instructions = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day12.txt"));
var registers = new Dictionary<char, int> { { 'a', 0}, {'b', 0}, {'c', 1}, {'d', 0}};
var pos = 0;

while (pos < instructions.Count())
{
	var parts = instructions[pos].Split(' ');
	switch (parts[0])
	{
		case "cpy":
			var i = int.TryParse(parts[1], out int v) ? v : registers[parts[1][0]];
			registers[parts[2][0]] = i;
			break;
			
		case "inc":
			registers[parts[1][0]]++;
			break;

		case "dec":
			registers[parts[1][0]]--;
			break;
			
		case "jnz":
			if ((int.TryParse(parts[1], out int val) && val != 0) || registers[parts[1][0]] != 0){
				
				var o = int.Parse(parts[2]);
				pos += o;
				continue;
			}
			break;
	}
	pos++;
}

registers.Dump();

