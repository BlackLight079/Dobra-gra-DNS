using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float movementSpeed;
    float speedX, speedY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        speedX = Input.GetAxisRaw("Horizontal") * movementSpeed;
		// speedY = Input.GetAxisRaw("Vertical");
        
        rb.linearVelocity = new Vector2(speedX, rb.linearVelocity.y);
    }
}
