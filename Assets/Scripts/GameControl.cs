using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameControl : MonoBehaviour {

    //the bouncing ball
    public GameObject ball;
    //the starting block
    public GameObject block;
    //reset button on Canvas that shows up when game ends
    public GameObject resetButton;
    //particle effect when pressed
    public GameObject pressParticle;
    //update the current standing block
    public GameObject currentBlock;
    //customizable blocks
    public GameObject[] BlockPrefabs;
    //set to true when ball touches the plane
    public bool gameEnd;
    //respawn direction 
    public Vector3 direction = new Vector3 (0, 0, 0);

    //pressTime = old time point - current time
    private float pressTime;
    private float startTime;
    //used to restore block shapes from squeezing
    private Vector3 originalScale = new Vector3 (0, 0, 0);
    private Vector3 newScale = new Vector3 (0, 0, 0);
    //get access to ball variables and functions
    private Ball BallScript;
    private Rigidbody ballRigidbody;
    private int indexRandom;
    private float random;
    private GameObject spawn;
    private GameObject newBlock;
   

    // Start is called before the first frame update
    void Start () {
        //making the next block to jump on
        MakeBlock ();
        //set current block to start block
        currentBlock = block;
        //hide reset
        resetButton.SetActive(false);
        pressParticle.SetActive (false);

        BallScript = ball.GetComponent<Ball>();
        originalScale = currentBlock.transform.localScale;
    }

    // Update is called once per frame
    void Update () {
        if (!gameEnd) {
            if (Input.GetMouseButtonDown (0)) {
                startTime = Time.time;
                pressParticle.SetActive (true);
            }
            if (Input.GetMouseButtonUp (0)) {
                pressParticle.SetActive (false);
                //calculate presstime
                pressTime = Time.time - startTime;
                BallScript.Jump (pressTime);
                currentBlock.transform.localScale = originalScale;
                originalScale = newScale;
            }
            if (Input.GetMouseButton (0)) {
                //squeeze the block by y-axis
                if (currentBlock.transform.localScale != Vector3.zero && currentBlock.transform.localScale.y > 0.03) {
                    currentBlock.transform.localScale += new Vector3 (0, -1, 0) * 0.15f * Time.deltaTime;
                }

            }
        }

    }

    /* This function is used to make the next block with different color, size and distance */
    public void MakeBlock () {
        if (!gameEnd) {
            //random the direction to go either left or right
            indexRandom = Random.Range (0, 2);
            if (indexRandom == 0) {
                direction = new Vector3 (1, 0, 0);
            } else {
                direction = new Vector3 (0, 0, 1);
            }

            //choose the next model randomize from array of blocks
            spawn = BlockPrefabs[Random.Range (0, BlockPrefabs.Length)];
            
            newBlock = Instantiate (spawn);
            newBlock.transform.position = currentBlock.transform.position + direction * Random.Range (0.35f, 0.5f);
            random = Random.Range (0.15f, 0.22f);
            newBlock.transform.localScale = new Vector3 (random, 0.1f, random);
            newScale = newBlock.transform.localScale;

            newBlock.GetComponent<Renderer> ().material.color = new Color (Random.Range (0f, 1), Random.Range (0f, 1), Random.Range (0f, 1));
        }
    }

    /* resetButton */
    public void Ending () {
        gameEnd = true;
        resetButton.SetActive(true);
    }

    public void Reset () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}