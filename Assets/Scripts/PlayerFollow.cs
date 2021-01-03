using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// como da problemas el camera follow he echo esto para tirar en test
// a modo de apaño
public class PlayerFollow : MonoBehaviour
{
    public Transform PlayerTransform;

    private Vector3 cameraOffset;

    [Range(0.01f,1.0f)]
    public float smoothFactor = 0.5f;

    public bool lookAtPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - PlayerTransform.position;
    }

    // it is called after Update method
    void LateUpdate()
    {
        Vector3 newPos = PlayerTransform.position + cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
        if (lookAtPlayer)
        {
            transform.LookAt(PlayerTransform);
        }
    }
}
