<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var instructions = File.ReadAllLines(Path.Combine(Path.GetDirectoryName (Util.CurrentQueryPath),"..","day7.txt")).Select(l => l.Split(new string [] { " -> "}, StringSplitOptions.None)).ToDictionary (x => x[1], x=> new Gate { Input = x[0]});
	
	// part 1
	Resolve(instructions);
	instructions["a"].Output.Dump();

	// part 2
	var answer = instructions["a"].Output;
	foreach (var i in instructions)
		i.Value.Output = null;
	instructions["b"].Input = answer.ToString();
	Resolve(instructions);
	instructions["a"].Output.Dump();

}

// Define other methods and classes here
public class Gate
{
	public string Input { get ;set; }
	public UInt16? Output { get; set; }
}

public ushort? Check(Dictionary<string, Gate> instructions, string input)
{
	ushort value;
	if (ushort.TryParse(input, out value))
		return value;
	else 
		if (instructions[input].Output != null)
			return  instructions[input].Output.Value;
	return null;
}

public void Resolve(Dictionary<string, Gate> instructions)
{
	while (instructions["a"].Output == null)
	{
		foreach(var i in instructions)
		{
			if (i.Value.Output != null)
				continue;
			UInt16 x;
			if (UInt16.TryParse(i.Value.Input, out x))
			{
				i.Value.Output =  x;
				continue;
			}
			var reg = Regex.Match(i.Value.Input, @"(.+) (AND|OR|LSHIFT|RSHIFT) (.+)");
			if (reg.Success)
			{
				var operand1 = reg.Groups[1].Value;
				var op = reg.Groups[2].Value;
				var operand2 = reg.Groups[3].Value;
				
				var op1 = Check(instructions, reg.Groups[1].Value);
				var op2 = Check(instructions, reg.Groups[3].Value);
				
				if (op1 == null || op2 == null)
					continue;
					
				switch(op)
				{
					case "AND": i.Value.Output = (UInt16 )(op1 & op2); break;
					case "OR" : i.Value.Output = (UInt16)(op1 | op2); break;
					case "LSHIFT": i.Value.Output = (UInt16)(op1 << op2); break;
					case "RSHIFT" : i.Value.Output = (UInt16)(op1 >> op2); break;
				}
				
				
				continue;
			}
			reg = Regex.Match(i.Value.Input, @"NOT (.*)");
			if (reg.Success) 
			{
				var op1 = Check(instructions, reg.Groups[1].Value);
				if (op1 == null) continue;
				
				i.Value.Output = (UInt16)~op1;
				continue;
			}
			if (instructions[i.Value.Input].Output == null)
				continue;
			i.Value.Output = instructions[i.Value.Input].Output.Value;
			continue;
		}
	
	}
	
}