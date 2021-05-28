using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollider : MonoBehaviour
{
    PowerUp powerUp;
    public string powerUpType;
    private bool collided = false;
    float targetTime = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        //we set the new PowerUp
        instancePowerUp();
    }

    private void instancePowerUp()
    {
        try
        {
            if (!System.String.IsNullOrEmpty(this.powerUpType))
            {
                Debug.Log("String powerUpType is not null or empty");
                switch (this.powerUpType)
                {
                    case "Accelerator":
                        this.powerUp = gameObject.AddComponent<Accelerator>();
                        break;
                    case "Blocker":
                        this.powerUp = gameObject.AddComponent<Obstacle>();
                        break;
                    case "Jumper":
                        this.powerUp = gameObject.AddComponent<Jumper>();
                        break;
                    default:
                        this.powerUp = gameObject.AddComponent<Accelerator>();
                        break;
                }
            }
            else
            {
                Debug.Log("String powerUpType is null or empty");
                int x = UnityEngine.Random.Range(0, 4);// 0-> Default case
                switch (x)
                {
                    case 1:
                        this.powerUp = gameObject.AddComponent<Accelerator>();
                        break;
                    case 2:
                        this.powerUp = gameObject.AddComponent<Obstacle>();
                        break;
                    case 3:
                        this.powerUp = gameObject.AddComponent<Jumper>();
                        break;
                    default:
                        this.powerUp = gameObject.AddComponent<Accelerator>();
                        break;
                }
            }
            // this.powerUp.create();
        }
        catch(NullReferenceException ex)
        {
            Debug.Log("Is powerUp NULL:" + powerUp == null);
            Debug.Log(ex.Message);
        }
        
    }
    //Detect collisions between the GameObjects with Colliders attached
    void OnTriggerEnter(Collider coll)
    {
        PhotonView PV = coll.transform.parent.parent.GetComponent<PhotonView>();
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (coll.gameObject.tag.Contains("Player"))
        {
            if (PV.IsMine)
            {
                GameObject p = coll.gameObject.transform.parent.parent.gameObject;
                PlayerController playerController = coll.gameObject.transform.parent.parent.GetComponent<PlayerController>();

                Debug.Log("GameObject with player controller exists?" + (p!=null));
                Debug.Log("does it has player controller?" + (playerController != null));

                if(playerController != null && p != null)
                {
                    //adding PowerUp to GameObject
                    coll.gameObject.transform.parent.parent.GetComponent<PlayerController>().addPowerUp(this.powerUp);
                    //associating PlayerController to powerUp
                    powerUp.setPlayer(coll.gameObject.transform.parent.parent.GetComponent<PlayerController>());
                    //making dissapear collider
                    hideCollider();
                    //associating new powerup
                    instancePowerUp();
                }
            } else
            {
                
                hideCollider();
            }
        }
    }
    private void hideCollider()
    {
        collided = true;
        GameObject ChildGameObject1 = this.transform.GetChild(0).gameObject;
        ChildGameObject1.GetComponent<MeshRenderer>().enabled = false;
        GameObject ChildGameObject2 = this.transform.GetChild(1).gameObject;
        ChildGameObject2.GetComponent<MeshRenderer>().enabled = false;
        GameObject lp2 = this.transform.GetChild(4).gameObject;
        GameObject pl2_pa = lp2.transform.GetChild(0).gameObject;
        pl2_pa.GetComponent<MeshRenderer>().enabled = false;
        GameObject pl2_pb = lp2.transform.GetChild(1).gameObject;
        pl2_pb.GetComponent<MeshRenderer>().enabled = false;
        GameObject pl2_pc = lp2.transform.GetChild(2).gameObject;
        pl2_pc.GetComponent<MeshRenderer>().enabled = false;
    }
    private void showCollider()
    {
        GameObject ChildGameObject1 = this.transform.GetChild(0).gameObject;
        ChildGameObject1.GetComponent<MeshRenderer>().enabled = true;
        GameObject ChildGameObject2 = this.transform.GetChild(1).gameObject;
        ChildGameObject2.GetComponent<MeshRenderer>().enabled = true;
        GameObject lp2 = this.transform.GetChild(4).gameObject;
        GameObject pl2_pa = lp2.transform.GetChild(0).gameObject;
        pl2_pa.GetComponent<MeshRenderer>().enabled = true;
        GameObject pl2_pb = lp2.transform.GetChild(1).gameObject;
        pl2_pb.GetComponent<MeshRenderer>().enabled = true;
        GameObject pl2_pc = lp2.transform.GetChild(2).gameObject;
        pl2_pc.GetComponent<MeshRenderer>().enabled = true;
    }
    private void Update()
    {
        if (collided)
        {
            targetTime -= Time.deltaTime;
            if (targetTime <= 0.0f)
            {
                collided = false;
                showCollider();
                targetTime = 3.0f;
            }
        }
    }
}
