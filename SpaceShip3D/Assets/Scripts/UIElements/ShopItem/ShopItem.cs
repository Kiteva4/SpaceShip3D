using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
	private ItemsManager itemManager;
	private GamePlayerSkins gamePlayerSkins;
	private GameCoins gameCoins;

	[SerializeField] int itemIndex;
	[SerializeField] int itemPrice;
	[SerializeField] TextMeshProUGUI itemPriceTextActive;
	[SerializeField] TextMeshProUGUI itemPriceTextPassive;

	[SerializeField] private GameObject buttonSelectStateOn;
	[SerializeField] private GameObject buttonSelectStateOff;
	[SerializeField] private GameObject buttonBuySateOn;
	[SerializeField] private GameObject buttonBuySateOff;

	private void Awake()
	{
		itemManager = FindObjectOfType<ItemsManager>();
		gamePlayerSkins = FindObjectOfType<GamePlayerSkins>();
		gameCoins = FindObjectOfType<GameCoins>();

		//gameCoins.OnCoinsCountChanged += SetupBuyButtons;
		gamePlayerSkins.OnSkinChanged += SetupSelectionButtons;

		if (itemIndex == 0)
			PlayerPrefs.SetInt("item_" + itemIndex, 1);

		AddListenedrToButtons();
	}

	private void Start()
	{
		SetupItemPriceText();

		SetupBuyButtons(gameCoins.GetCoinsCount());
		SetupSelectionButtons(gamePlayerSkins.GetSelectedSkinIndex());
	}

	private void AddListenedrToButtons()
	{
		buttonSelectStateOn.GetComponent<Button>().onClick.AddListener(delegate { HandlerOnButtonSelectSkinClicked(); });
		buttonBuySateOn.GetComponent<Button>().onClick.AddListener(delegate { HandlerOnButtonBuySkinClicked(); });
	}

	private void SetupBuyButtons(int coins)
	{
		if (!PlayerPrefs.HasKey("item_" + itemIndex))
		{
			if (itemPrice <= coins) ActivateOnlyButton(buttonBuySateOn);
			else ActivateOnlyButton(buttonBuySateOff);
		}
	}

	private void SetupSelectionButtons(int selectedIndex)
	{
		if (PlayerPrefs.GetInt("item_" + itemIndex) == 1)
		{
			if (itemIndex != selectedIndex) ActivateOnlyButton(buttonSelectStateOn);
			else ActivateOnlyButton(buttonSelectStateOff);
		}
	}

	//private void SetupSkinMaterial()
	//{
	//	//TODO
	//}

	//private void OnChangeSkinMaterialColor()
	//{
	//	//TODO
	//}

	private void HandlerOnButtonBuySkinClicked()
	{
		if(gameCoins.Payed(itemPrice))
		{
			PlayerPrefs.SetInt("item_" + itemIndex, 1);
			ActivateOnlyButton(buttonSelectStateOn);
		}
	}

	private void HandlerOnButtonSelectSkinClicked()
	{
		gamePlayerSkins.ChangeSelectedPlayerSkin(itemIndex);
	}

	private void ActivateOnlyButton(GameObject go)
	{
		buttonSelectStateOn.SetActive(false);
		buttonSelectStateOff.SetActive(false);
		buttonBuySateOn.SetActive(false);
		buttonBuySateOff.SetActive(false);

		go.SetActive(true);
	}

	private void SetupItemPriceText()
	{
		itemPriceTextActive.text = itemPrice.ToString();
		itemPriceTextPassive.text = itemPrice.ToString();
	}
}
