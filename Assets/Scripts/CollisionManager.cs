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
            other.rigidbody.AddForce(dir * 500, ForceMode.Impulse);
        }
    }
}
