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

    [Header("Movement Keys")]
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode jumpKey = KeyCode.W;

    [SerializeField] private Animator animator;
    [Header("Attack")]
    [SerializeField] private bool enableDoubleTapAttack = true;
    [SerializeField]
    [Tooltip("Maximum time (seconds) between taps to count as a double tap")]
    private float doubleTapTime = 0.25f;
    [SerializeField]
    [Tooltip("Animator trigger name for attack animation")]
    private string attackTrigger = "Attack";

    // double-tap state
    private float lastLeftTapTime = -1f;
    private float lastRightTapTime = -1f;

    // Update is called once per frame
    void Update()
    {
        horizontal = 0f;
        if (Input.GetKey(leftKey))
        {
            horizontal = -1f;




        }
        if (Input.GetKey(rightKey))
            horizontal = 1f;

        // Double-tap attack detection (left/right)
        if (enableDoubleTapAttack)
        {
            // Left key tapped
            if (Input.GetKeyDown(leftKey))
            {
                float time = Time.time;
                if (time - lastLeftTapTime <= doubleTapTime)
                {
                    // Double-tap detected: attack to the left
                    TryAttack(Vector2.left);
                    lastLeftTapTime = -1f; // reset
                }
                else
                {
                    lastLeftTapTime = time;
                }
            }

            // Right key tapped
            if (Input.GetKeyDown(rightKey))
            {
                float time = Time.time;
                if (time - lastRightTapTime <= doubleTapTime)
                {
                    // Double-tap detected: attack to the right
                    TryAttack(Vector2.right);
                    lastRightTapTime = -1f; // reset
                }
                else
                {
                    lastRightTapTime = time;
                }
            }
        }

        if (Input.GetKeyDown(jumpKey) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }
        if (Input.GetKeyUp(jumpKey) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (Mathf.Abs(horizontal) == 1 | Mathf.Abs(rb.linearVelocity.y) > 0)
        {
            animator.SetFloat("Speed", 1);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }




        Flip();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
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
        // TODO: Add hit detection (OverlapBox, Raycast) and gameplay effects here.
    }
}