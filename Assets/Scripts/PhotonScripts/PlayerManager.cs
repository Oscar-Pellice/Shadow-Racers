using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    GameManager gameManager;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameManager.CreatePlayer(0);
        }
        else
        {
            gameManager.CreatePlayer(1);
        }
        
    }
}
