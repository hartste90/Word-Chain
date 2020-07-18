using UnityEngine;
using System;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Collections;

//Copyright © 2018 Emmanuel(earrgames) Ramos. All rights reserved. Do not distribute.

/*How to use this asset: 

Please, check the "PlayerPrefsPro.pdf" for the first-time setup.

Warning: This data encryption solution provides more than enough protection against regular users. However, due to the unity builds structure,
the source files are easily accessible with plenty of the hacking tools around, making it ("almost"?) impossible to protect. People can inspect, extract or
even edit the script, assets, level data, or any other of the project files. A dedicated enough hacker can reverse engineer this script and modify the save file at will.
If you are working on an online game, always store sensitive data (as loaderboard's data) at the server side.
*/
public enum KeyEx{kInt,kString,kFloat,kBool,kDateTime,kVector2,kVector3,kVector4,kColor,kIntArray,kStringArray,kFloatArray,kBoolArray,kDateTimeArray,kVector2Array,kVector3Array,kVector4Array,kColorArray,kIntArray2D,kStringArray2D,kFloatArray2D,kBoolArray2D};

public class PlayerPrefsPro : MonoBehaviour {

//------------------------------------------------------------------------------------------------------------------------------	
//--------------------------------------PLEASE, EDIT THE CODE BELOW TO SUIT YOUR NEEDS!-----------------------------------------
//------------------------------------------------------------------------------------------------------------------------------
	private static int key1 = 99999999;//Must not be negative
	private static int key2 = 98712361;//Must not be negative
	private static string backupExtension = "bak";//The file extension of the backup files created when exporting keys. LOWERCASE ONLY!
	
	private static void ThrowCorruptFileWarning(string details,int errorType)//This method should do something when a corrupted value is detected.
	{
		//ALL ERRORS HERE ARE RELATED TO HACKING.
		if(errorType == 0){//Errors related to key value and checksum mismatch, because they modified either the registry or the save files.
			Debug.Log("<Hack_Error>: "+details);
			//Your custom code here
			//Example 1: SceneManagement.SceneManager.LoadScene("yourErrorSceneName");
			//Example 2: Application.Quit();
		}
	}
	
//------------------------------------------------------------------------------------------------------------------------------	
//------------------------------------- DO NOT MODIFY ANYTHING BELOW THIS POINT -----------------------------------------------
//--------------------------------------- Unless you know what you are doing. --------------------------------------------------
	
	public static void SetInt(string keyname,int content){PlayerPrefsPro.SetTypeX<int>(keyname,content,KeyEx.kInt);}
	public static int GetInt(string keyname){return GetInt(keyname,0);}
	public static int GetInt(string keyname,int defaultValue){return GetTypeX<int>(keyname,SerializeWJson<int>(defaultValue),KeyEx.kInt);}
	
	public static void SetString(string keyname,string content){PlayerPrefsPro.SetTypeX<string>(keyname,content,KeyEx.kString);}
	public static string GetString(string keyname){return GetString(keyname,"");}
	public static string GetString(string keyname,string defaultValue){return GetTypeX<string>(keyname,SerializeWJson<string>(defaultValue),KeyEx.kString);}
	
	public static void SetFloat(string keyname,float content){PlayerPrefsPro.SetTypeX<float>(keyname,content,KeyEx.kFloat);}
	public static float GetFloat(string keyname){return GetFloat(keyname,0f);}
	public static float GetFloat(string keyname,float defaultValue){return GetTypeX<float>(keyname,SerializeWJson<float>(defaultValue),KeyEx.kFloat);}
	
	public static void SetBool(string keyname,bool content){PlayerPrefsPro.SetTypeX<bool>(keyname,content,KeyEx.kBool);}
	public static bool GetBool(string keyname){return GetBool(keyname,false);}
	public static bool GetBool(string keyname,bool defaultValue){return GetTypeX<bool>(keyname,SerializeWJson<bool>(defaultValue),KeyEx.kBool);}
	
	public static void SetDateTime(string keyname,DateTime content){PlayerPrefsPro.SetTypeX<string>(keyname,content.ToString("MM/dd/yyyy HH:mm:ss"),KeyEx.kDateTime);}
	public static DateTime GetDateTime(string keyname){return GetDateTime(keyname,new DateTime(0));}
	public static DateTime GetDateTime(string keyname,DateTime defaultValue){return DateTime.ParseExact(GetTypeX<string>(keyname,SerializeWJson<string>(defaultValue.ToString("MM/dd/yyyy HH:mm:ss")),KeyEx.kDateTime),"MM/dd/yyyy HH:mm:ss",System.Globalization.CultureInfo.InvariantCulture);}
	
	public static void SetVector2(string keyname,Vector2 content){PlayerPrefsPro.SetTypeX<Vector2>(keyname,content,KeyEx.kVector2);}
	public static Vector2 GetVector2(string keyname){return GetVector2(keyname,Vector2.zero);}
	public static Vector2 GetVector2(string keyname,Vector2 defaultValue){return GetTypeX<Vector2>(keyname,SerializeWJson<Vector2>(defaultValue),KeyEx.kVector2);}
	
	public static void SetVector3(string keyname,Vector3 content){PlayerPrefsPro.SetTypeX<Vector3>(keyname,content,KeyEx.kVector3);}
	public static Vector3 GetVector3(string keyname){return GetVector3(keyname,Vector3.zero);}
	public static Vector3 GetVector3(string keyname,Vector3 defaultValue){return GetTypeX<Vector3>(keyname,SerializeWJson<Vector3>(defaultValue),KeyEx.kVector3);}
	
	public static void SetVector4(string keyname,Vector4 content){PlayerPrefsPro.SetTypeX<Vector4>(keyname,content,KeyEx.kVector4);}
	public static Vector4 GetVector4(string keyname){return GetVector4(keyname,Vector3.zero);}
	public static Vector4 GetVector4(string keyname,Vector4 defaultValue){return GetTypeX<Vector4>(keyname,SerializeWJson<Vector4>(defaultValue),KeyEx.kVector4);}
	
