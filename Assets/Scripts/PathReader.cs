using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathReader : MonoBehaviour
{
    //Struct de Informació guardada del path del jugador
    public struct Moment
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

    public struct RegisterInfo
    {
        public List<List<Moment>> carreresPlayer;
        public int playerId;
        public GameObject playerObject;

        public RegisterInfo(int id)
        {
            carreresPlayer = new List<List<Moment>>();
            playerId = id;
            playerObject = null;
        }

        public void AddObject(GameObject obj)
        {
            carreresPlayer.Add(new List<Moment>());
            playerObject = obj;
        }
    }

    public List<RegisterInfo> registre;

    // Array de jugadors a seguir
    private List<float> timesList;
    private List<Vector3> posList;

    // Segons de interval per guardar info
    private const int DistanceToSave = 1;

    private void Awake()
    {
        registre = new List<RegisterInfo>();
        timesList = new List<float>();
        posList = new List<Vector3>();
    }

    // Es crida quan es crea per passar els parametres a registrar
    public void CreatePlayer(int id)
    {
        registre.Add(new RegisterInfo(id));
    }

    public void AddRoundPlayer(int id, GameObject player)
    {
        Debug.LogWarning(player.transform.position);
        registre[id].AddObject(player);
        timesList.Add(Time.time);
        posList.Add(player.transform.GetChild(0).position);
        Debug.LogWarning(registre[0].playerObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < registre.Count; i++)
        {
            if (Vector3.Distance(posList[i],registre[i].playerObject.transform.GetChild(0).position) >= DistanceToSave)
            {
                registre[i].carreresPlayer[registre[i].carreresPlayer.Count-1].Add(new Moment(registre[i].playerObject.GetComponentInChildren<Rigidbody>().velocity.magnitude,
                    registre[i].playerObject.transform.GetChild(0).position,
                    Time.time - timesList[i]));
                posList[i] = registre[i].playerObject.transform.GetChild(0).position;
            }
        }
    }    

    public List<Moment> getRace(int player, int round)
    {
        return registre[player].carreresPlayer[round];
    }
}
