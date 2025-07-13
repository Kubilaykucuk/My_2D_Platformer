using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    private Animator anim;
    private PlayerMovement movement;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private GameObject swordHitbox;
    [SerializeField] private AudioClip swordSound;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && movement.canAttack())
            Attack();

        cooldownTimer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!swordHitbox.activeInHierarchy) return;

        if (collision.CompareTag("Enemy"))
        {
            Health enemyHealth = collision.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1);
            }
        }
    }

    private void Attack() 
    {
        SoundManager.instance.PlaySound(swordSound);
        anim.SetTrigger("attack");
        cooldownTimer = 0;
    }

    public void EnableHitbox()
    {
        swordHitbox.SetActive(true);
    }
    public void DisableHitbox()
    {
        swordHitbox.SetActive(false);
    }
}
