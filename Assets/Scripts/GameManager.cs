using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab = null; // Prefab del jugador
    [SerializeField] private GameObject phantomPlayerPrefab = null; // Prefab del phantom

    private PathReader pathReader;

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
    }

    private List<Player> playerList = new List<Player>();
    private int nextPlayerId = 0;
    private List<List<Player>> phantomList = new List<List<Player>>();
    private List<GameObject> cars = new List<GameObject>(); 

    private int round = 0;
    private const int MaxRounds = 3;
    private Vector3 startingPosition = new Vector3 (125,1,-20);

    // Start is called before the first frame update
    void Start()
    {
        pathReader = GetComponent<PathReader>();

        //Crear X jugadors
        //for X
        CreatePlayer();

        StartRound();

    }

    private void StartRound()
    {
        //Iniciar tots el jugadors
        StartCoroutine(StartPhantoms());
        
        //for x
        StartPlayer(0);
    }

    // Serveix per iniciar els phantoms al començament de la ronda
    IEnumerator StartPhantoms()
    {
        //for jugadors
        for (int i = 0; i < round; i++)
        {
            GameObject phantom = Instantiate(phantomPlayerPrefab, startingPosition, Quaternion.identity);
            cars.Add(phantom);
            

            phantom.GetComponent<IA_Car>().AssignRace(pathReader.getRace(0,i));
            yield return new WaitForSecondsRealtime(3);
        }
    }

    public void FinishRound()
    {
        StartCoroutine(EndRound());
    }

    IEnumerator EndRound()
    {
        yield return new WaitForSecondsRealtime(3);
        foreach (GameObject obj in cars) Destroy(obj);
        round++;
        if (round < MaxRounds)
        {
            StartRound();
        } else
        {
            //Acabar
        }
    }

    
    void CreatePlayer()
    {
        // Serveix per crear i inicialitzar el jugador
        Player p = new Player(nextPlayerId++);
        playerList.Add(p);

        pathReader.CreatePlayer(p.player_id);
    }

    private void StartPlayer(int id)
    {
        //Creem i guardem el jugador
        GameObject player = Instantiate(playerPrefab, startingPosition, Quaternion.identity);
        cars.Add(player);
        pathReader.AddRoundPlayer(id,player);
        
        //Creem i setejem la camera
        GameObject camera = GameObject.Find("Main Camera");
        camera.GetComponent<CameraFollow>().SetTarget(player.transform.GetChild(0));
    }
}

