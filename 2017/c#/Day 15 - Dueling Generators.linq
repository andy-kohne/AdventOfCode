<Query Kind="Statements" />

uint generate(uint last, int factor) => (uint)((ulong)(last * factor) % 2147483647);
bool isMatch(uint? a, uint? b) => ((a << 48) >> 48) == ((b << 48) >> 48);

Func<uint?, uint> GenA = last => generate(last ?? 516, 16807);
Func<uint?, uint> GenB = last => generate(last ?? 190, 48271);

int countMatches(Func<uint?, uint> a, Func<uint?, uint> b, int iterations)
{
	uint? lastA = null, lastB = null;
	int count = 0;
	for (var i = 0; i < iterations; i++)
	{
		lastA = a(lastA);
		lastB = b(lastB);
		if (isMatch(lastA, lastB))
			count++;
	}
	return count;
};

var part1 = countMatches(GenA, GenB, 40000000);
part1.Dump();

uint constrainedGenerate(Func<uint?, uint> gen, uint? last, int f)
{
	do
	{
		last = gen(last);
	} while ((last % f != 0));
	return last.Value;
};

var p2 = countMatches(last => constrainedGenerate(GenA, last, 4), last => constrainedGenerate(GenB, last, 8), 5000000);
p2.Dump();


