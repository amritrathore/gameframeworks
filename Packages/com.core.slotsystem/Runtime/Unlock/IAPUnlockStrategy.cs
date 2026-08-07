using System;

namespace Core.SlotSystem
{
    public sealed class IAPUnlockStrategy
        : ISlotUnlockStrategy
    {
        private readonly IIAPService iapService;
        private readonly string productId;

        public IAPUnlockStrategy(
            IIAPService iapService,
            string productId)
        {
            this.iapService =
                iapService ??
                throw new ArgumentNullException(
                    nameof(iapService));

            if (string.IsNullOrWhiteSpace(productId))
                throw new ArgumentException(
                    "Product ID cannot be empty.",
                    nameof(productId));

            this.productId = productId;
        }

        public bool CanUnlock(Slot slot)
        {
            if (slot.IsUnlocked)
                return true;

            return iapService.IsPurchased(productId);
        }

        public bool TryUnlock(Slot slot)
        {
            if (slot.IsUnlocked)
                return true;

            if (!iapService.IsPurchased(productId))
                return false;

            return slot.MarkUnlocked();
        }
    }
}