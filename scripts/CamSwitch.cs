using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;

    public GameObject bg1;
    public GameObject bg2;

    public RenderTexture target;
    public GameObject rawImage;
    public float zRangeCam;

    public AudioClip enterSound;
    public AudioSource gameAudio;

    void Start()
    {
        gameAudio = GetComponent<AudioSource>();
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Q)) {
            gameAudio.PlayOneShot(enterSound, 1.0f);
            cam1.GetComponent<Camera>().targetTexture = null;
            cam1.SetActive(true);
            rawImage.SetActive(true);
            cam1.GetComponent<Camera>().targetTexture = target;
            cam2.SetActive(false);
            bg1.SetActive(true);
            bg2.SetActive(false);
            Debug.Log("MainScreen");
        }


           
        if (Input.GetKeyDown(KeyCode.E) && cam1.GetComponent<CameraMovement>().inRange == true) {
            gameAudio.PlayOneShot(enterSound, 1.0f);
            cam1.GetComponent<Camera>().targetTexture = null;
            rawImage.SetActive(false);
            cam1.SetActive(false);
            cam2.SetActive(true);
            bg1.SetActive(false);
            bg2.SetActive(true);
            Debug.Log("ComputerScreen");
        }
        
        
        
        
    }
}
