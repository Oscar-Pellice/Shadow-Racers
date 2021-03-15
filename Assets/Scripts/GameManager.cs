using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private PathReader pathReader;
    public static GameManager Instance;

    public struct Player
    {
        public int player_id;
        public int checkPoint;
        public int lap;

        public Player(int id)
        {
            lap = 1;
            checkPoint = 0;
            player_id = id;
        }

        public void Reset()
        {
            lap = 1;
            checkPoint = 0;
        }
    }

    public float[] times;

    private Player player;

    private List<GameObject> phantomCars = new List<GameObject>();

    private GameObject playerGameObject = null;

    private int round = 0;
    public int roundFlag = 0;
    private const int MaxRounds = 3;
    private Vector3[] startingPosition = { new Vector3(5, 7, -512), new Vector3(-4, 7, -512), new Vector3(5, 7, -520), new Vector3(-4, 7, -520), new Vector3(5, 7, -528), new Vector3(-4, 7, -520) };

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        times = new float[12];
        pathReader = GetComponent<PathReader>();
    }

    public void CreatePlayer(int id)
    {
        // Serveix per crear i inicialitzar el jugador
        player = new Player(id);

        StartPlayer();
    }

    private void StartPlayer()
    {
        if (playerGameObject != null) Destroy(playerGameObject);

        if (PhotonNetwork.IsMasterClient)
        {
            playerGameObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), startingPosition[round*2 + player.player_id], Quaternion.identity);
            playerGameObject.name = "Player";
        }
        else
        {
            playerGameObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player2"), startingPosition[round*2 + player.player_id], Quaternion.identity);
            playerGameObject.name = "Player2";
        }

        pathReader.AddRoundPlayer(playerGameObject);

        //Creem i setejem la camera
        GameObject camera = playerGameObject.transform.Find("Camera").gameObject;
        camera.GetComponent<CameraFollow>().SetTarget(playerGameObject.transform.GetChild(0));
    }

    public void StartRound()
    {
        //Iniciar tots el jugadors
        StartCoroutine(StartCars());
        
    }

    // Serveix per iniciar els phantoms al començament de la ronda
    IEnumerator StartCars()
    {
        GameObject phantom;
        string prefabName = "Phantom Player2";
        if (player.player_id == 0)
        {
            prefabName = "Phantom Player";
        }

        for (int r = 0; r < round; r++)
        {
            phantom = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", prefabName), startingPosition[r*2 + player.player_id], Quaternion.identity);
            phantom.name = prefabName + " - " + r.ToString();
            phantomCars.Add(phantom);
            phantom.GetComponent<IA_Car>().AssignRace(pathReader.getRace(r));
            yield return new WaitForSecondsRealtime(1);
        }

        StartPlayer();
        UIManager.Instance.StartRoundUI();
        roundFlag = 0;
    }

    public void FinishRound()
    {
        //SaveInfo.Instance.SaveIntoJson(pathReader.getRace(0));

        StartCoroutine(EndRound());
    }

    IEnumerator EndRound()
    {
        roundFlag = 1;
        round++;
        yield return new WaitForSecondsRealtime(2);
        foreach (GameObject obj in phantomCars) Destroy(obj);
        if (round < MaxRounds)
        {
            StartRound();
        } else
        {
            //Acabar
        }
    }

    public void AddRoundTime(GameObject gameObject, float timer)
    {
        Debug.Log(gameObject.name);
        if (round == 0)
        {
            if (gameObject.name == "Player")
            {
                times[0] = timer;
                return;
            } else if (gameObject.name == "Player2")
            {
                times[1] = timer;
            }
        }
        else if (round == 1)
	    {
            if (gameObject.name == "Player")
            {
                times[2] = timer;
                return;
            }
            else if (gameObject.name == "Player2")
            {
                times[3] = timer;
            }
            else if (gameObject.name == "Phantom Player - 0")
            {
                times[4] = timer;
                return;
            }
            else if (gameObject.name == "Phantom Player2 - 0")
            {
                times[5] = timer;
            }
        }
        else if (round == 2)
        {
            if (gameObject.name == "Player")
            {
                times[6] = timer;
                return;
            }
            else if (gameObject.name == "Player2")
            {
                times[7] = timer;
            }
            else if (gameObject.name == "Phantom Player - 0")
            {
                times[8] = timer;
                return;
            }
            else if (gameObject.name == "Phantom Player2 - 0")
            {
                times[9] = timer;
            }
            else if (gameObject.name == "Phantom Player - 1")
            {
                times[10] = timer;
                return;
            }
            else if (gameObject.name == "Phantom Player2 - 1")
            {
                times[11] = timer;
            }
        }
    } 
}