	public static void SetColor(string keyname,Color content){PlayerPrefsPro.SetTypeX<Color>(keyname,content,KeyEx.kColor);}
	public static Color GetColor(string keyname){return GetColor(keyname,Color.black);}
	public static Color GetColor(string keyname,Color defaultValue){return GetTypeX<Color>(keyname,SerializeWJson<Color>(defaultValue),KeyEx.kColor);}
	
	public static void SetIntArray(string keyname,int[] content){PlayerPrefsPro.SetTypeX<int[]>(keyname,content,KeyEx.kIntArray);}
	public static int[] GetIntArray(string keyname){return GetIntArray(keyname,new int[0]);}
	public static int[] GetIntArray(string keyname,int[] defaultValue){return GetTypeX<int[]>(keyname,SerializeWJson<int[]>(defaultValue),KeyEx.kIntArray);}
	
	public static void SetStringArray(string keyname,string[] content){PlayerPrefsPro.SetTypeX<string[]>(keyname,content,KeyEx.kStringArray);}
	public static string[] GetStringArray(string keyname){return GetStringArray(keyname,new string[0]);}
	public static string[] GetStringArray(string keyname,string[] defaultValue){return GetTypeX<string[]>(keyname,SerializeWJson<string[]>(defaultValue),KeyEx.kStringArray);}
	
	public static void SetFloatArray(string keyname,float[] content){PlayerPrefsPro.SetTypeX<float[]>(keyname,content,KeyEx.kFloatArray);}
	public static float[] GetFloatArray(string keyname){return GetFloatArray(keyname,new float[0]);}
	public static float[] GetFloatArray(string keyname,float[] defaultValue){return GetTypeX<float[]>(keyname,SerializeWJson<float[]>(defaultValue),KeyEx.kFloatArray);}
	
	public static void SetBoolArray(string keyname,bool[] content){PlayerPrefsPro.SetTypeX<bool[]>(keyname,content,KeyEx.kBoolArray);}
	public static bool[] GetBoolArray(string keyname){return GetBoolArray(keyname,new bool[0]);}
	public static bool[] GetBoolArray(string keyname,bool[] defaultValue){return GetTypeX<bool[]>(keyname,SerializeWJson<bool[]>(defaultValue),KeyEx.kBoolArray);}
	
	public static void SetDateTimeArray(string keyname,DateTime[] content){PlayerPrefsPro.SetTypeX<string[]>(keyname,DateArrToStringArr(content),KeyEx.kDateTimeArray);}
	public static DateTime[] GetDateTimeArray(string keyname){return GetDateTimeArray(keyname,new DateTime[0]);}
	public static DateTime[] GetDateTimeArray(string keyname,DateTime[] defaultValue){return StringArrToDateArr(GetTypeX<string[]>(keyname,SerializeWJson<string[]>(DateArrToStringArr(defaultValue)),KeyEx.kDateTimeArray));}
	
	public static void SetVector2Array(string keyname,Vector2[] content){PlayerPrefsPro.SetTypeX<Vector2[]>(keyname,content,KeyEx.kVector2Array);}
	public static Vector2[] GetVector2Array(string keyname){return GetVector2Array(keyname,new Vector2[0]);}
	public static Vector2[] GetVector2Array(string keyname,Vector2[] defaultValue){return GetTypeX<Vector2[]>(keyname,SerializeWJson<Vector2[]>(defaultValue),KeyEx.kVector2Array);}
	
	public static void SetVector3Array(string keyname,Vector3[] content){PlayerPrefsPro.SetTypeX<Vector3[]>(keyname,content,KeyEx.kVector3Array);}
	public static Vector3[] GetVector3Array(string keyname){return GetVector3Array(keyname,new Vector3[0]);}
	public static Vector3[] GetVector3Array(string keyname,Vector3[] defaultValue){return GetTypeX<Vector3[]>(keyname,SerializeWJson<Vector3[]>(defaultValue),KeyEx.kVector3Array);}
	
	public static void SetVector4Array(string keyname,Vector4[] content){PlayerPrefsPro.SetTypeX<Vector4[]>(keyname,content,KeyEx.kVector4Array);}
	public static Vector4[] GetVector4Array(string keyname){return GetVector4Array(keyname,new Vector4[0]);}
	public static Vector4[] GetVector4Array(string keyname,Vector4[] defaultValue){return GetTypeX<Vector4[]>(keyname,SerializeWJson<Vector4[]>(defaultValue),KeyEx.kVector4Array);}
	
	public static void SetColorArray(string keyname,Color[] content){PlayerPrefsPro.SetTypeX<Color[]>(keyname,content,KeyEx.kColorArray);}
	public static Color[] GetColorArray(string keyname){return GetColorArray(keyname,new Color[3]);}
	public static Color[] GetColorArray(string keyname,Color[] defaultValue){return GetTypeX<Color[]>(keyname,SerializeWJson<Color[]>(defaultValue),KeyEx.kColorArray);}
	
	public static void SetIntArray2D(string keyname,int[,] content){PlayerPrefsPro.SetTypeX<string[]>(keyname,ToArrayFrom2DInt(content),KeyEx.kIntArray2D);}
	public static int[,] GetIntArray2D(string keyname){return GetIntArray2D(keyname,new int[0,0]);}
	public static int[,] GetIntArray2D(string keyname,int[,] defaultValue){return To2DFromArrayInt(GetTypeX<string[]>(keyname,SerializeWJson<string[]>(ToArrayFrom2DInt(defaultValue)),KeyEx.kIntArray2D));}
	
	public static void SetStringArray2D(string keyname,string[,] content){PlayerPrefsPro.SetTypeX<string[]>(keyname,ToArrayFrom2DString(content),KeyEx.kStringArray2D);}
	public static string[,] GetStringArray2D(string keyname){return GetStringArray2D(keyname,new string[0,0]);}
	public static string[,] GetStringArray2D(string keyname,string[,] defaultValue){return To2DFromArrayString(GetTypeX<string[]>(keyname,SerializeWJson<string[]>(ToArrayFrom2DString(defaultValue)),KeyEx.kStringArray2D));}
	
