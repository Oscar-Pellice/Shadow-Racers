using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject phantomPlayerPrefab = null; // Prefab del phantom

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

    private Player player;
    private List<Player> phantomList = new List<Player>();

    private List<GameObject> phantomCars = new List<GameObject>();
    private GameObject playerGameObject;

    private int round = 0;
    private const int MaxRounds = 3;
    private Vector3[] startingPosition = { new Vector3(125, 1, -20), new Vector3(130, 1, -20) };

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
        if (PhotonNetwork.IsMasterClient)
        {
            playerGameObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), startingPosition[player.player_id], Quaternion.identity);
        }
        else
        {
            playerGameObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player2"), startingPosition[player.player_id], Quaternion.identity);
        }

        pathReader.AddRoundPlayer(playerGameObject);

        //Creem i setejem la camera
        Camera camera = playerGameObject.GetComponentInChildren<Camera>();
        camera.GetComponent<CameraFollow>().SetTarget(playerGameObject.transform.GetChild(0));
    }

    public void StartRound()
    {
        //Iniciar tots el jugadors
        StartCoroutine(StartPhantoms());
        player.Reset();
        pathReader.AddRoundPlayer(playerGameObject);
        if (PhotonNetwork.IsMasterClient)
        {
            playerGameObject.transform.position = startingPosition[0];
            playerGameObject.transform.rotation = Quaternion.identity;
        }
        else
        {
            playerGameObject.transform.position = startingPosition[1];
            playerGameObject.transform.rotation = Quaternion.identity;
        }

    }

    // Serveix per iniciar els phantoms al començament de la ronda
    IEnumerator StartPhantoms()
    {
        GameObject phantom;
        string prefabName = "Phantom Player2";
        if (player.player_id == 0)
        {
            prefabName = "Phantom Player";
        }

        for (int r = 0; r < round; r++)
        {
            
            phantom = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", prefabName), startingPosition[player.player_id], Quaternion.identity);
            phantomCars.Add(phantom);
            phantom.GetComponent<IA_Car>().AssignRace(pathReader.getRace(r));
            yield return new WaitForSecondsRealtime(3);
        }
    }

    public void FinishRound()
    {
        StartCoroutine(EndRound());
    }

    IEnumerator EndRound()
    {
        yield return new WaitForSecondsRealtime(2);
        foreach (GameObject obj in phantomCars) Destroy(obj);
        round++;
        if (round < MaxRounds)
        {
            StartRound();
        } else
        {
            //Acabar
        }
    }
}

