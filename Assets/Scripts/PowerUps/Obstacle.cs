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
        var currentDirectory = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
        string prefabsPath = currentDirectory + "\\..\\..\\Assets\\Prefabs";
        myPrefab = Resources.Load(prefabsPath+"\\Blocker") as GameObject;
        
        base.Name = "Blocker";
        base.duration = 3.0f;
    }
    public override void StartPoweUp()
    {
        GameObject blocker = Instantiate(myPrefab);
        blocker.transform.position =base.player.getPosition();
        base.StartPoweUp();
        this.activated = true;
        //Instantiate(myPrefab, base.player.getPosition(), Quaternion.identity);
    }

    public override void End()
    {
        base.End();
    }
}
