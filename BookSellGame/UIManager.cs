using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;


[System.Serializable]
public class SachwithUI
{
    public string Name;
    public Sprite sprite;
}
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public List<shopManager> CrateUI;
    public CanvasGroup MainGame;
    public CanvasGroup MainMenu;
    public CanvasGroup Setting;
    public GameObject SettingUI;
    // Prefab for displaying a purchased book entry
    public GameObject SLbookPrefab; // assign a prefab with SLbook component
    public CanvasGroup Shop;
    public GameObject ShopUI;
    public CanvasGroup Decoy;
    public GameObject DecoyUI;
    public TextMeshProUGUI CoinText;
    public GameObject CoinUI;
    public GameObject CoinAndSetting;
    public GameObject BackUI;
   public CanvasGroup ShowSachAfterBuy;
    public GameObject ShowSachAfterBuyCha;
    public List<RectTransform> Vitrifake;
    public List<SachwithUI> SachIconA;
    public void Awake()
    {
        Instance = this;
        // Subscribe to wallet updates so CoinText stays in sync
        GameManager.OnWalletChanged += UpdateCoinDisplay;
    }
    void Start()
    {
        CoinAndSetting.SetActive(true);
        MainMenu.gameObject.SetActive(true) ;
        GameManager.OnStorageChanged += showSachAfter;
    }
    private void OnDisable()
    {
        GameManager.OnStorageChanged -= showSachAfter;
        GameManager.OnWalletChanged -= UpdateCoinDisplay;
    }
    public void showSachAfter()
    {
        // Refresh the UI that shows the books the player owns after a purchase
        PopulateBookUI();
        ShowSachAfterBuy.gameObject.SetActive(true);
    }
    public void onPressComplete()
    {
        foreach (Transform child in ShowSachAfterBuyCha.transform)
        {
            if (child.gameObject != null) Destroy(child.gameObject);
        }
        ShowSachAfterBuy.gameObject.SetActive(false);

    }
    private void UpdateCoinDisplay(int newAmount)
    {
        if (CoinText != null) CoinText.text = newAmount.ToString();
    }
    private void PopulateBookUI()
    {
        // Clear existing items in container
        foreach (Transform child in ShowSachAfterBuyCha.transform)
        {
            if (child.gameObject != null) Destroy(child.gameObject);
        }
        // Populate based on GameManager storage, positioning each entry using Vitrifake placeholders
        var bookList = GameManager.ins.GetOwnedBooks().ToList();
        for (int i = 0; i < bookList.Count && i < Vitrifake.Count; i++)
        {
            var bookData = bookList[i];
            var obj = Instantiate(GameManager.ins.SLbook, ShowSachAfterBuyCha.transform);
            var slBook = obj.GetComponent<SLbook>();
            if (slBook != null) slBook.Setup(bookData);
            // Set position using corresponding RectTransform placeholder
            var rt = obj.GetComponent<RectTransform>();
            if (rt != null && Vitrifake[i] != null)
            {
                rt.anchoredPosition = Vitrifake[i].anchoredPosition;
            }
        }
    }
    public void StartGame()
    {
        GameST(true);
    }
    public void GameBack()
    {
        GameST(false);
    }    
    public void GameST(bool S)
    {
        MainMenu.gameObject.SetActive(!S);
        MainGame.gameObject.SetActive(S);
     if (S==true) {  RandomInon.FadeOut(MainGame); CoinUI.SetActive(true);}
     else 
        { RandomInon.FadeOut(MainMenu); CoinUI.SetActive(false); } 
    }
    bool shopOn = true;
    public void GameShop()
    {

        Setting.gameObject.SetActive(false);
        Decoy.gameObject.SetActive(false);
        Shop.gameObject.SetActive(true);
        BackUI.SetActive(true);
        RandomInon.FadeOut(Shop);
        if (shopOn==true)
        { loadUIshop(); shopOn = false; }  
        
    }

    public void loadUIshop()
    {
        for (int e = 0; e<4;e++)
        {
            CrateUI[e].IdCrate = e;
            CrateUI[e].NameCrate.SetText( GameManager.ins.crateConfigs[e].NameCrate);
            CrateUI[e].DescriptionCrate.SetText(GameManager.ins.crateConfigs[e].DescriptionCrate);
            CrateUI[e].costCrate.SetText(GameManager.ins.crateConfigs[e].price.ToString());
            CrateUI[e].ShowNewIcon.SetActive(true);
        }    
    }    
    public void GameDecoy()
    {
        Setting.gameObject.SetActive(false);
        Shop.gameObject.SetActive(false);
        Decoy.gameObject.SetActive(true);
        RandomInon.FadeOut(Decoy);
        BackUI.SetActive(true);
    }    
    public void Back()
    {
        BackUI.SetActive(false);
        Setting.gameObject.SetActive(false);
        Shop.gameObject.SetActive(false);
        Decoy.gameObject.SetActive(false);
    }    
    public void OnGameSetting()
    {
        Back();
        Shop.gameObject.SetActive(false);
        Decoy.gameObject.SetActive(false);
        SettingUI.SetActive(false);
        Setting.gameObject.SetActive(true);
        RandomInon.FadeOut(Setting);

    }    
    public void OffGameSetting()
    {
        Back();
        SettingUI.SetActive(true);
        RandomInon.FadeIn(Setting);
    }    
   
}
