using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonHandler : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(gameObject.transform.position.y < -10)
            Destroy(gameObject);

        if((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Application.isEditor ? (Vector2) Input.mousePosition : Input.GetTouch(0).position);
            RaycastHit hit;
            //Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 100f);
            if(Physics.Raycast(ray, out hit)) {
                Debug.Log(hit.transform.name);
                if (hit.collider != null) {
                    GameObject touchedObject = hit.transform.gameObject;
                    
                    Debug.Log("Touched " + touchedObject.transform.name);
                    if(touchedObject == gameObject) {
                        // Touched self
                        Destroy(gameObject.GetComponent<Animator>());
                        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                        rb.AddForceAtPosition(Vector3.forward * 1000, hit.point);
                        player.GetComponent<PlayerScript>().gameStarted = true;
                    }
                }
            }
        }	
	}
}
