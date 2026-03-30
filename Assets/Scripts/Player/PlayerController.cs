using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Entity
{
    public float PropellerForce;
    public float AngularForce;
    public float ShootForceMin;
    public float ShootForceMax;
    public float MaxChargeTime;
    public float ShootingCooldown;

    public GameObject Canon;
    public GameObject ProjectilePrefab;

    private InputAction m_moveAction;
    private InputAction m_shootAction;
    
    private enum ShootingState
    {
        IDLE, CHARGING, SHOOTING,
    }
    private ShootingState m_shootingState;
    private float m_ChargingTime;

    protected override void Start()
    {
        base.Start();
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_shootAction = InputSystem.actions.FindAction("Shoot");

        m_shootingState = ShootingState.IDLE;
    }

    void FixedUpdate()
    {

        Vector2 moveValue = m_moveAction.ReadValue<Vector2>();
        float moveAngle = Vector2.SignedAngle(transform.right, moveValue);

        // Ajoute la force à l'hélice
        if (moveValue.magnitude > 0)
        {
            m_rigidbody.AddRelativeForce(Vector3.right * PropellerForce);
            // Tourne le sous marin
            if (Mathf.Abs(moveAngle) > 10)
                m_rigidbody.AddTorque(Mathf.Sign(moveAngle) * AngularForce);
            // Tourne plus doucement pour + de smoothness
            else
                m_rigidbody.AddTorque(Mathf.Sign(moveAngle) * AngularForce * Mathf.Abs(moveAngle) / 10);
        }

    }

    void Update()
    {

        // Tourne le canon pour qu'il pointe vers la souris
        if (m_shootingState != ShootingState.SHOOTING)
        {
            Vector2 canonDirection = GetCanonDirection();
            float zRotation = Vector2.SignedAngle(Vector2.right, canonDirection);
            Canon.transform.rotation = Quaternion.Euler(0, 0, zRotation);
        }

        // Charge si le bouton est pressé
        if (m_shootAction.IsPressed())
        {
            if (m_shootingState == ShootingState.IDLE)
            {
                m_shootingState = ShootingState.CHARGING;
                m_ChargingTime = 0;
            }
            if (m_shootingState == ShootingState.CHARGING)
            {
                m_ChargingTime += Time.deltaTime;
                if (m_ChargingTime > MaxChargeTime)
                    m_ChargingTime = MaxChargeTime;
            }
        }
        // On tire si le bouton est relâché + on est en train de charger
        else if (m_shootingState == ShootingState.CHARGING)
        {
            m_shootingState = ShootingState.SHOOTING;
            Shoot();
            Invoke(nameof(CooldownFinished), ShootingCooldown);
        }

    }

    // Tire si le bouton est pressé
    private void Shoot()
    {
        // Calcul de la force en fonction du temps passé à charger
        Debug.Log(m_ChargingTime);
        float shootForce = Mathf.Lerp(ShootForceMin, ShootForceMax, m_ChargingTime / MaxChargeTime);
        Vector2 canonDirection = GetCanonDirection();
        GameObject projectile = Instantiate(ProjectilePrefab, null);
        projectile.transform.position = transform.position + (Vector3)canonDirection.normalized * 1.5f;
        projectile.GetComponent<Rigidbody2D>().AddForce(canonDirection.normalized * shootForce, ForceMode2D.Impulse);
    }

    // Récupère un vecteur qui pointe vers la souris du pdv du canon
    private Vector2 GetCanonDirection()
    {
        Camera camera = Camera.main;
        Vector3 screenCoords = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0);
        Vector2 mousePos = camera.ScreenToWorldPoint(screenCoords);
        return mousePos - (Vector2)Canon.transform.position;
    }

    private void CooldownFinished()
    {
        m_shootingState = ShootingState.IDLE;
    }
}
