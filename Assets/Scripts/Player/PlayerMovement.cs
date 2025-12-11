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
    bool canJump, fightingStance, leftFloor, airStop, canAirStop, inWater;
    float jumpTimer, airStopTimer, attackTimer;

    float jumpTimerLimit = 0.06f;
    float airStopTimerLimit = 0.24f;
    float attackTimerLimit = 0.20f;

    float defaultGravity, waterGravity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        defaultGravity = rb.gravityScale;
        waterGravity = 1f;

        fightingStance = false;
        leftFloor = false;
        canAirStop = true;
        inWater = false;

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
            if (fightingStance) animator.SetTrigger("Draw Weapon");
            else animator.SetTrigger("Sheath Weapon");

            if (fightingStance && canAirStop)
            {
                canAirStop = false;
                airStopTimer = Time.time + airStopTimerLimit;
            }

            if (fightingStance && inWater) rb.gravityScale = defaultGravity;
            else if (!fightingStance && inWater) rb.gravityScale = waterGravity;
        }

        // Air stop
        if (Time.time < airStopTimer) AirStop();


        // Movement
        if (fightingStance) // Drawn Weapon
        {
            speedX = Input.GetAxisRaw("Horizontal") * fightingMovementSpeed;
            jump = Input.GetAxis("Jump") * fightingJumpSpeed;
        }
        else if (inWater) // in water
        {
            speedX = Input.GetAxisRaw("Horizontal") * movementSpeed;
            speedY = Input.GetAxisRaw("Vertical") * movementSpeed;
        }
        else // normal
        {
            speedX = Input.GetAxisRaw("Horizontal") * movementSpeed;
            jump = Input.GetAxis("Jump") * jumpSpeed;
        }
        rb.linearVelocityX = speedX;
        if (inWater && !fightingStance) rb.linearVelocityY = speedY;
        

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
            case "Water":
                EnterWater();
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
            case "Water":
                ExitWater();
                break;
        }
    }



    void AirStop()
    {
        rb.linearVelocity = new Vector2(0, 0);
    }


    void EnterWater()
    {
        inWater = true;
        if (!fightingStance) rb.gravityScale = waterGravity;
        Debug.Log("inWater");
    }

    void ExitWater()
    {
        inWater = false;
        rb.gravityScale = defaultGravity;
        Debug.Log("NOTinWater");
    }
}
