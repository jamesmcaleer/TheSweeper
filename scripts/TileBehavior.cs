using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileBehavior : MonoBehaviour
{
    private GameManager gameManager;
    public Material flag;

    public AudioClip clickSound;
    public AudioClip bombSound;
    public AudioClip flagSound;
    public AudioSource gameAudio;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        gameAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (gameManager.gameStarted)
        {
            if (gameObject.tag == "Bomb")
            {
                gameManager.GameOver("COMPUTER");

                //gameAudio.PlayOneShot(bombSound, 1.0f);
                //Debug.Log("lost");
            }
            else if (gameObject.tag == "Untouched" && gameManager.startedSweeper == false)
            {
                gameManager.GetRadius(gameObject.name);
                gameAudio.PlayOneShot(clickSound, 1.0f);
            }
            else if (gameObject.tag == "Untouched" && gameManager.startedSweeper == true)
            {
                string[] coords = gameObject.name.Split(' ');
                int x = Int32.Parse(coords[0]);
                int y = Int32.Parse(coords[1]);
                gameManager.CheckRadius(gameObject, x, y);
                gameAudio.PlayOneShot(clickSound, 1.0f);
            }
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && gameObject.tag != "Touched" && gameObject.tag != "Removed" && gameManager.numFlags > 0 && gameManager.gameStarted)
        {
            GetComponent<MeshRenderer>().material = flag;
            gameObject.tag = "Removed";
            gameManager.UpdateScore(1);
            gameAudio.PlayOneShot(flagSound, 1.0f);
        }
    }
}
