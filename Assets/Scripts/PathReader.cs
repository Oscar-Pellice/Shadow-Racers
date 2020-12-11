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
        private Rigidbody rb;
     
    // Temps
    private float timeStart;

    // Posicions
    private Vector3 posInit;
    private Vector3 posFinal;

    // Segons de interval per guardar info
    //private const float TimeToSave = 0.5f;
    private const int DistanceToSave = 1;

    // Es crida quan es crea per passar els parametres a registrar
    public void Create(int n, GameObject player)
    {
        registerId = n;
        playerObject = player;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        carTransform = playerObject.transform.GetChild(0);
        rb = playerObject.GetComponentInChildren<Rigidbody>();
        timeStart = Time.time;
        posInit = carTransform.position;
    }

    
    // Update is called once per frame
    void Update()
    {
        posFinal = carTransform.position;
        if (Vector3.Distance(posInit,posFinal) >= DistanceToSave)
        {
            AddNode(new GameManager.PathInfo(rb.velocity.magnitude, carTransform.position, Time.time - timeStart));
            posInit = posFinal;
        }
    }

    // Afegeix un node al PathRegister en el GameManager
    private void AddNode(GameManager.PathInfo node)
    {
        gameManager.pathRegister[registerId].Add(node);
    }

    
}
