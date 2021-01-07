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

    private float timer;
    #endregion

    #region Monobehaviour API

    private void Awake()
    {
        transform_ = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        //Importante si cambiamos el tag de player 
        //para otros modos habra que modificar esto
        if (other.gameObject.name == "Body")
        {
            ActivatePowerup();
        }
    }

    #endregion

    private void ActivatePowerup()
    {
        controller.ActivatePowerup(powerup);
        renderer_.enabled = false;
        timer = powerup.duration;
        
    }

    public void SetPowerup(PowerUp powerup)
    {
        this.powerup = powerup;
        gameObject.name = powerup.name;
    }
    private void Start()
    {
        renderer_ = GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        if (!renderer_.enabled)
        {
            timer -= 1 * Time.deltaTime;
            if(timer < 0)
            {
                timer = 0;
                renderer_.enabled = true;
            }
        }
    }
}
