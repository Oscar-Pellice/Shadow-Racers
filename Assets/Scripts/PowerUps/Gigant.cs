using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gigant : PowerUp
{
    private bool activated = false;
    public GameObject myPrefab;

    public override void Awake()
    {
        base.Awake();
        base.Name = "Gigant";
        base.duration = 3.0f;
    }
    public override void StartPoweUp()
    {

        base.player.changeScale(new Vector3(1.4f, 1.4f, 1.4f));
        base.player.jump(10);
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

            base.duration -= Time.deltaTime;
            if (base.duration <= 0.0f)
            {
                base.player.changeScale(new Vector3(1f, 1f, 1f));
                activated = false;
                End();
            }
        }
    }
}
