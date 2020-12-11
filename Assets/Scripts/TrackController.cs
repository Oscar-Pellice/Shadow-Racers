using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{
    public int lapNumber;
    public int checkPointIndex;

    // Start is called before the first frame update
    void Start()
    {
        lapNumber = 1;
        checkPointIndex = 0;
    }
}
