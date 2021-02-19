using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerator : PowerUp
{
    private bool activated = false;

    public Accelerator()
    {
        base.Name = "Accelarator";
        base.duration = 70.0f;
    }
    public override void Start()
    {
        base.Start();
        this.activated = true;
        Debug.Log("Accelerating");
    }

    public override void End()
    {
        base.End();
        
        Debug.Log("Slowing Down");
    }
    private void Update()
    {
        if (activated)
        {
            base.duration -= Time.deltaTime;
            if (base.duration <= 0.0f)
            {
                activated = false;
                End();
            }
        }
    }
    
}
