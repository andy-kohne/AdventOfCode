<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

var key = "yzbqklnj";
var keyBytes = Encoding.ASCII.GetBytes(key);


// part 1
using (MD5 md5Hash = MD5.Create())
{
	byte[] hash;
	var counter = -1;
	do{
		counter++;
		hash = md5Hash.ComputeHash(Encoding.ASCII.GetBytes(key + counter));
	} while (hash[0] != 0 || hash[1] != 0 || hash[2] > 15);
	
	counter.Dump();
}
	
	
// part 2
using (MD5 md5Hash = MD5.Create())
{
	byte[] data = keyBytes;
	byte[] hash;
	int currentLimit = -1;
	var counter = -1;
	int temp;
	int count;
	do{
		counter++;
		if (counter > currentLimit) 
		{
			Array.Resize(ref data, data.Length + 1);
			Array.Copy(keyBytes, data, keyBytes.Length);		
			currentLimit = (int)Math.Pow(10, data.Length - keyBytes.Length) - 1;
		}
		count = 0;
		temp = counter;
		while (temp > 0)
		{
			count++;
			data[data.Length - count] = (byte)(temp % 10 + 48);
			temp = temp / 10;
		}
		hash = md5Hash.ComputeHash(data);
	} while (hash[0] != 0 || hash[1] != 0 || hash[2] != 0);
	
	counter.Dump();
}