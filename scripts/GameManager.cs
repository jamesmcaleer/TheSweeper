using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int boardMaxVal = 5;
    private int boardMinVal = -5;
    public int numBombs = 10;
    public int numFlags = 10;
    public GameObject bomb;
    public GameObject tile;
    public GameObject camera;
    public GameObject computerCamera;
    public GameObject jumpscare;
    public GameObject doorFrame;


    public TextMeshProUGUI amountFlagsText;
    public TextMeshProUGUI winScreen;
    public TextMeshProUGUI loseScreen;
    public TextMeshProUGUI startScreen;
    public Button spaceText;
    public Button wText;
    public Button eText;
    public Button qText;
    public Button rulesText;
    public Button doorText;
    public Button fText;

    private DoorScript doorScript;

    public bool gameOver = false;
    public bool startedSweeper = false;
    public bool gameStarted = false;
    public bool tutorialStarted = false;
    public bool opening = false;

    private CameraMovement cameraMovement;

    public Material blank;
    public Material one;
    public Material two;
    public Material three;
    public Material four;
    public Material five;
    public Material bombMaterial;
    public Material flagMaterial;


    public AudioClip bombSound;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip creepySound;
    public AudioClip deadSound;
    public AudioClip doorSound;
    public AudioSource gameAudio;

    // Start is called before the first frame update
    void Start()
    {
        BuildBoard();
        UpdateScore(0);
        gameAudio = GetComponent<AudioSource>();
        cameraMovement = GameObject.Find("MineSweeperCamera").GetComponent<CameraMovement>();
        doorScript = GameObject.Find("MineSweeperCamera").GetComponent<DoorScript>();

    }

    void StartTutorial()
    {
        tutorialStarted = true;

        startScreen.gameObject.SetActive(false);

        spaceText.gameObject.SetActive(true);
    }

    void StartGame() {
        gameStarted = true;
    }

    public void UpdateScore(int num)
    {
        numFlags -= num;
        amountFlagsText.text = numFlags.ToString();
    }

    public IEnumerator OpenDoor()
    {
        opening = true;
        Animator anim = doorFrame.GetComponent<Animator>();
        anim.SetBool("Opened", true);

        gameAudio.PlayOneShot(doorSound, 1.0f);
        yield return new WaitForSeconds(20.0f);
        if (anim.GetBool("Opened"))
        {
            GameOver("DOOR");
        }
        opening = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!opening)
        {
            StartCoroutine(OpenDoor());
        }
        if (!gameOver)
        {
            CheckWin();
        }

        if (Input.GetKeyDown(KeyCode.Q) && !gameStarted && !tutorialStarted)
        {
            startScreen.gameObject.SetActive(false);
            StartGame();
            //StartTutorial();
        }

        if (Input.GetKeyDown(KeyCode.Space) && spaceText.gameObject.active)
        {
            spaceText.gameObject.SetActive(false);
            wText.gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.W) && wText.gameObject.active && cameraMovement.inRange)
        {
            wText.gameObject.SetActive(false);
            eText.gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.E) && eText.gameObject.active && cameraMovement.inRange)
        {
            eText.gameObject.SetActive(false);
            qText.gameObject.SetActive(true);
            rulesText.gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Q) && qText.gameObject.active)
        {
            qText.gameObject.SetActive(false);
            rulesText.gameObject.SetActive(false);
            doorText.gameObject.SetActive(true);
        }
        if (doorText.gameObject.active && camera.transform.position.z < -40.5f)
        {
            fText.gameObject.SetActive(true);
            doorText.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.F) && fText.gameObject.active && camera.transform.position.z < -40.5f)
        {
            fText.gameObject.SetActive(false);
            StartGame();
        }
    }

    void BuildBoard()
    {
        List<int[]> allBombCoords = GenerateBombs();

        // x val
        for (int i = boardMinVal + 1; i < boardMaxVal + 1; i++)
        {
            // z val
            for (int j = boardMinVal; j < boardMaxVal; j++)
            {
                //Debug.Log(i.ToString() + ", " + j.ToString());

                GameObject tilePlane = Instantiate(tile, new Vector3((((float)i - 0.5f) * 0.15f) + 0.894f, (((float)j + 0.5f) * 0.15f) + 2.565f, -27.62f), tile.transform.rotation);
                tilePlane.name = i.ToString() + " " + j.ToString();

                for (int n = 0; n < numBombs; n++)
                {
                    if(allBombCoords[n][0] == i && allBombCoords[n][1] == j)
                    {
                        Destroy(tilePlane);
                        //Debug.Log(i.ToString() + ", " + j.ToString());
                        GameObject bombPlane = Instantiate(bomb, new Vector3((((float)i - 0.5f) * 0.15f) + 0.894f, (((float)j + 0.5f) * 0.15f) + 2.565f, -27.62f), bomb.transform.rotation);
                        bombPlane.name = i.ToString() + " " + j.ToString();
                    }
                }
                
            }
        }
    }
    
    void CheckWin()
    {

        if (numFlags < 1)
        {
            GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");

            foreach (GameObject bombObject in bombs)
            {
                if (!(bombObject.gameObject.GetComponent<MeshRenderer>().material.ToString() == "flagMaterial (Instance) (UnityEngine.Material)"))
                {
                    
                    Debug.Log(bombObject.gameObject.GetComponent<MeshRenderer>().material.ToString());
                    GameOver("COMPUTER");
                  
                    
                    break;
                }
            }

            if (!gameOver)
            {
                GameOver("WIN");
            }
        }
    }

    public void GetRadius(string source)
    {
        startedSweeper = true;
        string[] coords = source.Split(' ');
        int sourceX = Int32.Parse(coords[0]);
        int sourceZ = Int32.Parse(coords[1]);
        // find toprightmost from source 3 squares out
        // 4    3
        int topBoundX = sourceX;
        int topBoundZ = sourceZ;

        for (int i = 0; i < 3; i++)
        {
            if (!(topBoundX + 1 > 5))
            {
                topBoundX += 1;
            }
            if (!(topBoundZ + 1 > 4))
            {
                topBoundZ += 1;
            }
        }

        int bottomBoundX = sourceX;
        int bottomBoundZ = sourceZ;

        for (int i = 0; i < 3; i++)
        {
            if (!(bottomBoundX - 1 < -4))
            {
                bottomBoundX -= 1;
            }
            if (!(bottomBoundZ - 1 < -5))
            {
                bottomBoundZ -= 1;
            }
        }


        for (int i = bottomBoundX; i < topBoundX + 1; i++)
        {
            for (int j = bottomBoundZ; j < topBoundZ + 1; j++)
            {
                GameObject selectedTile = GameObject.Find(i.ToString() + " " + j.ToString());
                if (!(selectedTile.tag == "Bomb"))
                {
                    CheckRadius(selectedTile, i, j);
                }
                
            }
        }

    }

    public void CheckRadius(GameObject source, int sourceX, int sourceZ)
    {
        int topBoundX = sourceX;
        int topBoundZ = sourceZ;

        
        if (!(topBoundX + 1 > 5))
        {
            topBoundX += 1;
        }
        if (!(topBoundZ + 1 > 4))
        {
            topBoundZ += 1;
        }
        
        int bottomBoundX = sourceX;
        int bottomBoundZ = sourceZ;

        
        if (!(bottomBoundX - 1 < -4))
        {
            bottomBoundX -= 1;
        }
        if (!(bottomBoundZ - 1 < -5))
        {
            bottomBoundZ -= 1;
        }

        int amountTouching = 0;
        for (int i = bottomBoundX; i < topBoundX + 1; i++)
        {
            for (int j = bottomBoundZ; j < topBoundZ + 1; j++)
            {
                GameObject selectedTile = GameObject.Find(i.ToString() + " " + j.ToString());
                if (selectedTile.tag == "Bomb")
                {
                    amountTouching += 1;
                }
                
            }
        }

        Debug.Log(amountTouching);
        
        if (amountTouching == 0)
        {
            source.transform.Rotate(Vector3.back * 180);
            source.tag = "Removed";
            //Destroy(source);
        }
        else if (amountTouching == 1)
        {
            source.GetComponent<MeshRenderer>().material = one;
            
            source.tag = "Touched";
        }
        else if (amountTouching == 2)
        {
            source.GetComponent<MeshRenderer>().material = two;

            source.tag = "Touched";
        }
        else if (amountTouching == 3)
        {
            source.GetComponent<MeshRenderer>().material = three;
            source.tag = "Touched";
        }
        else if (amountTouching == 4)
        {
            source.GetComponent<MeshRenderer>().material = four;
            source.tag = "Touched";
        }
        else if (amountTouching == 5)
        {
            source.GetComponent<MeshRenderer>().material = five;
            source.tag = "Touched";
        }

    }

    List<int[]> GenerateBombs()
    {
        List<int[]> allBombCoords = new List<int[]>();
        for (int i = 0; i < numBombs; i++)
        {
            
            int x = UnityEngine.Random.Range(-4, 5);
            int z = UnityEngine.Random.Range(-5, 4);
            for (int j = 0; j < allBombCoords.Count; j++)
            {
                if (allBombCoords[j][0] == x && allBombCoords[j][1] == z)
                {
                    x = UnityEngine.Random.Range(-4, 5);
                    z = UnityEngine.Random.Range(-5, 4);
                }
            }
            
            int[] bombCoords = { x, z };
            
            allBombCoords.Add(bombCoords);
        }
        Debug.Log(allBombCoords.Count);
        return allBombCoords;
    }

    void ShowBombs()
    {
        GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");
        
        foreach(GameObject bombObject in bombs)
        {
            bombObject.GetComponent<MeshRenderer>().material = bombMaterial;
            Debug.Log("himmy");
        }

    }

    public void GameOver(string ending)
    {
        gameOver = true;
        if (ending == "WIN")
        {
            ShowBombs();
            winScreen.gameObject.SetActive(true);

            gameAudio.PlayOneShot(winSound, 1.0f);
        }
        else if (ending == "COMPUTER")
        {
            ShowBombs();
            loseScreen.gameObject.SetActive(true);
            gameAudio.PlayOneShot(bombSound, 1.0f);
            gameAudio.PlayOneShot(loseSound, 1.0f);

        }
        StartCoroutine(EndScene(ending));
    }

    IEnumerator EndScene(string ending)
    {
        jumpscare.GetComponent<Camera>();
        camera.GetComponent<Camera>();
        computerCamera.GetComponent<Camera>();

        if (ending == "WIN")
        {
            yield return new WaitForSeconds(2.0f);
            computerCamera.SetActive(false);
            camera.SetActive(true);
            camera.transform.eulerAngles = new Vector3(180, 0, 0);
            gameAudio.PlayOneShot(deadSound, 1.0f);
            
            yield return new WaitForSeconds(4.0f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (ending == "COMPUTER")
        {
            yield return new WaitForSeconds(6.0f);
            camera.SetActive(false);
            computerCamera.SetActive(false);
            jumpscare.SetActive(true);
            yield return new WaitForSeconds(4.0f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (ending == "DOOR")
        {
            yield return new WaitForSeconds(1.0f);
            computerCamera.SetActive(false);
            camera.SetActive(true);
            camera.transform.eulerAngles = new Vector3(0, 180, 0);
            gameAudio.PlayOneShot(creepySound, 1.0f);
            yield return new WaitForSeconds(18.0f);
            camera.SetActive(false);
            jumpscare.SetActive(true);
            yield return new WaitForSeconds(4.0f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}




