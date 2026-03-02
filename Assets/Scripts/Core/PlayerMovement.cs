using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;

    [SerializeField] private PlayerRuntimeStats runtimeStats;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private float currentMoveSpeed;


    public void setInputReader(InputReader reader)
    {
        inputReader = reader;
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (runtimeStats != null)
            currentMoveSpeed = runtimeStats.MoveSpeed;
    }

    private void FixedUpdate()
    {
        Vector2 move = inputReader.MovementInput.normalized * currentMoveSpeed;
        rb.linearVelocity = move;
        StartAnimation(move);
        if (move.x != 0)
        {
            spriteRenderer.flipX = move.x < 0;
        }
    }

    private void OnEnable()
    {
        if (runtimeStats != null)
            runtimeStats.OnStatsChanged += UpdateMoveSpeed;
    }

    private void OnDisable()
    {
        if (runtimeStats != null)
            runtimeStats.OnStatsChanged -= UpdateMoveSpeed;
    }

    private void UpdateMoveSpeed()
    {
        if (runtimeStats != null)
            currentMoveSpeed = runtimeStats.MoveSpeed;
    }

    //Animation for Player --------------------------------------------------
    private void StartAnimation(Vector2 move)
    {
        if (move.y < 0)
        {
            animator.SetBool("isRunningDown", true);
            animator.SetBool("isRunningUp", false);
            animator.SetBool("isRunning", false);
        }
        else if (move.y > 0)
        {
            animator.SetBool("isRunningUp", true);
            animator.SetBool("isRunningDown", false);
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isRunning", move.x != 0);
            animator.SetBool("isRunningUp", false);
            animator.SetBool("isRunningDown", false);
        }
    }

    public void SetIdleIfStanding(string direction)
    {
        if (rb.linearVelocity == Vector2.zero)
        {
            switch (direction)
            {
                case "Up":
                    animator.Play("ShinobiIdleUp");
                    break;
                case "Down":
                    animator.Play("ShinobiIdleDown");
                    break;
            }
        }
    }

    public void SetRunAgain(string direction)
    {
        if (!(rb.linearVelocity == Vector2.zero))
        {
            switch (direction)
            {
                case "Up":
                    animator.Play("ShinobiRunUp");
                    break;
                case "Down":
                    animator.Play("ShinobiRunDown");
                    break;
            }
        }
    }
}