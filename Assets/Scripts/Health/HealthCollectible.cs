using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healthValue;
    [SerializeField] private AudioClip healthsound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            Health playerHealth = collision.GetComponent<Health>();

            // Only heal if currentHealth is less than startingHealth
            if (playerHealth.currentHealth < playerHealth.StartingHealth)
            {
                SoundManager.instance.PlaySound(healthsound);
                playerHealth.AddHealth(healthValue);
                gameObject.SetActive(false);
            }
        }
    }
}
