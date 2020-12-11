using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab = null; // Prefab del jugador
    [SerializeField] private GameObject pathRegisterPrefab = null; // Prefab del path register
    [SerializeField] private GameObject phantomPlayerPrefab = null; // Prefab del phantom

    //Struct de Informació guardada del path del jugador
    public struct PathInfo
    {
        private float velocity;
        private float time;
        private Vector3 position;

        public PathInfo(float rpm, Vector3 playerPosition, float temps) {
            velocity = rpm;
            position = playerPosition;
            time = temps;
        }

        public float GetVelocity() {
            return velocity;
        }
        public Vector3 GetPosition() {
            return position;
        }
        public float GetTime()
        {
            return time;
        }
    }

    public struct Player
    {
        private int player_id;
        private int checkPoint;
        private int lap;

        public Player(int l, int check, int id)
        {
            lap = l;
            checkPoint = check;
            player_id = id;
        }

        public int GetPlayerId()
        {
            return this.player_id;
        }
        public void SetPlayerId(int id)
        {
            this.player_id = id;
        }
        public int GetCheckPoint()
        {
            return this.checkPoint;
        }
        public void SetCheckPoint(int check)
        {
            this.checkPoint = check;
        }
        public int GetLap()
        {
            return this.lap;
        }
        public void SetLap(int num)
        {
            this.lap = num;
        }
    }

    public List<Player> playerList = new List<Player>();
    private int nextPlayerId = 0;
    private List<List<Player>> phantomList = new List<List<Player>>();

    public List<List<PathInfo>> pathRegister = new List<List<PathInfo>>(); // Conte els paths de totes les carreres
    private int playerRegisterCounter = 0; // Counter per adreçar a la llista
    private List<GameObject> pathReaders = new List<GameObject>(); // Diferents readers que hi ha actius
    
    private int round = 0;
    public Vector3 startingPosition = new Vector3 (-6,1,4);

    List<GameObject> cars = new List<GameObject>(); 


    // Start is called before the first frame update
    void Start()
    {
        //Crear configuració de circuit
        
        //Crear configuració de laps

        //Crear jugador
        CreatePlayer();
    }

    public void FinishRound()
    {
        StartCoroutine(EndRound());
    }

    IEnumerator EndRound()
    {
        yield return new WaitForSecondsRealtime(3);
        foreach (GameObject obj in cars) Destroy(obj);
        //foreach (Player player in playerList) Destroy(player);
        foreach (GameObject obj in pathReaders) Destroy(obj);
        round++;
        StartCoroutine(StartPhantoms());
    }

    // Serveix per iniciar els phantoms al començament de la ronda
    IEnumerator StartPhantoms()
    {
        for (int i = 0; i < round; i++)
        {
            GameObject phantom = Instantiate(phantomPlayerPrefab, startingPosition, Quaternion.identity);
            cars.Add(phantom);
            phantom.GetComponent<IA_Car>().Create(i);
            yield return new WaitForSecondsRealtime(3);
        }
        CreatePlayer();
    }

    // Serveix per crear i inicialitzar el jugador
    void CreatePlayer()
    {
        //Creem tots el jugadors


        //Creem i guardem el jugador
        GameObject player = Instantiate(playerPrefab, startingPosition, Quaternion.identity);
        player.GetComponent<PlayerController>().Create(nextPlayerId++);
        Player p = new Player(1, nextPlayerId, 0);
        playerList.Add(p);

        //Creem i setejem el seu registrador
        GameObject pathReaderObject = Instantiate(pathRegisterPrefab);
        pathReaders.Add(pathReaderObject);
        pathRegister.Add(new List<PathInfo>());
        pathReaderObject.GetComponent<PathReader>().Create(playerRegisterCounter, player);
        playerRegisterCounter++;

        //Creem i setejem la camera
        GameObject camera = GameObject.Find("Main Camera");
        camera.GetComponent<CameraFollow>().SetTarget(player.transform.GetChild(0));
    }
}
