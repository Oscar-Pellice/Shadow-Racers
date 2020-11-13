using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
    GameManager gameManager;
    private void OnTriggerEnter(Collider other)
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        gameManager.finishRound();
    }
}