	public static void SetFloatArray2D(string keyname,float[,] content){PlayerPrefsPro.SetTypeX<string[]>(keyname,ToArrayFrom2DFloat(content),KeyEx.kFloatArray2D);}
	public static float[,] GetFloatArray2D(string keyname){return GetFloatArray2D(keyname,new float[0,0]);}
	public static float[,] GetFloatArray2D(string keyname,float[,] defaultValue){return To2DFromArrayFloat(GetTypeX<string[]>(keyname,SerializeWJson<string[]>(ToArrayFrom2DFloat(defaultValue)),KeyEx.kFloatArray2D));}
	
	public static void SetBoolArray2D(string keyname,bool[,] content){PlayerPrefsPro.SetTypeX<string[]>(keyname,ToArrayFrom2DBool(content),KeyEx.kBoolArray2D);}
	public static bool[,] GetBoolArray2D(string keyname){return GetBoolArray2D(keyname,new bool[0,0]);}
	public static bool[,] GetBoolArray2D(string keyname,bool[,] defaultValue){return To2DFromArrayBool(GetTypeX<string[]>(keyname,SerializeWJson<string[]>(ToArrayFrom2DBool(defaultValue)),KeyEx.kBoolArray2D));}
	
	public static bool HasKey(string key)//key);//If a key exists in PlayerPrefsPro with a given name, regardless of type.(There can exist multiple types of the same key name)
	{
		if(firstRegLoad == false){
			LoadBankKeys();
		}
		
		foreach (KeyEx kval in Enum.GetValues(typeof(KeyEx)))
		{
			if(FindBankKeyIndex(GetKeyExtension(kval)+"$"+key) != -1){
				return true;
			}
		}
		
		return false;
	}
	
	public static bool HasKey(string key,KeyEx kType)//If a key exists in PlayerPrefsPro with a given name and of a specific Type.
	{
		if(firstRegLoad == false){
			LoadBankKeys();
		}
		
		if(FindBankKeyIndex(GetKeyExtension(kType)+"$"+key) != -1){
			return true;
		}else{
			return false;
		}
	}
	
	public static void DeleteKey(string key)//Delete all keys named key loaded at runtime. You need to call Save() afterwards (or at some point) to store the changes in preferences.
	{
		List<KeyEx> types = GetKeyType(key);
		foreach(KeyEx t in types){
			DeleteBankKey(GetKeyExtension(t)+"$"+key);
			DeleteBankKey(GetKeyExtension(t)+"$"+key+"_sys");
		}
	}
	
	public static void DeleteKey(string key,KeyEx type)//Deletes all the keys named “key” loaded at run-time. You need to call Save() afterwards (or at some point) to store the changes in preferences.
	{
		DeleteBankKey(GetKeyExtension(type)+"$"+key);
		DeleteBankKey(GetKeyExtension(type)+"$"+key+"_sys");
	}
	
	public static void DeleteAll()//Deletes all Unity's PlayerPrefs and PlayerPrefsPro keys from bank and registry.
	{
		PlayerPrefs.DeleteAll();
		keyBank = new string[100,2];
		lastIndex = -1;
	}
	
	public static void DeleteAllProKeys()//Deletes all the keys related to PlayerPrefsPro from key-bank and registry.
	{
		PlayerPrefs.DeleteKey(EnDecrypt("rsv_linex",key2));
		string info = "_"; int count = 0; string keyName;
		while(!string.IsNullOrEmpty(info)){
			keyName = EnDecrypt("rsv_line"+count++,(key1+count)/2);
			info = PlayerPrefs.GetString(keyName);
			PlayerPrefs.DeleteKey(keyName);
		}
		CleanBank();
	}
	
	public static void CleanBank()//Similar to DeleteAll() but only deletes the keys loaded at runtime, it does not affect the registry values.
	{
		keyBank = new string[100,2];
		lastIndex = -1;
	}
	
	public static void CleanBankExcludingKeys(string[] keysToExclude)//Like CleanBank() but excludes a list of key names from being deleted.
	{
		//1-Gather the keys and values to exclude from deleting
		string[] toKeep = GenBankKeyList(keysToExclude);//these keys are already in internal bank format.
		string[,] tmpBank = new string[toKeep.Length,2];
		int tIndex = 0;//index of next slot to assign key to tmpBank.
		int index; int x;
		for(x = 0; x < toKeep.Length; x++){
			index = FindBankKeyIndex(toKeep[x]);
			if(index != -1){
				tmpBank[tIndex,0] = keyBank[index,0];
				tmpBank[tIndex,1] = keyBank[index,1];
				tIndex++;
			}
		}
		
		//2-Clean all bank keys
		CleanBank();
		
		//3-now we add excluded keys back.
		for(x = 0; x < tIndex; x++){
			keyBank[x,0] = tmpBank[x,0];
			keyBank[x,1] = tmpBank[x,1];
			lastIndex++;
		}
	}
	
	public static void Save()//Saves information into the registry or PlayerPrefs location of the target platform.
	{
		SaveInfo(GenBankKeyList(), true, "",false);
	}
	
	public static void Save(string[] keysToExport)//Saves specific keys information into the registry. WARNING! All keys but these ones will be removed from the registry data block.
	{
		string[] toExport = GenBankKeyList(keysToExport);
		SaveInfo(toExport, true, "",false);
	}
	
	public static bool DoLocalFileExist(string fileName,bool checkForBackupToo)//Returns true if a file named “fileName” or its backup exist.
	{
		string path = PathFileToLowerCase(Application.persistentDataPath+"/Saves/"+fileName);
		if(System.IO.File.Exists(path)){
			return true;
		}else if(checkForBackupToo == true){
			string directory = Path.GetDirectoryName(path);
			string fName = Path.GetFileNameWithoutExtension(path);
			if(System.IO.File.Exists(directory+"/"+fName+"."+backupExtension)){
				return true;
			}	
		}
		return false;
	}
	
	//Data Export / Import
	public static void ExportFile(string fileName,string[] keysToExport, bool makeBackup)//Exports all keys named keysToExport including all types.
	{
		string[] keyList = GenBankKeyList(keysToExport);
		SaveInfo(keyList, false, fileName, makeBackup);
	}
	
