using UnityEngine;

public class SquareOfEvil : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Attack Hitbox")
        {
            Destroy(gameObject);
        }
    }
}
