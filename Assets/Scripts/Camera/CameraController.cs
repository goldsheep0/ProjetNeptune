using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;
    public float smoothStep = 0.2f;
    public float velocityScale = 0.5f;

    void Update()
    {
        Vector2 playerVelocity = Player.GetComponent<Rigidbody2D>().linearVelocity;
        Vector3 velocityModifier = new Vector3(playerVelocity.x, playerVelocity.y) * velocityScale;
        Vector3 target = Player.transform.position + velocityModifier;
        target.z = -10;
        transform.position = Vector3.Lerp(transform.position, target, smoothStep);
    }
}
