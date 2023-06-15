using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private GameObject explosionSFX;
    [SerializeField] private GameObject explosionVFX;

    [SerializeField] private float speed = 3f;

    private Vector2 direction = Vector2.right;
    private bool movingHorizontaly = true;
    private bool movingVerticaly = false;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateDirection();
        Move();
    }

    private void Move()
    {
        // Apply the velocity to the object
        rb.velocity = direction * speed;
    }

    private void UpdateDirection()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (input.x != 0 && !movingHorizontaly)
        {
            direction = new Vector2(input.x,0f);
            movingHorizontaly = true;
            movingVerticaly = false;
        }
        else if (input.y != 0 && !movingVerticaly)
        {
            direction = new Vector2(0f, input.y);
            movingVerticaly = true;
            movingHorizontaly = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            PlayExplosion();
        }
    }

    private void PlayExplosion()
    {
        GameObject explosionSound = Instantiate(explosionSFX);
        GameObject explosionParticules = Instantiate(explosionVFX, rb.transform.position, rb.transform.rotation);
        Destroy(explosionSound, 0.5f);        
        Destroy(explosionParticules, 0.5f);
        Destroy(gameObject);
    }
}
