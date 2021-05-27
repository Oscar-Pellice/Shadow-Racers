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
            if(GameManager.Instance.playerGameObject.name == "Player")
            {
                if(other.transform.parent.parent.name == "Phantom Player - 0" ||
                    other.transform.parent.parent.name == "Phantom Player - 1" ||
                    other.transform.parent.parent.name == "Phantom Player - 2" ||
                    other.transform.parent.parent.name == "Player" )
                {
                    return;
                }
            } else if (GameManager.Instance.playerGameObject.name == "Player2")
            {
                if (other.transform.parent.parent.name == "Phantom Player2 - 0" ||
                    other.transform.parent.parent.name == "Phantom Player2 - 1" ||
                    other.transform.parent.parent.name == "Phantom Player2 - 2" ||
                    other.transform.parent.parent.name == "Player2")
                {
                    return;
                }
            }

            LapController controller = other.GetComponentInParent<LapController>();
            if (controller.checkPointIndex == checkpointMax)
            {
                
                controller.checkPointIndex = 0;
                GameManager.Instance.ChangeCamara(true);

                if (GameManager.Instance.roundFlag == 0)
                {
                    InfoSaver.Instance.BroadcastWinner(other.transform.parent.parent.name);
                    GameManager.Instance.FinishRound();
                }
            } else
            {
                Debug.Log("Not completed");
            }
        } 
    }
}
