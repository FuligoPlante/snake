using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    public float speed = 10.0f;
    public float UpSpeed = 10.0f;
    public int bodyLength;
    public GameObject tailPrefab;
    public List<GameObject> tailList = new List<GameObject>();
    Animator animator;
    Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 preDirection;
    private bool isSpicy;

    Vector2[,] dirMap = new Vector2[12, 2] { { Vector2.right, Vector2.right }, { Vector2.left, Vector2.left }, { Vector2.up, Vector2.up }, { Vector2.down, Vector2.down },
                                            { Vector2.right, Vector2.down }, { Vector2.up, Vector2.left }, { Vector2.left, Vector2.down }, { Vector2.up, Vector2.right },
                                            { Vector2.right, Vector2.up }, { Vector2.down, Vector2.left }, { Vector2.left, Vector2.up }, { Vector2.down, Vector2.right }};
    string[] turnStates = new string[6] { "Horizontal", "Vertical", "DownLeft", "DownRight", "UpLeft", "UpRight" };
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i <= bodyLength; i++)
        {
            Vector3 bodyPos = new Vector3(transform.position.x - i * 1f, transform.position.y, transform.position.z);
            var newBody = Instantiate(tailPrefab, bodyPos, Quaternion.identity);
            tailList.Add(newBody);
        }
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        preDirection = new Vector2(1f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpicy)
        {
            //GoBackward(preDirection);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                moveDirection = Vector2.right;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                moveDirection = Vector2.left;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                moveDirection = Vector2.up;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                moveDirection = Vector2.down;
            }
            if (moveDirection != Vector2.zero && moveDirection != preDirection * -1)
            {
                SnakeMove(moveDirection);
            }

            moveDirection = Vector2.zero;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "food")
        {
            Destroy(collision.gameObject);
            if(tailList.Count == 0)
            {
                tailList.Add(Instantiate(tailPrefab, tailPrefab.transform.position, Quaternion.identity));
            }
            else
            {
                tailList.Add(Instantiate(tailPrefab, tailList[tailList.Count - 1].transform.position, Quaternion.identity));
            }
        }
        else if(collision.tag == "tail")
        {
            Destroy(gameObject);
        }
    }

    private void SnakeMove(Vector2 direction)
    {
        if (direction == Vector2.right)
        {
            animator.Play("Right");
        }
        else if (direction == Vector2.left)
        {
            animator.Play("Left");
        }
        else if (direction == Vector2.up)
        {
            animator.Play("Up");
        }
        else if (direction == Vector2.down)
        {
            animator.Play("Down");
        }
        transform.Translate(direction * speed);
        //for (int i = 0; i < tailList.Count; i++)
        //{
        //    Vector3 temp = tailList[i].transform.position;
        //    tailList[i].transform.position = transform.position;
        //    transform.position = temp;
        //}
        preDirection = direction;
    }
}
