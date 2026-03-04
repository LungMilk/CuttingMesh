using UnityEngine;

public class RipCord : MonoBehaviour
{
    public Transform player;
    public GameObject handle;
    public float modifier = 1f;
    private void Update()
    {
        FollowMouse();
    }

    void FollowMouse()
    {
        //not getting the players rotation, as well as not oriented in their view
        Vector3 pos = Input.mousePosition + (Vector3.forward * modifier);
        handle.transform.position = Camera.main.ScreenToViewportPoint(pos);
    }
}
