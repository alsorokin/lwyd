using UnityEngine;

class CameraMovementController : MonoBehaviour
{
    public GameObject player;
    public float xThreshold = 4;
    public float yThreshold = 2;
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

    private void FixedUpdate()
    {
        if (PlayerX > CameraX + xThreshold)
        {
            CameraX = PlayerX - xThreshold;
        }

        if (PlayerX < CameraX - xThreshold)
        {
            CameraX = PlayerX + xThreshold;
        }

        if (PlayerY > CameraY + yThreshold)
        {
            CameraY = PlayerY - yThreshold;
        }

        if (PlayerY < CameraY - yThreshold)
        {
            CameraY = PlayerY + yThreshold;
        }
    }
}

