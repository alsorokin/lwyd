using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Actor
{

    private float timeElapsed;
    private GameObject _prototype;
    private Actor actor;

    public GameObject prototype
    {
        get
        {
            return _prototype;
        }
        set
        {
            _prototype = value;
            actor = _prototype.GetComponent<Actor>();
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

        if (timeElapsed >= cloneInterval && myLevel.DoIHaveSomewhereToGo(this))
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
