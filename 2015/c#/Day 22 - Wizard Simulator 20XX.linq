<Query Kind="Program" />

void Main()
{
	var game = new GameState
	{
		playerHitPoints = 50,
		playerMana = 500,
		bossDamage = 10,
		bossHitPoints = 71,
		spellStates = new(),
	};

	var part1 = game.PlayerPlays()
		.Where(p => p.playerHitPoints > 0)
		.OrderBy(p => p.playerManaSpent)
		.First();
	part1.playerManaSpent.Dump();

	var part2 = game.PlayerPlays(true)
		.Where(p => p.playerHitPoints > 0)
		.OrderBy(p => p.playerManaSpent)
		.First();
	part2.playerManaSpent.Dump();
}

public class GameState
{
	public int playerManaSpent;
	public int playerHitPoints;
	public int playerDamage;
	public int playerMana;
	public int playerArmor;

	public int bossHitPoints;
	public int bossDamage;

	public Dictionary<Spell, int> spellStates;
}

static class Helpers
{
	static GameState Clone(this GameState gameState)
	{
		return new GameState
		{
			playerManaSpent = gameState.playerManaSpent,
			playerHitPoints = gameState.playerHitPoints,
			playerDamage = gameState.playerDamage,
			playerMana = gameState.playerMana,
			playerArmor = gameState.playerArmor,
			bossHitPoints = gameState.bossHitPoints,
			bossDamage = gameState.bossDamage,
			spellStates = gameState.spellStates.ToDictionary(s => s.Key, s => s.Value),
		};
	}

	static void CastSpell(this GameState gameState, Spell spell)
	{
		gameState.playerMana -= spell.Cost;
		gameState.playerManaSpent += spell.Cost;
		gameState.spellStates[spell] = spell.Timer;
		spell.Cast(gameState);
	}

	static bool IsOver(this GameState gameState) =>
		gameState.bossHitPoints <= 0 || gameState.playerHitPoints <= 0;

	public static bool CanCast(this GameState state, Spell spell) =>
		state.playerMana >= spell.Cost && (!state.spellStates.TryGetValue(spell, out var s) || s == 0);

	public static IEnumerable<GameState> PlayerPlays(this GameState gameState, bool hardMode = false)
	{
		if (hardMode)
			gameState.playerHitPoints--;

		gameState.ApplySpells();

		foreach (var spell in Spells.Where(s => gameState.CanCast(s)))
		{
			var cloned = gameState.Clone();
			cloned.CastSpell(spell);
			if (cloned.IsOver())
				yield return cloned;
			else
			{
				cloned.BossPlays();
				if (cloned.IsOver())
					yield return cloned;
				else
					foreach (var s in cloned.PlayerPlays(hardMode))
						yield return s;
			}
		}
	}

	static void BossPlays(this GameState gameState)
	{
		gameState.ApplySpells();
		if (gameState.bossHitPoints > 0)
			gameState.playerHitPoints -= (gameState.bossDamage - gameState.playerArmor);
	}

	static void ApplySpells(this GameState gameState)
	{
		foreach (var spell in gameState.spellStates.Where(s => s.Value > 0).Select(s => s.Key))
		{
			gameState.spellStates[spell]--;
			spell.Assess(gameState, spell);
		}
	}
}

public class Spell
{
	public int Timer { get; set; }
	public int Cost { get; set; }
	public Action<GameState> Cast = _ => { };
	public Action<GameState, Spell> Assess = (_, _) => { };
}

public static List<Spell> Spells = new()
{
	new Spell
	{
		// Magic Missile
		Cost = 53,
		Cast = state => state.bossHitPoints -= 4
	},
	new Spell
	{
		// Drain 
		Cost = 73,
		Cast = state =>
		{
			state.bossHitPoints -= 2;
			state.playerHitPoints += 2;
		}
	},
	new Spell
	{
		// Shield
		Cost = 113,
		Timer = 6,
		Cast = state => state.playerArmor = 7,
		Assess = (state, spell) => state.playerArmor = state.spellStates[spell] == 0 ? 0 : state.playerArmor
	},
	new Spell
	{
		// Poison
		Cost = 173,
		Timer = 6,
		Assess = (state, spell) => state.bossHitPoints -= 3
	},
	new Spell
	{
		// Recharge
		Cost = 229,
		Timer = 5,
		Assess = (state, spell) => state.playerMana += 101
	},
};
