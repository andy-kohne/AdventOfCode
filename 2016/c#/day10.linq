<Query Kind="Program" />

void Main()
{
	var instructions = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day10.txt"));
	
	var botRe = new Regex(@"bot (\d+) gives low to (bot|output) (\d+) and high to (bot|output) (\d+)");
	var valueRe = new Regex(@"value (\d+) goes to bot (\d+)");
	
	var bots = instructions.Where(i => i.StartsWith("bot "))
						   .Select(i => botRe.Match(i))
						   .Select(m => new Bot { Id = int.Parse(m.Groups[1].Value), Instruction = m })
						   .ToDictionary(b => b.Id, b => b);
	var outputs = new Dictionary<int, List<int>>();
	
	foreach (var ins in instructions.Where(i => i.StartsWith("value ")))
	{
		var m = valueRe.Match(ins);
		var bot = int.Parse(m.Groups[2].Value);
		var val = int.Parse(m.Groups[1].Value);
		bots[bot].Values.Add(val);
	}

	while (bots.Any(b => b.Value.Values.Count == 2))
	{
		var bot = bots.First(b => b.Value.Values.Count == 2).Value;
		
		if (bot.Values.Contains(61) && bot.Values.Contains(17))
		{
			$"bot {bot.Id} comapring 61 and 17".Dump();	
		}
		
		if (bot.Instruction.Groups[2].Value == "bot")
		{
			bots[int.Parse(bot.Instruction.Groups[3].Value)].Values.Add(bot.Low);
		}
		else 
		{
			var output = int.Parse(bot.Instruction.Groups[3].Value);
			if (!outputs.ContainsKey(output))
			{
				outputs.Add(output, new List<int>());
			}
			outputs[output].Add(bot.Low);
		}

		if (bot.Instruction.Groups[4].Value == "bot")
		{
			bots[int.Parse(bot.Instruction.Groups[5].Value)].Values.Add(bot.High);
		}
		else
		{
			var output = int.Parse(bot.Instruction.Groups[5].Value);
			if (!outputs.ContainsKey(output))
			{
				outputs.Add(output, new List<int>());
			}
			outputs[output].Add(bot.High);
		}

		bot.Values.Clear();	
	}
	
	var product = outputs[0].Concat(outputs[1]).Concat(outputs[2]).Aggregate((x, y) => x * y);
	product.Dump();
}

class Bot
{
	public int Id { get; set; }
	public Match Instruction { get ;set; }
	public List<int> Values { get ;set; } = new List<int>();
	public int High => Values.Max();
	public int Low => Values.Min();
}
