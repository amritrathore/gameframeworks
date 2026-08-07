namespace Core.SlotSystem
{
    public interface ICurrencyService
    {
        bool HasEnough(
            string currencyId,
            int amount);

        bool TrySpend(
            string currencyId,
            int amount);
    }
}