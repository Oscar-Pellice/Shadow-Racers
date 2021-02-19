using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PowerUp: MonoBehaviour
{
    [SerializeField]
    public string Name { get { return Name; } set { Name = value; } }

    [SerializeField]
    public float duration { get { return duration; } set { duration = value; } }

    [SerializeField]
    public UnityEvent startAction;

    [SerializeField]
    public UnityEvent endAction;

    [SerializeField]
    public PlayerController player;
    public virtual void End()
    {
        if(endAction != null)
        {
            endAction.Invoke();
        }
    }
    public virtual void Start()
    {
        if(startAction != null)
        {
            startAction.Invoke();
        }
    }
    public void setPlayer(PlayerController player)
    {
        this.player = player;
    }

}
