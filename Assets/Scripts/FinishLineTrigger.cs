using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
    private GameManager gameManager;
    public int checkpointAmt;

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        if (other.GetComponentInParent<LapController>())
        {
            LapController controller = other.GetComponentInParent<LapController>();
            if (controller.checkPointIndex == checkpointAmt)
            {
                controller.checkPointIndex = 0;
                controller.lapNumber++;
                gameManager.FinishRound();
            } else
            {
                Debug.Log("Not completed");
            }
        } 
    }
}
