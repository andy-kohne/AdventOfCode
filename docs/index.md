---
layout: default
---

## 2017
### Day 1 - [Inverse Captcha](https://github.com/andy-kohne/AdventOfCode/blob/master/2017/c%23/Day%201.linq)

```csharp
var part1 = input.Zip(input.Skip(1).Concat(input), (a, b) => a == b ? a - '0' : 0).Sum();
var part2 = input.Zip(input.Skip(input.Length / 2).Concat(input), (a, b) => a == b ? a - '0' : 0).Sum();
```