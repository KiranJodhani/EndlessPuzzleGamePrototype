using UnityEngine;

public class Side : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    //side follow the camera on y axis
    void Update()
    {
        transform.position = new Vector2(transform.position.x, cam.transform.position.y);
    }
}
