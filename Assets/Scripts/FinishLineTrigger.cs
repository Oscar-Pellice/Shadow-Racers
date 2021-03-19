using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
    public int checkpointMax;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<LapController>())
        {
            LapController controller = other.GetComponentInParent<LapController>();
            if (controller.checkPointIndex == checkpointMax)
            {
                GameManager.Instance.winnerString = other.transform.parent.parent.name;
                controller.checkPointIndex = 0;
                Destroy(other.transform.parent.parent.gameObject);
                GameManager.Instance.ChangeCamara(true);
                

                if (GameManager.Instance.roundFlag == 0)
                {
                    GameManager.Instance.FinishRound();
                }
            } else
            {
                Debug.Log("Not completed");
            }
        } 
    }
}
