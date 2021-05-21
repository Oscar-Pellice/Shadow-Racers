using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;


public class Obstacle : PowerUp
{
    private bool activated = false;
    public GameObject myPrefab;

    public override void Awake()
    {
       
        base.Awake();
        
        base.Name = "Blocker";
        base.duration = 3.0f;
        base.id = 0;
    }
    public override void StartPoweUp()
    {


        var currentDirectory = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
        string prefabsPath = currentDirectory + "\\..\\..\\Assets\\Prefabs";
        Vector3 position = base.player.getBackPosition();
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Flan"), position, Quaternion.identity);
        base.StartPoweUp();
        this.activated = true;
        
    }

    public override void End()
    {
        base.End();
    }
    private void Update()
    {
        if (this.activated)
        {
            base.duration -= Time.deltaTime;
            if (base.duration <= 0.0f)
            {
                this.activated = false;
                base.player.RestoreMotor();
                base.duration = 3.0f;
            }
        }
    }
}
