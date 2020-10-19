using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathReader : MonoBehaviour
{
    private GameObject playerObject; // Jugador el qual guardem el path
        private Transform carTransform; // Acces al seu transform
        private PlayerController carControllerScript; // Acces a variables del controlador
    private int registerId; // Id per accedir al array de registre de paths

    private float t0Time; //Temps inicial
    private float t1Time; //Temps final

    private const float TimeToSave = 0.5f; // Segons de interval per guardar info

    // Es crida quan es crea per passar els parametres a registrar
    public void create(int n, GameObject player)
    {
        registerId = n;
        playerObject = player;
        Debug.Log(registerId);
    }

    // Start is called before the first frame update
    void Start()
    {
        t0Time = Time.time;
        carTransform = playerObject.transform.GetChild(0);
        carControllerScript = playerObject.GetComponent<PlayerController>();
    }

    
    // Update is called once per frame
    void Update()
    {
        t1Time = Time.time;
        if (t1Time - t0Time >= TimeToSave)
        {
            addNode(new GameManager.PathInfo(carControllerScript.rpm,carTransform.position));
            Debug.Log("Node");
            t0Time = t1Time;
        }
    }

    // Afegeix un node al PathRegister en el GameManager
    private void addNode(GameManager.PathInfo node)
    {
        GameManager.PathRegister[registerId].Add(node);
    }
}
