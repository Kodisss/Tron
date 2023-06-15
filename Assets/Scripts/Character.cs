using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("FX")]
    [SerializeField] private GameObject explosionSFX;
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private GameObject trail;

    [Header("Game Constants")]
    [SerializeField] private float speed = 3f;
    private float spawnRate;

    private bool canExplode = true;

    private float timer = 0f;

    private Vector2 direction = Vector2.right;
    private bool movingHorizontaly = true;
    private bool movingVerticaly = false;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // it's a great spawn rate for it to not spawn too much object but also to have a trail without holes
        spawnRate = (1f / 40f) * speed; 
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateDirection();
        TrailGestion();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void TrailGestion()
    {
        // Increment the timer by the elapsed time since the last frame
        timer += Time.deltaTime;

        // Check if the desired interval has passed
        if (timer >= spawnRate)
        {
            Instantiate(trail, rb.transform.position, rb.transform.rotation);

            timer = 0f;
        }
    }

    private void Move()
    {
        // Apply the velocity to the object
        rb.velocity = direction * speed;
    }

    // checks for input and change direction accordingly you also cannot turn around
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
        if (collision.gameObject.CompareTag("Wall") && canExplode)
        {
            Instantiate(trail, rb.transform.position, rb.transform.rotation); // place a last trail before you die
            PlayExplosion();
            canExplode = false;
        }
    }

    // do everything you need before diying
    private void PlayExplosion()
    {
        GameObject explosionSound = Instantiate(explosionSFX);
        GameObject explosionParticules = Instantiate(explosionVFX, rb.transform.position, rb.transform.rotation);
        Destroy(explosionSound, 0.5f);        
        Destroy(explosionParticules, 0.5f);
        Destroy(gameObject);
    }
}
