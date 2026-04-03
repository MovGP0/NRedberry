```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8037/25H2/2025Update/HudsonValley2)
AMD Ryzen 9 7900X 4.70GHz, 1 CPU, 24 logical and 12 physical cores
.NET SDK 10.0.201
  [Host]   : .NET 10.0.5 (10.0.5, 10.0.526.15411), X64 RyuJIT x86-64-v4
  ShortRun : .NET 10.0.5 (10.0.5, 10.0.526.15411), X64 RyuJIT x86-64-v4

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method                | Mean     | Error    | StdDev    | Gen0   | Gen1   | Allocated |
|---------------------- |---------:|---------:|----------:|-------:|-------:|----------:|
| SparsePseudoRemainder | 3.629 μs | 1.406 μs | 0.0771 μs | 0.4654 | 0.0038 |   7.66 KB |
