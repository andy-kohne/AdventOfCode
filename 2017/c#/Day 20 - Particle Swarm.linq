<Query Kind="Program" />

void Main()
{
	var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day20.txt"));
	var particles = input.Select(i => i.Split(new[] { ", " }, StringSplitOptions.None))
						.Select(i => new Particle
						{
							Position = new Point3D(i[0]),
							Velocity = new Point3D(i[1]),
							Acceleration = new Point3D(i[2])
						})
						.ToList();

	var part1 = particles.IndexOf(particles.OrderBy(p => p.Acceleration.Manhattan).First());
	part1.Dump();

	int timeSinceLastCollision = 0;
	while (timeSinceLastCollision < 1000)
	{
		var count = particles.Count();
		foreach (var p in particles)
		{
			p.Velocity.Add(p.Acceleration);
			p.Position.Add(p.Velocity);
		}
		particles = particles.GroupBy(p => new { p.Position.X, p.Position.Y, p.Position.Z })
							 .Where(p => p.Count() == 1)
							 .Select(p => p.Single())
							 .ToList();
		timeSinceLastCollision++;
		if (particles.Count() != count)
			timeSinceLastCollision = 0;
	}
	var part2 = particles.Count();
	part2.Dump();
}

public class Point3D
{
	static Regex re = new Regex(@"<([^,]*),([^,]*),([^,]*)>");
	public long X { get; set; }
	public long Y { get; set; }
	public long Z { get; set; }
	public Point3D(string s)
	{
		var m = re.Match(s);
		X = int.Parse(m.Groups[1].Value);
		Y = int.Parse(m.Groups[2].Value);
		Z = int.Parse(m.Groups[3].Value);
	}
	public long Manhattan => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
	public void Add(Point3D other)
	{
		X += other.X;
		Y += other.Y;
		Z += other.Z;
	}
}

public class Particle
{
	public Point3D Position { get; set; }
	public Point3D Velocity { get; set; }
	public Point3D Acceleration { get; set; }
}