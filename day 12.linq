<Query Kind="Program">
  <Output>DataGrids</Output>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
</Query>

void Main()
{
	var path = Path.Combine(Path.GetDirectoryName (Util.CurrentQueryPath),"day12.txt");
	
	// part 1
	var reader = new JsonTextReader(new StringReader(File.ReadAllText(path)));
	long count = 0;
	while (reader.Read())
	{
		if (reader.TokenType == JsonToken.Integer)
			count += (long)reader.Value;
	}
	count.Dump();
	
	// part 2
	var s = File.ReadAllText(path);
	reader = new JsonTextReader(new StringReader(s));
//	reader.Read();
	Count(reader).Dump();
}

// Define other methods and classes here
private long Count(JsonTextReader reader) 
{
	string.Format("Up Level: {0}", reader.Depth).Dump();
	long ret = 0;
//	int level = reader.Depth;
	bool isRed= false;
	bool isObject = reader.TokenType == JsonToken.StartObject;
	while(reader.Read())
	{
		reader.Depth.Dump();
		if (reader.TokenType == JsonToken.EndObject)
		{
			string.Format(" returning {0}", isRed ? 0 : ret).Dump();;
			return isRed ? 0 : ret;
		}
		if (reader.TokenType == JsonToken.Integer)
			ret += (long)reader.Value;
		if (reader.TokenType == JsonToken.StartObject)
			ret += Count(reader);
		if (isObject && reader.TokenType == JsonToken.String && string.Equals(reader.Value as string, "red", StringComparison.InvariantCultureIgnoreCase))
			isRed=true;
	
			
	}

	throw new Exception();
}
