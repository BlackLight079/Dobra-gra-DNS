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
    float jumpTimer, airStopTimer, attackTimer;

    float jumpTimerLimit = 0.06f;
    float airStopTimerLimit = 0.24f;
    float attackTimerLimit = 0.20f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        fightingStance = false;
        leftFloor = false;
        canAirStop = true;

        jumpTimer = 0;
        airStopTimer = 0;
        attackTimer = 0;
    }

    void Update()
    {
        // Attacking
        if (Input.GetButtonDown("Fire1") && fightingStance)
        {
            attackHitbox.SetActive(true);
            attackTimer = Time.time + attackTimerLimit;
        }


        // Rotation
        if (Time.time > attackTimer)
        {
            attackHitbox.SetActive(false);
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
                airStopTimer = Time.time + airStopTimerLimit;
            }
        }

        // Air stop
        if (Time.time < airStopTimer) AirStop();


        // Movement
        if (fightingStance) // Drawn Weapon
        {
            speedX = Input.GetAxisRaw("Horizontal") * fightingMovementSpeed;
            // speedY = Input.GetAxisRaw("Vertical") * fightingMovementSpeed;
            jump = Input.GetAxis("Jump") * fightingJumpSpeed;
        }
        else // normal
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

        // Jumping after leaving ground
        if (leftFloor && Time.time > jumpTimer)
        {
            canJump = false;
            leftFloor = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Floor":
                canJump = true;
                leftFloor = false;
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
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Floor":
                canAirStop = true;
                leftFloor = true;
                jumpTimer = Time.time + jumpTimerLimit;
                break;
        }
    }



    void AirStop()
    {
        rb.linearVelocity = new Vector2(0, 0);
    }
}
