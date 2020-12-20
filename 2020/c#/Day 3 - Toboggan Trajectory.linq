<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day3.txt"));

bool isTree(int row, int col) => input[row][col % input[row].Length] == '#';

var part1 = Enumerable.Range(0, input.Length).Count(i => isTree(i, i*3));
part1.Dump();

var part2 = (long)Enumerable.Range(0, input.Length).Count(i => isTree(i, i))
		  * Enumerable.Range(0, input.Length).Count(i => isTree(i, i * 3))
		  * Enumerable.Range(0, input.Length).Count(i => isTree(i, i * 5))
		  * Enumerable.Range(0, input.Length).Count(i => isTree(i, i * 7))
		  * Enumerable.Range(0, input.Length / 2).Count(i => isTree(i * 2, i));
part2.Dump();
