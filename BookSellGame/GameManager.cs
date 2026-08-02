using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BookSellGame.Core;

[System.Serializable]
public class CratePriceConfig
{
    public int price;
    public string NameCrate;
    public string DescriptionCrate;
    public List<BookProbability> bookProbabilities;
}
[System.Serializable]
public enum typeBook
{
    crime,
    fantasy,
    drama,
  kid,
  classic,
  travel,
  fact,

}
public enum sizeBook
{
    size1,
    size2,
    size3,
}
public enum khuVuc
{
    khu1,
    khu2,
    khu3,
    khu4,
    khu5,

}

[System.Serializable]
public class BookProbability
{
    public typeBook type;
    public float percentage; // 0-100, sum should be 100
}

[System.Serializable]
public class StorageData
{
    public Dictionary<typeBook, int> bookCounts = new Dictionary<typeBook, int>();
    public int maxCapacity = 88;
    public int CurrentCount
    {
        get
        {
            int total = 0;
            foreach (var kv in bookCounts) total += kv.Value;
            return total;
        }
    }
    public bool CanAdd(int amount) => CurrentCount + amount <= maxCapacity;
    public void Add(typeBook bookId, int amount)
    {
        if (!bookCounts.ContainsKey(bookId)) bookCounts[bookId] = 0;
        bookCounts[bookId] += amount;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager ins;
    public int PlayerWallet;
    public GameStartSetting GameStartSetting;
    public SLbook SLbook;
public RegionConfig CurrentRegionConfig;
public static event System.Action<RegionConfig> OnRegionChanged;
    
    // Crate configurations now come from GameStartSetting ScriptableObject
    public List<CratePriceConfig> crateConfigs;

    public StorageData Storage = new StorageData();

    public delegate void OnChangeState();
    public static event OnChangeState onChangeStat;

    public delegate void StorageChanged();
    public static event StorageChanged OnStorageChanged;
    public delegate void WalletChanged(int newAmount);
    public static event WalletChanged OnWalletChanged;
   
    // Public method to raise storage changed event from other classes
    public static void NotifyStorageChanged()
    {
        OnStorageChanged?.Invoke();
        EventBus.RaiseStorageChanged();
    }

    // Public method to raise region change event from other classes
    public static void NotifyRegionChanged(RegionConfig newRegion)
    {
        OnRegionChanged?.Invoke(newRegion);
    }/// Returns true if successful, false if insufficient funds.
    /// </summary>
    public bool TrySpendTravelFee(int fee)
    {
        if (fee <= 0) return true; // no cost
        if (PlayerWallet < fee)
        {
            Debug.LogWarning($"Insufficient funds for travel fee {fee}. Current wallet: {PlayerWallet}");
            return false;
        }
        PlayerWallet -= fee;
        OnWalletChanged?.Invoke(PlayerWallet);
        return true;
    }

    void Awake()
    {
        if (ins == null) ins = this;
        else Destroy(gameObject);
    }
    public void traveler(int i)
    {
        
        switch (i)
        {
            case (int)khuVuc.khu1:
                {
                    PlayerWallet -= 20;
                    OnWalletChanged?.Invoke(PlayerWallet);
                    onChangeStat?.Invoke();
                    break;
                }
            case (int)khuVuc.khu2:
                {
                    PlayerWallet -= 40;
                    OnWalletChanged?.Invoke(PlayerWallet);
                    onChangeStat?.Invoke();
                    break;

                }
            case (int)khuVuc.khu3:
                {

                    PlayerWallet -= 50;
                    OnWalletChanged?.Invoke(PlayerWallet);
                    onChangeStat?.Invoke();
                    break;
                }
            case (int)khuVuc.khu4:
                {
                    PlayerWallet -= 50;
                    OnWalletChanged?.Invoke(PlayerWallet);
                    onChangeStat?.Invoke();
                    break;

                }
            case (int)khuVuc.khu5:
                {
                    PlayerWallet -= 80;
                    OnWalletChanged?.Invoke(PlayerWallet);
                    onChangeStat?.Invoke();
                    break;
                }
           
        }
    }    
    void Start()
    {
       
        randCrate();
          
        if (GameStartSetting != null)
        {
            PlayerWallet = GameStartSetting.moneyStart;
            
            crateConfigs = GameStartSetting.crateConfigs.Take(4).ToList();
        }
        else
        {
            Debug.LogError("GameStartSetting ScriptableObject not assigned in GameManager.");
        }
        // Optional: initialize storage for known book types
    }

    public void randCrate()
    {
        GameStartSetting.crateConfigs.Shuffle();
    }
        

    // Public method to purchase a crate by index (set in UI)
    public bool PurchaseCrate(int crateIndex)
    {
        if (crateConfigs == null || crateIndex < 0 || crateIndex >= crateConfigs.Count)
        {
            Debug.LogWarning("Invalid crate index requested.");
            return false;
        }
        var cfg = crateConfigs[crateIndex];
        if (PlayerWallet < cfg.price)
        {
            Debug.Log("Not enough money to purchase crate.");
            return false;
        }


        // Deduct money
        PlayerWallet -= cfg.price;
       
            EventBus.RaiseWalletChanged(PlayerWallet);

        // Determine which books to grant based on percentages
        List<typeBook> granted = new List<typeBook>();
        foreach (var prob in cfg.bookProbabilities)
        {
            // Simple chance roll per probability entry
            if (Random.value * 100f <= prob.percentage)
            {
                granted.Add(prob.type);
            }
        }

        // Add books to storage respecting capacity
        foreach (var bookId in granted)
        {
            if (Storage.CanAdd(1))
            {
                Storage.Add(bookId, 1);
            }
            else
            {
                Debug.Log("Storage capacity reached; remaining books discarded.");
                break;
            }
        }
        OnWalletChanged?.Invoke(PlayerWallet);
        OnStorageChanged?.Invoke();
            EventBus.RaiseStorageChanged();
        Debug.Log($"Crate purchased. Wallet: {PlayerWallet}. Storage count: {Storage.CurrentCount}/{Storage.maxCapacity}");
        return true;
    }
    // Returns owned books as a collection of id/count pairs
    public IEnumerable<KeyValuePair<typeBook, int>> GetOwnedBooks()
    {
        return Storage.bookCounts;
    }
}
