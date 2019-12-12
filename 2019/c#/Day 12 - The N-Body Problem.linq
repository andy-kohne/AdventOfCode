<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day12.txt"));

var moons = input.Select(i => Regex.Match(i, @"x=(\-?\d+), y=(\-?\d+), z=(\-?\d+)")).Select(i => (x: int.Parse(i.Groups[1].Value), y: int.Parse(i.Groups[2].Value), z: int.Parse(i.Groups[3].Value))).ToArray();
var velocity = Enumerable.Range(0, 4).Select(e => (x: 0, y: 0, z: 0)).ToArray();

int energy((int x, int y, int z) e) => Math.Abs(e.x) + Math.Abs(e.y) + Math.Abs(e.z);
void ApplyVelocity()
{
	for (int i = 0; i < 4; i++)
	{
		moons[i].x += velocity[i].x;
		moons[i].y += velocity[i].y;
		moons[i].z += velocity[i].z;
	}
}
void ApplyGravity()
{
	for (int left = 0; left < 4; left++)
	{
		for (int right = left + 1; right < 4; right++)
		{
			if (moons[left].x > moons[right].x)
			{
				velocity[left].x--;
				velocity[right].x++;
			}
			else if (moons[left].x < moons[right].x)
			{
				velocity[left].x++;
				velocity[right].x--;
			}

			if (moons[left].y > moons[right].y)
			{
				velocity[left].y--;
				velocity[right].y++;
			}
			else if (moons[left].y < moons[right].y)
			{
				velocity[left].y++;
				velocity[right].y--;
			}

			if (moons[left].z > moons[right].z)
			{
				velocity[left].z--;
				velocity[right].z++;
			}
			else if (moons[left].z < moons[right].z)
			{
				velocity[left].z++;
				velocity[right].z--;
			}
		}

	}
}


for (int step = 0; step < 1000; step++)
{
	ApplyGravity();
	ApplyVelocity();
}

var part1 = Enumerable.Range(0, 4).Select(e => energy(moons[e]) * energy(velocity[e])).Sum();
part1.Dump();



(int, int, int, int, int, int, int, int) key(Func<(int x, int y, int z), int> sel) => (sel(moons[0]), sel(velocity[0]), sel(moons[1]), sel(velocity[1]), sel(moons[2]), sel(velocity[2]), sel(moons[3]), sel(velocity[3]));
long lcm(long a, long b) => Math.Abs(a * b) / gcd(a, b); 
long gcd(long a, long b) => b == 0 ? a : gcd(b, a % b);

var startingx = key(a => a.x);
var startingy = key(a => a.y);
var startingz = key(a => a.z);

long counter = 0, xperiod = 0, yperiod = 0, zperiod = 0;

do
{
	counter++;
	ApplyGravity();
	ApplyVelocity();
	if (xperiod == 0 && key(a => a.x) == startingx) xperiod = counter;
	if (yperiod == 0 && key(a => a.y) == startingy) yperiod = counter;
	if (zperiod == 0 && key(a => a.z) == startingz) zperiod = counter;
} while (xperiod == 0 || yperiod == 0 || zperiod == 0);

var part2 = new[] { xperiod, yperiod, zperiod }.Aggregate(lcm);
part2.Dump();

