using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private PlayerRuntimeStats runtimeStats;

    private Rigidbody2D rb;
    private float currentMoveSpeed;


    public void setInputReader(InputReader reader)
    {
        inputReader = reader;
    }
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (runtimeStats != null)
            currentMoveSpeed = runtimeStats.MoveSpeed;
    }

    private void FixedUpdate()
    {
        Vector2 move = inputReader.MovementInput.normalized * currentMoveSpeed;
        rb.linearVelocity = move;
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
}
