---
layout: default
---

## 2017

### Day 2 - [Corruption Checksum](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day%202%20-%20Corruption%20Checksum.linq)

```csharp
var part1 = lines.Select(l => l.Max() - l.Min()).Sum();

Func<int[], int> EvenlyDivisible = src => { for (int o = 0; o < src.Length; o++) { for (int i = 0; i < src.Length; i++){ if (i != o && src[o] % src[i] == 0) return src[o]/src[i];}}; return 0;};
var part2 = lines.Select(l => EvenlyDivisible(l.ToArray())).Sum();
```

### Day 1 - [Inverse Captcha](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day%201.linq)

```csharp
var part1 = input.Zip(input.Skip(1).Concat(input), (a, b) => a == b ? a - '0' : 0).Sum();
var part2 = input.Zip(input.Skip(input.Length / 2).Concat(input), (a, b) => a == b ? a - '0' : 0).Sum();
```

or, without duplicating the input...

```csharp
var part1 = input.Where((c, i) => c == input[(i + 1) % input.Length]).Sum(c => c - '0');
var part2 = input.Where((c, i) => c == input[(i + input.Length/2) % input.Length]).Sum(c => c - '0');
```