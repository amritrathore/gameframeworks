using System;

namespace Core.SlotSystem
{
    public sealed class CurrencyUnlockStrategy
        : ISlotUnlockStrategy
    {
        private readonly ICurrencyService currencyService;
        private readonly string currencyId;
        private readonly int cost;

        public CurrencyUnlockStrategy(
            ICurrencyService currencyService,
            string currencyId,
            int cost)
        {
            this.currencyService =
                currencyService ??
                throw new ArgumentNullException(
                    nameof(currencyService));

            if (string.IsNullOrWhiteSpace(currencyId))
                throw new ArgumentException(
                    "Currency ID cannot be empty.",
                    nameof(currencyId));

            if (cost < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(cost));

            this.currencyId = currencyId;
            this.cost = cost;
        }

        public bool CanUnlock(Slot slot)
        {
            if (slot.IsUnlocked)
                return true;

            return currencyService.HasEnough(
                currencyId,
                cost);
        }

        public bool TryUnlock(Slot slot)
        {
            if (slot.IsUnlocked)
                return true;

            if (!currencyService.TrySpend(
                    currencyId,
                    cost))
            {
                return false;
            }

            return slot.MarkUnlocked();
        }
    }
}