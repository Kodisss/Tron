using UnityEngine;

public class CPU : Player
{
    [SerializeField] private Transform target;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void InitializeDirection()
    {
        int randomNumberX = Random.Range(-1, 2);
        int randomNumberY;

        if (randomNumberX != 0)
        {
            randomNumberY = 0;
            movingHorizontally = true;
            movingVertically = false;
        }
        else
        {
            randomNumberY = Random.Range(0, 2) * 2 - 1;
            movingHorizontally = false;
            movingVertically = true;
        }

        direction = new Vector2(randomNumberX, randomNumberY);
    }

    protected override void UpdateDirection()
    {
        if (Random.Range(0, 1000) == 0)
        {
            int randomNumberX = Random.Range(-1, 2);
            int randomNumberY;

            if (randomNumberX != 0)
            {
                randomNumberY = 0;
            }
            else
            {
                randomNumberY = Random.Range(0, 2) * 2 - 1;
            }

            if (randomNumberX != 0 && !movingHorizontally)
            {
                direction = new Vector2(randomNumberX, 0f);
                movingHorizontally = true;
                movingVertically = false;
            }
            else if (randomNumberY != 0 && !movingVertically)
            {
                direction = new Vector2(0f, randomNumberY);
                movingVertically = true;
                movingHorizontally = false;
            }
        }
    }
}