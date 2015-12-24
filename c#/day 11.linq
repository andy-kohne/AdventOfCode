<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var password = "hepxcrrq";
	
	password = FindNextPassword(password); 
	password.Dump();
	
	password = FindNextPassword(password); 
	password.Dump();
}

// Define other methods and classes here
public Regex Incrementing = new Regex("(abc|bcd|cde|def|efg|fgh|ghi|hij|ijk|jkl|klm|lmn|mno|nop|opq|pqr|qrs|rst|stu|tuv|uvw|vwx|wxy|xyz)");
public Regex Forbidden = new Regex("[iol]");

public string FindNextPassword(string currentPassword)
{
	do
	{
		currentPassword = Increment(currentPassword);
	} while (Forbidden.IsMatch(currentPassword) || !Incrementing.IsMatch(currentPassword) || !HasRepeatingPairs(currentPassword));
	return currentPassword;
}

public bool HasRepeatingPairs(string input)
{
	var Repeats = new List<char>();
	for (var i = 0; i < input.Length - 1; i++)
	{
		if (input[i] == input[i+1])
			Repeats.Add(input[i]);
	}
	return Repeats.Distinct().Count() >= 2;
}

public string Increment(string input)
{
	var chars = input.ToCharArray();
	for (var i = 1; i <= chars.Length; i++)
	{
		if (chars[chars.Length-i] == 'z')
		{
			chars[chars.Length-i] = 'a';
			continue;
		}
		chars[chars.Length-i]++;
		break;
	}
	return new string(chars);
}