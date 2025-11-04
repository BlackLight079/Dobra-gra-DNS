using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D col;

    [SerializeField] float movementSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float fightingMovementSpeed;
    [SerializeField] float fightingJumpSpeed;
    float speedX, speedY, jump;
    bool canJump, fightingStance;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        fightingStance = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire3")) fightingStance = !fightingStance;

        if (fightingStance)
        {
            speedX = Input.GetAxisRaw("Horizontal") * fightingMovementSpeed;
            // speedY = Input.GetAxisRaw("Vertical") * fightingMovementSpeed;
            jump = Input.GetAxis("Jump") * fightingJumpSpeed;
        }
        else
        {
            speedX = Input.GetAxisRaw("Horizontal") * movementSpeed;
            // speedY = Input.GetAxisRaw("Vertical") * movementSpeed;
            jump = Input.GetAxis("Jump") * jumpSpeed;
        }

        rb.linearVelocity = new Vector2(speedX, rb.linearVelocity.y);
        
        if (Input.GetButtonDown("Jump") && canJump)
        {
            canJump = false;
            rb.linearVelocityY = jump;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.tag)
        {
            case "Floor":
                canJump = true;
                break;
        }
    }
}
