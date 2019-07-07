using UnityEngine;
using System;

class CameraMovementController : MonoBehaviour
{
    public GameObject Player;
    public Level Level;

    public float XThreshold => (float)Math.Round((double)Screen.width / 500, 2);

    public float YThreshold => (float)Math.Round((double)Screen.height / 500, 2);

    private Camera Camera => gameObject.GetComponent<Camera>();

    private float PlayerX => Player.transform.position.x;

    private float PlayerY => Player.transform.position.y;

    private float CameraX
    {
        get => Camera.transform.position.x;
        set
        {
            Camera.transform.position = new Vector3(value, CameraY, CameraZ);
        }
    }

    private float CameraY
    {
        get => Camera.transform.position.y;
        set
        {
            Camera.transform.position = new Vector3(CameraX, value, CameraZ);
        }
    }

    private float CameraZ
    {
        get => Camera.transform.position.z;
    }

    private bool CanMoveCameraLeft()
    {
        if (Camera.WorldToScreenPoint(Level.LeftmostTile.GameObject.transform.position).x < 0)
        {
            return true;
        }

        return false;
    }

    private bool CanMoveCameraRight()
    {
        if (Camera.WorldToScreenPoint(Level.RightmostTile.GameObject.transform.position).x > Camera.pixelWidth)
        {
            return true;
        }

        return false;
    }

    private bool CanMoveCameraUp()
    {
        if (Camera.WorldToScreenPoint(Level.TopmostTile.GameObject.transform.position).y > Camera.pixelHeight)
        {
            return true;
        }

        return false;
    }

    private bool CanMoveCameraDown()
    {
        if (Camera.WorldToScreenPoint(Level.BottommostTile.GameObject.transform.position).y < 0)
        {
            return true;
        }

        return false;
    }

    private void FixedUpdate()
    {
        if ((PlayerX > CameraX + XThreshold) && CanMoveCameraRight())
        {
            CameraX = PlayerX - XThreshold;
        }

        if ((PlayerX < CameraX - XThreshold) && CanMoveCameraLeft())
        {
            CameraX = PlayerX + XThreshold;
        }

        if ((PlayerY > CameraY + YThreshold) && CanMoveCameraUp())
        {
            CameraY = PlayerY - YThreshold;
        }

        if ((PlayerY < CameraY - YThreshold) && CanMoveCameraDown())
        {
            CameraY = PlayerY + YThreshold;
        }
    }
}

