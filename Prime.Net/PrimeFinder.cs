using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Net {
    internal class PrimeFinder {
        // This class implements a bitwise Sieve of Eratosthenes algorithm.
        // It supports finding primes up to 2^37 because it use a single ulong arrary for storing bits.

        private const ulong Supremum = 1UL << 37;

        private const int ULBits = 64;

        private const int LogULBits = 6;

        private ulong Maximum { get; set; }

        private int NumBlock { get; set; }

        private ulong[] Bits { get; set; }

        private bool Verbose { get; set; }

        private bool GetBit(ulong index) => (Bits[index >> LogULBits] & (1UL << (int)(index & (ULBits - 1)))) != 0;

        private void PressBit(ulong index) => Bits[index >> LogULBits] &= ~(1UL << (int)(index & (ULBits - 1)));

        private void LiftBit(ulong index) => Bits[index >> LogULBits] |= 1UL << (int)(index & (ULBits - 1));

        internal PrimeFinder(ulong maximum, bool verbose) {
            if (maximum >= Supremum) {
                throw new ArgumentOutOfRangeException(nameof(maximum), "Maximum must be less than 2^37.");
            }

            Maximum = maximum;
            Verbose = verbose;
            NumBlock = (int)Math.Ceiling((decimal)Maximum / ULBits);
            Bits = new ulong[NumBlock];

            // NumBits[0]: -------- -------- -------- -------- -------- -------- -------- --------
            //            63  ...    ......   ......   ......   ......   ......   ......  7......0
            //       7 6 5 4 3 2 1 0
            // fact: Y N Y N Y Y N N
            // init: Y N Y N Y N Y N
            for (int i = 0; i < NumBlock; i++) {
                Bits[i] = 0xAAAAAAAAAAAAAAAAUL;
            }

            PressBit(1UL); // 1 is not prime, by definition
            LiftBit(2UL); // 2 is prime
            Verbose = verbose;
        }

        internal void Find(ulong start = 3UL, ulong end = 0UL) {
            end = end == 0UL ? Maximum : end;

            // 2 is prime, start from 3
            for (ulong i = start; i * i <= end; i += 2UL) {
                if (GetBit(i)) {
                    // flip all multiples of i
                    if (Verbose) {
                        Console.WriteLine($"Flipping multiples of {i}......");
                    }

                    for (ulong j = i * i; j <= end; j += i) {
                        PressBit(j);
                    }

                    if (Verbose) {
                        Console.WriteLine("Done flipping multiples of {i}......\n");
                    }
                }
            }
        }

        internal ulong[] GetPrimes() {
            List<ulong> primes = [2UL];

            for (ulong i = 3; i <= Maximum; i += 2) {
                if (GetBit(i)) {
                    primes.Add(i);
                }
            }

            return primes.ToArray();
        }


    }
}
