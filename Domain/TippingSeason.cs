using System;

namespace TipItService.Domain
{
    public class TippingSeason
    {
        public int Value { get; }
        public TippingSeason(int season)
        {
            if (season < 2020 || season > 2044)
                throw new ArgumentException(
                    "Tipping Season must be between 2020 and 2044");
            Value = season;
        }
    }
}
