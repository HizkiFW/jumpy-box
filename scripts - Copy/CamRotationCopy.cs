using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotationCopy : MonoBehaviour {

    public GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.rotation = Quaternion.Euler(0, 0, target.transform.rotation.eulerAngles.z);
	}
}
