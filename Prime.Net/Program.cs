using Prime.Net;
using System;


ulong maximum = 1UL << 32;
PrimeFinder primeFinder = new PrimeFinder(maximum);

//primeFinder.Find();
primeFinder.FindParallel();
ulong[] primes = primeFinder.GetPrimes();

for (int i = 0; i < primes.Length; i++) {
    Console.WriteLine($"The {i + 1}-th prime is {primes[i]}!");
}


