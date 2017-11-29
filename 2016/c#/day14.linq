<Query Kind="Statements">
  <Output>DataGrids</Output>
</Query>

var salt = "ihaygndm";
var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

var hashes = new List<string>();

Func<string, string> hashFunc = s => BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(s))).Replace("-","").ToLower();
Func<int, string> generateHash = i => hashFunc($"{salt}{i}");
Func<string, int, char[]> findSets = (src, count) => Regex.Matches(src, @"(.)\1{" + (count -1) + ",}").Cast<Match>().Select(m => m.Groups[1].Value[0] ).ToArray();  
Func<int, bool> isKey = i => findSets(hashes[i],3).Take(1).Any(c => Enumerable.Range(i + 1, 1000).Any(e => findSets(hashes[e],5).Contains(c))) ;
Func<int, bool> enoughHashes = i => hashes.Count()  - i > 1000;

var keys = new List<int>();
var currentIndex = 0;

while (keys.Count < 64)
{
	do
	{
		currentIndex++;
		while (!enoughHashes(currentIndex))
		{
			hashes.Add(generateHash(hashes.Count ));
		}
	}while (!isKey(currentIndex));
	keys.Add(currentIndex);
}
keys[63].Dump();


//part 2
var keys2 = new List<int>();
hashes.Clear();
currentIndex = 0;

while (keys2.Count < 64)
{
	do
	{
		currentIndex++;
		while (!enoughHashes(currentIndex))
		{
			var h = generateHash(hashes.Count());
			for (int i = 0; i < 2016; i++)
			{
				h = hashFunc(h);
			}
			hashes.Add(h);
		}
	} while (!isKey(currentIndex));
	keys2.Add(currentIndex);
}
keys2[63].Dump();
