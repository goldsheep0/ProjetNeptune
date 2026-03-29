using UnityEditor;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float Health;

    protected Rigidbody2D m_Rigidbody;
    protected virtual void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Formule de calcul des dégâts
        Health -= collision.relativeVelocity.magnitude * collision.rigidbody.mass;
        if (Health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.35f);
        Invoke(nameof(DestroyGameObject), 1f);
    }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
