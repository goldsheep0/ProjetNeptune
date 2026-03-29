using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Entity
{
    public float PropellerForce;
    public float AngularForce;
    public float ShootForce;
    public GameObject Canon;
    public GameObject ProjectilePrefab;

    private InputAction m_MoveAction;
    private InputAction m_ShootAction;

    protected override void Start()
    {
        base.Start();
        m_MoveAction = InputSystem.actions.FindAction("Move");
        m_ShootAction = InputSystem.actions.FindAction("Shoot");
        m_ShootAction.performed += OnShoot;
    }

    void FixedUpdate()
    {

        Vector2 moveValue = m_MoveAction.ReadValue<Vector2>();
        float moveAngle = Vector2.SignedAngle(transform.right, moveValue);

        // Ajoute la force à l'hélice
        if (moveValue.magnitude > 0)
        {
            m_Rigidbody.AddRelativeForce(Vector3.right * PropellerForce);
            // Tourne le sous marin
            if (moveAngle != 0)
                m_Rigidbody.AddTorque(Mathf.Sign(moveAngle) * AngularForce);
        }

    }

    void Update()
    {

        // Tourne le canon pour qu'il pointe vers la souris
        Vector2 canonDirection = GetCanonDirection();
        float zRotation = Vector2.SignedAngle(Vector2.right, canonDirection);
        Canon.transform.rotation = Quaternion.Euler(0, 0, zRotation);

    }

    // Tire si le bouton est pressé
    private void OnShoot(InputAction.CallbackContext context)
    {
        Vector2 canonDirection = GetCanonDirection();
        GameObject projectile = Instantiate(ProjectilePrefab, null);
        projectile.transform.position = transform.position + (Vector3)canonDirection.normalized * 1.5f;
        projectile.GetComponent<Rigidbody2D>().AddForce(canonDirection.normalized * ShootForce, ForceMode2D.Impulse);
    }

    // Récupère la position de la souris dans les coordonnées du jeu
    private Vector2 GetCanonDirection()
    {
        Camera camera = Camera.main;
        Vector3 screenCoords = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0);
        Vector2 mousePos = camera.ScreenToWorldPoint(screenCoords);
        return mousePos - (Vector2)Canon.transform.position;
    }
}
