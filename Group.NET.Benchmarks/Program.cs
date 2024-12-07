using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;

[ShortRunJob]
[MemoryDiagnoser]
public class TreeBenchmark
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<TreeBenchmark>();
    }
}
