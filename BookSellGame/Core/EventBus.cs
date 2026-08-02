// EventBus.cs - simple decoupled event system for the game
using System;
using BookSellGame.Data;

namespace BookSellGame.Core
{
    public static class EventBus
    {
        // Wallet updates
        public static event Action<int> OnWalletChanged; // new amount
        public static void RaiseWalletChanged(int amount) { OnWalletChanged?.Invoke(amount); }

        // Storage updates
        public static event Action OnStorageChanged;
        public static void RaiseStorageChanged() { OnStorageChanged?.Invoke(); }

        // Decor equipped / purchased
        public static event Action<DecorItem> OnDecorEquipped;
        public static void RaiseDecorEquipped(DecorItem decor) { OnDecorEquipped?.Invoke(decor); }

        // Memento collected
        public static event Action<MementoItem> OnMementoCollected;
        public static void RaiseMementoCollected(MementoItem memento) { OnMementoCollected?.Invoke(memento); }

        // Special customer event (success)
        public static event Action<typeBook> OnSpecialCustomerSuccess; // the book type that satisfied the customer
        public static void RaiseSpecialCustomerSuccess(typeBook type) { OnSpecialCustomerSuccess?.Invoke(type); }
    }
}
