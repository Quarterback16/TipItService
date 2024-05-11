namespace TipItService.Interfaces
{
    public interface ITipster
    {
        string ShowTips(
            string league,
            int round);
    }
}
