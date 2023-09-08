<Query Kind="Program" />

void Main()
{
	var playerChoices =
		from w in Weapons
		from a in Armor.Append(new KeyValuePair<string, Stats>("None", new Stats()))
		from r1 in Rings.Append(new KeyValuePair<string, Stats>("None", new Stats()))
		from r2 in Rings.Append(new KeyValuePair<string, Stats>("None", new Stats()))
		where r1.Key != r2.Key
		select ($"{w.Key}; {a.Key}; {r1.Key}; {r2.Key}", new Stats
		{
			HitPoints = 100,
			Cost = w.Value.Cost + a.Value.Cost + r1.Value.Cost + r2.Value.Cost,
			Armor = w.Value.Armor + a.Value.Armor + r1.Value.Armor + r2.Value.Armor,
			Damage = w.Value.Damage + a.Value.Damage + r1.Value.Damage + r2.Value.Damage,
		});

	var part1 = playerChoices.Where(p => Fight(p.Item2, new Stats { HitPoints = 109, Damage = 8, Armor = 2 })).OrderBy(p => p.Item2.Cost).First().Item2.Cost;
	part1.Dump();

	var part2 = playerChoices.Where(p => !Fight(p.Item2, new Stats { HitPoints = 109, Damage = 8, Armor = 2 })).OrderByDescending(p => p.Item2.Cost).First().Item2.Cost;
	part2.Dump();
}


bool Fight(Stats player, Stats enemy)
{
	while (true)
	{
		enemy.HitPoints -= GetDamage(player, enemy);
		if (enemy.HitPoints <= 0) return true;
		player.HitPoints -= GetDamage(enemy, player);
		if (player.HitPoints <= 0) return false;
	}
}

int GetDamage(Stats attacker, Stats defender) =>
	Math.Max(1, attacker.Damage - defender.Armor);

class Stats
{
	public int Cost { get; set; }
	public int HitPoints { get; set; }
	public int Damage { get; set; }
	public int Armor { get; set; }
}

Dictionary<string, Stats> Weapons = new Dictionary<string, Stats>() {
	{ "Dagger", new() { Cost = 8, Damage = 4 }},
	{ "Shortsword", new() { Cost = 10, Damage = 5 }},
	{ "Warhammer", new() { Cost = 25, Damage = 6 }},
	{ "Longsword", new() { Cost = 40, Damage = 7 }},
	{ "Greataxe", new() { Cost = 74, Damage = 8 }},
};

Dictionary<string, Stats> Armor = new Dictionary<string, Stats>() {
	{ "Leather", new() { Cost = 13, Armor = 1 }},
	{ "Chainmail", new() { Cost = 31, Armor = 2 }},
	{ "Splintmail", new() { Cost = 53, Armor = 3 }},
	{ "Bandedmail", new() { Cost = 75, Armor = 4 }},
	{ "Platemail", new() { Cost = 102, Armor = 5 }},
};

Dictionary<string, Stats> Rings = new Dictionary<string, Stats>() {
	{ "Damage +1", new() { Cost = 25, Damage = 1 }},
	{ "Damage +2", new() { Cost = 50, Damage = 2 }},
	{ "Damage +3", new() { Cost = 100, Damage = 3 }},
	{ "Defense +1", new() { Cost = 20, Armor = 1 }},
	{ "Defense +2", new() { Cost = 40, Armor = 2 }},
	{ "Defense +3", new() { Cost = 80, Armor = 3 }},
};