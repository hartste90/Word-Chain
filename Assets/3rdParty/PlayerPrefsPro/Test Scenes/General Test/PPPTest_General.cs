using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PPPTest_General : MonoBehaviour {
	
	//Generic refs
	public Text hoverDescriptionDisplay;//the text in middle screen to show what each thing does.
	public Button upperReset;
	public Button lowerReset;
	
	//Settings panel left items refs.
	public Slider intSlider;//the slider from "int"
	public Text intSliderText;//the text from "int"
	public Image colorBox;//the box which displays current color
	public Button colorRandom;//to set random color.
	public InputField stringField;//the field to input an string.
	public Text stringArrayDisplay;//the text to show content of string array
	public Button stringArrayRandom;//the button to set random values to string array.
	public Text vector3Display;//the text to show content of vector3
	public Button vector3Random;//button for random vector3.
	
	//Settings panel left buttons refs.
	public Button savePrefs;
	public Button deleteAllPrefs;
	public Button exportFile;
	public Button importFile;
	public Button deleteFile;
	
	//Actual data types of settings refs. 
	private int intValue;
	private Color colorValue;
	private string stringValue;
	private string[] stringArrayValue;
	private Vector3 vector3Value;
	
	//Profile panel refs
	public Text title;//the big text showing profile selected. 0 if no specific profile is being used.
	
	public Text dateTimeDisplay;//the text to show content of DateTime
	public Button setCurrentTime;//button to set value of dateTime as current computer time.
	public Toggle boolToggleBox;//checkbox of the boolean value.
	public Text vector2ArrayDisplay;//the text to show the content of vector2Array
	public Button vector2ArrayRandom;//the button to set random values to Vector2 array.
	public Text floatArray2DDisplay;//the text to show the content of floatArray2D
	public Button floatArray2DRandom;//the button to set random values to floatArray2D array.
	
	public Button profile1Save;
	public Button profile1Load;
	public Button profile1Delete;
	public Button profile2Save;
	public Button profile2Load;
	public Button profile2Delete;
	public Button profile3Save;
	public Button profile3Load;
	public Button profile3Delete;
	
	//Actual datatypes of profile refs.
	private System.DateTime dateTimeValue;
	private bool boolValue;
	private Vector2[] vector2ArrayValue;
	private float[,] floatArray2D;
	
	void Start () {
		
		UpdateHoverDescription("Hover an element to reveal the related information.");
		SetProfileTitle(0);
		CheckProfileButtons();
		UpdateSettings();
		UpdateProfileInfo();
		
		upperReset.onClick.AddListener(delegate {ResetUpperPanel();});
		lowerReset.onClick.AddListener(delegate {ResetLowerPanel();});
		
		//Settings panel left buttons
		intSlider.onValueChanged.AddListener(delegate {UpdateSliderValue();SetKeyValues();});
		colorRandom.onClick.AddListener(delegate {RandomColorBox();SetKeyValues();});
		stringField.onValueChanged.AddListener(delegate {UpdateStringFieldValue();SetKeyValues();});
		stringArrayRandom.onClick.AddListener(delegate {RandomStringArray();SetKeyValues();});
		vector3Random.onClick.AddListener(delegate {RandomVector3();SetKeyValues();});
		
		//Settings panel right buttons
		savePrefs.onClick.AddListener(delegate {SaveSettingsPrefs();});
		deleteAllPrefs.onClick.AddListener(delegate {PlayerPrefsPro.DeleteAll();UpdateSettings();});
		exportFile.onClick.AddListener(delegate {ExportSettings();UpdateSettings();});
		importFile.onClick.AddListener(delegate {
			int error = PlayerPrefsPro.ImportFile("settings.cfg",true);
			if(error != 0){
				Debug.Log("Load File error code: "+error);
			}
			UpdateSettings();
		});
		deleteFile.onClick.AddListener(delegate {PlayerPrefsPro.DeleteSaveFile("settings.cfg");UpdateSettings();});
		
		//Profile panel left buttons
		setCurrentTime.onClick.AddListener(delegate {dateTimeValue = System.DateTime.Now;dateTimeDisplay.text = dateTimeValue.ToString();});
		vector2ArrayRandom.onClick.AddListener(delegate {RandomVector2Array();});
		floatArray2DRandom.onClick.AddListener(delegate {RandomFloat2DArray();});
		boolToggleBox.onValueChanged.AddListener(delegate {boolValue = boolToggleBox.isOn;});
		
		//Profile panel right buttons
		profile1Save.onClick.AddListener(delegate {SaveProfile(1);CheckProfileButtons();});
		profile1Load.onClick.AddListener(delegate {LoadProfile(1);CheckProfileButtons();});
		profile1Delete.onClick.AddListener(delegate {DeleteProfile(1);CheckProfileButtons();});
		profile2Save.onClick.AddListener(delegate {SaveProfile(2);CheckProfileButtons();});
		profile2Load.onClick.AddListener(delegate {LoadProfile(2);CheckProfileButtons();});
		profile2Delete.onClick.AddListener(delegate {DeleteProfile(2);CheckProfileButtons();});
		profile3Save.onClick.AddListener(delegate {SaveProfile(3);CheckProfileButtons();});
		profile3Load.onClick.AddListener(delegate {LoadProfile(3);CheckProfileButtons();});
		profile3Delete.onClick.AddListener(delegate {DeleteProfile(3);CheckProfileButtons();});
	}
	
	void ResetUpperPanel()
	{
		PlayerPrefsPro.DeleteKey("settings_int");
		PlayerPrefsPro.DeleteKey("settings_color");
		PlayerPrefsPro.DeleteKey("settings_string");
		PlayerPrefsPro.DeleteKey("settings_stringArray");
		PlayerPrefsPro.DeleteKey("settings_vector3");
		UpdateSettings();
	}
	
	void ResetLowerPanel()
	{
		PlayerPrefsPro.DeleteKey("profile_dateTime");
		PlayerPrefsPro.DeleteKey("profile_bool");
		PlayerPrefsPro.DeleteKey("profile_v2array");
		PlayerPrefsPro.DeleteKey("profile_floatArray2D");
		UpdateProfileInfo();
	}
	
	void SaveProfile(int num)
	{
		PlayerPrefsPro.SetDateTime("profile_dateTime",dateTimeValue);
		PlayerPrefsPro.SetBool("profile_bool",boolValue);
		PlayerPrefsPro.SetVector2Array("profile_v2array",vector2ArrayValue);
		PlayerPrefsPro.SetFloatArray2D("profile_floatArray2D",floatArray2D);
		string[] keysToExport = new string[]{"profile_dateTime","profile_bool","profile_v2array","profile_floatArray2D"};
		PlayerPrefsPro.ExportFile("Profile"+num+".sav",keysToExport,true);
	}
	
	void LoadProfile(int num){
		int error = PlayerPrefsPro.ImportFile("profile"+num+".sav",true);
		if(error != 0){
			Debug.Log("Load File error code: "+error);
		}
		UpdateProfileInfo();
	}
	
	void DeleteProfile(int num){
		PlayerPrefsPro.DeleteSaveFile("profile"+num+".sav");
	}
	
	void ExportSettings()//export settings panel info to a local file.
	{
		string[] keysToExport = new string[]{"settings_int","settings_color","settings_string","settings_stringArray","settings_vector3"};//specify what keys to export. They don't really need to exist, they will try to be exported if any.
		PlayerPrefsPro.ExportFile("settings.cfg",keysToExport,true);//File names are case sentitive.
		/*The "true" at the end is to create a backup file, in case main file becomes corrupted for whatever reason. It is called the same but ends in ."bak".
		Yoy can edit that extension in the PlayerPrefsPro file settings. Look for the "backupExtension" variable.*/
	}
	
	void UpdateSettings()//this tries to get the playerprefs value of each key, if it exist or not.
	{
		//Fetch data types values
		intValue = PlayerPrefsPro.GetInt("settings_int");//will return 0 if "settings_int" key does not exist. A different default value can be set as second argument.
		colorValue = PlayerPrefsPro.GetColor("settings_color");
		stringValue = PlayerPrefsPro.GetString("settings_string");
		stringArrayValue = PlayerPrefsPro.GetStringArray("settings_stringArray");
		vector3Value = PlayerPrefsPro.GetVector3("settings_vector3");
		
		//Update controls and displays
		intSlider.value = intValue;
		UpdateSliderValue();
		colorBox.color = colorValue;
		stringField.text = stringValue;
		UpdateStringArray();
		UpdateVector3();
		
		//Enable/disable some buttons
		if(PlayerPrefsPro.DoLocalFileExist("settings.cfg",true)){
			importFile.interactable = true;
			deleteFile.interactable = true;
		}else{
			importFile.interactable = false;
			deleteFile.interactable = false;
		}
	}
	
	void UpdateProfileInfo(){//this tries to get the playerprefs value of each profile key, if it exist or not.
		//Fetch data types values
		dateTimeValue = PlayerPrefsPro.GetDateTime("profile_dateTime");//will return year 1 by default if no key is found. 
		boolValue = PlayerPrefsPro.GetBool("profile_bool");
		vector2ArrayValue = PlayerPrefsPro.GetVector2Array("profile_v2array");
		floatArray2D = PlayerPrefsPro.GetFloatArray2D("profile_floatArray2D");

		//Update controls and displays
		dateTimeDisplay.text = dateTimeValue.ToString();
		boolToggleBox.isOn = boolValue; 
		UpdateVector2Array();
		UpdateFloat2DArray();
		
	}
	
	void SetKeyValues(){//update values in the keybank but do not save them to registry yet.
		PlayerPrefsPro.SetInt("settings_int",intValue);
		PlayerPrefsPro.SetColor("settings_color",colorValue);
		PlayerPrefsPro.SetString("settings_string",stringValue);
		PlayerPrefsPro.SetStringArray("settings_stringArray",stringArrayValue);
		PlayerPrefsPro.SetVector3("settings_vector3",vector3Value);
	}
	
	void SaveSettingsPrefs(){//save the upper paner preferences to the registry or playerpref designed location of target platform.
		SetKeyValues();
		PlayerPrefsPro.Save();//this line sets and encrypt the keys into the registry, but is not necesary for Set___("keyname") methods to work.
	}
	
	void CheckProfileButtons()//If related files exist it will enable the buttons for loading profiles.
	{
		//Profile 1
		if(PlayerPrefsPro.DoLocalFileExist("Profile1.sav",true)){
			profile1Load.interactable = true;
			profile1Delete.interactable = true;
		}else{
			profile1Load.interactable = false;
			profile1Delete.interactable = false;
		}
		
		//Profile 2
		if(PlayerPrefsPro.DoLocalFileExist("Profile2.sav",true)){
			profile2Load.interactable = true;
			profile2Delete.interactable = true;
		}else{
			profile2Load.interactable = false;
			profile2Delete.interactable = false;
		}
		
		//Profile 3
		if(PlayerPrefsPro.DoLocalFileExist("Profile3.sav",true)){
			profile3Load.interactable = true;
			profile3Delete.interactable = true;
		}else{
			profile3Load.interactable = false;
			profile3Delete.interactable = false;
		}
	}
	
	
	
	//Below is Panel data related stuff, not relevant to the plugin usage.
	void UpdateSliderValue(){
		intSliderText.text = intSlider.value.ToString();
		intValue = (int)intSlider.value;
	}
	
	void RandomColorBox(){
		colorValue = new Color(Random.value,Random.value,Random.value);
		colorBox.color = colorValue;
	}
	
	void UpdateStringFieldValue(){
		stringValue = stringField.text; 
	}
	
	void RandomStringArray(){
		string[] words = new string[]{"Apples","Rocks","Coconuts","Bag","Bread","Oil","Capsicum","Basket","Bottles","Jack","Tartarus","Chalice","PC","Jelly"};
		string[] words2 = new string[]{"Red","Green","Dry","Old","Rusty","Shiny","Boring","Kickass","Sharp","Some","Other","Black","White","Dumb"};
		stringArrayValue = new string[2+Random.Range(0,3)];
		for(int x = 0; x < stringArrayValue.Length; x++){
			stringArrayValue[x] = words2[Random.Range(0,words2.Length)]+" "+words[Random.Range(0,words.Length)];
		}
		UpdateStringArray();
	}
	
	void UpdateStringArray(){
		string text = "";
		for(int x = 0; x < stringArrayValue.Length; x++){
			text += "["+x+"] - "+stringArrayValue[x]+"\n";
		}
		if(string.IsNullOrEmpty(text)){
			stringArrayDisplay.text = "[-] Empty";
		}else{
			stringArrayDisplay.text = text;
		}
	}
	
	void RandomVector2Array(){
		vector2ArrayValue = new Vector2[2+Random.Range(0,3)];
		for(int x = 0; x < vector2ArrayValue.Length; x++){
			vector2ArrayValue[x] = new Vector2((Random.value-0.5f)*99,(Random.value-0.5f)*99);
		}
		UpdateVector2Array();
	}
	
	void UpdateVector2Array(){
		string text = "";
		for(int x = 0; x < vector2ArrayValue.Length; x++){
			text += "["+x+"] - "+vector2ArrayValue[x]+"\n";
		}
		if(string.IsNullOrEmpty(text)){
			vector2ArrayDisplay.text = "[-] Empty";
		}else{
			vector2ArrayDisplay.text = text;
		}
	}
	
	//Float[,]
	void RandomFloat2DArray(){
		floatArray2D = new float[2+Random.Range(0,3),2+Random.Range(0,3)];
		for(int x = 0; x < floatArray2D.GetLength(0); x++){
			for(int y = 0; y < floatArray2D.GetLength(1); y++){
				floatArray2D[x,y] = Random.value*9.99f;
			}
		}
		UpdateFloat2DArray();
	}
	
	void UpdateFloat2DArray(){
		string text = "";
		for(int x = 0; x < floatArray2D.GetLength(0); x++){
			for(int y = 0; y < floatArray2D.GetLength(1); y++){
				text += "["+floatArray2D[x,y].ToString("n1")+"]";
			}
			text += "\n";
		}
		if(string.IsNullOrEmpty(text)){
			floatArray2DDisplay.text = "[-] Empty";
		}else{
			floatArray2DDisplay.text = text;
		}
	}
	
	void RandomVector3(){
		vector3Value = new Vector3((Random.value-0.5f)*99,(Random.value-0.5f)*99,(Random.value-0.5f)*99);
		UpdateVector3();
	}
	
	void UpdateVector3(){
		vector3Display.text = "("+vector3Value.x.ToString("n1")+", "+vector3Value.y.ToString("n1")+", "+vector3Value.z.ToString("n1")+")";
	}
	
	void UpdateHoverDescription(string text){
		hoverDescriptionDisplay.text = text;
	}
	
	void SetProfileTitle(int num){
		title.text = "Profile "+num; 
	}

}
