using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public int currentHP;
    public int maxHP = 100;

    public bool IsAlive => currentHP > 0;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        if (damage == 0) return;
        
        currentHP -= damage;
        Debug.Log($"{gameObject.name}의 체력이 {currentHP}남았습니다.");

        if(!IsAlive)
        {

            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name}이 사망하였습니다.");
        Destroy(gameObject);

    }
}
