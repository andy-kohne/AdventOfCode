<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var input = 
			File.ReadAllLines(Path.Combine(Path.GetDirectoryName (Util.CurrentQueryPath),"..","day23.txt"))
				.ToArray();
			
	var registers = new Dictionary<char,uint> { {'a', 0}, {'b', 0}};
	
	
	// part 1
	UseTrulyStateOfTheArtTechnologyToRunProgram(registers, input);
	registers.Dump();
	
	
	// part 2
	registers = new Dictionary<char,uint> { {'a', 1}, {'b', 0}};
	UseTrulyStateOfTheArtTechnologyToRunProgram(registers, input);
	registers.Dump();

}

void UseTrulyStateOfTheArtTechnologyToRunProgram(Dictionary<char,uint> registers, string[] program)
{
	var index = 0;
	do 
	{
		var i = program[index];
		switch (i.Substring(0,3))
		{
			case "hlf" : registers[i[4]] /= 2; index++; break;
			case "inc" : registers[i[4]] += 1; index++; break;
			case "tpl" : registers[i[4]] *= 3; index++; break;
			case "jmp" : index += int.Parse(i.Substring(4)); break;
			case "jie" : index += registers[i[4]] % 2 == 0 ? int.Parse(i.Substring(7)) : 1; break;
			case "jio" : index += registers[i[4]] == 1 ? int.Parse(i.Substring(7)) : 1; break;
		}
	} while (index < program.Length);	
}