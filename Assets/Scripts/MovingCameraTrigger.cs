using System;
using Unity.VisualScripting;
using UnityEngine;

public class MovingCameraTrigger : MonoBehaviour
{
	Rigidbody2D cameraRB; // Rigidbody2D of Camera

    enum Direction
	{
		___,
		left,
		right,
		up,
		down
	}

	[SerializeField] Direction direction;


    private void Start()
    {
        cameraRB = transform.parent.GetComponent<Rigidbody2D>();
    }


    private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
            switch (direction)
			{
				case Direction.right:
                    if (collision.attachedRigidbody.linearVelocityX > 0)
                        cameraRB.linearVelocityX = collision.attachedRigidbody.linearVelocityX;
                    break;
				case Direction.left:
                    if (collision.attachedRigidbody.linearVelocityX < 0)
                        cameraRB.linearVelocityX = collision.attachedRigidbody.linearVelocityX;
                    break;
				case Direction.up:
                    if (collision.attachedRigidbody.linearVelocityY > 0)
                        cameraRB.linearVelocityY = collision.attachedRigidbody.linearVelocityY;
                    break;
				case Direction.down:
					if (collision.attachedRigidbody.linearVelocityY < 0)
                        cameraRB.linearVelocityY = collision.attachedRigidbody.linearVelocityY;
                    break;
				default:
					Debug.Log("No direction set");
					break;
			}
		}
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") cameraRB.linearVelocity = new Vector2(0, 0);
    }
}
