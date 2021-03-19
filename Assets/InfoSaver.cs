using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoSaver : MonoBehaviour
{
    private PhotonView PV;
    public static InfoSaver Instance;

    public int CarSelected = 0;
    public int EnemyCarSelected = 0;

    public int mapSelected = 0;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public void SendInfoMulti()
    {
        if (PV.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PV.RPC("RPC_SendMap", RpcTarget.AllBuffered, mapSelected);
            }
            PV.RPC("RPC_SendCar", RpcTarget.AllBuffered, CarSelected);
        }
    }

    [PunRPC]
    void RPC_SendCar(int car)
    {
        EnemyCarSelected = car;
    }

    [PunRPC]
    void RPC_SendMap(int map)
    {
        mapSelected = map;
    }

    public void SaveSelection(int car, int map)
    {
        CarSelected = car;
        mapSelected = map;
        SendInfoMulti();
    }
}