	public static void ExportFile(string fileName,string[] keysToExport, KeyEx[] types, bool makeBackup)//Exports all keys in the “keysToExport” array of a specific type (In case duplicated key names of different types exist) to a local path.
	{
		string[] keyList = GenBankKeyList(keysToExport,types);
		SaveInfo(keyList, false, fileName, makeBackup);
	}
	
	public static int ImportFile(string fileName,bool checkForBackup)//Imports a file from the local path if it exists. It will return an error code.
	{
		/*This method will return one of the following error codes when called:
		0 : no error
		1 : file was not found (If backup check = false).
		2 : file was corrupted or hacked (If backup check = false).
		3 : file was corrupted or hacked and the backup was not found.
		4 : file was corrupted or hacked and the backup was corrupted too.
		*/
		string path = PathFileToLowerCase(Application.persistentDataPath+"/Saves/"+fileName);
		if(!System.IO.File.Exists(path)){//If file Do not exist.
			if(!checkForBackup){
				return 1;
			}
		}		
		
		int status = TryToLoadFile(path);
		if(checkForBackup && status != 0){
			string directory = Path.GetDirectoryName(path);
			string fName = Path.GetFileNameWithoutExtension(path);
			status = TryToLoadFile(directory+"/"+fName+"."+backupExtension) + 2;
			if(status == 2){
				status = 0;
				CloneSaveFile(fName+"."+backupExtension,fileName);//If backup works, restore original file.
			}
		}
		
		return status;
	}
	
	public static void DeleteSaveFile(string fileName)//Deletes a save file and its associated backup if it exists.
	{		
		
		string path = PathFileToLowerCase(Application.persistentDataPath+"/Saves/"+fileName);
		if(System.IO.File.Exists(path)){//If file Do not exist.
			System.IO.File.Delete(path);//Delete File.
		}
		
		string directory = Path.GetDirectoryName(path);
		string fName = Path.GetFileNameWithoutExtension(path);
		if(System.IO.File.Exists(directory+"/"+fName+"."+backupExtension)){
			System.IO.File.Delete(directory+"/"+fName+"."+backupExtension);//Delete backup
		}
	}
	
	public static bool CloneSaveFile(string fileNameOrigin, string fileNameTarget)//Clones an existing save file into another named “fileNameTarget”.
	{//Will return false if there was any error.
		string path =  Application.persistentDataPath+"/Saves/";
		string pathAndOrigin = PathFileToLowerCase(path+fileNameOrigin);
		string pathAndTarget = PathFileToLowerCase(path+fileNameTarget);
		try{
			File.Copy(path+fileNameOrigin,path+fileNameTarget,true);
		}catch{
			Debug.Log("<MISSING FILE ERROR>:"+fileNameOrigin+" could not be cloned because it doesn't exist.");//You can delete this line if you want.
			//You can include a custom behaviour here, like loading an scene with an error or something.
			return false;
		}	
		//Clone the backup too if exists
		string path1 = Path.GetDirectoryName(pathAndOrigin);
		string fName1 = Path.GetFileNameWithoutExtension(pathAndOrigin);
		string path2 = Path.GetDirectoryName(pathAndTarget);
		string fName2 = Path.GetFileNameWithoutExtension(pathAndTarget);
		
		if(System.IO.File.Exists(path1+"/"+fName1+"."+backupExtension)){	
			if((path1+"/"+fName1+"."+backupExtension) == (path2+"/"+fName2+"."+backupExtension)){return true;}
			try{
				File.Copy(path1+"/"+fName1+"."+backupExtension,path2+"/"+fName2+"."+backupExtension,true);
			}catch{
				Debug.Log("<COPY ERROR>:"+fileNameOrigin+" might not have been cloned because it doesn't exist or is being used by another process.");//You can delete this line if you want.
				//You can include a custom behaviour here, like loading an scene with an error or something.
				return false;
			}
		}	
		return true;
	}
	
	public static bool RenameSaveFile(string sourceFile, string newName)//Renames a save file in the local path. Will return false if there was any error.
	{
		string path =  Application.persistentDataPath+"/Saves/";
		string pathAndSource = PathFileToLowerCase(path+sourceFile);
		string pathAndNewName = PathFileToLowerCase(path+newName);
		if(System.IO.File.Exists(pathAndSource)){
			System.IO.File.Move(pathAndSource,path+newName);
		}else{
			return false;
		}
		//Check if the backup file exists.
		string path1 = Path.GetDirectoryName(pathAndSource);
		string fName1 = Path.GetFileNameWithoutExtension(pathAndSource);
		string path2 = Path.GetDirectoryName(pathAndNewName);
		string fName2 = Path.GetFileNameWithoutExtension(pathAndNewName);
		if(System.IO.File.Exists(path1+"/"+fName1+"."+backupExtension)){
			System.IO.File.Move(path1+"/"+fName1+"."+backupExtension,path2+"/"+fName2+"."+backupExtension);
		}
		return true;
	}
	
	public static void ForceLoadPrefs()//Loads the registry information (PlayerPrefs) into memory.
	{
		/*In a few cases you might want to store registry data into memory at a specific moment, for example, if the data to retrieve
		is big enough like to cause hiccups while calling it. This is automatically called when you use a Get"Type"() method.
		This can be used to reload data into the game again; In case you overwrite some keys at runtime, but didn't call the Save() method after,
		the original values can still be recovered by using this method.
		*/
		LoadBankKeys();
	}
	
	//Coroutines. Check the Documentation section called "How to display a Loading/Saving dialogue" to understand how to use.
	public IEnumerator WaitForForceLoadPrefs()
	{
		requestStatus = -1;
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		LoadBankKeys();
	}
	
	public IEnumerator WaitForExportFile(string fileName,string[] keysToExport, bool makeBackup)
	{
		requestStatus = -1;
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		ExportFile(fileName,keysToExport,makeBackup);
	}
	
	public IEnumerator WaitForExportFile(string fileName,string[] keysToExport, KeyEx[] types, bool makeBackup)
	{
		requestStatus = -1;
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		ExportFile(fileName,keysToExport,types,makeBackup);
	}
	
