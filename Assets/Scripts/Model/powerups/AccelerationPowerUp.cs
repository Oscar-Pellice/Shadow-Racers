using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationPowerUp : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    public void HighSpeedStartAction()
    {
        playerController.motorForce *= 2f;
        Debug.Log("start");
    }

    public void HighSpeedEndAction()
    {
        playerController.motorForce /= 2f;
        Debug.Log("end");
    }
}
