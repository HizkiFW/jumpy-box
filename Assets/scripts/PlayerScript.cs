using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    public float maxJumpForce = 160.0f; // Amount of force for a single jump
    public GameObject world;
    public GameObject camera;
    public GameObject aimer;
    public ScoreTracker scoreTracker;
    public float deathThreshold = 25; // Kill player if this much below max height

    public GameObject gameOverSection; // GameOver parent object
    public TextMesh gameOverScoreText; // Current score
    public TextMesh gameOverBestText; // Record score
    public Mesh gameOverNewBestText; // Mesh to replace Game Over message
    public GameObject gameOverText; // The text that flies in
    public Transform gameOverTarget; // Where the camera will go when player dies

    public bool isReady = false; // Can the game be started yet?
    public bool gameStarted = false; // Is the play button clicked yet?
    public bool isDead = false; // True if player is dead

    private Rigidbody2D rb;
    private Text scoreUI;
    private ParticleSystem aimps;
    private AudioSource au;
    private bool canJump = false;
    private float maxHeight = 0;
    //private float lastAngle = 0;

    void Start () {
        // Initialize variables
        rb = GetComponent<Rigidbody2D>();
        aimps = aimer.GetComponent<ParticleSystem>();
        au = GetComponent<AudioSource>();
    }
    
    void Update () {
        // Rotate Camera according to accelerometer
        /* float angle = -Mathf.Atan2(Input.acceleration.x, -Input.acceleration.y)*Mathf.Rad2Deg;
        lastAngle = Mathf.Lerp(lastAngle, angle, 5*Time.deltaTime);
        if(Application.isEditor) lastAngle = -45*((Input.mousePosition.x - (Screen.width/2))/(Screen.width/2));
        camera.transform.rotation = Quaternion.Euler(0, 0, lastAngle); */

        // Check max height
        if(gameObject.transform.position.y > maxHeight)
            maxHeight = gameObject.transform.position.y;

        // Check if dead
        if(gameObject.transform.position.y < maxHeight - deathThreshold) {
            isDead = true;
            gameOverSection.SetActive(true);
            camera.GetComponent<UnityStandardAssets.Cameras.AutoCam>().m_Target = gameOverTarget;
            gameOverScoreText.text = scoreTracker.score.ToString();
            if(scoreTracker.score > ScoreTracker.LoadScore()) {
                Debug.Log("new high score, switching mesh");
                gameOverText.GetComponent<MeshFilter>().mesh = gameOverNewBestText;
                gameOverBestText.text = scoreTracker.score.ToString();
            } else gameOverBestText.text = ScoreTracker.LoadScore().ToString();
            gameObject.SetActive(false);
            // SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        // Stop if the game isn't started yet
        if(!gameStarted) return;

        // Aiming calculator
        if(Input.touchCount == 1) {
            if(Input.GetTouch(0).phase == TouchPhase.Began) {
                // Create particles
                aimer.SetActive(true);
            } else if(Input.GetTouch(0).phase == TouchPhase.Moved) {
                // Aim particles
                Vector3 touchPos = Application.isEditor ? Input.mousePosition : (Vector3) Input.GetTouch(0).position;
                touchPos.z = Vector3.Distance(camera.transform.position, gameObject.transform.position);
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(touchPos);

                aimer.transform.LookAt(worldPos);

                float force = Vector3.Distance(worldPos, gameObject.transform.position);
                ParticleSystem.MainModule psmain = aimps.main;
                psmain.startSpeed = force;
            } else if(Input.GetTouch(0).phase == TouchPhase.Ended) {
                // Kill particles
                aimer.SetActive(false);
            }
        }

        // Jumping
        if((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended) || (Input.touchCount == 0 && Application.isEditor && Input.GetMouseButtonDown(0))) {
            // User tapped the screen
            Debug.Log("click");

            if(canJump) {
                Vector3 touchPos = Application.isEditor ? Input.mousePosition : (Vector3) Input.GetTouch(0).position;
                touchPos.z = Vector3.Distance(camera.transform.position, gameObject.transform.position);
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(touchPos);

                float jumpForce = Vector3.Distance(worldPos, gameObject.transform.position) * maxJumpForce;
                rb.AddForce((worldPos - gameObject.transform.position).normalized * jumpForce);

                au.Play();
                canJump = false;
                /*
                float touchX = touchPos.x,
                      touchY = touchPos.y,
                      direction = (touchX - (Screen.width/2))/(Screen.width/2),
                      angle = 45*direction*Mathf.Deg2Rad,
                      jumpForce = maxJumpForce * (touchY / Screen.height);

                Debug.Log(touchX + " " + angle + " " + Mathf.Sin(angle) + " " + Mathf.Abs(Mathf.Cos(angle)));

                rb.AddForce(new Vector3(jumpForce*Mathf.Sin(angle), jumpForce*Mathf.Abs(Mathf.Cos(angle)), 0));
                //rb.AddForce(camera.transform.up * jumpForce);
                */
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {
        canJump = true;
        isReady = true;

        gameObject.transform.SetParent(coll.gameObject.transform.parent);
    }

    void OnCollisionExit2D(Collision2D coll) {
        gameObject.transform.SetParent(null);
    }
}
