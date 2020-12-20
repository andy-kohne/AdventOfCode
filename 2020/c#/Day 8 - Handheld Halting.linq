<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day8.txt")).Select(i => i.Split(' ').ToList()).ToList();

(bool, int) Run(List<List<string>> code)
{
	var executed = new Dictionary<int, int>();

	var ptr = 0;
	var acc = 0;
	while (ptr < code.Count)
	{
		if (executed.ContainsKey(ptr))
			return (false, acc);
		
		executed[ptr] = 1;
		var cmd = code[ptr];
		switch (cmd[0])
		{
			case "acc":
				acc += int.Parse(cmd[1]);
				ptr++;
				break;
			case "nop":
				ptr++;
				break;
			case "jmp":
				ptr += int.Parse(cmd[1]);
				break;
		}
	}
	return (true, acc);
}

(_, var part1) = Run(input);
part1.Dump();

for (int i = 0; i < input.Count(); i++){
	var altered = input.Select(o => o.Select(ii => ii).ToList()).ToList();
	if (altered[i][0] == "nop"){
		altered[i][0] = "jmp";
	}
	else if (altered[i][0] == "jmp"){
		altered[i][0] = "nop";
	}
	else{
		continue;
	}
	(var result, var part2) = Run(altered);
	if (result == true){
		part2.Dump();
	}
}
