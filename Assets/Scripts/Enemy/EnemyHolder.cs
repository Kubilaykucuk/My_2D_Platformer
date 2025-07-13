using UnityEngine;

public class EnemyHolder : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    void Update()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x = Mathf.Sign(enemy.localScale.x) * Mathf.Abs(currentScale.x) * -1;
        transform.localScale = currentScale;
    }
}
