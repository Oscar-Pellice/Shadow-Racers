using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathReader : MonoBehaviour
{
    // Game Manager
    private GameManager gameManager;
    private int registerId;
    
    // Jugador a seguir
    private GameObject playerObject; 
        private Transform carTransform;
        private PlayerController carControllerScript; 
     
    // Temps
    private float t0Time; 
    private float t1Time;

    private Vector3 posInit;
    private Vector3 posFinal;

    // Segons de interval per guardar info
    private const float TimeToSave = 0.5f;
    private const int DistanceToSave = 1;

    // Es crida quan es crea per passar els parametres a registrar
    public void create(int n, GameObject player)
    {
        registerId = n;
        playerObject = player;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        carTransform = playerObject.transform.GetChild(0);
        carControllerScript = playerObject.GetComponent<PlayerController>();
        //t0Time = Time.time;
        posInit = carTransform.position;
    }

    
    // Update is called once per frame
    void Update()
    {
        t1Time = Time.time;
        if ((t1Time - t0Time) * carControllerScript.rpm >= DistanceToSave)
        {
            addNode(new GameManager.PathInfo(carControllerScript.rpm,carTransform.position));
            t0Time = t1Time;
        }

        posFinal = carTransform.position;
        if (Vector3.Distance(posInit,posFinal) >= DistanceToSave)
        {
            addNode(new GameManager.PathInfo(carControllerScript.rpm, carTransform.position));
            posInit = posFinal;
        }

    }

    // Afegeix un node al PathRegister en el GameManager
    private void addNode(GameManager.PathInfo node)
    {
        gameManager.pathRegister[registerId].Add(node);
    }

    // Serveix per destuir el objecte
    private void OnDestroy()
    {
        addNode(new GameManager.PathInfo(0, gameManager.startingPosition)); //Provisional per circuits circular
    }
}
