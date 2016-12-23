<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

var input = "cxdnnyjw";
var key = "";

using (MD5 md5Hash = MD5.Create())
{
	byte[] hash;
	var counter = 0;

	while (key.Length < 8)
	{
		do
		{
			counter++;
			hash = md5Hash.ComputeHash(Encoding.ASCII.GetBytes(input + counter));
		} while (hash[0] != 0 || hash[1] != 0 || hash[2] > 0xf);
		var hs = BitConverter.ToString(hash).Replace("-", "");
		key += hs[5];
	}
}
Console.WriteLine($"Part 1: {key}");


var newKey = new char?[8];
using (MD5 md5Hash = MD5.Create())
{
	byte[] hash;
	var counter = 0;

	while (newKey.Any(k => k == null))
	{
		do
		{
			counter++;
			hash = md5Hash.ComputeHash(Encoding.ASCII.GetBytes(input + counter));
		} while (hash[0] != 0 || hash[1] != 0 || hash[2] > 0xf);
		var hs = BitConverter.ToString(hash).Replace("-", "");
		if (hs[5] < '0' || hs[5] > '7') continue;
		var pos = int.Parse(hs.Substring(5,1));
		if (newKey[pos].HasValue) continue;
		newKey[pos] = hs[6];
	}
}
Console.WriteLine($"Part 2: {new string(newKey.Select(c => c.Value).ToArray())}");
