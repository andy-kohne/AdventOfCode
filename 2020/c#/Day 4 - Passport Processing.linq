<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "..", "day4.txt"));

IEnumerable<Dictionary<string, string>> Parse(string[] raw) {
	var e = raw.GetEnumerator();
	var dict = new Dictionary<string,string>();
	while(e.MoveNext()){
		var line = (string)e.Current;
		if (string.IsNullOrEmpty(line))
		{
			yield return dict;
			dict = new Dictionary<string, string>();
			continue;
		}
		var items = line.Split(' ');
		foreach (var item in items)
		{
			var parts = item.Split(':');
			dict.Add(parts[0], parts[1]);
		}
	}
	yield return dict;
}
var passports = Parse(input);


var required = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
bool isValid(Dictionary<string,string> passport) => required.All(r => passport.ContainsKey(r));
var part1 = passports.Count(isValid);
part1.Dump();


var fields = new Dictionary<string, Func<string, bool>> {
	{ "byr", i => { var n = int.Parse(i); return n >= 1920 && n <= 2002; }},
	{ "iyr", i => { var n = int.Parse(i); return n >= 2010 && n <= 2020; }},
	{ "eyr", i => { var n = int.Parse(i); return n >= 2020 && n <= 2030; }},
	{ "hgt", i => { var m = Regex.Match(i, @"^(\d+)(cm|in)$"); return m.Success && int.TryParse(m.Groups[1].Value, out int n) && (m.Groups[2].Value == "cm" ? n >= 150 && n <= 193 : n>=59 && n <=76 ); }},
	{ "hcl", i => { return Regex.IsMatch(i, @"^#[0-9a-f]{6}$"); }},
	{ "ecl", i => { return i == "amb" || i == "blu" || i == "brn" || i == "gry" || i == "grn" || i == "hzl" || i == "oth"; }},
	{ "pid", i => { return Regex.IsMatch(i, @"^\d{9}$"); }},
};
bool isValidPartTwo (Dictionary<string,string> passport) => fields.All(f => passport.ContainsKey(f.Key) && f.Value(passport[f.Key]));
var part2 = passports.Count(isValidPartTwo);
part2.Dump();
