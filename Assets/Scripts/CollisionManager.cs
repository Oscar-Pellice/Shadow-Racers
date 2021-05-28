using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 dir = other.contacts[0].point - transform.position;
            other.rigidbody.AddForce(new Vector3(dir.x,0,dir.z) * 500, ForceMode.Impulse);
        }
    }
}
