using System.Collections;
using System.Collections.Generic;
using HnSF.Sample.TDAction;
using UnityEngine;

public class SimulationBehaviour : MonoBehaviour, ISimulationObject
{
    public GameManager Manager
    {
        get { return manager; }
    }
    private GameManager manager;

    public void SimInitialize(GameManager gameManager)
    {
        manager = gameManager;
    }

    public virtual void SimAwake()
    {
        
    }

    public virtual void SimUpdate()
    {
        
    }

    public virtual void SimLateUpdate()
    {
        
    }
}
