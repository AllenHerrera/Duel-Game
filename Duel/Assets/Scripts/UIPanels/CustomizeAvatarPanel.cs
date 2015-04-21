using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class CustomizeAvatarPanel : menuPanel {

	private animationController avatar;
	private GameObject toggles;
	private GameObject Shop;
	private GameObject nullgoldtext;
	public Sprite[] vestsOptionsAvatar;
	public Sprite[] pantsOptionsAvatar;
	public Sprite[] hatsOptionsAvatar;
	public Sprite[] gunsOptionsAvatar;

	public Sprite[] vestsUnlocked;
	public Sprite[] hatsUnlocked;
	public Sprite[] pantsUnlocked;
	public Sprite[] gunsUnlocked;

	public int[] vestsIndex;
	public int[] hatsIndex;
	public int[] gunsIndex;
	public int[] pantsIndex;

	public int CurrentHatIndex;
	public int CurrentVestIndex;
	public int CurrenGunIndex;
	public int CurrentPantsIndex;

	public int CurrentHatSaleIndex;
	public int CurrentVestSaleIndex;
	public int CurrenGunSaleIndex;
	public int CurrentPantsSaleIndex;

	public int hatsUnlockedCounter;
	public int vestsUnlockedCounter;
	public int gunsUnlockedCounter;
	public int pantsUnlockedCounter;

	public Button shopMinusBtn;
	public Button shopPlusBtn;

	private int gold;
	
	public Text lockedText;
	public Text goldText;

	public Text costText;
	public Text allCostText;
	public int cost;
	public string mostRecentItemChangedTo;
	public string forSaleType;

	public Image currentImage;
	public Image vestImage;
	public Image gunImage;
	public Image pantsImage;

	GameObject optionpanelsound;
	// Use this for initialization
	void Start () {

		base.Start ();

	
		costText = GameObject.Find ("CostText").GetComponent<Text> ();
		currentImage = GameObject.Find ("HatsImage").GetComponent<Image> ();
		vestImage = GameObject.Find ("ShirtsImage").GetComponent<Image> ();
		gunImage = GameObject.Find ("GunsImage").GetComponent<Image> ();
		pantsImage = GameObject.Find ("PantsImage").GetComponent<Image> ();
		goldText = GameObject.Find ("GoldNumberText").GetComponent<Text> ();
		shopMinusBtn = GameObject.Find ("SaleBtnMinus1").GetComponent<Button>();
		shopPlusBtn = GameObject.Find ("SaleBtnPlus1").GetComponent<Button>();
		lockedText = GameObject.Find ("LockedorNotText").GetComponent<Text> ();

		currentImage.enabled = false;
		shopMinusBtn.enabled = false;
		shopPlusBtn.enabled = false;

		nullgoldtext = GameObject.Find ("NullGoldTextGameObject");
		nullgoldtext.SetActive (false);

		Shop= GameObject.Find ("ShopSection");
		Shop.SetActive (false);

		setImages ();//sets shop images to false

		toggles = GameObject.Find ("Toggles");
		toggles.SetActive (false);

		costText.enabled = false;

		vestsOptionsAvatar = Resources.LoadAll<Sprite> ("Vests");
		pantsOptionsAvatar = Resources.LoadAll<Sprite> ("Pants");
		hatsOptionsAvatar = Resources.LoadAll<Sprite> ("Hats");
		gunsOptionsAvatar = Resources.LoadAll<Sprite> ("Guns");

		vestsUnlocked = new Sprite [ vestsOptionsAvatar.Length];
		hatsUnlocked = new Sprite [hatsOptionsAvatar.Length];
		gunsUnlocked = new Sprite [gunsOptionsAvatar.Length];
		pantsUnlocked =new Sprite [ pantsOptionsAvatar.Length];

		hatsIndex= new int [ hatsOptionsAvatar.Length];
		vestsIndex = new int[vestsOptionsAvatar.Length];
		gunsIndex = new int[gunsOptionsAvatar.Length];
		pantsIndex = new int[pantsOptionsAvatar.Length]; 

		CurrentHatSaleIndex = 0;
		CurrentVestSaleIndex = 0;
		CurrenGunSaleIndex = 0;
		CurrentPantsSaleIndex = 0;

		LoadPlayerPrefsCustomize ();
	
		avatar = GameObject.Find("Player1").GetComponent<animationController>();
		 
		optionpanelsound = GameObject.Find ("OptionsPanel");
		SetPlayerSprite ();

		UpdateHatShop ();
		UpdateVestShop ();
		UpdateGunShop ();
		UpdatePantsShop ();


	}
	public void setImages (string Type)
	{
		if(Type == "hat")
		currentImage.enabled = true;
		   else 
		   currentImage.enabled = false;
		  
		if(Type == "vest")
			vestImage.enabled = true;
		else 
			vestImage.enabled = false;

		if(Type =="gun")
			gunImage.enabled = true;
		else 
			gunImage.enabled = false;

		if(Type == "pants")
			pantsImage.enabled = true;
		else 
			pantsImage.enabled = false;
		}
	public void setImages ()
	{
		currentImage.enabled = false;
		vestImage.enabled = false;
		gunImage.enabled = false;
		pantsImage.enabled = false;
	}

	public void LoadPlayerPrefsCustomize(){

		if (PlayerPrefs.GetInt ("Hat") == null)
			PlayerPrefs.SetInt ("Hat", 0);
		CurrentHatIndex = PlayerPrefs.GetInt ("Hat");
		if (PlayerPrefs.GetInt ("Vest") == null)
			PlayerPrefs.SetInt ("Vest", 0);
		CurrentVestIndex = PlayerPrefs.GetInt ("Vest");
		if (PlayerPrefs.GetInt ("Gun") == null)
			PlayerPrefs.SetInt ("Gun", 0);
		CurrenGunIndex = PlayerPrefs.GetInt ("Gun");
		if (PlayerPrefs.GetInt ("Pants") == null)
			PlayerPrefs.SetInt ("Pants", 0);
		CurrentPantsIndex = PlayerPrefs.GetInt ("Pants");

		updateGold ();

		updateSpriteArrays ();

	}
		public void updateSpriteArrays(){
		Debug.Log(PlayerPrefs.GetString ("savedSpritesArray"));
		//if saved array of sprites has been set up - do that
		if (PlayerPrefs.GetString ("savedSpritesArray") == "") {

			string temp = "hat";
			int DefaultCost = 5;
	
		/*-------------- Arraylike initailization ----------*/
			for (int i =1 ; i<hatsOptionsAvatar.Length;i++)
			{
				PlayerPrefs.SetInt(temp+i,0);
				PlayerPrefs.SetInt("cost"+temp+i,DefaultCost);

			}
			temp = "vest";
			for (int i =1 ; i<vestsOptionsAvatar.Length;i++)
			{
				PlayerPrefs.SetInt(temp+i,0);
				PlayerPrefs.SetInt("cost"+temp+i,DefaultCost);
				
			}
			temp = "gun";
			for (int i =1 ; i<gunsOptionsAvatar.Length;i++)
			{
				PlayerPrefs.SetInt(temp+i,0);
				PlayerPrefs.SetInt("cost"+temp+i,DefaultCost);
				
			}
			temp = "pants";
			for (int i =1 ; i<pantsOptionsAvatar.Length;i++)
			{
				PlayerPrefs.SetInt(temp+i,0);
				PlayerPrefs.SetInt("cost"+temp+i,DefaultCost);
				
			}

			/* ------------- Set Special Costs Here---------------*/
			PlayerPrefs.SetInt("costhat4",15);
			PlayerPrefs.SetInt("costvest4",15);
			PlayerPrefs.SetInt("costgun4",15);
			PlayerPrefs.SetInt("costpants4",15);
			
			/*-------------- Free Sprites -----------------------*/
			PlayerPrefs.SetInt("hat0",1);
			PlayerPrefs.SetInt("vest0",1);
			PlayerPrefs.SetInt("gun0",1);
			PlayerPrefs.SetInt("pants0",1);
			
		/*	PlayerPrefs.SetInt("hat3",1);
			PlayerPrefs.SetInt("vest3",1);
			PlayerPrefs.SetInt("gun3",1);
			PlayerPrefs.SetInt("pants3",1);  */

			Debug.Log("set saved skins: " + PlayerPrefs.GetInt("hat0") + "  ___ not null: " + PlayerPrefs.GetInt("hat2"));
			PlayerPrefs.SetString ("savedSpritesArray", "done");

		}


	

		/*-----load unlocked gear -----------------*/
		string temp2 = "hat";
		hatsUnlockedCounter = 0;
		for (int i =0 ; i<hatsOptionsAvatar.Length;i++)
		{
			if (PlayerPrefs.GetInt(temp2+i)== 1)
			{
				hatsUnlocked[hatsUnlockedCounter]= hatsOptionsAvatar[i];
				hatsIndex[hatsUnlockedCounter] = i;
				hatsUnlockedCounter++;			}
		}
		temp2 = "vest";
		vestsUnlockedCounter = 0;
		for (int i =0 ; i<vestsOptionsAvatar.Length;i++)
		{
			if (PlayerPrefs.GetInt(temp2+i)== 1)
			{
				vestsUnlocked[vestsUnlockedCounter]= vestsOptionsAvatar[i];
				vestsIndex[vestsUnlockedCounter] = i;
				vestsUnlockedCounter++;
			}
			
		}
		temp2 = "gun";
		gunsUnlockedCounter = 0;
		for (int i =0 ; i<gunsOptionsAvatar.Length;i++)
		{
			if (PlayerPrefs.GetInt(temp2+i)== 1)
			{
				gunsUnlocked[gunsUnlockedCounter]= gunsOptionsAvatar[i];
				gunsIndex[gunsUnlockedCounter] = i;
				gunsUnlockedCounter++;
			}
			
		}
		temp2 = "pants";
		pantsUnlockedCounter = 0;
		for (int i =0 ; i<pantsOptionsAvatar.Length;i++)
		{
			if (PlayerPrefs.GetInt(temp2+i)== 1)
			{
				pantsUnlocked[pantsUnlockedCounter]= pantsOptionsAvatar[i];
				pantsIndex[pantsUnlockedCounter] = i;
				pantsUnlockedCounter++;
			}
			
		} 
	}

	public void SetPlayerSprite()
	{
		avatar.setHat (CurrentHatIndex);
		avatar.setShirt (CurrentVestIndex);
		avatar.setGuns (CurrenGunIndex);
		avatar.setLegs (CurrentPantsIndex);

	}
	protected override void ProcessButtonPress(ButtonAction btn)
	{
		switch (btn) {
		case ButtonAction.returnToMain:
			uiController.instance.ShowPanel (uiController.instance.MainPanel);
			break;
		case ButtonAction.options:
			optionpanelsound.GetComponent<AudioSource> ().Play ();
			uiController.instance.ShowPanel(uiController.instance.OptionsPanel);
			break;
		}
	}
	public void returnBackToProfile() {

		ProcessButtonPress(ButtonAction.options);
	}
	public void updateGold(){

		gold = PlayerPrefs.GetInt ("gold");
		goldText.text = gold.ToString();

	}

	public void UpdateHatPlus1()
	{
		nullgoldtext.SetActive (false);
		if (CurrentHatIndex + 1 < hatsUnlockedCounter) {
			optionpanelsound.GetComponent<AudioSource> ().Play ();
			avatar.setHat (hatsIndex[CurrentHatIndex +1]);
			CurrentHatIndex += 1;
		} else {

			CurrentHatIndex = 0;
				avatar.setHat (hatsIndex[CurrentHatIndex]);
		}
		mostRecentItemChangedTo = "hat";



	}
	public void UpdateHatMinus1()
	{
		nullgoldtext.SetActive (false);
		if (CurrentHatIndex - 1 >= 0) {
			optionpanelsound.GetComponent<AudioSource> ().Play ();
			avatar.setHat (hatsIndex[CurrentHatIndex -1]);
			CurrentHatIndex -= 1;
		} else {

			CurrentHatIndex = hatsUnlockedCounter-1;
			avatar.setHat (hatsIndex[CurrentHatIndex]);
		}
		mostRecentItemChangedTo = "hat";


	}
	public void UpdateVestPlus1()
	{
		nullgoldtext.SetActive (false);
		if (CurrentVestIndex + 1 < vestsUnlockedCounter) {
			optionpanelsound.GetComponent<AudioSource> ().Play ();
			avatar.setShirt (vestsIndex[CurrentVestIndex +1]);
			CurrentVestIndex += 1;
		}
		else {
			
			CurrentVestIndex = 0;
			avatar.setShirt (vestsIndex[CurrentVestIndex]);
		}
		mostRecentItemChangedTo = "vest";
	
	}
	public void UpdateVestMinus1()
	{
		nullgoldtext.SetActive (false);
		if (CurrentVestIndex - 1 >= 0) {
			optionpanelsound.GetComponent<AudioSource> ().Play ();
				avatar.setShirt (vestsIndex[CurrentVestIndex -1]);
			CurrentVestIndex -= 1;
		}
		else {
			
			CurrentVestIndex = vestsUnlockedCounter-1;
				avatar.setShirt (vestsIndex[CurrentVestIndex]);
		}
		mostRecentItemChangedTo = "vest";

	}
	public void UpdataeGunPlus1()
	{
		nullgoldtext.SetActive (false);
		if (CurrenGunIndex + 1 < gunsUnlockedCounter) {
			optionpanelsound.GetComponent<AudioSource> ().Play ();
			avatar.setGuns (gunsIndex[CurrenGunIndex +1]);
			CurrenGunIndex += 1;
		}
		else {
			
			CurrenGunIndex = 0;
				avatar.setGuns (gunsIndex[CurrenGunIndex]);
		}
		mostRecentItemChangedTo = "gun";

	}
	public void UpdateGunMinus1()
	{
		nullgoldtext.SetActive (false);
		if (CurrenGunIndex - 1 >= 0) {
			optionpanelsound.GetComponent<AudioSource> ().Play ();
				avatar.setGuns (gunsIndex[CurrenGunIndex -1]);
			CurrenGunIndex -= 1;
		}
		else {
			
			CurrenGunIndex = gunsUnlockedCounter-1;
			avatar.setGuns (gunsIndex[CurrenGunIndex]);
		}
		mostRecentItemChangedTo = "gun";

	}
	public void UpdatePantsPlus1()
	{	
		nullgoldtext.SetActive (false);
		if (CurrentPantsIndex + 1 < pantsUnlockedCounter) {
			optionpanelsound.GetComponent<AudioSource> ().Play ();
				avatar.setLegs (pantsIndex[CurrentPantsIndex +1]);
			CurrentPantsIndex += 1;
		}
		else {
			
			CurrentPantsIndex = 0;
				avatar.setLegs (pantsIndex[CurrentPantsIndex]);
		}
		mostRecentItemChangedTo = "pants";

	}
	public void UpdatePantsMinus1()
	{
		nullgoldtext.SetActive (false);
		if (CurrentPantsIndex - 1 >= 0) {
			optionpanelsound.GetComponent<AudioSource> ().Play ();
				avatar.setLegs (pantsIndex[CurrentPantsIndex -1]);
			CurrentPantsIndex -= 1;
		}
		else {
			
			CurrentPantsIndex = pantsUnlockedCounter-1;
				avatar.setLegs (pantsIndex[CurrentPantsIndex]);
		}
		mostRecentItemChangedTo = "pants";

	}

	public void MasterBuyButton(){

	if (cost <= gold) {

			//only 1 will be bought
			if (mostRecentItemChangedTo == "hat") {
				BuyHatBtn ();
				PlayerPrefs.SetInt ("cost" + mostRecentItemChangedTo + CurrentHatSaleIndex, 0);
			}
			if (mostRecentItemChangedTo == "vest") {
				BuyVestBtn ();
				PlayerPrefs.SetInt ("cost" + mostRecentItemChangedTo + CurrentVestSaleIndex, 0);
			}
			if (mostRecentItemChangedTo == "gun") {
				BuyGunBtn ();
				PlayerPrefs.SetInt ("cost" + mostRecentItemChangedTo + CurrenGunSaleIndex, 0);
			}
			if (mostRecentItemChangedTo == "pants") {
				BuyPantstBtn ();
				PlayerPrefs.SetInt ("cost" + mostRecentItemChangedTo + CurrentPantsSaleIndex, 0);
			}
			Debug.Log ("buying shit");
			Debug.Log ("cost: " +cost);
			updateSpriteArrays();
			costText.text = "Cost: 0";
			gold = gold - cost;
			PlayerPrefs.SetInt ("gold", gold);
			updateGold();
			cost = 0;


		} else {
			// not enough gold
			nullgoldtext.SetActive (true);

		}
			
	}

	public void UpdateHatShop () {
		nullgoldtext.SetActive (false);
		if (PlayerPrefs.GetInt ("hat" + CurrentHatSaleIndex) == 0) {
			lockedText.text = "Locked";
			lockedText.color = Color.red;
		} else {
			lockedText.text = "Unlocked";
			lockedText.color = Color.green;
		}

		cost = PlayerPrefs.GetInt ("costhat" + CurrentHatSaleIndex);
		costText.text = "Cost: " + PlayerPrefs.GetInt ("costhat" + CurrentHatSaleIndex);
	}

	public void BuyHatBtn () {
		
		if (PlayerPrefs.GetInt ("hat" + CurrentHatSaleIndex) == 0) {
			lockedText.text = "Unlocked";
			lockedText.color = Color.green;
			PlayerPrefs.SetInt ("hat" + CurrentHatSaleIndex, 1);
		} else {
			lockedText.text = "Already Unlocked";
			lockedText.color = Color.green;
		}
	}
	public void hatsForSaleBtn (){
		// show neccessary image to true
		Debug.Log ("made it to hats");

		currentImage.sprite = hatsOptionsAvatar [CurrentHatSaleIndex];
		costText.enabled = true;
	
		//update locked or unlocked and cost
		forSaleType = "hat";
		setImages (forSaleType);
		UpdateHatShop ();
	}
	public void vestsForSaleBtn (){
		// show neccessary image to true
	
		vestImage.sprite = vestsOptionsAvatar [CurrentVestSaleIndex];
		costText.enabled = true;
	
		//update locked or unlocked and cost
		forSaleType = "vest";
		setImages (forSaleType);
		UpdateVestShop ();
	}
	public void gunsForSaleBtn (){
		// show neccessary image to true

		gunImage.sprite = gunsOptionsAvatar [CurrenGunSaleIndex];
		costText.enabled = true;

		//update locked or unlocked and cost
		forSaleType = "gun";
		setImages (forSaleType);
		UpdateGunShop ();
	}
	public void pantsForSaleBtn (){
		// show neccessary image to true

		pantsImage.sprite = pantsOptionsAvatar [CurrentPantsSaleIndex];

		costText.enabled = true;
		//update locked or unlocked and cost
		forSaleType = "pants";
		setImages (forSaleType);
		UpdatePantsShop ();
	}
	public void ForSaleBtnPlus1(){
		if (forSaleType =="hat")
		{ 
			mostRecentItemChangedTo = forSaleType;

			Debug.Log ("should be hat: " +mostRecentItemChangedTo);
			if (CurrentHatSaleIndex +1 < hatsOptionsAvatar.Length)
			{
			CurrentHatSaleIndex++;
			currentImage.sprite = hatsOptionsAvatar [CurrentHatSaleIndex];
			UpdateHatShop ();
			}
			else
			{
				CurrentHatSaleIndex = 0;
				currentImage.sprite = hatsOptionsAvatar [CurrentHatSaleIndex];
				UpdateHatShop ();
			}

		}
		if (forSaleType =="vest")
		{ 
			mostRecentItemChangedTo = forSaleType;
			if (CurrentVestSaleIndex +1 < vestsOptionsAvatar.Length)
			{
				CurrentVestSaleIndex++;
				vestImage.sprite = vestsOptionsAvatar [CurrentVestSaleIndex];
				UpdateVestShop ();
			}
			else
			{
				CurrentVestSaleIndex = 0;
				vestImage.sprite = vestsOptionsAvatar [CurrentVestSaleIndex];
				UpdateVestShop ();
			}
			
		}
		if (forSaleType =="gun")
		{
			mostRecentItemChangedTo = forSaleType;
			if (CurrenGunSaleIndex +1 < gunsOptionsAvatar.Length)
			{
				CurrenGunSaleIndex++;
				gunImage.sprite = gunsOptionsAvatar [CurrenGunSaleIndex];
				UpdateGunShop ();
			}
			else
			{
				CurrenGunSaleIndex = 0;
				gunImage.sprite = gunsOptionsAvatar [CurrenGunSaleIndex];
				UpdateGunShop ();
			}
			
		}
		if (forSaleType =="pants")
		{
			mostRecentItemChangedTo = forSaleType;
			if (CurrentPantsSaleIndex +1 < pantsOptionsAvatar.Length)
			{
				CurrentPantsSaleIndex++;
				pantsImage.sprite = pantsOptionsAvatar [CurrentPantsSaleIndex];
				UpdatePantsShop ();
			}
			else
			{
				CurrentPantsSaleIndex = 0;
				pantsImage.sprite = pantsOptionsAvatar [CurrentPantsSaleIndex];
				UpdatePantsShop ();
			}
			
		}
	}

	public void ForSaleBtnMinus1(){
		if (forSaleType =="hat")
		{
			mostRecentItemChangedTo = forSaleType;
			if (CurrentHatSaleIndex -1 >= 0)
			{
				CurrentHatSaleIndex--;
				currentImage.sprite = hatsOptionsAvatar [CurrentHatSaleIndex];
				costText.text = "Cost: "+ PlayerPrefs.GetInt("cost"+forSaleType+CurrentHatSaleIndex);
				UpdateHatShop();
			}
			else
			{
				CurrentHatSaleIndex = hatsOptionsAvatar.Length-1;
				currentImage.sprite = hatsOptionsAvatar [CurrentHatSaleIndex];
				costText.text = "Cost: "+ PlayerPrefs.GetInt("cost"+forSaleType+CurrentHatSaleIndex);
				UpdateHatShop();
			}
			
		}
		if (forSaleType =="vest")
		{
			mostRecentItemChangedTo = forSaleType;
			if (CurrentVestSaleIndex -1 >= 0)
			{
				CurrentVestSaleIndex--;
				vestImage.sprite = vestsOptionsAvatar [CurrentVestSaleIndex];
				UpdateVestShop ();
			}
			else
			{
				CurrentVestSaleIndex = vestsOptionsAvatar.Length-1;
				vestImage.sprite = vestsOptionsAvatar [CurrentVestSaleIndex];
				UpdateVestShop ();
			}
			
		}
		if (forSaleType =="gun")
		{
			mostRecentItemChangedTo = forSaleType;
			if (CurrenGunSaleIndex -1 >= 0)
			{
				CurrenGunSaleIndex--;
				gunImage.sprite = gunsOptionsAvatar [CurrenGunSaleIndex];
				UpdateGunShop ();
			}
			else
			{
				CurrenGunSaleIndex = gunsOptionsAvatar.Length-1;
				gunImage.sprite = gunsOptionsAvatar [CurrenGunSaleIndex];
				UpdateGunShop ();
			}
			
		}
		if (forSaleType =="pants")
		{
			mostRecentItemChangedTo = forSaleType;
			if (CurrentPantsSaleIndex -1 >= 0)
			{
				CurrentPantsSaleIndex--;
				pantsImage.sprite = pantsOptionsAvatar [CurrentPantsSaleIndex];
				UpdatePantsShop ();
			}
			else
			{
				CurrentPantsSaleIndex = pantsOptionsAvatar.Length-1;
				pantsImage.sprite = pantsOptionsAvatar [CurrentPantsSaleIndex];
				UpdatePantsShop ();
			}
			
		}
	}
	public void UpdateVestShop () {
		nullgoldtext.SetActive (false);
		if (PlayerPrefs.GetInt ("vest" + CurrentVestSaleIndex) == 0){
			lockedText.text = "Locked";
		lockedText.color = Color.red;
	}
		else
		{
			lockedText.text = "Unlocked";
		lockedText.color = Color.green;
	}

		cost = PlayerPrefs.GetInt ("costvest" + CurrentVestSaleIndex);
		costText.text = "Cost: " + PlayerPrefs.GetInt ("costvest" + CurrentVestSaleIndex);
	}
	public void BuyVestBtn () {
		
		if (PlayerPrefs.GetInt ("vest" + CurrentVestSaleIndex) == 0) {
			lockedText.text = "Unlocked";
			lockedText.color = Color.green;
			PlayerPrefs.SetInt ("vest" + CurrentVestSaleIndex, 1);
		} else {
			lockedText.text = "Already Unlocked";
			lockedText.color = Color.green;
		}
	}
	public void UpdateGunShop () {
		nullgoldtext.SetActive (false);
		if (PlayerPrefs.GetInt ("gun" + CurrenGunSaleIndex) == 0) {
			lockedText.text = "Locked";
			lockedText.color = Color.red;
		} else {
			lockedText.text = " Unlocked";
			lockedText.color = Color.green;
		}

		cost = PlayerPrefs.GetInt ("costgun" + CurrenGunSaleIndex);
		costText.text = "Cost: " + PlayerPrefs.GetInt ("costgun" + CurrenGunSaleIndex);
	}
	public void BuyGunBtn () {
		
		if (PlayerPrefs.GetInt ("gun" + CurrenGunSaleIndex) == 0) {
			lockedText.text = "Unlocked";
			lockedText.color = Color.green;
			PlayerPrefs.SetInt ("gun" + CurrenGunSaleIndex, 1);
		} else {
			lockedText.text = "Already Unlocked";
			lockedText.color = Color.green;
		}
	}

	public void UpdatePantsShop () {
		
		if (PlayerPrefs.GetInt ("pants" + CurrentPantsSaleIndex) == 0) {
			lockedText.text = "Locked";
			lockedText.color = Color.red;
		} else {
			lockedText.text = "Unlocked";
			lockedText.color = Color.green;
		}

		cost = PlayerPrefs.GetInt ("costgun" + CurrentPantsSaleIndex);
		costText.text = "Cost: " + PlayerPrefs.GetInt ("costpants" + CurrentPantsSaleIndex);
	}
	public void BuyPantstBtn () {
		nullgoldtext.SetActive (false);
		if (PlayerPrefs.GetInt ("pants" + CurrentPantsSaleIndex) == 0) {
			lockedText.text = "Unlocked";
			lockedText.color = Color.green;
			PlayerPrefs.SetInt ("pants" + CurrentPantsSaleIndex, 1);
		} else {
			lockedText.text = "Already Unlocked";
			lockedText.color = Color.green;
		}
	}

	public override void TransitionIn()
	{	
		CurrentHatIndex = PlayerPrefs.GetInt ("Hat");
		CurrentVestIndex = PlayerPrefs.GetInt ("Vest");
		CurrenGunIndex = PlayerPrefs.GetInt ("Gun");
		CurrentPantsIndex = PlayerPrefs.GetInt ("Pants");
		GameObject.Find ("AvatarTitle").GetComponent<Text> ().text = "";
		//GameObject.Find ("AvatarTitle").GetComponent<Text> ().text = PlayerPrefs.GetString ("playerProfile");
		toggles.SetActive (true);
		Shop.SetActive (true);
		nullgoldtext.SetActive (false);
		shopMinusBtn.enabled = true;
		shopPlusBtn.enabled = true;
		setImages ();
		updateGold ();
		hatsForSaleBtn ();
		base.TransitionIn();
	}
	public override void TransitionOut()
	{
		PlayerPrefs.SetInt ("Hat", hatsIndex[CurrentHatIndex]);
		PlayerPrefs.SetInt ("Vest", vestsIndex[CurrentVestIndex]);
		PlayerPrefs.SetInt ("Gun", gunsIndex[CurrenGunIndex]);
		PlayerPrefs.SetInt ("Pants", pantsIndex[CurrentPantsIndex]);
        socketController.instance.updateAppearance();
		toggles.SetActive (false);
		nullgoldtext.SetActive (false);
		shopMinusBtn.enabled = false;
		shopPlusBtn.enabled = false;
		setImages ();
		base.TransitionOut ();
	}

	public static int[] GetSpriteArray()
	{
		int [] CurrentSpriteArray = {PlayerPrefs.GetInt("Hat"),PlayerPrefs.GetInt("Vest"),PlayerPrefs.GetInt("Gun"),PlayerPrefs.GetInt("Pants")  };
		return CurrentSpriteArray;
	}

}
