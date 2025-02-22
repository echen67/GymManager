using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
        offset = transform.position;
    }
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
