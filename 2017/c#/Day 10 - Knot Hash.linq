<Query Kind="Statements">
  <Namespace>System.Drawing</Namespace>
</Query>

var input = new[] { 34,88,2,222,254,93,150,0,199,255,39,32,137,136,1,167};


int[] knot(int[] key, int cycles)
{
	var clist = Enumerable.Range(0,256).ToArray();	var pos = 0;
	var skip = 0;
	
	foreach (var i in key)
	{
		for (int s=0; s<i/2; s++){
			var a = (pos+s)%clist.Length;
			var b = (pos+i-s-1)%clist.Length;
			var t = clist[a];
			clist[a] = clist[b];
			clist[b] = t;
		}
	
		pos += (i + skip);
		pos = pos % clist.Length;
	
		skip++;
	}
	
	return clist;
}

var output = knot(input, 1);

var part1 = output[0] * output[1];
part1.Dump();



