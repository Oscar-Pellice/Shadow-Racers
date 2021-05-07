using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : PowerUp
{

    public override void Awake()
    {
        base.Awake();
        base.Name = "Gigant";
        base.duration = 3.0f;
    }
    public override void StartPoweUp()
    {
        base.player.jump(10);
        base.StartPoweUp();
    }

    public override void End()
    {
        base.End();
    }
}