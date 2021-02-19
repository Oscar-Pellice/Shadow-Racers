using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollider : MonoBehaviour
{
    PowerUp powerUp;
    public string powerUpType;
    private bool collided = false;
    float targetTime = 30.0f;
    // Start is called before the first frame update
    void Start()
    {
        //we set the new PowerUp
        instancePowerUp();
    }

    private void instancePowerUp()
    {
        if (this.powerUpType != null)
        {
            switch (this.powerUpType)
            {
                case "Accelerator":
                    powerUp = new Accelerator();
                    break;
                default:
                    powerUp = new Accelerator();
                    break;
            }
        }
        else
        {
            int x = Random.Range(0, 1);// 0-> Default case
            switch (x)
            {
                case 1:
                    powerUp = new Accelerator();
                    break;
                default:
                    powerUp = new Accelerator();
                    break;
            }
        }
    }
    //Detect collisions between the GameObjects with Colliders attached
    void OnCollisionEnter(Collision collision)
    {
       
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag.Contains("Player"))
        {
            Debug.Log("PowerUp Collided");
            collided = true;
            //adding PowerUp to GameObject 
            collision.gameObject.GetComponent<PlayerController>().addPowerUp(this.powerUp);
            //associating PlayerController to powerUp
            powerUp.setPlayer(collision.gameObject.GetComponent<PlayerController>());
            //making dissapear collider
            GetComponent<MeshRenderer>().enabled = false;
            //associating new powerup
            instancePowerUp();
        }
    }
    private void Update()
    {
        if (collided)
        {
            targetTime -= Time.deltaTime;
            if (targetTime <= 0.0f)
            {
                collided = false;
                GetComponent<MeshRenderer>().enabled = true;
                targetTime = 30.0f;
            }
        }
    }
}
