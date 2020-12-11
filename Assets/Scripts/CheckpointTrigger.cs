using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] public int index;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<LapController>())
        {
            LapController controller = other.GetComponentInParent<LapController>();
            if (controller.checkPointIndex == index + 1 || controller.checkPointIndex == index - 1)
            {
                controller.checkPointIndex = index;
                Debug.Log(index);
            }
            else
            {
                Debug.Log("NO");
            }
        } 
    }
}
