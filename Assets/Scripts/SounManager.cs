using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SounManager : MonoBehaviour
{

    float velocity;

    // Start is called before the first frame update
    void Start()
    {
        velocity = GameManager.Instance.playerGameObject.GetComponentInChildren<Rigidbody>().velocity.magnitude;

    }

    // Update is called once per frame
    void Update()
    {
        velocity = GameManager.Instance.playerGameObject.GetComponentInChildren<Rigidbody>().velocity.magnitude;

    }
}
