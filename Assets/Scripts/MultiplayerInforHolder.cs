using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerInforHolder : MonoBehaviour
{
    //private PhotonView PV;
    //public static MultiplayerInforHolder Instance; 

    //private Hashtable hashtable;

    //private void Awake()
    //{
    //    if (Instance)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    DontDestroyOnLoad(gameObject);
    //    Instance = this;
    //}

    //// Start is called before the first frame update
    //void Start()
    //{
    //    hashtable = new Hashtable();
    //    PV = GetComponent<PhotonView>();
    //}

    //public void AddToTable(string name, int node)
    //{
    //    if (hashtable.ContainsKey(name))
    //    {
    //        hashtable[name] = node;
    //    } else
    //    {
    //        hashtable.Add(name, node);
    //    }
    //    MakeQualification();
    //}

    //public void SendInfoMulti()
    //{
    //    if (PV.IsMine)
    //    {
    //        PV.RPC("RPC_SendPosition", RpcTarget.AllBuffered, hashtable[GameManager.Instance.playerGameObject.name], GameManager.Instance.playerGameObject.name);
    //    }
    //}

    //[PunRPC]
    //void RPC_SendPosition(int position, string name)
    //{
    //    AddToTable(name,position);
    //}

    //public void MakeQualification()
    //{
    //    int playerValue = (int)hashtable[GameManager.Instance.playerGameObject.name];
    //    int position = 0;

    //    foreach (int item in hashtable.Values)
    //    {
    //        if (item > playerValue)
    //        {
    //            position++;
    //        }
    //    }

    //    UIManager.Instance.ChangePosition(position);
    //}
}
