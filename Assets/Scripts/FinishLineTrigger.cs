using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
    private GameManager gameManager;
    public int checkpointMax;

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        if (other.GetComponentInParent<LapController>())
        {
            LapController controller = other.GetComponentInParent<LapController>();
            if (controller.checkPointIndex == checkpointMax)
            {
                controller.checkPointIndex = 0;
                gameManager.AddRoundTime(other.transform.parent.parent.gameObject, UIManager.Instance.timer);
                if (gameManager.roundFlag == 0)
                {
                    gameManager.FinishRound();
                }
            } else
            {
                Debug.Log("Not completed");
            }
        } 
    }
}
