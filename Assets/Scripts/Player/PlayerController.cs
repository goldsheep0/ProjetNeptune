using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float PropellerForce;
    public float AngularForce;
    public float MaxSpeed;

    private Rigidbody2D m_Rigidbody;

    private InputAction m_MoveAction;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();

        m_MoveAction = InputSystem.actions.FindAction("Move");
    }

    void FixedUpdate()
    {

        Vector2 moveValue = m_MoveAction.ReadValue<Vector2>();
        float moveAngle = Vector2.SignedAngle(transform.right, moveValue);
        Debug.Log(transform.right);

        // Ajoute la force à l'hélice
        if (moveValue.magnitude > 0)
        {
            m_Rigidbody.AddRelativeForce(Vector3.right * PropellerForce);
            // Tourne le sous marin
            if (moveAngle != 0)
                m_Rigidbody.AddTorque(Mathf.Sign(moveAngle) * AngularForce);
        }

    }
}
