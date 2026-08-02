using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using BookSellGame.Core;


[System.Serializable]
public class SachwithUI
{
    public typeBook Name;
    public Sprite sprite;
}
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public List<shopManager> CrateUI;
    public CanvasGroup ShopMGame;
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
    public CanvasGroup GameSapBook;
    public GameObject Giasach;
    public GameObject Map;
    public CanvasGroup UIChonSach;
    public CanvasGroup UISoluongSach;

    public void Awake()
    {
        Instance = this;
        // Subscribe to wallet updates so CoinText stays in sync
        EventBus.OnWalletChanged += UpdateCoinDisplay;
    }
    void Start()
    {
        CoinAndSetting.SetActive(true);
        MainMenu.gameObject.SetActive(true) ;
        GameManager.OnStorageChanged += showSachAfter;
        GameManager.onChangeStat += GotoXapXepbook;
    }
    private void OnDisable()
    {
        GameManager.OnStorageChanged -= showSachAfter;
        EventBus.OnWalletChanged -= UpdateCoinDisplay;
        GameManager.onChangeStat -= GotoXapXepbook;
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
    public void onCompleteToGo()
    {
        Map.SetActive(true);
        UISoluongSach.gameObject.SetActive(true);
        RandomInon.FadeOut(UISoluongSach);
        GameSapBook.gameObject.SetActive(false);
        Giasach.SetActive(false);
    }    
    public void chooseBook ()
    { }
    private void UpdateCoinDisplay(int newAmount)
    {
        if (CoinText != null) CoinText.text = newAmount.ToString();
    }
    private void PopulateBookUI()
    {
        
        foreach (Transform child in ShowSachAfterBuyCha.transform)
        {
            if (child.gameObject != null) Destroy(child.gameObject);
        }
       
        var bookList = GameManager.ins.GetOwnedBooks().ToList();
        for (int i = 0; i < bookList.Count && i < Vitrifake.Count; i++)
        {
            var bookData = bookList[i];
            
            var placeholder = Vitrifake[i];
            var obj = Instantiate(SLbookPrefab, ShowSachAfterBuyCha.transform);
            var slBook = obj.GetComponent<SLbook>();
            if (slBook != null) slBook.Setup(bookData);
            
            var rt = obj.GetComponent<RectTransform>();
            if (rt != null && placeholder != null)
            {
                rt.SetParent(ShowSachAfterBuyCha.transform);
                rt.anchoredPosition = placeholder.anchoredPosition;
                rt.position = placeholder.position;
                rt.pivot = placeholder.pivot;
            }
        }
    }
    public void GotoXapXepbook()
    {
      Sapxep(false);
    } 
    public void XapXepToShop()
    {
        Sapxep(true);
    }    
    
    public void StartGame()
    {
        GameST(true);
    }
    public void GameBack()
    {
        GameST(false);
    }
    private void Sapxep(bool S)
    {
        if (S == true)
        {
            ShopMGame.gameObject.SetActive(S);
            RandomInon.FadeOut(ShopMGame);
        }
        else
        {
            RandomInon.FadeIn(ShopMGame);
            ShopMGame.gameObject.SetActive(S);
        }
        Giasach.SetActive(!S);
        GameSapBook.gameObject.SetActive(!S);
        RandomInon.FadeOut(GameSapBook);
    }    
    public void GameST(bool S)
    {
        MainMenu.gameObject.SetActive(!S);
        ShopMGame.gameObject.SetActive(S);
     if (S==true) {  RandomInon.FadeOut(ShopMGame); CoinUI.SetActive(S);}
     else 
        { RandomInon.FadeOut(MainMenu); CoinUI.SetActive(S); } 
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
