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
    private List<GameObject> tail = new List<GameObject>();
    private Vector2 moveDirection;
    private Vector2 preDirection;
    private Vector2 prepreDirection;
    Animator animator;
    Vector2[,] dirMap = new Vector2[12, 2] { { Vector2.right, Vector2.right }, { Vector2.left, Vector2.left }, { Vector2.up, Vector2.up }, { Vector2.down, Vector2.down },
                                            { Vector2.right, Vector2.down }, { Vector2.up, Vector2.left }, { Vector2.left, Vector2.down }, { Vector2.up, Vector2.right },
                                            { Vector2.right, Vector2.up }, { Vector2.down, Vector2.left }, { Vector2.left, Vector2.up }, { Vector2.down, Vector2.right }};
    string[] turnStates = new string[6] { "Horizontal", "Vertical", "DownLeft", "DownRight", "UpLeft", "UpRight" };
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        preDirection = Vector2.right;
        prepreDirection = Vector2.right;
        addTail();
    }

    // Update is called once per frame
    void Update()
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "food")
        {
            Destroy(collision.gameObject);
            addTail();
        }
        else if (collision.tag == "tail")
        {
            Destroy(gameObject);
        }
    }
    private void addTail()
    {
        if (tail.Count == 0)
        {
            tail.Add(Instantiate(tailPrefab, transform.position - new Vector3(preDirection.x, preDirection.y, 0), Quaternion.identity));
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
}
