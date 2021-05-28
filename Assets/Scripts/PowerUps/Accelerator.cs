using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Accelerator : PowerUp
{
    private bool activated = false;

    public override void Awake()
    {
        base.Awake();
        base.Name = "Accelarator";
        base.duration = 7.0f;
        base.id = 2;
    }
    public override void StartPoweUp()
    {
        base.StartPoweUp();
        this.activated = true;
        
    }

    public override void End()
    {
        base.End();
    }
    private void Update()
    {
        if (activated)
        {
            base.player.AddBoost(25000);
            activated = false;
            //base.player.HandleMotor(4f);
            //base.duration -= Time.deltaTime;
            //if (base.duration <= 0.0f)
            //{
            //    activated = false;
            //    End();
            //}

        }
    }
    
}
