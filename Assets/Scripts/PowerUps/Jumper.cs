using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : PowerUp
{

    public override void Awake()
    {
        base.Awake();
        base.Name = "Jumper";
        base.duration = 3.0f;
        base.id = 1;
    }
    public override void StartPoweUp()
    {
        base.player.jump(15000, 20000);
        base.StartPoweUp();
    }

    public override void End()
    {
        base.End();
    }
}