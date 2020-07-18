using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {

	void Update () {
		transform.Rotate(transform.forward*Time.deltaTime*100);
		transform.localScale = Vector3.Slerp(new Vector3(0.5f,0.5f,0.5f), new Vector3(1.5f,1.5f,1.5f),Mathf.PingPong(Time.time,1));
	}
}
