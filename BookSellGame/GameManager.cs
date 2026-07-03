using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class CratePriceConfig
{
    public int price;
    public string NameCrate;
    public string DescriptionCrate;
    public List<BookProbability> bookProbabilities;
}

[System.Serializable]
public class BookProbability
{
    public string bookId;
    public float percentage; // 0-100, sum should be 100
}

[System.Serializable]
public class StorageData
{
    public Dictionary<string, int> bookCounts = new Dictionary<string, int>();
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
    public void Add(string bookId, int amount)
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
    // Crate configurations now come from GameStartSetting ScriptableObject
    public List<CratePriceConfig> crateConfigs;

    public StorageData Storage = new StorageData();

    // Events for UI updates
    public delegate void StorageChanged();
    public static event StorageChanged OnStorageChanged;
    public delegate void WalletChanged(int newAmount);
    public static event WalletChanged OnWalletChanged;
   
    // Public method to raise storage changed event from other classes
    public static void NotifyStorageChanged()
    {
        OnStorageChanged?.Invoke();
    }

    void Awake()
    {
        if (ins == null) ins = this;
        else Destroy(gameObject);
        
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
        OnWalletChanged?.Invoke(PlayerWallet);

        // Determine which books to grant based on percentages
        List<string> granted = new List<string>();
        foreach (var prob in cfg.bookProbabilities)
        {
            // Simple chance roll per probability entry
            if (Random.value * 100f <= prob.percentage)
            {
                granted.Add(prob.bookId);
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

        OnStorageChanged?.Invoke();
        Debug.Log($"Crate purchased. Wallet: {PlayerWallet}. Storage count: {Storage.CurrentCount}/{Storage.maxCapacity}");
        return true;
    }
    // Returns owned books as a collection of id/count pairs
    public IEnumerable<KeyValuePair<string, int>> GetOwnedBooks()
    {
        return Storage.bookCounts;
    }
}
