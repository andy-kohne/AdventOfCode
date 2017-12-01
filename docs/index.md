---
layout: default
---

## 2017
### Day 1 - [Inverse Captcha](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day%201.linq)

```csharp
var part1 = chars.Zip(chars.Skip(1).Concat(chars), (a, b) => a == b ? a - '0' : 0).Sum();
var part2 = chars.Zip(chars.Skip(chars.Count() / 2).Concat(chars), (a, b) => a == b ? a - '0' : 0).Sum();
```