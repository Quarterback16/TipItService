using TipItService.Models;

namespace TipItService.Interfaces
{
    public interface IGameProcessor
    {
        void ProcessGame(Game g, int num);
    }
}
