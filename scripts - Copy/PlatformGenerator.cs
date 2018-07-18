using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour {

    public int heightStep = 5;
    public int sideStep = 12;
    public int generationDistance = 3;
    public GameObject platformObject;
    public GameObject player;

    private List<GameObject> activePlatforms;
    private int currentHeight = 0;
    private int currentX = 0;
    private int platformsCreated = 0; // Used for platform priority

	// Use this for initialization
	void Start () {
		activePlatforms = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        // Check if player is ready
        if(!player.GetComponent<PlayerScript>().isReady)
            return;

        // Make new platforms
	    if(player.transform.position.y >= currentHeight - (generationDistance * heightStep)) {
            currentHeight += heightStep;
            activePlatforms.Add(makePlatform(0, currentHeight));
        }
        if(Mathf.Abs(player.transform.position.x) > currentX) {
            currentX += sideStep;
            int playerHeight = 0;

            while(playerHeight < player.transform.position.y)
                playerHeight += heightStep;

            activePlatforms.Add(makePlatform(sideStep, playerHeight));
            activePlatforms.Add(makePlatform(sideStep, playerHeight+heightStep));
            activePlatforms.Add(makePlatform(sideStep, playerHeight-heightStep));
        }

        // Check if platform still on screen
        foreach(GameObject platform in activePlatforms) {
            if(platform.transform.position.y < player.transform.position.y - (generationDistance * heightStep)
                    || platform.transform.position.y < 0) {
                // Assume platform below screen, kill it for performance
                Debug.Log("Kill this platform!");
                Destroy(platform);
                activePlatforms.Remove(platform);
            }
        }
	}

    GameObject makePlatform(int x, int y) {
        GameObject newPlatform = Object.Instantiate(platformObject);
        float playerX = player.transform.position.x;
        Vector3 newPosition = new Vector3(x + Random.Range(playerX-5.0f, playerX+5.0f), y, 0);
        newPlatform.transform.position = newPosition;
        newPlatform.GetComponent<PlatformController>().priority = platformsCreated;
        platformsCreated++;
        return newPlatform;
    }
}