	public IEnumerator WaitForSave()
	{
		requestStatus = -1;
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Save();
	}
	
	public IEnumerator WaitForSave(string[] keysToExport)
	{
		requestStatus = -1;
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Save(keysToExport);
	}
	
	public IEnumerator WaitForImportFile(string fileName,bool checkForBackup)
	{
		requestStatus = -1;
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		requestStatus = ImportFile(fileName,checkForBackup);
	}
	
	//Utility.
	public static void ShowKeyBankContent()//This will show the content of the key-bank in a pseudodecrypted way, you will rarely need to use this method. It doesn't work very well with large amounts of data.
	{//NOTE: 2D arrays are notorious for producing a blank Console Debug.
		string text = "Keybank values:(lastIndex = "+lastIndex+" | array Length:"+keyBank.GetLength(0)+")\n";
		for(int x = 0; x < (lastIndex+1); x++){
			if(string.IsNullOrEmpty(keyBank[x,0])){continue;}
			text += "@("+x.ToString("D3")+")["+keyBank[x,0]+" = ";
			if(keyBank[x,0].Substring(keyBank[x,0].Length-4,4) != "_sys"){
				text += keyBank[x,1];
			}else{
				text += keyBank[x,1];
			}
			text+= "]\n";
		}
		Debug.Log(text);
	}
	
	public static string[] GetKeyBankKeyNames()//This returns the names of all the keys in the keybank at the moment.
	{
		List<string> nameList = new List<string>();
		for(int x = 0; x < keyBank.GetLength(0); x+=2){
			if(string.IsNullOrEmpty(keyBank[x,0])){
				//break;
			}else{
				//remove the type part from the name("___$")
				nameList.Add("- "+keyBank[x,0].Substring(keyBank[x,0].IndexOf('$')+1));
			}
		}
		
		return nameList.ToArray();
	}
	
	
	//------- Private methods below, you should not need to call any of these directly, unless behaviour needs to be changed. -----------------
	
	private static string[,] keyBank = new string[100,2];//[x,0] = keyname, [x,1] = key Value.
	private static int lastIndex = -1;//The last index where a key was placed.
	private static bool firstRegLoad = false;//This becomes true if teh registry info is loaded.
	private static PlayerPrefsPro instancex;//the temporal instance of this script, only used when loading file with coroutine
	public static PlayerPrefsPro instance{get{if(!instancex){GameObject tmp = new GameObject();instancex = tmp.AddComponent<PlayerPrefsPro>();}return instancex;}set{instancex = value;}}//the temporal instance of this script, only used when loading file with coroutine
	public static int requestStatus;//the error code returned from any of the WaitFor_ method calls. "-1" = nothing has happened.
	//2D Array Int[,]
	private static string[] ToArrayFrom2DInt(int[,] arr2D)//Turns a 2D array into a regular string array with each row as a json formated text.
	{
		string[] finalArray = new string[arr2D.GetLength(0)];
		string[] tmpArray;
		for(int x = 0; x < finalArray.Length; x++){
			tmpArray = new string[arr2D.GetLength(1)];
			for(int y = 0; y < arr2D.GetLength(1); y++){
				tmpArray[y] = arr2D[x,y].ToString(); 
			}	
			finalArray[x] = SerializeWJson<string[]>(tmpArray);//Here we serialize the rows
		}
		return finalArray;
	}
	
	private static int[,] To2DFromArrayInt(string[] arr)//Inverse of ToArrayFrom2DInt()
	{
		string[] tmpArray;
		int[,] finalArray = new int[0,0];
		for(int x = 0; x < arr.Length; x++){
			tmpArray = DeserializeWJson<string[]>(arr[x]);
			if(finalArray.GetLength(0) == 0){finalArray = new int[arr.Length,tmpArray.Length];}//inicializar array final
			for(int y = 0; y < finalArray.GetLength(1); y++){
				finalArray[x,y] = int.Parse(tmpArray[y]);
			}	
		}	
		return finalArray;
	}
	
	//2D Array String[,]
	private static string[] ToArrayFrom2DString(string[,] arr2D)
	{
		string[] finalArray = new string[arr2D.GetLength(0)];
		string[] tmpArray;
		for(int x = 0; x < finalArray.Length; x++){
			tmpArray = new string[arr2D.GetLength(1)];
			for(int y = 0; y < arr2D.GetLength(1); y++){
				tmpArray[y] = arr2D[x,y]; 
			}	
			finalArray[x] = SerializeWJson<string[]>(tmpArray);
		}
		return finalArray;
	}
	
	private static string[,] To2DFromArrayString(string[] arr)
	{
		string[] tmpArray;
		string[,] finalArray = new string[0,0];
		for(int x = 0; x < arr.Length; x++){
			tmpArray = DeserializeWJson<string[]>(arr[x]);
			if(finalArray.GetLength(0) == 0){finalArray = new string[arr.Length,tmpArray.Length];}
			for(int y = 0; y < finalArray.GetLength(1); y++){
				finalArray[x,y] = tmpArray[y];
			}	
		}	
		return finalArray;
	}
	
	//2D Array Float[,]
	private static string[] ToArrayFrom2DFloat(float[,] arr2D)
	{
		string[] finalArray = new string[arr2D.GetLength(0)];
		string[] tmpArray;
		for(int x = 0; x < finalArray.Length; x++){
			tmpArray = new string[arr2D.GetLength(1)];
			for(int y = 0; y < arr2D.GetLength(1); y++){
				tmpArray[y] = arr2D[x,y].ToString(); 
			}	
			finalArray[x] = SerializeWJson<string[]>(tmpArray);
		}
		return finalArray;
	}
	
	private static float[,] To2DFromArrayFloat(string[] arr)
	{
		string[] tmpArray;
		float[,] finalArray = new float[0,0];
		for(int x = 0; x < arr.Length; x++){
			tmpArray = DeserializeWJson<string[]>(arr[x]);
			if(finalArray.GetLength(0) == 0){finalArray = new float[arr.Length,tmpArray.Length];}
			for(int y = 0; y < finalArray.GetLength(1); y++){
				finalArray[x,y] = float.Parse(tmpArray[y]);
			}	
		}	
		return finalArray;
	}
	
