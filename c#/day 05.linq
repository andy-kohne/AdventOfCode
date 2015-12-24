<Query Kind="Statements">
  <Output>DataGrids</Output>
</Query>

var strings = File.ReadLines(Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName (Util.CurrentQueryPath)),"..","day5.txt"));


// part 1
var regexVowels = new Regex(@"(.*[aeiou].*){3,}", RegexOptions.IgnoreCase);
var regexDoubled = new Regex(@"(.)\1", RegexOptions.IgnoreCase);
var regexExclude = new Regex(@"(ab|cd|pq|xy)", RegexOptions.IgnoreCase);

strings.Where (s => regexVowels.IsMatch(s) && regexDoubled.IsMatch(s) && !regexExclude.IsMatch(s)).Count().Dump();


// part 2
var regex1 = new Regex(@"(.{2}).*\1", RegexOptions.IgnoreCase);
var regex2 = new Regex(@"(.).\1", RegexOptions.IgnoreCase);

strings.Where (s => regex1.IsMatch(s) && regex2.IsMatch(s)).Count().Dump();;