using UnityEngine;

public class QuickDamage : MonoBehaviour
{
    [SerializeField] private int Damage;
    private void OnTriggerEnter2D(Collider2D Source)
    {
        if (Source.tag == "Entity")
        {
            Source.TryGetComponent<Entity>(out var entity);
            if (entity == null) return;
            entity.TakeDamage(Damage);
        }
    }
}
