using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineCamera))]
public class DynamicCameraSize : MonoBehaviour
{
    public Transform player;
    public float defaultZoom = 4f;
    public float zoomedOut = 6f;
    public float smoothSpeed = 5f;

    public float checkDistanceDown = 5f;
    public float checkDistanceSide = 3f;

    public LayerMask groundLayer;

    private CinemachineCamera vcam;
    private float targetZoom;

    void Start()
    {
        vcam = GetComponent<CinemachineCamera>();
        targetZoom = defaultZoom;
    }

    void Update()
    {
        if (player == null) return;

        targetZoom = defaultZoom;

        // Check for air below
        bool isAirBelow = !Physics2D.Raycast(player.position, Vector2.down, checkDistanceDown, groundLayer);

        // Determine facing direction
        Vector2 facingDirection = player.localScale.x > 0 ? Vector2.right : Vector2.left;

        Vector3 frontOrigin = player.position + (Vector3)(facingDirection * 0.5f);
        Vector3 backOrigin = player.position - (Vector3)(facingDirection * 0.5f);

        bool isAirAhead = !Physics2D.Raycast(frontOrigin, Vector2.down, checkDistanceDown, groundLayer);
        bool isAirBehind = !Physics2D.Raycast(backOrigin, Vector2.down, checkDistanceDown, groundLayer);

        if (isAirBelow || isAirAhead || isAirBehind)
        {
            targetZoom = zoomedOut;
        }

        // Smooth zoom transition
        vcam.Lens.OrthographicSize = Mathf.Lerp(vcam.Lens.OrthographicSize, targetZoom, Time.deltaTime * smoothSpeed);
    }

    void OnDrawGizmos()
    {
        if (player == null) return;

        Vector2 facingDirection = player.localScale.x > 0 ? Vector2.right : Vector2.left;
        Vector3 frontOrigin = player.position + (Vector3)(facingDirection * 0.5f);
        Vector3 backOrigin = player.position - (Vector3)(facingDirection * 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(player.position, player.position + Vector3.down * checkDistanceDown);
        Gizmos.DrawLine(frontOrigin, frontOrigin + Vector3.down * checkDistanceDown);
        Gizmos.DrawLine(backOrigin, backOrigin + Vector3.down * checkDistanceDown);
    }
}
