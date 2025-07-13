using UnityEngine;

public class MushroomEnemy : MonoBehaviour
{
    [SerializeField] private float contactdamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(contactdamage);
        }
    }
}