	//2D Array Bool[,]
	private static string[] ToArrayFrom2DBool(bool[,] arr2D)
	{
		string[] finalArray = new string[arr2D.GetLength(0)];
		string[] tmpArray;
		for(int x = 0; x < finalArray.Length; x++){
			tmpArray = new string[arr2D.GetLength(1)];
			for(int y = 0; y < arr2D.GetLength(1); y++){
				tmpArray[y] = arr2D[x,y].ToString(); 
			}	
			finalArray[x] = SerializeWJson<string[]>(tmpArray);
		}
		return finalArray;
	}
	
	private static bool[,] To2DFromArrayBool(string[] arr)
	{
		string[] tmpArray;
		bool[,] finalArray = new bool[0,0];
		for(int x = 0; x < arr.Length; x++){
			tmpArray = DeserializeWJson<string[]>(arr[x]);
			if(finalArray.GetLength(0) == 0){finalArray = new bool[arr.Length,tmpArray.Length];}
			for(int y = 0; y < finalArray.GetLength(1); y++){
				finalArray[x,y] = StringToBool(tmpArray[y]);
			}	
		}	
		return finalArray;
	}
	
	private static string[] DateArrToStringArr(DateTime[] arr)
	{
		string[] final = new string[arr.Length];
		for(int x = 0; x < arr.Length; x++){
			final[x] = arr[x].ToString("MM/dd/yyyy HH:mm:ss");
		}
		return final;
	}
	
	private static DateTime[] StringArrToDateArr(string[] arr)
	{
		DateTime[] final = new DateTime[arr.Length];
		for(int x = 0; x < arr.Length; x++){
			final[x] = DateTime.ParseExact(arr[x],"MM/dd/yyyy HH:mm:ss",System.Globalization.CultureInfo.InvariantCulture);
		}	
		return final;	
	}
	
	private static void SetBankKey(string keyName,string keyValue)//This method Sets keynames with the type as prefix, ex: int$myVar
	{
		if(firstRegLoad == false){
			LoadBankKeys();
		}
		int index = FindBankKeyIndex(keyName);
		if(index != -1){//If the keyname already exist.
			keyBank[index,1] = keyValue;//We replace value at index.
		}else{//we add the key to the list
			if(lastIndex >= (keyBank.GetLength(0)-1)){//We make sure there are enough slots available.
				Debug.Log(lastIndex+" >=? "+(keyBank.GetLength(0)-1));
				string[,] copy = keyBank.Clone() as string[,];
				int targetLength = ((lastIndex/100)+1)*100;
				keyBank = new string[targetLength,2];//We add 100 new key slots.
				for(int x = 0; x < copy.GetLength(0); x++){
					keyBank[x,0] = copy[x,0];
					keyBank[x,1] = copy[x,1];
				}
			}
			//Assign value
			lastIndex++;
			keyBank[lastIndex,0] = keyName;
			keyBank[lastIndex,1] = keyValue;
		}
	}
	
	private static string GetBankKey(string keyName, string defaultValue)
	{
		if(firstRegLoad == false){
			LoadBankKeys();
		}
		int index = FindBankKeyIndex(keyName);
		if(index != -1){
			return keyBank[index,1];//keyvalue
		}else{//Key name doesn't exist.
			return defaultValue;
		}
	}
	
	private static void DeleteBankKey(string keyName)
	{
		int index = FindBankKeyIndex(keyName);
		if(index != -1){
			keyBank[index,0] = keyBank[lastIndex,0];
			keyBank[index,1] = keyBank[lastIndex,1];
			keyBank[lastIndex,0] = "";
			keyBank[lastIndex,1] = "";
			lastIndex--;
			//if(lastIndex == -1){lastIndex = 0;}
		}
	}
	
	private static int FindBankKeyIndex(string keyName)
	{
		for(int x = 0; x < (lastIndex+1); x++){
			if(keyBank[x,0] == keyName){
				return x;
			}
		}
		return -1;//The key was not found.
	}
	
	private static void LoadBankKeys()//Load keys from registry (if any exist).
	{
		UnityEngine.Profiling.Profiler.BeginSample("Load Sample");
		#pragma warning disable 168
		int count = 0; string line = "x"; string final = ""; List<string> strList = new List<string>();int x;string regKey;
		//Retrieve all info
		int lineAmt = 0;
		line = PlayerPrefs.GetString(EnDecrypt("rsv_linex",key2));
		//here we read the amount of lines that are stored in registry
		if(!string.IsNullOrEmpty(line)){
			line = EnDecrypt(line,(key1+key2)/2);
			string[] comp = line.Split(new string[]{"_"}, StringSplitOptions.None);
			if(comp.Length != 2 || GetMd5Hash(comp[0]) != comp[1]){
				ThrowCorruptFileWarning("Registry was modified!",0);
			}
			lineAmt = int.Parse(comp[0]);
			//now we delete old keys which are beyond the line range
			count = lineAmt;
			while(true){
				string name = "rsv_line"+count++;
				regKey = EnDecrypt(name,(key1+count)/2);
				line = PlayerPrefs.GetString(regKey,"");
				PlayerPrefs.DeleteKey(regKey);
				if(string.IsNullOrEmpty(line)){
					break;
				}
			}
		}
		count = 0;
		for(x = 0; x < lineAmt; x++){
			regKey = EnDecrypt("rsv_line"+count++,(key1+count)/2);
			line = PlayerPrefs.GetString(regKey,"");
			strList.Add(line);
		}
		final = String.Concat(strList.ToArray());
		if(string.IsNullOrEmpty(final)){firstRegLoad = true;return;}//Nothing to load.
		//Check integrity
		try{line = final.Substring(final.Length-5,5);}
		catch(ArgumentOutOfRangeException e){
			ThrowCorruptFileWarning("Registry was modified!",0);
		}
		if(line != "<¦@¦>"){
			ThrowCorruptFileWarning("Registry was modified!",0);
		}
		final = final.Substring(0,final.Length-5);
		final = EnDecrypt(final,(key1+key2)/2);
		int result = DataToBank(final);
		if(result == 2){
			ThrowCorruptFileWarning("Registry was modified!",0);
		}
		#pragma warning restore 168
		UnityEngine.Profiling.Profiler.EndSample();
	}
	
