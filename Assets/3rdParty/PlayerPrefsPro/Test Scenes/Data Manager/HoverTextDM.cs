using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HoverTextDM : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	
	public enum DescT{
		btn_qaSettings,
		btn_qaLevel,
		fld_inpTextOnly,
		fld_inpNumbersOnly,
		btn_setValuesPNfo,
		btn_setValuesGSett,
		btn_savKetbk2reg,
		btn_savPlInfoOnly,
		btn_forceLoadProK,
		btn_cleanBank,
		btn_cleanBankExcPInfo,
		btn_delAllProKeys,
		btn_delAllUniPPrefK,
		btn_delAllKeys,
		txt_keybankTitle,
		//Level panel items (not loaded)
		txt_nolvlData,
		btn_genLevel,
		btn_cloneLevel,
		//Level panel items (loaded)
		img_map,
		idsList,
		btn_generate,
		btn_delFrmKbank,
		btn_delFile
	}
	public DescT descriptionType; 
	
	private static PPPTest_DataManager cmpcache;
	
	void Start()
	{
		if(!cmpcache){cmpcache = Camera.main.GetComponent<PPPTest_DataManager>();}
	}
	
	public void OnPointerEnter(PointerEventData eventData) {	
		cmpcache.UpdateHoverDescription(GetText(descriptionType));
	}	
	
	public void OnPointerExit(PointerEventData eventData) {
		cmpcache.UpdateHoverDescription(cmpcache.noHoverTextB);
	}	
	
	string GetText(DescT type)
	{
		switch(type){
			case DescT.btn_qaSettings: return "Shows the user settings panel.";
			case DescT.btn_qaLevel: return "Shows the level data from 1 to 8.";
			case DescT.fld_inpTextOnly: return "Insert TEXT only.";
			case DescT.fld_inpNumbersOnly: return "Insert NUMBERS only.";
			case DescT.btn_setValuesPNfo: return "Sets the player Info to PlayerPrefsPro key-bank (not the registry). <b>SetString(string keyname,string content);</b>";
			case DescT.btn_setValuesGSett: return "Sets the game settings using the regular PlayerPrefs class. <b>PlayerPrefs.SetInt(int number, int value);</b>";
			case DescT.btn_savKetbk2reg: return "Writes and encrypts current key-bank keys to the registry. <b>PlayerPrefsPro.Save();</b>";
			case DescT.btn_savPlInfoOnly: return "Writes and encrypts a specific list of keys to the registry. <b>PlayerPrefsPro.Save(string[] keysToExport)</b>";
			case DescT.btn_forceLoadProK: return "Forces the registry data block into the key-bank. This process is normally automatic. <b>PlayerPrefsPro.ForceLoadPrefs();</b>";
			case DescT.btn_cleanBank: return "Deletes all keys from the key-bank only. The registry data block is not affected.<b>PlayerPrefsPro.CleanBank();</b>";
			case DescT.btn_cleanBankExcPInfo: return "Same as CleanBank() but it excludes certain keys from deletion. <b>CleanBankExcludingKeys(string[] keysToExclude);</b>";
			case DescT.btn_delAllProKeys: return "Deletes all the keys related to PlayerPrefsPro from the key-bank and registry.<b>PlayerPrefsPro.DeleteAllProKeys();</b>";
			case DescT.btn_delAllUniPPrefK: return "Deletes all the Unity's PlayerPrefs keys only.<b>PlayerPrefs.DeleteAll();</b>";
			case DescT.btn_delAllKeys: return "Deletes all Unity's PlayerPrefs and PlayerPrefsPro keys from key-bank and registry.<b>PlayerPrefsPro.DeleteAll();</b>";
			case DescT.txt_keybankTitle: return "Here are shown all the keys that exist in the memory at the moment, which are handled by PlayerPrefsPro.<b>PlayerPrefsPro.GetKeyBankKeyNames();</b>";
			case DescT.txt_nolvlData: return "We checked if the file exists by using <b>PlayerPrefsPro.DoLocalFileExist(string fileName,bool checkForBackupToo);</b>";
			case DescT.btn_genLevel: return "We <Set> three keys and pass their key-names to an array, then we create a local file containing them. <b>PlayerPrefsPro.ExportFile(string fileName,string[] keysToExport, bool makeBackup);</b>";
			case DescT.btn_cloneLevel: return "This clones an existing local file into another file name/path. <b>PlayerPrefsPro.CloneSaveFile(string fileNameOrigin, string fileNameTarget);</b>";
			case DescT.img_map: return "This map shows the data from the array fetched with PlayerPrefsPro.GetIntArray2D('levelMap'), which we created while generating the level.";
			case DescT.idsList: return "Representation of a generic int array fetched with <b>PlayerPrefsPro.GetIntArray(string keyname);</b>";
			case DescT.btn_generate: return "Does the same thing as the generation button we used to generate the level.";
			case DescT.btn_delFrmKbank: return "Deletes the level-associated keys from the key-bank using <b>PlayerPrefsPro.DeleteKey(string key);</b>. This button is disabled if no keys exist. <b>PlayerPrefsPro.HasKey('levelMap');</b>";
			case DescT.btn_delFile: return "Deletes the local file using <b>PlayerPrefsPro.DeleteSaveFile(string fileName)</b>.";
		}
		return "";
	}
}
