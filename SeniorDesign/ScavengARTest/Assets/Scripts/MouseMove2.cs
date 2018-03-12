using UnityEngine;

public class MouseMove2 : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    Camera m_MainCamera;

    float minFov = 15f;
    float maxFov = 90f;
    float sensitivity = 10f;


    private void Start()
    {
        m_MainCamera = Camera.main;
        //This enables Main Camera
        m_MainCamera.enabled = true;
    }
    void Update()
    {
        // Moving with mouse
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }
        if (!Input.GetMouseButton(0)) return;

      
        Vector3 pos = m_MainCamera.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

        transform.Translate(move, Space.World);

        //

    }

}

