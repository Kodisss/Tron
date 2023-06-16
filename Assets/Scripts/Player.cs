using UnityEngine;

public abstract class Player : MonoBehaviour
{
    protected Rigidbody2D rb;

    [Header("FX")]
    [SerializeField] protected GameObject explosionSFX;
    [SerializeField] protected GameObject explosionVFX;
    [SerializeField] protected GameObject trail;

    [Header("Game Constants")]
    protected float speed;
    protected float spawnRate = 0.015f;

    protected bool canExplode = true;

    protected float timer = 0f;

    protected Vector2 direction;
    protected bool movingHorizontally;
    protected bool movingVertically;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = (PlayerPrefs.GetInt("Speed") + 1) * 4;
        InitializeDirection();
    }

    protected virtual void Update()
    {
        UpdateDirection();
        TrailGestion();
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected abstract void InitializeDirection();

    protected void TrailGestion()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            Instantiate(trail, rb.transform.position, rb.transform.rotation);
            timer = 0f;
        }
    }

    protected void MovingHorizontally()
    {
        movingHorizontally = true;
        movingVertically = false;
    }

    protected void MovingVertically()
    {
        movingHorizontally = false;
        movingVertically = true;
    }

    protected void Move()
    {
        rb.velocity = direction * speed;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && canExplode)
        {
            Instantiate(trail, rb.transform.position, rb.transform.rotation);
            PlayExplosion();
            canExplode = false;
        }
    }

    protected void PlayExplosion()
    {
        GameObject explosionSound = Instantiate(explosionSFX);
        GameObject explosionParticles = Instantiate(explosionVFX, rb.transform.position, rb.transform.rotation);
        CameraShake.Instance.ShakeCamera(5f, 0.1f);
        Destroy(explosionSound, 0.5f);
        Destroy(explosionParticles, 0.5f);
        Destroy(gameObject);
    }

    protected abstract void UpdateDirection();
}