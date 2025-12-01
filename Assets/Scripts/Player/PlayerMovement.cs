using System.Linq.Expressions;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    [SerializeField] GameObject attackHitbox;

    [SerializeField] float movementSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float fightingMovementSpeed;
    [SerializeField] float fightingJumpSpeed;
    float speedX, speedY, jump;
    bool canJump, fightingStance, leftFloor, airStop, canAirStop;
    int jumpTimer, airStopTimer, attackTimer;

    int jumpTimerLimit = 3;
    int airStopTimerLimit = 12;
    int attackTimerLimit = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        fightingStance = false;
        leftFloor = false;
        canAirStop = true;

        jumpTimer = 0;
        airStopTimer = 0;
    }

    void Update()
    {
        // Attacking
        if (Input.GetButtonDown("Fire1") && fightingStance)
        {
            attackHitbox.SetActive(true);
            attackTimer = 0;
        }


        // Rotation
        if (attackTimer == attackTimerLimit)
        {
            if (speedX > 0) attackHitbox.transform.localPosition = new Vector2(1, 0);
            if (speedX < 0) attackHitbox.transform.localPosition = new Vector2(-1, 0);
        }


        // Changing stances
        if (Input.GetButtonDown("Fire3")) // Lshift
        {
            fightingStance = !fightingStance;
            animator.SetTrigger("Draw Weapon");
            if (fightingStance && canAirStop)
            {
                canAirStop = false;
                airStop = true;
                airStopTimer = 0;
            }
        }
        if (airStop) rb.linearVelocity = new Vector2(0, 0);


        // Movement
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
        

        // Jumping
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

        if (attackTimer < attackTimerLimit)
        {
            attackTimer++;
            if (attackTimer == attackTimerLimit) attackHitbox.SetActive(false);
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
                canAirStop = true;
                break;
            case "ChangeSceneTrigger":
                collision.GetComponent<ChangeSceneTrigger>().ChangeScene();
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
