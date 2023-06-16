using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.UI.Image;

public class CPU : Player
{
    private string targetName;
    private Transform target;

    private float timeBetweenMoves = 0.3f;
    private float cooldown = 0f;
    private bool canMove = true;

    protected override void Start()
    {
        base.Start();
        PickATarget();
    }

    protected override void Update()
    {
        FocusTarget();
        Debug.Log(target);
        base.Update();
        CoolDownGestion();
    }

    private void FocusTarget()
    {
        if(GameObject.FindWithTag(targetName) != null) // if player is alive, follow them
        {
            target = GameObject.FindWithTag(targetName).GetComponent<Transform>();
        }
        else // if not, make the list of all the CPUs and target one at random
        {
            GameObject[] listOfCPU = GameObject.FindGameObjectsWithTag("CPU");
            if(listOfCPU.Length > 0)
            {
                target = listOfCPU[Random.Range(0, listOfCPU.Length)].GetComponent<Transform>();
            }
            else // if no CPU alive, you win ! So just don't move and die
            {
                canMove = false;
            }
        }
    }

    private void PickATarget()
    {
        if (PlayerPrefs.GetInt("GameMode") == 0)
        {
            if(Random.Range(0, 2) == 0)
            {
                targetName = "Player1";
            }
            else
            {
                targetName = "Player2";
            }
        }
        else
        {
            targetName = "Player1";
        }
    }

    protected override void InitializeDirection()
    {
        int randomNumberX = Random.Range(-1, 2);
        int randomNumberY;

        if (randomNumberX != 0)
        {
            randomNumberY = 0;
            MovingHorizontally();
        }
        else
        {
            randomNumberY = Random.Range(0, 2) * 2 - 1;
            MovingVertically();
        }

        direction = new Vector2(randomNumberX, randomNumberY);
    }

    protected override void UpdateDirection()
    {
        WallAvoiding();
        if (canMove) PathFinding();
    }

    private void CoolDownGestion()
    {
        cooldown += Time.deltaTime;

        if (cooldown >= timeBetweenMoves)
        {
            canMove = true;
            cooldown = 0f;
        }
        else
        {
            canMove = false;
        }
    }

    private void WallAvoiding()
    {
        LayerMask layerMaskWall = LayerMask.GetMask("Wall");
        Vector2 origine = new Vector2(transform.position.x, transform.position.y);
        Vector2 viseDroit = new Vector2(direction.x * 0.75f, direction.y * 0.75f);

        Debug.DrawRay(origine, viseDroit, Color.green);
        //Debug.DrawRay(transform.position, new Vector2((direction.x + direction.x) * 0.75f, (direction.y - direction.y) * 0.75f), Color.blue);
        //Debug.DrawRay(transform.position, new Vector2(direction.x * 0.75f, direction.y * 0.75f), Color.red);

        //RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, layerMaskWall);
    }

    private void PathFinding()
    {
        Vector2 distance = new Vector2(target.position.x - this.transform.position.x, target.position.y - this.transform.position.y);
        Vector2 distanceNorm = new Vector2(Mathf.Sign(distance.x), Mathf.Sign(distance.y));

        //Debug.Log("x = " + distance.x + " y = " + distance.y);
        //Debug.Log("x = " + distanceNorm.x + " y = " + distanceNorm.y);
        
        // if you need to go vertically but you're not currently doing it then go !
        if(Mathf.Abs(distance.x) < Mathf.Abs(distance.y) && !movingVertically)
        {
            //Debug.Log("I go Vertically along " + distanceNorm.y);
            direction = new Vector2(0f, distanceNorm.y);
            MovingVertically();
        }
        // if you need to go vertically but you're are currently doing it move horizontally instead in the player direction
        else if (Mathf.Abs(distance.x) < Mathf.Abs(distance.y) && movingVertically)
        {
            //Debug.Log("I go Horizontally along " + distanceNorm.x);
            direction = new Vector2(distanceNorm.x, 0f);
            MovingHorizontally();
        }
        // if you need to go horizontally but you're not currently doing it then go !
        else if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y) && !movingHorizontally)
        {
            //Debug.Log("I go Horizontally along " + distanceNorm.x);
            direction = new Vector2(distanceNorm.x, 0f);
            MovingHorizontally();
        }
        // if you need to go horizontally but you're are currently doing it move vertically instead in the player direction
        else if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y) && movingHorizontally)
        {
            //Debug.Log("I go Vertically along " + distanceNorm.y);
            direction = new Vector2(0f, distanceNorm.y);
            MovingVertically();
        }
    }
}