<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var input = "1113222113";
	
	
	// part 1
	var value = input;
	for (var counter = 0; counter < 40; counter++)
		value = LookAndSay(value);
	value.Length.Dump();
	
	
	// part 2
	value = input;
	for (var counter = 0; counter < 50; counter++)
		value = LookAndSay(value);
	value.Length.Dump();
	
}

string LookAndSay(string i)
{
	var ret = new StringBuilder();
	int pos = 0;
	while (pos < i.Length)
	{
		var counter = 0;
		var current = i[pos];
		while ((pos+counter) < i.Length && i[pos+counter] == current)
			counter++;
		ret.AppendFormat("{0}{1}", counter, current);
		pos += counter;
	}
	return ret.ToString();
}