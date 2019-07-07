using System;
using UnityEngine;

public class Spawner : Actor
{
    public float CloneInterval = 5.0f;

    private float _timeElapsed;
    private Actor _actor;

    public Spawner(GameObject proto)
    {
        _actor = proto.GetComponent<Actor>();
    }
    
    public override GameObject Clone()
    {
        throw new Exception("Don't you dare to clone the clone masters!");
    }

    private void FixedUpdate()
    {
        if (_actor == null)
        {
            return;
        }

        if (_timeElapsed < CloneInterval)
        {
            _timeElapsed += Time.deltaTime;
        }

        if (_timeElapsed >= CloneInterval)
        {
            _timeElapsed = 0;
            _actor.Clone();
        }
    }
}