	private static int DataToBank(string final)//Adds the registry or a file string to the bank values.
	{
		string[] info = final.Split(new string[]{"<¦%¦>"}, StringSplitOptions.None);
		if(info.Length != 2){
			return 2;//The file is corrupted.
		}
		if(GetMd5Hash(info[0]) != info[1]){
			return 2;//The file is corrupted.
		}
		string[] pprefs = DeserializeWJson<string[]>(info[0]);
		//Assign keys to keyBank
		int x;
		if(!firstRegLoad){
			int targetLength = ((pprefs.Length/100)+1)*100;
			keyBank = new string[targetLength,2];
			for(x = 0; x < (pprefs.Length/2); x++){
				keyBank[x,0] = pprefs[x*2];
				keyBank[x,1] = pprefs[(x*2)+1];
			}
			lastIndex = x-1;
		}else{
			for(x = 0; x<(pprefs.Length/2); x++){
				SetBankKey(pprefs[x*2],pprefs[x*2 + 1]);
			}
		}	
		firstRegLoad = true;
		return 0;//Load success.
	}
	
	private static List<KeyEx> GetKeyType(string key)//This function returns all the types a variable exists for. EmptyArray means there is no type for it.
	{
		List<KeyEx> types = new List<KeyEx>();
		foreach (KeyEx kval in Enum.GetValues(typeof(KeyEx)))
		{
			if(HasKey(key,kval)){
				types.Add(kval);
			}
		}
		
		return types;
	}
	
	private static string[] GenBankKeyList()//list of all the available keys.
	{
		List<string> finalKeys = new List<string>();
		for(int x = 0; x < (lastIndex+1); x++){
			finalKeys.Add(keyBank[x,0]);
		}

		return finalKeys.ToArray();	
	}
	
	private static string[] GenBankKeyList(string[] basicKeyNames)//Generates the list of real key names bsed on any type.
	{
		List<string> finalKeys = new List<string>();
		for(int x = 0; x < basicKeyNames.Length; x++){
			List<KeyEx> keyTypes = GetKeyType(basicKeyNames[x]);//All the available types of actual keyname is it exist.
			foreach(KeyEx kType in keyTypes){
				finalKeys.Add(GetKeyExtension(kType)+"$"+basicKeyNames[x]);
				finalKeys.Add(GetKeyExtension(kType)+"$"+basicKeyNames[x]+"_sys");
			}
		}
		
		return finalKeys.ToArray();
	}
	
	private static string[] GenBankKeyList(string[] basicKeyNames, KeyEx[] keyTypes)//Generates the list of real key names bsed on specific type.
	{
		List<string> finalKeys = new List<string>();
		for(int x = 0; x < basicKeyNames.Length; x++){
			finalKeys.Add(GetKeyExtension(keyTypes[x])+"$"+basicKeyNames[x]);
			finalKeys.Add(GetKeyExtension(keyTypes[x])+"$"+basicKeyNames[x]+"_sys");
		}

		return finalKeys.ToArray();	
	}
	
