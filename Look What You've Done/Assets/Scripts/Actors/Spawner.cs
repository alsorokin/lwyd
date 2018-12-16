using System;
using UnityEngine;

public class Spawner : Actor
{

    private float timeElapsed;
    private GameObject prototype;
    private Actor actor;

    public GameObject Prototype
    {
        get
        {
            return prototype;
        }
        set
        {
            prototype = value;
            actor = prototype.GetComponent<Actor>();
        }
    }
    public float cloneInterval = 5.0f;

    private void FixedUpdate()
    {
        if (actor == null)
        {
            return;
        }

        if (timeElapsed < cloneInterval)
        {
            timeElapsed += Time.deltaTime;
        }

        if (timeElapsed >= cloneInterval && myLevel.DoIHaveSomewhereToGo(this.Prototype.GetComponent<Actor>()))
        {
            timeElapsed = 0;
            actor.Clone();
        }
    }

    public override GameObject Clone()
    {
        throw new Exception("Don't you dare to clone the clone masters!");
    }
}
