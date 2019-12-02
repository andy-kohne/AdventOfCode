<Query Kind="Statements">
  <Reference>&lt;RuntimeDirectory&gt;\System.configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Namespace>System.Configuration</Namespace>
</Query>

var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day2.txt")).Split(',').Select(int.Parse).ToArray();

void IntCodeComputer(int[] instructions) {
	var ptr = 0;
	while (instructions[ptr] != 99)
	{
		var regA = instructions[instructions[ptr + 1]];
		var regB = instructions[instructions[ptr + 2]];
		instructions[instructions[ptr + 3]] =
			instructions[ptr] == 1
				? regA + regB
				: regA * regB;
		ptr += 4;
	}	
}

var part1 = input.Select(i => i).ToArray();
part1[1] = 12;
part1[2] = 2;
IntCodeComputer(part1);
part1[0].Dump();

for (var noun = 0; noun < 100; noun++){
	for (var verb = 0; verb < 100; verb++)
	{
		var part2 = input.Select(i => i).ToArray();
		part2[1] = noun;
		part2[2] = verb;
		IntCodeComputer(part2);
		if (part2[0] == 19690720)
		{
			(100*noun+verb).Dump();
			return;
		}
	}
}
	


