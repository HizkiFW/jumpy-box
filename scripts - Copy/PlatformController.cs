using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

    public int priority;

    [SerializeField] private float journeyTime;
    [SerializeField] private float travelDistance;
    [SerializeField] private float startTime;
    [SerializeField] private bool direction;
    [SerializeField] private Vector3 origin, target;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
        direction = Random.value > 0.5;
        origin = gameObject.transform.position;
        travelDistance = (Random.value > 0.5 ? -1 : 1) * Random.Range(3, 10);
        target = new Vector3(travelDistance + origin.x, origin.y, 0);
		journeyTime = (float) 0.25 * Mathf.Abs(travelDistance) * Random.Range(2, 3);
	}
	
	// Update is called once per frame
	void Update () {
        // Does it need to turn back?
        if(Time.time - startTime >= journeyTime) {
            startTime = Time.time;
            direction = !direction;
        }

        // Interpolate location
        Vector3 pos;
		if(direction)
            pos = Vector3.Slerp(origin, target, (Time.time - startTime) / journeyTime);
        else
            pos = Vector3.Slerp(target, origin, (Time.time - startTime) / journeyTime);

        // Fix Slerp curving the path
        pos.y = origin.y;
        pos.z = origin.z;

        gameObject.transform.position = pos;
	}
}
