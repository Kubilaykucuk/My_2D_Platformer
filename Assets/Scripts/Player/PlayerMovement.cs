using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;

    [Header ("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float coyoteCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallJumpY;

    [Header ("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header ("Sound")]
    [SerializeField] private AudioClip jumpsound;

    [Header("Wall Sliding")]
    [SerializeField] private float slideSpeed = 2f; // Limit vertical sliding speed
    [SerializeField] private float wallSlideGravity = 0.5f;
    [SerializeField] private float wallSlideDelay = 0.2f; // Delay before slide starts
    [SerializeField] private float wallSlideAcceleration = 15f;  // how fast sliding accelerates
    [SerializeField] private float maxWallSlideSpeed = -10f;
    private bool hasEnteredWallSlide;
    private float wallTouchTime;

    private bool isWallSliding;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //flip the character
        if (horizontalInput > 0.01f) 
        {
            var scale = transform.localScale;
            if (scale.x < 0.0f) 
            scale.x = scale.x * -1;

            transform.localScale = scale;
        }
        else if (horizontalInput  < -0.01f) 
        {
            var scale = transform.localScale;
            if(scale.x > 0.0f)
            scale.x = scale.x * -1;

            transform.localScale = scale;
        }

        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());
        anim.SetBool("onwall", onWall());

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }

        if (Input.GetKeyUp(KeyCode.W) && body.linearVelocityY > 0)
        {
            body.linearVelocity = new Vector2(body.linearVelocityX, body.linearVelocityY / 2);
        }


        bool touchingWall = onWall();
        bool grounded = isGrounded();
        bool movingTowardWall = horizontalInput != 0 && Mathf.Sign(horizontalInput) == Mathf.Sign(transform.localScale.x);
        bool shouldStartSliding = touchingWall && !grounded && movingTowardWall;

        if (shouldStartSliding || (hasEnteredWallSlide && touchingWall && !grounded))
        {
            wallTouchTime += Time.deltaTime;

            if (wallTouchTime >= wallSlideDelay)
            {
                isWallSliding = true;
                hasEnteredWallSlide = true;

                // Freeze downward speed at slideSpeed
                float newYVelocity = body.linearVelocity.y - wallSlideAcceleration * Time.deltaTime;
                newYVelocity = Mathf.Max(newYVelocity, maxWallSlideSpeed); // Clamp to max fall speed
                body.linearVelocity = new Vector2(body.linearVelocity.x, newYVelocity);

                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("slide"))
                {
                    anim.SetTrigger("slide");
                }
            }
            else
            {
                isWallSliding = false;
                body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);
            }
        }
        else
        {
            wallTouchTime = 0f;
            isWallSliding = false;
            hasEnteredWallSlide = false;
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);
        }
        anim.SetBool("onwall", isWallSliding);

    }


    private void Jump() 
    {
        if (coyoteCounter < 0 && !onWall()) return;

        SoundManager.instance.PlaySound(jumpsound);

        if (onWall())
            WallJump();
        else 
        {
            if (isGrounded())
                body.linearVelocity = new Vector2(body.linearVelocityX, jumpSpeed);
            else
            {
                if (coyoteCounter > 0) 
                    body.linearVelocity = new Vector2(body.linearVelocityX, jumpSpeed);
            }

            coyoteCounter = 0;
        }

    }

    private void WallJump()
    {
        Vector2 jumpDir = new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY);
        body.linearVelocity = jumpDir;

        isWallSliding = false;
        wallJumpCooldown = 0.2f;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack() 
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
