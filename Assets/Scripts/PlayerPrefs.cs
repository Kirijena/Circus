using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float smoothTime = 0.1f;
    private int diceRollResult;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector3 velocity = Vector3.zero;
    private Vector3 previousPosition;
    
    void Start()
    {
        diceRollResult = PlayerPrefs.GetInt("DiceRollResult", 1);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        previousPosition = transform.position;
    }

    void Update()
    {
        MovePlayer();
        CheckMovement();
        FlipSprite();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal") * moveSpeed * diceRollResult * Time.deltaTime;
        float moveZ = Input.GetAxisRaw("Vertical") * moveSpeed * diceRollResult * Time.deltaTime;

        Vector3 movement = new Vector3(moveX, 0, moveZ);
        Vector3 targetPosition = transform.position + movement;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    void FlipSprite()
    {
        float movementX = transform.position.x - previousPosition.x;

        if (movementX > 0.01f) 
        {
            spriteRenderer.flipX = true;
        }
        else if (movementX < -0.01f) 
        {
            spriteRenderer.flipX = false;
        }

        previousPosition = transform.position;
    }

    void CheckMovement()
    {
        if ((transform.position - previousPosition).magnitude > 0.001f)
        {
            animator.SetBool("isMoving", true);
        }
        else
        { 
            animator.SetBool("isMoving", false);
        }
    }

    public void SetDiceRollResult(int rollResult)
    {
        diceRollResult = rollResult;
        PlayerPrefs.SetInt("DiceRollResult", diceRollResult);
    }
}
