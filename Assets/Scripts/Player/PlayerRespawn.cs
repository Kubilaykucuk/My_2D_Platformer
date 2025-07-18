using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindFirstObjectByType<UIManager>();
    }

    public void CheckRespawn()
    {
        if(currentCheckpoint == null)
        {
            uiManager.GameOver();
            return;
        }
        transform.position = currentCheckpoint.position;
        playerHealth.Respawn();

        Camera.main.GetComponent<CameraController>().MoveToPlayer(currentCheckpoint.parent);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform;
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("appear");
        }

        if (collision.CompareTag("OutofBounds"))
        {
            playerHealth.TakeDamage(playerHealth.StartingHealth);
        }

        if (collision.CompareTag("Finish"))
        {
            Win();
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("appear");
        }

    }

    private void Win()
    {
        uiManager.Win();
    }
}
