using System;
using System.Linq;

namespace TipItService.Domain
{
    public class LeagueCode
    {
        public string Code { get; }
        public LeagueCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentException(
                    "code cant be empty");
            var validCodes = new string[]
            {
                "NRL",
                "AFL",
            };
            if (!validCodes.Contains(code))
                throw new ArgumentException(
                    $"{code} is not a valid league code");
            Code = code;
        }
    }
}
