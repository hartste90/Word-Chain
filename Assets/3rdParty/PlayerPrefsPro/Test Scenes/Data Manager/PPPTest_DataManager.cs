using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PPPTest_DataManager : MonoBehaviour {

	[Header("General references")]
	public GameObject[] panels; //0-settings, 1-level data missing, 2-show level data
	public Button[] upperButtons;
	public Text keyListText;
	public Text hoverDescriptionDisplay;
	[HideInInspector]
	public string noHoverTextB;
	
	[Header("Settings panel controls")]
	public InputField infield_pname;
	public InputField infield_pxp;
	public InputField infield_pgold;
	public Button setValues_pinfo;
	public Slider slider_vol;
	public Toggle toggle_sfx;
	public Dropdown ddown_dif;
	public Button setValues_gsett;
	
	[Header("Settings panel lower buttons")]
	public Button btn_saveKeyBankToRegistry;
	public Button btn_savePlayerInfoOnly;
	public Button btn_forceLoadProKeys;
	public Button btn_cleanBank;
	public Button btn_cleanBankExcludingProKeys;
	public Button btn_deleteAllProKeys;
	public Button btn_deleteAllUnityPPKeys;
	public Button btn_deleteAllKeys;
	
	[Header("Level panel references (Unloaded)")]
	public Text tx_mainTextLine;
	public Text tx_mainTextLineSub;
	public Button btn_generateLevel;
	public Dropdown dd_cloneLevel;
	
	[Header("Level panel references (Loaded)")]
	public Text tx_mapTitle;
	public Image img_map;
	public Image[] img_monstersPicRef;
	public Image[] img_itemsPicRef;
	public Button btn_DeleteLevelKeys;
	public Button btn_DeleteLevelFile;
	public Button btn_Generate;
	
	private int currentLevelPanel = 0;
	// Use this for initialization

	void Start () {
		noHoverTextB = "<color=#ffffffff>Hover an element to reveal the related information.</color>";
		PlayerPrefsPro.ForceLoadPrefs();//This should be automatically called after requesting a Get<Type> method. We use it anyway to isolate the loading time to the scene start.
		UpdateKeyList();
		ConfigQuickAccessButtons();
		ConfigSettingsPanelButtons();
		ConfigLevelButtons();
		LoadPanel(0,0);//Settings panel is set by default
		UpdateHoverDescription(noHoverTextB);
	}
	
	void ConfigSettingsPanelButtons(){
		//Save Player Info Button
		setValues_pinfo.onClick.AddListener(delegate {
			PlayerPrefsPro.SetString("pnfo_name",infield_pname.text);
			PlayerPrefsPro.SetString("pnfo_xp",infield_pxp.text);
			PlayerPrefsPro.SetString("pnfo_gold",infield_pgold.text);
			UpdateKeyList();
		});
		//Save Game Settings Button
		setValues_gsett.onClick.AddListener(delegate {
			PlayerPrefs.SetFloat("gsett_vol",slider_vol.value);
			PlayerPrefs.SetInt("gsett_sfx", toggle_sfx.isOn ? 1 : 0);
			PlayerPrefs.SetInt("gsett_dif",ddown_dif.value);
		});
		//Other buttons in the panel below.
		btn_saveKeyBankToRegistry.onClick.AddListener(delegate{
			PlayerPrefsPro.Save();
		});
		btn_savePlayerInfoOnly.onClick.AddListener(delegate {
			string[] keysToSave = new string[]{"pnfo_name","pnfo_xp","pnfo_gold"};
			PlayerPrefsPro.Save(keysToSave);
		});
		btn_forceLoadProKeys.onClick.AddListener(delegate {
			PlayerPrefsPro.ForceLoadPrefs();
			UpdateKeyList();
		});	
		btn_cleanBank.onClick.AddListener(delegate {
			PlayerPrefsPro.CleanBank();
			UpdateKeyList();
		});
		btn_cleanBankExcludingProKeys.onClick.AddListener(delegate {
			string[] keysToExclude = new string[]{"pnfo_name","pnfo_xp","pnfo_gold"};//We don't want to delete the player info, but all other keys.
			PlayerPrefsPro.CleanBankExcludingKeys(keysToExclude);
			UpdateKeyList();
		});
		btn_deleteAllProKeys.onClick.AddListener(delegate {
			PlayerPrefsPro.DeleteAllProKeys();
			UpdateKeyList();
		});
		btn_deleteAllUnityPPKeys.onClick.AddListener(delegate {
			PlayerPrefs.DeleteAll();
		});
		btn_deleteAllKeys.onClick.AddListener(delegate {
			PlayerPrefsPro.DeleteAll();
			UpdateKeyList();
		});
	}
	
	void ConfigQuickAccessButtons(){
		upperButtons[0].onClick.AddListener(delegate {LoadPanel(0,0);}); //Settings
		for(int x = 1; x < upperButtons.Length; x++){
			int copy = x;
			upperButtons[x].onClick.AddListener(delegate {SortTargetPanel(copy);}); //Level info
		}
	}
	
	void ConfigLevelButtons(){
		//Missing level buttons
		btn_generateLevel.onClick.AddListener(delegate {
			GenerateLevelData(currentLevelPanel);
			SortTargetPanel(currentLevelPanel);
		});
		btn_Generate.onClick.AddListener(delegate {//Same action than the first one, but the button is in a different place.
			GenerateLevelData(currentLevelPanel);
			SortTargetPanel(currentLevelPanel);
		});
		//Found Level buttons
		btn_DeleteLevelKeys.onClick.AddListener(delegate{
			PlayerPrefsPro.DeleteKey("levelMap");
			PlayerPrefsPro.DeleteKey("levelMonsters");
			PlayerPrefsPro.DeleteKey("levelItems");
			UpdateKeyList();
			btn_DeleteLevelKeys.interactable = false;
		});
		btn_DeleteLevelFile.onClick.AddListener(delegate{
			PlayerPrefsPro.DeleteSaveFile("DataManager/Levels/level"+currentLevelPanel+".dat");
			btn_DeleteLevelFile.interactable = false;
		});
		
		dd_cloneLevel.onValueChanged.AddListener(delegate {
			if(dd_cloneLevel.value == 0){return;}
			PlayerPrefsPro.CloneSaveFile("DataManager/Levels/level"+dd_cloneLevel.value+".dat","DataManager/Levels/level"+currentLevelPanel+".dat");
			StartCoroutine(DropDownTimingFix());
			
		});
	}
	
	IEnumerator DropDownTimingFix(){
		yield return new WaitForSeconds(0.2f);
		SortTargetPanel(currentLevelPanel);
		dd_cloneLevel.value = 0;
	}
	
	void SortTargetPanel(int levelID)
	{
		string path = "DataManager/Levels/level"+levelID+".dat";
		if(PlayerPrefsPro.DoLocalFileExist(path,true)){
			LoadPanel(2,levelID);
		}else{
			LoadPanel(1,levelID);
		}
	}
	
	void LoadPanel(int num,int levelID)//num:0-settings, 1-level data missing, 2-show level data
	{
		for(int x = 0; x < panels.Length; x++){
			if(x == num){continue;}
			panels[x].SetActive(false);
		}
		panels[num].SetActive(true);
		
		//Setup Level Info
		if(num != 0){
			currentLevelPanel = levelID;
			
			if(num == 1){//setup data missing info
				UpdateMissingLevelPanelContent();
			}else{//setup level content info
				UpdateFoundLevelPanelContent();
			}
		}else{
			UpdateSettingsPanelContent();
		}
	}
	
	void UpdateMissingLevelPanelContent()
	{
		tx_mainTextLine.text = "No Level "+currentLevelPanel+" Data Found.";
		tx_mainTextLineSub.text = "'level"+currentLevelPanel+".dat' is a file in the <Application.dataPath>/DataManager/Levels/";
		
		btn_generateLevel.transform.GetChild(0).GetComponent<Text>().text = "Generate 'level"+currentLevelPanel+".dat'";
		//Dropdown setup; We will include the levels which info is available.
		dd_cloneLevel.ClearOptions();
		List<string> options = new List<string>();
		options.Add("Don't clone");
		for(int x = 1; x < 9; x++){
			if(PlayerPrefsPro.DoLocalFileExist("DataManager/Levels/level"+x+".dat",true)){//If a key of a level map exists, we assume all the others exist too (monsters and items).
				options.Add("Level "+x);
			}
		}
		dd_cloneLevel.AddOptions(options);
	}
	
	void GenerateLevelData(int levelID){
		//Generate level Map
		int[,] levelMap = GenerateMap();
		PlayerPrefsPro.SetIntArray2D("levelMap",levelMap);
		//Generate Monster List
		int[] levelMonsters = GenerateIDList((int)(5+Random.value*9),6);
		PlayerPrefsPro.SetIntArray("levelMonsters",levelMonsters);
		//Generate Item List
		int[] levelItems = GenerateIDList((int)(5+Random.value*9),6);
		PlayerPrefsPro.SetIntArray("levelItems",levelItems);
		string[] keysToExport = new string[]{"levelMap","levelMonsters","levelItems"};
		PlayerPrefsPro.ExportFile("DataManager/Levels/level"+levelID+".dat",keysToExport,true);
	}
	
	void UpdateFoundLevelPanelContent()
	{
		int status = PlayerPrefsPro.ImportFile("DataManager/Levels/level"+currentLevelPanel+".dat",true);
		Debug.Log("status: "+status);
		UpdateKeyList();
		tx_mapTitle.text = "Level "+currentLevelPanel+" Map";
		img_map.sprite = GenerateMapImage(PlayerPrefsPro.GetIntArray2D("levelMap"));
		//Monsters and items
		UpdateMonsterList(PlayerPrefsPro.GetIntArray("levelMonsters"));
		UpdateItemsList(PlayerPrefsPro.GetIntArray("levelItems"));
		
		btn_DeleteLevelKeys.interactable = PlayerPrefsPro.HasKey("levelMap");
		btn_DeleteLevelFile.interactable = true;
	}
	
	void UpdateSettingsPanelContent()
	{
		infield_pname.text = PlayerPrefsPro.GetString("pnfo_name");//If it doesn't exist, it will return the default value.
		infield_pxp.text = PlayerPrefsPro.GetString("pnfo_xp");//If it doesn't exist, it will return the default value.
		infield_pgold.text = PlayerPrefsPro.GetString("pnfo_gold");//If it doesn't exist, it will return the default value.
		
		slider_vol.value = PlayerPrefs.GetFloat("gsett_vol");
		toggle_sfx.isOn = PlayerPrefs.GetInt("gsett_sfx") == 1? true : false;
		ddown_dif.value = PlayerPrefs.GetInt("gsett_dif");
	}
	
	void UpdateKeyList()
	{
		string[] keyList = PlayerPrefsPro.GetKeyBankKeyNames();
		string final = "";
		for(int x = 0; x < keyList.Length; x++){
			final+= keyList[x]+"\n";
		}
		keyListText.text = final;
	
	}
	
	int[,] GenerateMap()
	{
		int[,] map = new int[50,26];
		int nodes = 15 + (int)Random.value*70;
		int direction; //0-up, 1-Right, 2-down, 3-left
		int steps;//Current line length
		int count = 0; 
		int posX = Random.Range(0,map.GetLength(0)-1);
		int posY = Random.Range(0,map.GetLength(1)-1);
		int oldDir = -1;
		//Crate path and nodes
		while(count < nodes){
			while(true){
				direction = Random.Range(0,4);
				if(direction == 0 && posY == 0){direction = 2;}
				else if(direction == 2 && posY == (map.GetLength(1)-1)){direction = 0;}
				else if(direction == 1 && posX == (map.GetLength(0)-1)){direction = 3;}
				else if(direction == 3 && posX == 0){direction = 1;}
				if(direction != oldDir){break;}
			}
			steps = 8+(int)Random.value*50;
			int x; int y;
			for(x = 0; x < steps; x++){
				if(direction == 0){
					if(posY > 0){posY--;}else{count--;break;}
				}else if(direction == 2){
					if(posY < (map.GetLength(1)-1)){posY++;}else{count--;break;}
				}else if(direction == 1){
					if(posX < (map.GetLength(0)-1)){posX++;}else{count--;break;}
				}else if(direction == 3){
					if(posX > 0){posX--;}else{count--;break;}
				}
				map[posX,posY] = 1;
			}
			//We create a room at that spot
			int roomSize = Random.Range(0,3);
			int[] indexes = new int[2];
			for(x = 0; x < roomSize*3;x++){
				for(y = 0; y < roomSize*3;y++){
					indexes[0] = posX+x - roomSize;
					indexes[1] = posY+y - roomSize;
					if((indexes[0] < map.GetLength(0) && indexes[0] > 0) && (indexes[1] < map.GetLength(1) && indexes[1] > 0)){
						map[indexes[0],indexes[1]] = 1;
					}
				}
			}
			count++;
			oldDir = direction;
		}
		return map;
	}
	
	Sprite GenerateMapImage(int[,] arr)
	{
		Texture2D tex = new Texture2D(arr.GetLength(0),arr.GetLength(1));
		for(int x = 0; x < tex.width; x++){
			for(int y = 0; y < tex.height; y++){
				if(arr[x,y] == 1){
					tex.SetPixel(x,y,new Color(1f,0.7f,0.3f));
				}else{
					tex.SetPixel(x,y,Color.black);
				}
			}
		}
		tex.filterMode = FilterMode.Point;
		tex.wrapMode = TextureWrapMode.Clamp;
		tex.Apply();
		return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
	}
	
	int[] GenerateIDList(int length, int idMax)
	{
		int[] arr = new int[length];
		for(int x = 0; x < length; x++){
			arr[x] = Random.Range(0,idMax);
		}
		return arr;
	}
	
	Color GetMonsterInfo(int id)
	{
		Color[] colors = new Color[]{Color.white,Color.black,Color.red,Color.blue,Color.green,Color.yellow};
		return colors[id];
	}
	
	Color GetItemsInfo(int id)
	{
		Color[] colors = new Color[]{Color.white,Color.black,Color.red,Color.blue,Color.green,Color.yellow};
		return colors[id];
	}
	
	void UpdateMonsterList(int[] mIds)
	{
		int x; Image picref = img_monstersPicRef[0];
		//Delete old images
		for(x = 1; x < img_monstersPicRef.Length; x++){
			if(img_monstersPicRef[x]!=null){Destroy(img_monstersPicRef[x].gameObject);}
		}
		//Create new ones
		img_monstersPicRef = new Image[mIds.Length];
		for(x = 0; x < img_monstersPicRef.Length; x++){
			if(x != 0){img_monstersPicRef[x] = Instantiate(img_monstersPicRef[0]);}else{img_monstersPicRef[x] = picref;}
			img_monstersPicRef[x].transform.SetParent(img_monstersPicRef[0].transform.parent);
			img_monstersPicRef[x].color = GetMonsterInfo(mIds[x]); 
		}
	}
	
	void UpdateItemsList(int[] mIds)
	{
		int x; Image picref = img_itemsPicRef[0];
		//delete old images
		for(x = 1; x < img_itemsPicRef.Length; x++){
			if(img_itemsPicRef[x]!=null){Destroy(img_itemsPicRef[x].gameObject);}
		}
		//create new ones
		img_itemsPicRef = new Image[mIds.Length];
		for(x = 0; x < img_itemsPicRef.Length; x++){
			if(x != 0){img_itemsPicRef[x] = Instantiate(img_itemsPicRef[0]);}else{img_itemsPicRef[x] = picref;}
			img_itemsPicRef[x].transform.SetParent(img_itemsPicRef[0].transform.parent);
			img_itemsPicRef[x].color = GetItemsInfo(mIds[x]); 
		}
	}
	
	public void UpdateHoverDescription(string text){
		hoverDescriptionDisplay.text = text;
	}
}

