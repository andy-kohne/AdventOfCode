<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day21.txt")).ToList();
var re = new Regex(@"(.+) \(contains (.+)\)");

var foods = input.Select(i => re.Match(i)).Select(i => (i.Groups[1].Value.Split(' ').ToArray(), i.Groups[2].Value.Split(new[] { ", "}, StringSplitOptions.None).ToArray())).ToArray();
var allergens = foods.SelectMany(f => f.Item2.Select(a => (a,food:f.Item1))).GroupBy(a=> a.Item1).Select(g => (g.Key, g.SelectMany(i => i.food).Where(f => g.All(o => o.food.Contains(f))).Distinct().ToArray())).ToArray();
var safeIngredients = foods.SelectMany(f => f.Item1).Except(allergens.SelectMany(a => a.Item2)).ToArray();
var part1 = foods.Sum(f => f.Item1.Count(fi => safeIngredients.Contains(fi)));
part1.Dump();

var identified = new Dictionary<string,string>();
while (allergens.Any(a => !identified.ContainsValue(a.Key)))
{
	foreach (var a in allergens.Where(a => !identified.ContainsKey(a.Key))){
		var extra = a.Item2.Except(identified.Keys);
		if (extra.Count() == 1){
			identified[extra.Single()]=a.Key;			
		}
		
	}
}

var part2 = string.Join(",",identified.OrderBy(t => t.Value).Select(t => t.Key));
part2.Dump();
