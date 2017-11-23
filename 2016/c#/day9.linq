<Query Kind="Program" />

void Main()
{
	var compressed = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day9.txt"));
	
	var part1 = DecompressVersion1(compressed);
	part1.Length.Dump();

	GetVersion2Length(compressed).Dump();
}

Regex re = new Regex(@"\G\((\d+)x(\d+)\)");

string DecompressVersion1(string compressed)
{
	var uncompressed = new StringBuilder();

	var offset = 0;
	while (offset < compressed.Length)
	{
		var match = re.Match(compressed, offset);
		if (match.Success)
		{
			offset += match.Length;
			for (int repeat = 0; repeat < int.Parse(match.Groups[2].Value); repeat++)
			{
				uncompressed.Append(compressed.Substring(offset, int.Parse(match.Groups[1].Value)));
			}
			offset += int.Parse(match.Groups[1].Value);
		}
		else
		{
			uncompressed.Append(compressed[offset]);
			offset++;
		}
	}

	return uncompressed.ToString();
}

long GetVersion2Length(string compressed)
{
	long ret = 0;

	var offset = 0;
	while (offset < compressed.Length)
	{
		var match = re.Match(compressed, offset);
		if (match.Success)
		{
			offset += match.Length;
			var chunk = compressed.Substring(offset, int.Parse(match.Groups[1].Value));
			var decompressedchunk = GetVersion2Length(chunk);
			ret += (decompressedchunk * int.Parse(match.Groups[2].Value));
			offset += int.Parse(match.Groups[1].Value);
		}
		else
		{
			ret++;
			offset++;
		}
	}

	return ret;
}