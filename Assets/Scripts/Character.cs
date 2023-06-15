using UnityEngine;

public class Character : Player
{
    [SerializeField] private string playerName = "Player 1";

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
        Vector2 input = Vector2.zero;

        if (playerName == "Player 1")
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }
        else if (playerName == "Player 2")
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal2"), Input.GetAxisRaw("Vertical2")).normalized;
        }

        if (input.x != 0 && !movingHorizontally)
        {
            direction = new Vector2(input.x, 0f);
            movingHorizontally = true;
            movingVertically = false;
        }
        else if (input.y != 0 && !movingVertically)
        {
            direction = new Vector2(0f, input.y);
            movingVertically = true;
            movingHorizontally = false;
        }
    }
}