	private static void SaveInfo(string[] keyList,bool regOrFile,string fileName,bool makeBackup)//"keyList" has to be in KEYBANK Name format! ex: type$keyname. It must include the "_sys" key too.
	{	
		if(keyList.Length == 0){DeleteAllProKeys();return;}//Nothing to save! We actually delete all old keys instead.
		int x; int index;
		List<string> pprefs = new List<string>();
		for(x = 0; x<keyList.Length;x++){
			if(FindBankKeyIndex(keyList[x]) == -1){//if a requested key doesnt exist, skip it.
				#if UNITY_EDITOR
				Debug.Log(keyList[x]+" is missing from key bank.");
				#endif
				continue;
			}
			index = FindBankKeyIndex(keyList[x]);
			pprefs.Add(keyBank[index,0]);//key
			pprefs.Add(keyBank[index,1]);//value
		}
		
		string json = SerializeWJson<string[]>(pprefs.ToArray());
		string md5 = GetMd5Hash(json);
		string final = json + "<¦%¦>" + md5;
		final = EnDecrypt(final,(key1+key2)/2);
		//Split in fragments.
		int fragmentLength = 10000;
		x = 0; int count = 0;
		string lineName;
		if(regOrFile){//If will be saved on playerprefs.
			while(x < final.Length){
				string part;
				if((x+fragmentLength) < final.Length){
					part = final.Substring(x,fragmentLength);
					x+=fragmentLength;
				}else{
					part = final.Substring(x,final.Length-x)+"<¦@¦>";
					x = final.Length;
				}
				lineName = EnDecrypt("rsv_line"+count++,(key1+count)/2);
				PlayerPrefs.SetString(lineName,part);
			}
			//Key of amount of lines to load next time.
			lineName = EnDecrypt("rsv_linex",key2);
			PlayerPrefs.SetString(lineName,EnDecrypt(count+"_"+GetMd5Hash(count.ToString()),(key1+key2)/2));
			PlayerPrefs.Save();
		
		}else{//If will be saved on file.
			
			string path = PathFileToLowerCase(Application.persistentDataPath+"/Saves/"+fileName);
			if(!System.IO.Directory.Exists(Path.GetDirectoryName(path))){
				System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path));
			}
			System.IO.File.WriteAllText(path,final);//Main save file.
			if(makeBackup){	
				string directory = Path.GetDirectoryName(path);
				string fName = Path.GetFileNameWithoutExtension(path);
				System.IO.File.WriteAllText(directory+"/"+fName+"."+backupExtension,final);//Backup save file.
			}
		}

	}
	
	private static int TryToLoadFile(string path)
	{
		#pragma warning disable 168 //This disables a silly error type warning.
		//We try to read the file.
		string content;
		try{
			content = System.IO.File.ReadAllText(PathFileToLowerCase(path));
		}catch(FileNotFoundException e){
			return 1;
		}
		
		content = EnDecrypt(content,(key1+key2)/2);
		
		return DataToBank(content);
		
		#pragma warning restore 168
	}
	
	private static bool ValuesMatch(string val1, string val2,bool areEncrypted)//Returns false if values mismatch or the type doesn't make sense.
	{
		bool correct = false;
		
		//Decrypt content if passed values are encrypted
		if(areEncrypted){
			val1 = GetMd5Hash(val1);
		}
		//Check if values are same (This is different than being valid values).
		if(val1 == val2){
			correct = true;
		}
		
		return correct;
	}	
		
	private static string EnDecrypt(string content,int key)
	{
		System.Text.StringBuilder a = new System.Text.StringBuilder(content);//clone string for faster access.
		System.Text.StringBuilder b = new System.Text.StringBuilder(content.Length);//allocate size
		char c;
		int round = 1;
		for (int x = 0; x < content.Length; x++){
			c = a[x];
			c = (char)(c ^ (key + round));
			b.Append(c);
			
			round+=1;
			if(round > 99){round = 1;}
		}
		return b.ToString();
	}
	
	private static bool StringToBool(string str)
	{
		if(str.ToLower() == "true"){return true;}
		else {return false;}
	}
	
	private static void SetTypeX<T>(string keyname,T content,KeyEx kType)
	{
		if(string.IsNullOrEmpty(keyname)){Debug.LogError("<PlayerPrefsPro Error>: keyNames cannot be null!");}
		//Encrypt content of "content".
		string content1 = keyname+"<¦§¦>"+SerializeWJson<T>(content);
		string content2 = GetMd5Hash(content1);
		SetBankKey(GetKeyExtension(kType)+"$"+keyname, content1);
		SetBankKey(GetKeyExtension(kType)+"$"+keyname+"_sys", content2);
	}
	
	private static T GetTypeX<T>(string keyname, string defaultValue,KeyEx kType)//The default value is ALWAYS a serialized Json.
	{
		if(string.IsNullOrEmpty(keyname)){Debug.LogError("<PlayerPrefsPro Error>: keyNames cannot be null!");}
		string content1 = GetBankKey(GetKeyExtension(kType)+"$"+keyname, defaultValue);
		string content2 = GetBankKey(GetKeyExtension(kType)+"$"+keyname+"_sys", defaultValue);
		//Decrypt content of "content".
		GenericPPPObj<T> obj = new GenericPPPObj<T>();
		if(content1 == defaultValue){//No stored value exist.
			return DeserializeWJson<T>(defaultValue);//Data here will be null.
		}else{//If an stored string was found:
			if(ValuesMatch(content1,content2,true)){//Since they are json strings, we use string comparison for all weird data types.
				//Here we check if the value actually originate from same key name, to prevent registry value replacement with similar types.
				string[] tmp = content1.Split(new string[]{"<¦§¦>"}, StringSplitOptions.None);
				if(tmp.Length != 2){ThrowCorruptFileWarning(keyname+" entry is corrupted",0);}//this can happen if format was altered.
				if(tmp[0] != keyname){ThrowCorruptFileWarning(keyname+" entry is corrupted",0);}//if the value doesnt belong to this key.
				obj.data = DeserializeWJson<T>(tmp[1]);
					return obj.data;
			}else{
				ThrowCorruptFileWarning(keyname+" entry is corrupted",0);
				return obj.data;//this value means error.
			}
		}
	}
	
	private static string GetKeyExtension(KeyEx keys)
	{
		switch(keys)
		{
			case KeyEx.kInt: return "int";
			case KeyEx.kString: return "str";
			case KeyEx.kFloat: return "flo";
			case KeyEx.kBool: return "boo";
			case KeyEx.kDateTime: return "dat";
			case KeyEx.kVector2: return "ve2";
			case KeyEx.kVector3: return "ve3";
			case KeyEx.kVector4: return "ve4";
			case KeyEx.kColor: return "clr";
			case KeyEx.kIntArray: return "inta";
			case KeyEx.kStringArray: return "stra";
			case KeyEx.kFloatArray: return "floa";
			case KeyEx.kBoolArray: return "booa";
			case KeyEx.kDateTimeArray: return "data";
			case KeyEx.kVector2Array: return "ve2a";
			case KeyEx.kVector3Array: return "ve3a";
			case KeyEx.kVector4Array: return "ve4a";
			case KeyEx.kColorArray: return "clra";
			case KeyEx.kIntArray2D: return "inta2d";
			case KeyEx.kStringArray2D: return "stra2d";
			case KeyEx.kFloatArray2D: return "floa2d";
			case KeyEx.kBoolArray2D: return "booa2d";
			default: return "_";
		}
	}
	
	private static string SerializeWJson<T>(T toSerialize)
	{
		GenericPPPObj<T> a = new GenericPPPObj<T>(toSerialize);
		return JsonUtility.ToJson(a); 
	}
	
	private static T DeserializeWJson<T>(string toDeserialize)
	{
		GenericPPPObj<T> a = new GenericPPPObj<T>();
		a = JsonUtility.FromJson<GenericPPPObj<T>>(toDeserialize);
		return a.data;
	}
	
	private static string GetMd5Hash(string input)
    {
       System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
       byte[] data = md5Hasher.ComputeHash(System.Text.Encoding.Default.GetBytes(input));
	   
       System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();

       for (int i = 0; i < data.Length; i++)
       {
           sBuilder.Append(data[i].ToString("x2"));
       }

       return sBuilder.ToString();
    }
	
	public static string PathFileToLowerCase(string path)//converts the file name of a path to lowerCase
	{
		string dir = Path.GetDirectoryName(path);
		string fileName = Path.GetFileName(path).ToLowerInvariant();
		return Path.Combine(dir,fileName);
	}
	
}

[Serializable]
public class GenericPPPObj<T>
{
	public T data;
	
	public GenericPPPObj(){}
	public GenericPPPObj(T zdata)
    {

		data = zdata;
    }
}
