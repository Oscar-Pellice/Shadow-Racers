using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static List<GameObject> playerList = new List<GameObject>(); // LLista de tots els jugadors

    [SerializeField] private GameObject playerPrefab = null; // Prefab del jugador
    [SerializeField] private GameObject pathRegisterPrefab = null; // Prefab del path register
    [SerializeField] private GameObject phantomPlayerPrefab = null; // Prefab del phantom

    //Struct de Informació guardada del path del jugador
    public struct PathInfo
    {
        private float velocity;
        private Vector3 position;

        public PathInfo(float rpm, Vector3 playerPosition)
        {
            velocity = rpm;
            position = playerPosition;
        }

        public float getVelocity()
        {
            return velocity;
        }
        public Vector3 getPosition()
        {
            return position;
        }
    }

    public static List<List<PathInfo>> PathRegister = new List<List<PathInfo>>(); // Conte els paths de totes les carreres
    private int playerRegisterCounter = 0; // Counter per adreçar a la llista
    private List<GameObject> pathReaders = new List<GameObject>(); // Diferents readers que hi ha actius
    
    private bool spawned = false;

    // Start is called before the first frame update
    void Start()
    {
        createPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > 10 && spawned == false)
        {
            spawned = true;
            Instantiate(phantomPlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        }
    }

    void createPlayer()
    {
        //Creem i guardem el jugador
        GameObject player = Instantiate(playerPrefab, new Vector3(0,1,0), Quaternion.identity);
        playerList.Add(player);
        Debug.Log("Player created");
        //Creem i guardem el seu registrador
        GameObject pathReaderObject = Instantiate(pathRegisterPrefab);
        pathReaders.Add(pathReaderObject);
        Debug.Log("Reader created");
        PathRegister.Add(new List<PathInfo>());
        pathReaderObject.GetComponent<PathReader>().create(playerRegisterCounter, player);
        playerRegisterCounter++;

        GameObject camera = GameObject.Find("Main Camera");
        camera.GetComponent<CameraFollow>().setTarget(player.transform.GetChild(0));
        Debug.Log("Camera Assigned");
    }
}
