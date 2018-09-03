﻿using System;
using System.Linq;
using SDDH.Metrics.Utils;

namespace SDDH.Metrics.MetricData
{
    /// <summary>
    /// The value reported by a Meter Metric
    /// </summary>
    public sealed class MeterValue
    {
        public struct SetItem
        {
            public readonly string Item;
            public readonly double Percent;
            public readonly MeterValue Value;

            public SetItem(string item, double percent, MeterValue value)
            {
                this.Item = item;
                this.Percent = percent;
                this.Value = value;
            }
        }

        public readonly long Count;
        public readonly long UnitCount;
        public readonly double MeanRate;
        public readonly double OneMinuteRate;
        public readonly double FiveMinuteRate;
        public readonly double FifteenMinuteRate;
        public readonly TimeUnit RateUnit;
        public readonly SetItem[] Items;

        internal MeterValue(long count,long unitCount, double meanRate, double oneMinuteRate, double fiveMinuteRate, double fifteenMinuteRate, TimeUnit rateUnit)
            : this(count,unitCount, meanRate, oneMinuteRate, fiveMinuteRate, fifteenMinuteRate, rateUnit, new SetItem[0]) { }

        public MeterValue(long count,long unitCount, double meanRate, double oneMinuteRate, double fiveMinuteRate, double fifteenMinuteRate, TimeUnit rateUnit, SetItem[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            this.Count = count;
            this.UnitCount = unitCount;
            this.MeanRate = meanRate;
            this.OneMinuteRate = oneMinuteRate;
            this.FiveMinuteRate = fiveMinuteRate;
            this.FifteenMinuteRate = fifteenMinuteRate;
            this.RateUnit = rateUnit;
            this.Items = items;
        }
        public MeterValue Scale(TimeUnit unit)
        {
            if (unit == this.RateUnit)
            {
                return this;
            }

            var factor = unit.ScalingFactorFor(TimeUnit.Seconds);
            return new MeterValue(this.Count,
                this.UnitCount,
                this.MeanRate * factor,
                this.OneMinuteRate * factor,
                this.FiveMinuteRate * factor,
                this.FifteenMinuteRate * factor,
                unit,
                this.Items.Select(i => new SetItem(i.Item, i.Percent, i.Value.Scale(unit))).ToArray());
        }
    }

    /// <summary>
    /// Combines the value of the meter with the defined unit and the rate unit at which the value is reported.
    /// </summary>
    public sealed class MeterValueSource : MetricValueSource<MeterValue>
    {
        public MeterValueSource(string name, MetricValueProvider<MeterValue> value, Unit unit, TimeUnit rateUnit, MetricTags tags)
            : base(name, new ScaledValueProvider<MeterValue>(value, v => v.Scale(rateUnit)), unit, tags)
        {
            this.RateUnit = rateUnit;
        }

        public TimeUnit RateUnit { get; private set; }
    }
}
