using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathReader : MonoBehaviour
{
    private int round = 0;

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

    public class PowerReg
    {
        public float time;
        public int id;

        public PowerReg(float time, int id)
        {
            this.time = time;
            this.id = id;
        }
    }

    public class RegisterInfo
    {
        public List<List<PowerReg>> powerRegister;
        public List<List<Moment>> carreresPlayer;
        public GameObject playerObject;

        public RegisterInfo()
        {
            powerRegister = new List<List<PowerReg>>();
            carreresPlayer = new List<List<Moment>>();
            playerObject = null;
        }

        public void AddObject(GameObject obj)
        {
            carreresPlayer.Add(new List<Moment>());
            powerRegister.Add(new List<PowerReg>());
            playerObject = obj;
        }
    }

    public RegisterInfo registre;

    // Array de jugadors a seguir
    private float time;
    private Vector3 position;

    // Segons de interval per guardar info
    private const int DistanceToSave = 1;

    private bool able = false;

    private void Awake()
    {
        registre = new RegisterInfo();
    }

    public void AddRoundPlayer(GameObject player)
    {
        registre.AddObject(player);
        time = Time.time;
        position = player.transform.GetChild(0).position;
        round++;
    }

    // Update is called once per frame
    void Update()
    {
        if (registre.playerObject == null) return;
        if (!able) return;

        if (Vector3.Distance(position,registre.playerObject.transform.GetChild(0).position) >= DistanceToSave)
        {
            registre.carreresPlayer[registre.carreresPlayer.Count-1].Add(new Moment(registre.playerObject.GetComponentInChildren<Rigidbody>().velocity.magnitude,
                registre.playerObject.transform.GetChild(0).position,
                Time.time - time));
            position = registre.playerObject.transform.GetChild(0).position;
        }
        
    }    

    public List<Moment> getRace(int round)
    {
        return registre.carreresPlayer[round];
    }

    public List<PowerReg> getPowerups(int round)
    {
        return registre.powerRegister[round];
    }

    public void ActivateReader(bool value)
    {
        able = value;
    }

    public void addPowerup(int id)
    {
        this.registre.powerRegister[round-1].Add(new PowerReg(Time.time - time, id));
    }
}
