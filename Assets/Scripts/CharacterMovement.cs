using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CharacterMovement : MonoBehaviour
{
    [Header("Attached GameObjects")]
    [SerializeField] protected GameObject explosionSFX;
    [SerializeField] protected GameObject explosionVFX;

    [Header("Player Constants")]
    [SerializeField] protected float speed = 10f;

    [Header("Trail Gestion")]
    [SerializeField] protected GameObject trailSegmentPrefab;
    [SerializeField] protected float trailSegmentSpacing = 1.8f;

    protected Vector3 previousPosition;

    // boolean used for movement gestion
    protected bool canExplode = true;
    protected bool alive = true;
    protected bool canMove = true;
    protected bool goesHorizontal = false;
    protected bool goesVertical = false;
    
    protected Vector2 movement;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        previousPosition = rb.transform.position;
        InitializeDirection();
    }

    protected virtual void FixedUpdate()
    {
        if (alive)
        {
            AuthoriseMovement();
            DetectIfYoureSafeFromYourTrail();
            Move();
        }
    }

    protected void Move()
    {
        if ((movement.x != 0 || movement.y != 0) && canMove)
        {
            rb.velocity = movement * speed;
        }
    }


    protected void InitializeDirection()
    {
        int randomNumberX = Random.Range(-1, 2);
        int randomNumberY;

        if (randomNumberX != 0)
        {
            randomNumberY = 0;
            MakeHorizontal();
        }
        else
        {
            randomNumberY = Random.Range(0, 2) * 2 - 1; //either 1 or -1
            MakeVertical();
        }

        movement = new Vector2(randomNumberX, randomNumberY);
        rb.velocity = movement * speed;
    }

    protected void AuthoriseMovement()
    {
        bool diagonal = !(movement.x == -1 || movement.x == 0 || movement.x == 1 || movement.y == -1 || movement.y == 0 || movement.y == 1);

        // cannot stop
        if (movement.x == 0 && movement.y == 0)
        {
            canMove = false;
            return;
        }

        // don't move diagonally
        if (diagonal)
        {
            canMove = false;
            return;
        }

        // Can't U-turn horizontally
        if (goesHorizontal && movement.x != 0)
        {
            canMove = false;
            return;
        }
        // Can't U-turn vertically
        if (goesVertical && movement.y != 0)
        {
            canMove = false;
            return;
        }

        if (movement.x == 0) MakeVertical();
        if (movement.y == 0) MakeHorizontal();

        canMove = true;
    }

    protected void MakeHorizontal()
    {
        goesHorizontal = true;
        goesVertical = false;
    }

    protected void MakeVertical()
    {
        goesHorizontal = false;
        goesVertical = true;
    }

    protected void DetectIfYoureSafeFromYourTrail()
    {
        if (Vector2.Distance(transform.position, previousPosition) >= trailSegmentSpacing)
        {
            SpawnTrail();
        }
    }

    protected void SpawnTrail()
    {
        Instantiate(trailSegmentPrefab, previousPosition, Quaternion.identity);

        previousPosition = rb.transform.position;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (alive)
        {
            Instantiate(trailSegmentPrefab, rb.transform.position, Quaternion.identity);
            Die();
        }
    }

    protected void Die()
    {
        alive = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        GameObject explosionSound = Instantiate(explosionSFX);
        GameObject explosionParticles = Instantiate(explosionVFX, rb.transform.position, rb.transform.rotation);
        CameraShake.Instance.ShakeCamera(5f, 0.1f);
        Destroy(explosionSound, 0.5f);
        Destroy(explosionParticles, 1f);
    }
}
