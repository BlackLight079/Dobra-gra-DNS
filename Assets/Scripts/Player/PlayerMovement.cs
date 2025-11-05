using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float movementSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float fightingMovementSpeed;
    [SerializeField] float fightingJumpSpeed;
    float speedX, speedY, jump;
    bool canJump, fightingStance, leftFloor, airStop;
    int jumpTimer, airStopTimer;

    int jumpTimerLimit = 3;
    int airStopTimerLimit = 12;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        fightingStance = false;
        leftFloor = false;

        jumpTimer = 0;
        airStopTimer = 0;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            fightingStance = !fightingStance;
            if (fightingStance)
            {
                airStop = true;
                airStopTimer = 0;
            }
        }
        if (airStop) rb.linearVelocity = new Vector2(0, 0);

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

        rb.linearVelocityX = speedX;
        
        if (Input.GetButtonDown("Jump") && canJump)
        {
            canJump = false;
            rb.linearVelocityY = jump;
        }
    }

    private void FixedUpdate()
    {
        //if (leftFloor) jumpTimer++;
        //if (jumpTimer > 5) canJump = false;

        if (leftFloor)
        {
            jumpTimer++;
            if (jumpTimer > jumpTimerLimit)
            {
                canJump = false;
                leftFloor = false;
            }
        }

        if (airStop)
        {
            airStopTimer++;
            if (airStopTimer > airStopTimerLimit) airStop = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Floor":
                canJump = true;
                leftFloor = false;
                jumpTimer = 0;
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Floor":
                canJump = true;
                leftFloor = false;
                jumpTimer = 0;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Floor":
                leftFloor = true;
                break;
        }
    }
}
