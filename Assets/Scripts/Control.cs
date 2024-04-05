using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class Control : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1.0f;
    public GameObject tailPrefab;
    public GameObject firePrefab;
    public int DieCount = 100;

    private GameObject fire;
    private List<GameObject> tail = new List<GameObject>();
    private Vector2 moveDirection;
    private Vector2 preDirection;
    private Vector2 prepreDirection;
    Animator animator;

    int[,] rotationMatrix = new int[2, 2] { { 0,0},{ 0,0} };

    public float dropSpeed = 3.0f;
    private Vector2 dropTargetDirection;
    private bool isDrop = false;

    public bool eatPepper = false;
    private Vector2 flyDirection = Vector2.zero;
    public float flySpeed = 10.0f;

    private float pixelOffet = 0.031f;

    private GameObject cave;

    public GameObject canvas;
    private bool isPause = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        preDirection = Vector2.right;
        prepreDirection = Vector2.right;
        addTail();
        cave = GameObject.FindWithTag("Cave");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pause");
            if(isPause)
            {
                Time.timeScale = 1;
                isPause = false;
                canvas.SetActive(false);
                canvas.GetComponent<UIControl>().OpenPauseMenu();
            }
            else
            {
                Time.timeScale = 0;
                isPause = true;
                canvas.SetActive(true);
            }
        }
        if(isPause)
        {
            return;
        }
        if (isDrop)
        {
            transform.Translate(Vector2.down * dropSpeed * Time.deltaTime);
            if (transform.position.y < dropTargetDirection.y)
            {
                Destroy(gameObject);
            }
            --DieCount;
            if(DieCount == 0)
            {
                Destroy(gameObject);
            }
        }
        else if(eatPepper)
        {
            flyFlyFly(flyDirection);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                moveDirection = Vector2.right;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                moveDirection = Vector2.left;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                moveDirection = Vector2.down;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                moveDirection = Vector2.up;
            }
            if (moveDirection != Vector2.zero && moveDirection != preDirection * -1)
            {
                if (!IsCollided(moveDirection))
                {
                    SnakeMove(moveDirection);
                }
            }
            moveDirection = Vector2.zero;
        }
        
    }
    private void addTail()
    {
        if (tail.Count == 0)
        {
            tail.Add(Instantiate(tailPrefab, transform.position - new Vector3(preDirection.x, preDirection.y+ pixelOffet, 0), Quaternion.identity));
            tail.First().GetComponent<TailControl>().setDirection(prepreDirection);
            tail.First().GetComponent<TailControl>().setNextDirection(preDirection);
            tail.First().GetComponent<TailControl>().IsTail(true);
            tail.First().GetComponent<TailControl>().setAnimator();
        }
        else
        {
            GameObject tmp = tail.Last();
            tail.Add(Instantiate(tailPrefab, tail[tail.Count - 1].GetComponent<TailControl>().calNewTail(), Quaternion.identity));
            tmp.GetComponent<TailControl>().tailNext = tail.Last();
            tail.Last().GetComponent<TailControl>().IsTail(true);
            tmp.GetComponent<TailControl>().IsTail(false);
            tmp.GetComponent<TailControl>().setAnimator();
            tmp.GetComponent<TailControl>().setTailNextDirection();
        }
    }
    private void SnakeMove(Vector2 direction)
    {
        setAnimator(direction);
        if (tail.Count > 0)
        {
            tail.First().GetComponent<TailControl>().setDirection(preDirection);
            tail.First().GetComponent<TailControl>().setNextDirection(direction);
            tail.First().GetComponent<TailControl>().moveTail();
        }
        prepreDirection = preDirection;
        preDirection = direction;
        
        transform.Translate(direction * speed);
        DropOrNot();
        align();
    }

    private void DropOrNot()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Empty"));
        //Debug.Log(hit.collider);
        // 如果射线检测到地面，则返回 true
        if (hit.collider == null)
        {
            return;
        }
        bool isDrop = true;
        if (tail.Count > 0)
        {
            foreach(var i in tail)
            {
                if (i.GetComponent<TailControl>().isOnGround())
                {
                    isDrop = false;
                }
            }

        }
        if(isDrop)
        {
            this.isDrop = true;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = SortingLayer.GetLayerValueFromName("DyingPlayer");
            
            foreach (var i in tail)
            {
                i.GetComponent<TailControl>().Drop();
            }
        }
    }

    private bool IsCollided(Vector2 direction)
    {
        RaycastHit2D hitObjects = Physics2D.Raycast(transform.position, direction,1.0f,LayerMask.GetMask("Obstacle"));
        if(hitObjects.collider == null)
        {
            return false;
        }
        //Debug.Log(hitObjects.collider.tag);
        if (hitObjects.collider.tag == "Stone")
        {
            return true;
        }
        if(hitObjects.collider.tag == "food")
        {
            if(cave == null)
            {
                cave = GameObject.FindWithTag("Cave");
            }
            if(!hitObjects.collider.GetComponent<foodControl>().canMove(direction))
            {
                if(hitObjects.collider.GetComponent<foodControl>().isPepper)
                {
                    eatPepper = true;
                    SnakeMove(direction);
                    fire = Instantiate(firePrefab, transform.position + new Vector3(direction.x, direction.y, 0), Quaternion.identity);
                    fire.transform.parent = transform;
                    if(direction == Vector2.up)
                    {
                        fire.transform.Rotate(0, 0, -90);
                        rotationMatrix = new int[2, 2] { { 0, -1 }, { 1, 0 } };
                    }
                    else if(direction == Vector2.down)
                    {
                        fire.transform.Rotate(0, 0, 90);
                        rotationMatrix = new int[2, 2] { { 0, 1 }, { -1, 0 } };
                    }
                    else if(direction == Vector2.left)
                    {
                        fire.transform.Rotate(0, 0, 180);
                        rotationMatrix = new int[2, 2] { { -1, 0 }, { 0, -1 } };
                    }
                    //fire.transform.Translate(matrixMultiplication(rotationMatrix,Vector2.up));
                    Destroy(hitObjects.collider.gameObject);
                    flyDirection = -direction;
                    cave.GetComponent<caveControl>().minusFoodCount();
                    return true;
                }
                else if(hitObjects.collider.GetComponent<foodControl>().isIce)
                {
                    return true;
                }
                else
                {
                    Destroy(hitObjects.collider.gameObject);
                    SnakeMove(direction);
                    addTail();
                    cave.GetComponent<caveControl>().minusFoodCount();
                    return true;
                }
            }
            else
            {
                hitObjects.collider.GetComponent<foodControl>().move(direction);
            }
        }
        return false;
    }
    private void setAnimator(Vector2 direction)
    {
        if(direction == Vector2.up)
        {
            animator.SetBool("Up", true);
            animator.SetBool("Down", false);
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
        }
        else if(direction == Vector2.down)
        {
            animator.SetBool("Up", false);
            animator.SetBool("Down", true);
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
        }
        else if(direction == Vector2.left)
        {
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
            animator.SetBool("Left", true);
            animator.SetBool("Right", false);
        }
        else if(direction == Vector2.right)
        {
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
            animator.SetBool("Left", false);
            animator.SetBool("Right", true);
        }
    }

    private void flyFlyFly(Vector2 direction)
    {
        //头飞
        transform.Translate(direction * flySpeed*Time.deltaTime);
        //身子飞
        foreach(var i in tail)
        {
            if(!i.GetComponent<TailControl>().IsCollided(direction))
            {
                i.transform.Translate(direction * flySpeed * Time.deltaTime);
            }
            else
            {
                //飞不动说明碰到了障碍物，那么就停止飞行
                eatPepper = false;
                //场景中所有的食物设置isflying为false
                GameObject[] foods = GameObject.FindGameObjectsWithTag("food");
                foreach(var j in foods)
                {
                    j.GetComponent<foodControl>().isFlying = false;
                }
                Destroy(fire,0.1f);
                align();
            }
        }
        //火焰飞
        //fire.transform.Translate(matrixMultiplication(rotationMatrix, direction * flySpeed * Time.deltaTime));
        //Debug.Log(matrixMultiplication(rotationMatrix, direction * flySpeed));
    }   

    Vector2 matrixMultiplication(int[,] a, Vector2 b)
    {
        return new Vector2(a[0, 0] * b.x + a[0, 1] * b.y, a[1, 0] * b.x + a[1, 1] * b.y);
    }

    private void align()
    {
        float x = transform.position.x + 0.5f;
        float y = transform.position.y + 0.5f;
        transform.position = new Vector3(Mathf.Round(x)-0.5f, Mathf.Round(y)-0.5f, 0);
        foreach(var i in tail)
        {
            x = i.transform.position.x + 0.5f;
            y = i.transform.position.y + 0.5f;
            i.transform.position = new Vector3(Mathf.Round(x) - 0.5f, Mathf.Round(y) - 0.5f - pixelOffet, 0);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Tail")
        {
            foreach(var i in tail)
            {
                Destroy(i);
            }
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        canvas.SetActive(true);
        canvas.GetComponent<UIControl>().OpenFailMenu();
        for(int i = 0; i < tail.Count; ++i)
        {
            Destroy(tail[i]);
        }
    }

    public void OpenSuccessMenu()
    {
        canvas.SetActive(true);
        canvas.GetComponent<UIControl>().OpenSuccessMenu();
    }
}
