﻿#if PADDED_ATOMIC_LONG
using System.Runtime.InteropServices;
#endif
using System.Threading;
namespace SDDH.Metrics.Utils
{
    /// <summary>
    /// Atomic long.
    /// TBD: implement optimizations behind LongAdder from 
    /// <a href="https://github.com/dropwizard/metrics/blob/master/metrics-core/src/main/java/com/codahale/metrics/LongAdder.java">metrics-core</a>
    /// </summary>
#if PADDED_ATOMIC_LONG
    [StructLayout(LayoutKind.Explicit, Size = 64 * 2)]
#endif
    public struct AtomicLong
    {

#if PADDED_ATOMIC_LONG
        [FieldOffset(64)]
#endif
        private long value;

        public AtomicLong(long value)
        {
            this.value = value;
        }

        public long Value
        {
            get
            {
                return Thread.VolatileRead(ref this.value);
            }
        }

        public void SetValue(long value)
        {
            Thread.VolatileWrite(ref this.value, value);
        }

        public long Add(long value)
        {
            return Interlocked.Add(ref this.value, value);
        }

        public long Increment()
        {
            return Interlocked.Increment(ref this.value);
        }

        public long Decrement()
        {
            return Interlocked.Decrement(ref this.value);
        }

        public long GetAndReset()
        {
            return GetAndSet(0L);
        }

        public long GetAndSet(long value)
        {
            return Interlocked.Exchange(ref this.value, value);
        }

        public bool CompareAndSet(long expected, long updated)
        {
            var value = Interlocked.CompareExchange(ref this.value, updated, expected);
            return value == expected;
        }
    }
}
