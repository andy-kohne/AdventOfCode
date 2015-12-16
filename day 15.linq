<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var ingredients = 
			File.ReadAllLines(Path.Combine(Path.GetDirectoryName (Util.CurrentQueryPath),"day15.txt"))
				.Select(s => 
				{
					var m = Regex.Match(s, @"(.+)\: capacity (-?\d+), durability (-?\d+), flavor (-?\d+), texture (-?\d+), calories (\d+)" );
					return new Ingredient
					{
						Name = m.Groups[1].Value,
						Capacity = int.Parse(m.Groups[2].Value),
						Durability = int.Parse(m.Groups[3].Value),
						Flavor = int.Parse(m.Groups[4].Value),
						Texture = int.Parse(m.Groups[5].Value),
						Calories = int.Parse(m.Groups[6].Value),
					};
				}).ToArray();

	var recipes = GetAll().Select (x => new [] { new IngredientAmount { Ingredient = ingredients[0], Amount = x[0] },
												 new IngredientAmount { Ingredient = ingredients[1], Amount = x[1] },
												 new IngredientAmount { Ingredient = ingredients[2], Amount = x[2] },
												 new IngredientAmount { Ingredient = ingredients[3], Amount = x[3] } });
	
	var scores = recipes.Select(r => ScoreCookie(r));

	
	// part 1
	scores.OrderByDescending (r => r.Item1).First().Item1.Dump();
	
	// part 2
	scores.Where(r => r.Item2 == 500).OrderByDescending (r => r.Item1).First().Item1.Dump();
}


public IEnumerable<int[]> GetAll()
{
	for (var a=0; a <= 100; a++) {
		for (var b=0; b <= 100 -a ; b++) {
			for (var c=0; c <= 100 -a -b; c++) {
				yield return new [] { a,b,c,100-a-b-c};
			}
		}
	}
}

public class IngredientAmount 
{
	public Ingredient Ingredient { get ;set; }
	public int Amount { get ;set; }
}

public class Ingredient
{
	public string Name { get ;set; }
	public int Capacity { get ;set; }
	public int Durability { get ;set; }
	public int Flavor { get ;set; }
	public int Texture { get ;set; }
	public int Calories { get ;set; }
}

public Tuple<int, int> ScoreCookie(IEnumerable<IngredientAmount> recipe)
{
	var total = new Ingredient
	{
		Capacity = recipe.Sum(r => r.Amount * r.Ingredient.Capacity),
		Durability = recipe.Sum(r => r.Amount * r.Ingredient.Durability),
		Flavor = recipe.Sum(r => r.Amount * r.Ingredient.Flavor),
		Texture = recipe.Sum(r => r.Amount * r.Ingredient.Texture),
		Calories = recipe.Sum(r => r.Amount * r.Ingredient.Calories),
	};
	
	return new Tuple<int, int>( (total.Capacity < 0 ? 0 : total.Capacity) *  (total.Durability < 0 ? 0 : total.Durability) *  (total.Flavor < 0 ? 0 : total.Flavor) *  (total.Texture < 0 ? 0 : total.Texture) , total.Calories);
}