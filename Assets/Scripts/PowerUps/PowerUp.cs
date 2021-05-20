using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PowerUp: MonoBehaviour
{
    [SerializeField]
    public int id;

    [SerializeField]
    public string Name;

    [SerializeField]
    public float duration;

    [SerializeField]
    public UnityEvent startAction;

    [SerializeField]
    public UnityEvent endAction;

    [SerializeField]
    public PlayerController player;
    public virtual void Awake()
    {

    }
    public virtual void End()
    {
        if(endAction != null)
        {
            endAction.Invoke();
        }
    }
    public virtual void StartPoweUp()
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
