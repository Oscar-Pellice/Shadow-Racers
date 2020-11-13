﻿using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public PathInfo(float rpm, Vector3 playerPosition) {
            velocity = rpm;
            position = playerPosition;
        }

        public float getVelocity() {
            return velocity;
        }
        public Vector3 getPosition() {
            return position;
        }
    }

    public List<List<PathInfo>> pathRegister = new List<List<PathInfo>>(); // Conte els paths de totes les carreres
    private int playerRegisterCounter = 0; // Counter per adreçar a la llista
    private List<GameObject> pathReaders = new List<GameObject>(); // Diferents readers que hi ha actius
    
    private int round = 0;
    public Vector3 startingPosition = new Vector3 (-6,1,4);

    List<GameObject> cars = new List<GameObject>(); 


    // Start is called before the first frame update
    void Start()
    {
        createPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void finishRound()
    {
        foreach (GameObject obj in cars) Destroy(obj);
        foreach (GameObject obj in playerList) Destroy(obj);
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
            phantom.GetComponent<IA_Car>().create(i);
            yield return new WaitForSecondsRealtime(3);
        }
        createPlayer();
    }

    // Serveix per crear i inicialitzar el jugador
    void createPlayer()
    {
        //Creem i guardem el jugador
        GameObject player = Instantiate(playerPrefab, startingPosition, Quaternion.identity);
        playerList.Add(player);

        //Creem i setejem el seu registrador
        GameObject pathReaderObject = Instantiate(pathRegisterPrefab);
        pathReaders.Add(pathReaderObject);
        pathRegister.Add(new List<PathInfo>());
        pathReaderObject.GetComponent<PathReader>().create(playerRegisterCounter, player);
        playerRegisterCounter++;

        //Creem i setejem la camera
        GameObject camera = GameObject.Find("Main Camera");
        camera.GetComponent<CameraFollow>().setTarget(player.transform.GetChild(0));
    }
}
