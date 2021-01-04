using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathReader : MonoBehaviour
{
    //Struct de Informació guardada del path del jugador
    public class Moment
    {
        public float velocity;
        public float time;
        public Vector3 position;

        public Moment(float rpm, Vector3 playerPosition, float temps)
        {
            velocity = rpm;
            position = playerPosition;
            time = temps;
        }
    }

    public class RegisterInfo
    {
        public List<List<Moment>> carreresPlayer;
        public GameObject playerObject;

        public RegisterInfo()
        {
            carreresPlayer = new List<List<Moment>>();
            playerObject = null;
        }

        public void AddObject(GameObject obj)
        {
            carreresPlayer.Add(new List<Moment>());
            playerObject = obj;
        }
    }

    public RegisterInfo registre;

    // Array de jugadors a seguir
    private float timesList;
    private Vector3 posList;

    // Segons de interval per guardar info
    private const int DistanceToSave = 1;

    private void Awake()
    {
        registre = new RegisterInfo();
    }

    public void AddRoundPlayer(GameObject player)
    {
        registre.AddObject(player);
        timesList = Time.time;
        posList = player.transform.GetChild(0).position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Vector3.Distance(posList,registre.playerObject.transform.GetChild(0).position) >= DistanceToSave)
        {
            registre.carreresPlayer[registre.carreresPlayer.Count-1].Add(new Moment(registre.playerObject.GetComponentInChildren<Rigidbody>().velocity.magnitude,
                registre.playerObject.transform.GetChild(0).position,
                Time.time - timesList));
            posList = registre.playerObject.transform.GetChild(0).position;
        }
        
    }    

    public List<Moment> getRace(int round)
    {
        return registre.carreresPlayer[round];
    }
}
