using System.Collections.Generic;

namespace TipItService.Domain
{
    public class TipSet
    {
        public List<Tip> Tips { get; set; } = new List<Tip>();

        public TipSet()
        {
        }

        public TipSet(List<Tip> enumerable)
        {
            Tips = enumerable;
        }

    }
}
