using UnityEngine;

public class EnemyHEalth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Destroy(gameObject);
        }
    }
}