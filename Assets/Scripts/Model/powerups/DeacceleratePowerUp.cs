using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeacceleratePowerUp : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    public void SlowSpeedStartAction()
    {
        playerController.motorForce /= 2f;
        Debug.Log("start");
    }

    public void SlowSpeedEndAction()
    {
        playerController.motorForce *= 2f;
        Debug.Log("end");
    }
}
