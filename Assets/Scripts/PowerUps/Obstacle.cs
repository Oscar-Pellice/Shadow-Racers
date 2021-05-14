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
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerManager"), Vector3.zero, Quaternion.identity);
        base.Awake();
        
        base.Name = "Blocker";
        base.duration = 3.0f;
    }
    public override void StartPoweUp()
    {


        var currentDirectory = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
        string prefabsPath = currentDirectory + "\\..\\..\\Assets\\Prefabs";
        Vector3 position = base.player.getPosition();
        
        var localVelocity = transform.InverseTransformDirection(base.player.getVelocity());
        Debug.Log("vel: x=" + localVelocity.x + " y=" + localVelocity.y);
        Debug.Log("pos: x=" + position.x + " y=" + position.y);
        Debug.Log("player: x=" + base.player.getPosition().x + " y=" + base.player.getPosition().y);
        position.x -= localVelocity.x/2;
        //position.y -= localVelocity.y/2;
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Blocker"), position, Quaternion.identity);
        base.StartPoweUp();
        this.activated = true;
        
    }

    public override void End()
    {
        base.End();
    }
}
