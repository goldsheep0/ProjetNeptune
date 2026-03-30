using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float smoothStep = 0.2f;
    public float velocityScale = 0.5f;

    private GameObject m_Player;

    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        Vector2 playerVelocity = m_Player.GetComponent<Rigidbody2D>().linearVelocity;
        Vector3 velocityModifier = new Vector3(playerVelocity.x, playerVelocity.y) * velocityScale;
        Vector3 target = m_Player.transform.position + velocityModifier;
        target.z = -10;
        transform.position = Vector3.Lerp(transform.position, target, smoothStep);
    }
}
