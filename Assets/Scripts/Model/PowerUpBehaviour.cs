using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour
{
    #region Attributes
    #region Component References
    public PowerUpController controller;

    [SerializeField]
    private PowerUp powerup;

    private Renderer renderer_;

    private Transform transform_;

    public Material PowerupMaterial
    {
        get { return renderer_.material; }
        set { renderer_.material = value; }
    }
    #endregion
    

    #endregion

    #region Monobehaviour API

    private void Awake()
    {
        transform_ = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Importante si cambiamos el tag de player 
        //para otros modos habra que modificar esto
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Collision");
            ActivatePowerup();
        }
    }

    #endregion

    private void ActivatePowerup()
    {
        controller.ActivatePowerup(powerup);
    }

    public void SetPowerup(PowerUp powerup)
    {
        this.powerup = powerup;
        gameObject.name = powerup.name;
    }
}
