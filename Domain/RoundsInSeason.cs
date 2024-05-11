using System;

namespace TipItService.Domain
{
    public class RoundsInSeason
    {
        public int Value { get; }
        public RoundsInSeason(int rounds)
        {
            if (rounds < 20 || rounds > 32)
                throw new ArgumentException(
                    "rounds must be between 20 and 32");
            Value = rounds;
        }
    }
}
