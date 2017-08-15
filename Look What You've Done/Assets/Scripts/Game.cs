using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void LevelChangedEventHandler(object sender, LevelChangedEventArgs e);

public class LevelChangedEventArgs : EventArgs
{
    public Level newLevel;

    public LevelChangedEventArgs(Level newLevel)
    {
        this.newLevel = newLevel;
    }
}

public class Game : MonoBehaviour {
    public GameObject grassPrefab;
    public Texture2D levelTiles;
    public Level CurrentLevel {
        get
        {
            return currentLevel;
        }
        set
        {
            currentLevel = value;
            OnChanged(new LevelChangedEventArgs(value));
        }
    }

    public event LevelChangedEventHandler LevelChanged;

    private Level currentLevel;

    protected virtual void OnChanged(LevelChangedEventArgs e)
    {
        if (LevelChanged != null)
        {
            LevelChanged(this, e);
        }
    }

    // Use this for initialization
    void Start () {
        CurrentLevel = new Level(7, 7, levelTiles);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
