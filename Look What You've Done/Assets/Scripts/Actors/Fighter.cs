using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Fighter : Actor
{
    public float AttackStrength
    {
        get; set;
    }

    public override GameObject Clone()
    {
        throw new Exception("You've tried to clone a Fighter. This should never happen.");
    }
}
