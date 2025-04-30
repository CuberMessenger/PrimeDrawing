using Prime.Net;
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.IO;
using System.IO.Compression;
using System.IO.Pipes;

(double, T) RunWithMsTimer<T>(Func<T> function) {
    Stopwatch stopwatch = new();

    stopwatch.Start();
    T result = function();
    stopwatch.Stop();

    return (stopwatch.Elapsed.TotalMilliseconds, result);
}

ulong[] TestSerial(ulong maximum) {
    PrimeFinder primeFinder = new(maximum, false);

    (double costOfFind, int dummy) = RunWithMsTimer(() => {
        primeFinder.Find();
        return 0;
    });

    (double costOfGetPrimes, ulong[] primes) = RunWithMsTimer(primeFinder.GetPrimes);

    Console.WriteLine($"Serial: {costOfFind} ms for Find, {costOfGetPrimes} ms for GetPrimes");

    return primes;
}

int viewChunkSize = 8;
//ulong maximum = 1UL << 32;
ulong maximum = 1UL << 32;

ulong[] serialPrimes = TestSerial(maximum);

Console.WriteLine($"There are {serialPrimes.Length} primes within the range [0, {maximum}]");

for (int i = 0; i < viewChunkSize; i++) {
    Console.WriteLine($"{i + 1}-th: {serialPrimes[i]}");
}

Console.WriteLine("......");

for (int i = serialPrimes.Length - viewChunkSize; i < serialPrimes.Length; i++) {
    Console.WriteLine($"{i + 1}-th: {serialPrimes[i]}");
}

// write the primes to a file
string outputFilePath = $"primes[{maximum}].bin";

try {
    using var fileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);
    using var gzipStream = new GZipStream(fileStream, CompressionMode.Compress);
    using var writer = new BinaryWriter(gzipStream);
    for (int i = 0; i < serialPrimes.Length; i++) {
        writer.Write(serialPrimes[i]);
    }
}
catch (Exception ex) {
    Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
}

/*
 It cost about 30 seconds on a Ryzen 5900HX to find all primes within the range [0, 2^32].
 Then cost a few dozen seconds to compress and write the result to a 297 MB binary file.
 */





