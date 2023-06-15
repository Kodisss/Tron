using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.Windows;

public class CPU : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("FX")]
    [SerializeField] private GameObject explosionSFX;
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private GameObject trail;

    [Header("Game Constants")]
    private float speed;
    private float spawnRate = 0.02f;

    private bool canExplode = true;

    private float timer = 0f;

    private Vector2 direction;
    private bool movingHorizontaly = false;
    private bool movingVerticaly = false;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = (PlayerPrefs.GetInt("Speed") + 1) * 2;
        UpdateDirection();
    }

    // Update is called once per frame
    private void Update()
    {
        if(Random.Range(0,1000) == 0) UpdateDirection();
        TrailGestion();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void UpdateDirection()
    {
        int randomNumberX = Random.Range(-1, 2);
        int randomNumberY;

        if(randomNumberX != 0)
        {
            randomNumberY = 0;
        }
        else
        {
            randomNumberY = Random.Range(0, 2) * 2 - 1;
        }

        if (randomNumberX != 0 && !movingHorizontaly)
        {
            direction = new Vector2(randomNumberX, 0f);
            movingHorizontaly = true;
            movingVerticaly = false;
        }
        else if (randomNumberY != 0 && !movingVerticaly)
        {
            direction = new Vector2(0f, randomNumberY);
            movingVerticaly = true;
            movingHorizontaly = false;
        }
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
