using UnityEngine;

public class DuckMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;

    [Header("Movement Keys")]
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode jumpKey = KeyCode.W;
    [SerializeField] private KeyCode attackKey = KeyCode.S;

    [SerializeField] private Animator animator;
    [Header("Attack")]
    [SerializeField] private bool enableDoubleTapAttack = true;
    [SerializeField]
    [Tooltip("Maximum time (seconds) between taps to count as a double tap")]
    private float doubleTapTime = 0.25f;
    [SerializeField]
    [Tooltip("Animator trigger name for attack animation")]
    private string attackTrigger = "Attack";

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] attackClips;

    // double-tap state
    private float lastLeftTapTime = -1f;
    private float lastRightTapTime = -1f;

    // Update is called once per frame
    void Update()
    {
        horizontal = 0f;
        bool leftPressed = Input.GetKey(leftKey);
        bool rightPressed = Input.GetKey(rightKey);
        if (leftPressed && !rightPressed)
            horizontal = -1f;
        else if (rightPressed && !leftPressed)
            horizontal = 1f;
        // If both are pressed, horizontal stays 0

        // Attack only on attackKey (S)
        if (Input.GetKeyDown(attackKey))
        {
            Attack();
        }

        if (Input.GetKeyDown(jumpKey) && IsGrounded())
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        if (Input.GetKeyUp(jumpKey) && rb.linearVelocity.y > 0f)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);

        // Set animator speed parameter: 1 when moving horizontally or when in the air, otherwise 0
        if (animator != null)
        {
            bool movingHorizontally = Mathf.Abs(horizontal) > 0.01f;
            bool inAir = Mathf.Abs(rb.linearVelocity.y) > 0.01f;
            animator.SetFloat("Speed", (movingHorizontally || inAir) ? 1f : 0f);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        // Add leeway for ground detection by slightly increasing the raycast distance
        float leeway = 0.05f; // 5 pixels if pixels per unit is 100
        bool groundBelow = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.25f + leeway, groundLayer);

        // Check for other players directly below, ignore self
        Collider2D[] hits = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f + leeway, playerLayer);
        bool playerBelow = false;
        foreach (var hit in hits)
        {
            if (hit.gameObject != this.gameObject)
            {
                playerBelow = true;
                break;
            }
        }

        return groundBelow || playerBelow;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    /// <summary>
    /// Attempt to attack in the requested direction (world space). The attack will only
    /// execute if the character is currently facing that direction.
    /// </summary>
    /// <param name="dir">Direction of desired attack (Vector2.left or Vector2.right)</param>
    private void TryAttack(Vector2 dir)
    {
        if (!enableDoubleTapAttack) return;

        bool facingRightNow = isFacingRight;
        // Determine if dir matches facing
        if ((dir.x < 0 && !facingRightNow) || (dir.x > 0 && facingRightNow))
        {
            Attack();
        }
        else
        {
            // Optionally flip to face the attack direction before attacking.
            // For now, only attack when already facing that direction.
        }
    }

    /// <summary>
    /// Performs the attack: triggers animator and can be expanded to do hit checks.
    /// </summary>
    public void Attack()
    {
        if (animator != null && !string.IsNullOrEmpty(attackTrigger))
        {
            animator.SetTrigger(attackTrigger);
        }
        // Play a random attack sound
        if (audioSource != null && attackClips != null && attackClips.Length > 0)
        {
            int idx = Random.Range(0, attackClips.Length);
            audioSource.PlayOneShot(attackClips[idx]);
        }
        // TODO: Add hit detection (OverlapBox, Raycast) and gameplay effects here.
    }
}