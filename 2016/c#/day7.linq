<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day7.txt"));

var re = new Regex(@"^(([a-z]+)?(\[[a-z]+\])?)*$");

Func<string, bool> SupportsTls = (address) => 
{
	
};
