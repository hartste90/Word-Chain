using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HoverTexts : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public enum DescT{
		btn_savePrefs,
		btn_deletaAllprefs,
		btn_exportFile,
		btn_importFile,
		btn_deleteFile,
		btn_rand,
		btn_setCurrent,
		btn_profileSave,
		btn_profileLoad,
		btn_profileDelete,
		btn_resetValues
		}
	public DescT descriptionType; 
	
	public void OnPointerEnter(PointerEventData eventData) {
		Camera.main.GetComponent<PPPTest_General>().hoverDescriptionDisplay.text = GetText(descriptionType);
	}	
	
	public void OnPointerExit(PointerEventData eventData) {
		Camera.main.GetComponent<PPPTest_General>().hoverDescriptionDisplay.text = "Hover an element to reveal the related information.";
	}	
	
	string GetText(DescT type)
	{
		switch(type){
			case DescT.btn_savePrefs: return "Save this panel current values into the registry or target platform default playerprefs location. 'PlayerPresPro.Save();'";
			case DescT.btn_deletaAllprefs: return "Delete All the keys stored in the registry(Including keys not made with this plugin). 'PlayerPresPro.DeleteAll();'. Specific keys can be deleted by using 'PlayerPresPro.DeleteKey(keyName);'";
			case DescT.btn_exportFile: return "Export current keys to a file called settings.cfg. This file is stored at Application.persistentDataPath/Saves/fileName. 'PlayerPresPro.ExportKeys(string fileName,string[] keysToExport, bool makeBackup);'";
			case DescT.btn_importFile: return "Import setting keys from a file called settings.cfg. This file is stored at Application.persistentDataPath/Saves/fileName. 'PlayerPresPro.ImportFile(string fileName,bool checkForBackup);'";
			case DescT.btn_deleteFile: return "If a file called settings.cfg exist, delete it. 'PlayerPresPro.DeleteSaveFile(string fileName);'";
			case DescT.btn_rand: return "Assigns a random value to the field at the left.";
			case DescT.btn_setCurrent: return "Sets value to current system time. 'PlayerPresPro.SetDateTime(string keyname,DateTime content);'";
			case DescT.btn_profileSave: return "Create a file in the harddrive containing the current panel information. 'PlayerPresPro.ExportFile(string fileName,string[] keysToExport, bool makeBackup);'";
			case DescT.btn_profileLoad: return "Load a file in the harddrive containing the current panel information if it exist. 'PlayerPresPro.ImportFile(string fileName,bool checkForBackup);'";
			case DescT.btn_profileDelete: return "Detele the profile file in the harddrive if it exist. 'PlayerPresPro.DeleteSaveFile(string fileName);'";
			case DescT.btn_resetValues: return "Reset default panel values without modifying preferences saved on disc.";
		}
		return "";
	}
}
