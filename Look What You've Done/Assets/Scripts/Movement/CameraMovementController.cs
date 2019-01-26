using UnityEngine;

class CameraMovementController : MonoBehaviour
{
    public GameObject player;
    public Level level;

    public float XThreshold
    {
        get
        {
            return Screen.width / 500;
        }
    }

    public float YThreshold
    {
        get
        {
            return Screen.height / 500;
        }
    }

    private Camera Camera => gameObject.GetComponent<Camera>();

    private float PlayerX {
        get
        {
            return player.transform.position.x;
        }
    }

    private float PlayerY
    {
        get
        {
            return player.transform.position.y;
        }
    }

    private float CameraX
    {
        get
        {
            return Camera.transform.position.x;
        }
        set
        {
            Camera.transform.position = new Vector3(value, CameraY, CameraZ);
        }
    }

    private float CameraY
    {
        get
        {
            return Camera.transform.position.y;
        }
        set
        {
            Camera.transform.position = new Vector3(CameraX, value, CameraZ);
        }
    }

    private float CameraZ
    {
        get
        {
            return Camera.transform.position.z;
        }
    }

    private bool CanMoveCameraLeft()
    {
        if (this.Camera.WorldToScreenPoint(level.GetLeftmostTile().gameObject.transform.position).x < 0)
        {
            return true;
        }

        return false;
    }

    private bool CanMoveCameraRight()
    {
        if (this.Camera.WorldToScreenPoint(level.GetRightmostTile().gameObject.transform.position).x > this.Camera.pixelWidth)
        {
            return true;
        }

        return false;
    }

    private bool CanMoveCameraUp()
    {
        if (this.Camera.WorldToScreenPoint(level.GetTopmostTile().gameObject.transform.position).y > this.Camera.pixelHeight)
        {
            return true;
        }

        return false;
    }

    private bool CanMoveCameraDown()
    {
        if (this.Camera.WorldToScreenPoint(level.GetBottommostTile().gameObject.transform.position).y < 0)
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

