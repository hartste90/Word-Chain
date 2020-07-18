using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PPPTest_LoadSign : MonoBehaviour {

	public GameObject savePanel;
	public Text xText;
	
	void Start () {
		Save();
	}
	
	void Save(){
		StartCoroutine(SaveCo());
	}
	
	IEnumerator SaveCo(){
		
		//Show loading... sign.
		savePanel.SetActive(true);
		xText.text = "Loading...";
		yield return PlayerPrefsPro.instance.WaitForForceLoadPrefs();//first time read from registry.
		savePanel.SetActive(false);
		
		//We set a delay to save game after 1 second.
		yield return new WaitForSeconds(1);
		
		//We create a heavy array to freeze the game while saving/loading
		float[,] farray = new float[500,500];
		for(int x = 0; x < farray.GetLength(0); x++){
			for(int y = 0; y < farray.GetLength(1); y++){
				farray[x,y] = Random.value;
			}
		}
		
		PlayerPrefsPro.SetFloatArray2D("2DFloatArrayTest",farray);
		
		//We show sign and save.
		savePanel.SetActive(true);
		xText.text = "Saving...";
		yield return PlayerPrefsPro.instance.WaitForSave();
		savePanel.SetActive(false);
	}
}
