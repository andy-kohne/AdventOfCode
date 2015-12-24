<Query Kind="Program">
  <Output>DataGrids</Output>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
</Query>

void Main()
{
	var path = Path.Combine(Path.GetDirectoryName (Util.CurrentQueryPath),"..","day12.txt");
	
	
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
	Count(reader).Dump();
}

private long Count(JsonTextReader reader) 
{
	long ret = 0;
	bool isRed= false;
	bool isObject = reader.TokenType == JsonToken.StartObject;
	while(reader.Read())
	{
		if (reader.TokenType == JsonToken.EndObject || reader.TokenType == JsonToken.EndArray)
			return isRed ? 0 : ret;
		if (reader.TokenType == JsonToken.Integer)
			ret += (long)reader.Value;
		if (reader.TokenType == JsonToken.StartObject || reader.TokenType == JsonToken.StartArray)
			ret += Count(reader);
		if (isObject && string.Equals(reader.Value as string, "red"))
			isRed=true;
	}
	return ret;
}