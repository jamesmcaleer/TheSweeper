using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 55f;
    float forwardInput;
    public float zRangeForward;
    public float zRangeBackward;
    public bool inRange = false;


    public AudioClip[] stepSounds;
    public AudioClip turnSound;
    public AudioSource gameAudio;
    // Start is called before the first frame update
    void Start()
    {
        gameAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //forwardInput = Input.GetAxis("Vertical");
        // Vector3 movedirection = new Vector3(turnAround, 0.0f, 0);
        // transform.Rotate(Vector3.back, turnSpeed * Time.deltaTime * turnAround);
        // transform.Translate(Vector3.forward * turnAround * speed * Time.deltaTime);
        if(Input.GetKeyDown(KeyCode.Space)) {

            transform.Rotate(Vector3.up, 180 );
            gameAudio.PlayOneShot(turnSound, 1.0f);

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position += transform.forward * 2;
            int index = Random.Range(0, 4);
            gameAudio.PlayOneShot(stepSounds[index], 1.0f);
        }
        

        if(transform.position.z > zRangeForward) {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRangeForward);
        }   
        if(transform.position.z < zRangeBackward) {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRangeBackward);
        }
    }

    private void OnTriggerStay(Collider other) {
        inRange = true;
        Debug.Log("Entered Zone");
    }

    private void OnTriggerExit(Collider other) {
        inRange = false;
    }

   
    
  
}
