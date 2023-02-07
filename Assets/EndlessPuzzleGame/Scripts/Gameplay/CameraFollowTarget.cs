using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [Header("Object to follow")]
    public GameObject TargetYFollow;
    //public GameObject TargetXFollow;

    [Header("Follow speed")]
    [Range(0.0f, 50.0f)]
    public float speed = 6f;

    [Header("Camera Offset")]
    [Range(-10.0f, 10.0f)]
    public float yOffset;
    //[Range(-10.0f, 10.0f)]
    //public float xOffset;

    [Space(15)]
    public UIManager uIManager;

    float interpolation;
    Vector3 position;

    void Start()
    {
    }

    //camera follow the player
    void LateUpdate()
    {
        if (uIManager.gameState == GameState.PLAYING)
        {
            interpolation = speed * Time.deltaTime;

            position = transform.position;

            if (TargetYFollow.transform.position.y + yOffset > transform.position.y)
                position.y = Mathf.Lerp(transform.position.y, TargetYFollow.transform.position.y + yOffset, interpolation);
            //position.x = Mathf.Lerp(transform.position.x, TargetXFollow.transform.position.x + xOffset, interpolation);

            transform.position = position;
        }
    }

    //reset camera position
    public void ResetCameraPosition()
    {
        transform.position = new Vector3(0, 0, -10);
    }
}