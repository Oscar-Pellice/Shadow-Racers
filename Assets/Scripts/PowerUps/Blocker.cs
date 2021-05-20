using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider coll)
    {
        Debug.Log("Blocker collided");
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (coll.gameObject.tag.Equals("Player"))
        {

            GameObject p = coll.gameObject.transform.parent.parent.gameObject;
            PlayerController playerController = coll.gameObject.transform.parent.parent.GetComponent<PlayerController>();

            if (playerController != null && p != null)
            {

                //adding PowerUp to GameObject
                coll.gameObject.transform.parent.parent.GetComponent<PlayerController>().SlowDown();
                GetComponent<AudioSource>().Play();
                Destroy(this.gameObject);
            }

        }
    }
}
