using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInfo : MonoBehaviour
{
    public static SaveInfo Instance;
    public Guardat infoGuardada = new Guardat();

    public class Guardat
    {
        public List<float> temps;
        public List<float> vel;
        public List<Vector3> pos;
    }
    
    private void Awake()
    {
        Instance = this;
        infoGuardada.vel = new List<float>();
        infoGuardada.temps = new List<float>();
        infoGuardada.pos = new List<Vector3>();
    }

    public void SaveIntoJson(List<PathReader.Moment> list)
    {
        foreach (PathReader.Moment item in list)
        {
            infoGuardada.temps.Add(item.time);
            infoGuardada.vel.Add(item.velocity);
            infoGuardada.pos.Add(item.position);
        }

        var race = JsonUtility.ToJson(infoGuardada);
        System.IO.File.WriteAllText(Application.dataPath + "/StreamingAssets" + "RaceData.json", race);
    }

    public List<PathReader.Moment> ReturnJson()
    {
        List<PathReader.Moment> returnable = new List<PathReader.Moment>(); 

        var inputString = System.IO.File.ReadAllText(Application.dataPath + "/StreamingAssets" + "/RaceData.json");
        Guardat info = JsonUtility.FromJson<Guardat>(inputString);


        for (int i = 0; i < info.vel.Count; i++)
        {
            PathReader.Moment moment = new PathReader.Moment(info.vel[i], info.pos[i], info.temps[i]);
            returnable.Add(moment);
        }

        return returnable;
    }
}
