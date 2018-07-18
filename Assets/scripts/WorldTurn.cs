using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTurn : MonoBehaviour {

    public float maxAngle = 25;
    public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float angle = Mathf.Atan2(Input.acceleration.x, -Input.acceleration.y)*Mathf.Rad2Deg;

        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.RotateAround(player.transform.position, Vector3.forward, -angle);
	}
}
