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

    private float signX;
    private float signY;

    protected override void Start()
    {
        PickATarget();
        FocusTarget();
        base.Start();
    }

    protected override void Update()
    {
        FocusTarget();
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

        Vector2 distance = new Vector2(target.position.x - this.transform.position.x, target.position.y - this.transform.position.y);
        Vector2 distanceNorm = new Vector2(Mathf.Sign(distance.x), Mathf.Sign(distance.y));

        signX = Mathf.Sign(distanceNorm.x);
        signY = Mathf.Sign(distanceNorm.y);
    }

    protected override void UpdateDirection()
    {
        if (canMove) WallAvoiding();
    }

    /*protected override void UpdateDirection()
    {
        if (canMove) PathFinding();
    }*/

    private void CoolDownGestion()
    {
        cooldown += Time.deltaTime;

        if (cooldown >= timeBetweenMoves)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }
    }

    private void WallAvoiding()
    {
        LayerMask layerMaskWall = LayerMask.GetMask("Wall");
        float offset = 0.5f;

        Vector2 origin = new Vector2(transform.position.x + offset * direction.x, transform.position.y + offset * direction.y);
        Vector2 straight = new Vector2(direction.x * offset, direction.y * offset);
        Vector2 leftSide = new Vector2(-direction.y * offset, direction.x * offset);
        Vector2 rightSide = new Vector2(direction.y * offset, -direction.x * offset);

        //Vector2 originLeft = new Vector2(transform.position.x + offset * leftSide.x, transform.position.y + offset * leftSide.y);
        //Vector2 originRight = new Vector2(transform.position.x + offset * rightSide.x, transform.position.y + offset * rightSide.y);

        // this is for debug and vizualisation purposes
        /*Debug.DrawRay(origin, straight, Color.green);
        Debug.DrawRay(origin, leftSide, Color.blue);
        Debug.DrawRay(origin, rightSide, Color.red);*/

        RaycastHit2D hitStraight = Physics2D.Raycast(origin, straight, offset, layerMaskWall);
        RaycastHit2D hitLeft = Physics2D.Raycast(origin, leftSide, offset, layerMaskWall);
        RaycastHit2D hitRight = Physics2D.Raycast(origin, rightSide, offset, layerMaskWall);

        if (hitStraight.collider != null)
        {
            //Debug.Log("Wall straight !");
            if(hitLeft.collider != null)
            {
                Debug.Log("I go right !");
                direction = rightSide;
            }
            else if (hitRight.collider != null)
            {
                Debug.Log("I go left !");
                direction = leftSide;
            }

            // tell where you're going
            if (direction.x == 0)
            {
                MovingVertically();
            }
            else
            {
                MovingHorizontally();
            }
            cooldown = 0f;
        }
        else
        {
            PathFinding();
        }
    }

    private void PathFinding()
    {
        Vector2 distance = new Vector2(target.position.x - this.transform.position.x, target.position.y - this.transform.position.y);
        Vector2 distanceNorm = new Vector2(Mathf.Sign(distance.x), Mathf.Sign(distance.y));

        //Debug.Log("x = " + distance.x + " y = " + distance.y);
        //Debug.Log("x = " + distanceNorm.x + " y = " + distanceNorm.y);

        if (signX == distanceNorm.x && signY == distanceNorm.y) return;
        
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

        signX = distanceNorm.x;
        signY = distanceNorm.y;

        cooldown = 0f;
    }
}