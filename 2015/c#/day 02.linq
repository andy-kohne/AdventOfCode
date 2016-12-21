<Query Kind="Statements">
  <Output>DataGrids</Output>
</Query>

var boxes = File.ReadAllLines(Path.Combine(Path.GetDirectoryName (Util.CurrentQueryPath),"..","day2.txt")).Where(b => !string.IsNullOrWhiteSpace(b)).Select (b => b.Split('x').Select(l => int.Parse(l)).ToArray());

// part 1
var paper = boxes.Select(b => 
{
	var surfaces = new [] { b[0] * b[1], b[1] * b[2], b[2] * b[0] };
	
	return surfaces.Sum (s => 2 * s) + surfaces.Min ( );
}).Sum();
paper.Dump(); 

// part 2
var ribbon = boxes.Select (b => 
{
	var perimeter = (new [] { b[0] + b[1], b[1] + b[2], b[2] + b[0] }).Min() * 2;
	var bow = b[0] * b[1] * b[2];
	return perimeter + bow;
}).Sum();
ribbon.Dump();