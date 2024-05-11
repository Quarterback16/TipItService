using System;
using TipItService.Domain;

namespace TipItService.Models
{
    public class Widget
    {
        public string Name { get; set; }
        public string InsertionTag { get; set; }
        public Func<TipSet, string, int, string> MdFunc { get; set; }

    }
}
