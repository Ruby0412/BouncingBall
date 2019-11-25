using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    //the main camera
    public GameObject camera;
    //main game controller(mainly about the map)
    public GameObject GameController;
    //score(based on block top surface touched)
    public GameObject textScore;
    //adjust the force index
    public float adjustIndex = 1000f;
    //int score
    public int score = 0;

    //get the script under game controll
    private GameControl GameScript;
    private Rigidbody ballRigidbody;
    private Vector3 cam = new Vector3 (0, 0, 0);

    // Start is called before the first frame update
    void Start () {
        GameScript = GameController.GetComponent<GameControl> ();
        ballRigidbody = GetComponent<Rigidbody> ();
        //get the relative position norm
        cam = camera.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update () {

    }

    public void Jump (float pressTime) {
        //add force for jumping based on direction and presstime
        ballRigidbody.AddForce (new Vector3 (0, 3f, 0) + GameScript.direction * pressTime * adjustIndex, ForceMode.Impulse);
    }

    void OnCollisionEnter (Collision collision) {
        //Debug.Log (collision.gameObject);
        if (collision.gameObject.name == "Plane") {
            GameScript.Ending ();
        } else if (collision.gameObject != GameScript.currentBlock && collision.gameObject.tag == "box" && GameScript.gameEnd == false) {
            score++;
            textScore.GetComponent<UnityEngine.UI.Text> ().text = score + "";
            StartCoroutine (FollowBall (transform.position + cam, 2.5f));
            GameScript.currentBlock = collision.gameObject;
            GameScript.MakeBlock ();
        }

    }

    //move the camera to the position of next block the ball standing
    IEnumerator FollowBall (Vector3 end, float speed) {
        while (Vector3.Distance (camera.transform.position, end) > speed * Time.deltaTime) {
            camera.transform.position = Vector3.MoveTowards (camera.transform.position, end, speed * Time.deltaTime);
            yield return 0;
        }
        camera.transform.position = end;
    }

}