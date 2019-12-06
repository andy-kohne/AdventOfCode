<Query Kind="Statements" />

bool HasDoubleDigit(string pw) => pw[0] == pw[1] || pw[1] == pw[2] || pw[2] == pw[3] || pw[3] ==pw[4] || pw[4] == pw[5];
bool IsAscending(string pw) => pw[0] <= pw[1] && pw[1] <= pw[2] && pw[2] <= pw[3] && pw[3] <= pw[4] && pw[4] <= pw[5];
bool HasNotTripledDoubleDigit(string pw) => (pw[0] == pw[1] && pw[2] != pw[1]) ||
											(pw[0] != pw[1] && pw[1] == pw[2] && pw[2] != pw[3]) ||
											(pw[1] != pw[2] && pw[2] == pw[3] && pw[3] != pw[4]) ||
											(pw[2] != pw[3] && pw[3] == pw[4] && pw[4] != pw[5]) ||
											(pw[3] != pw[4] && pw[4] == pw[5]);

int part1 = 0, part2 = 0;
for (var pw = 254032; pw <= 789860; pw++){
	var pws = pw.ToString();
	if (HasDoubleDigit(pws) && IsAscending(pws))
	{
		part1++;
		if (HasNotTripledDoubleDigit(pws))
			part2++;
	}
	
}

part1.Dump();
part2.Dump();