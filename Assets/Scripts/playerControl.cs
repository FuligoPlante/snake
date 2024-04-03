using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    public float speed = 10.0f;
    public float UpSpeed = 10.0f;
    public GameObject tailPrefab;
    private List<Transform> tail = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down* UpSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * UpSpeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "food")
        {
            Destroy(collision.gameObject);
            if(tail.Count == 0)
            {
                tail.Add(Instantiate(tailPrefab, tailPrefab.transform.position, Quaternion.identity).transform);
            }
            else
            {
                tail.Add(Instantiate(tailPrefab, tail[tail.Count - 1].position, Quaternion.identity).transform);
            }
        }
        else if(collision.tag == "tail")
        {
            Destroy(gameObject);
        }
    }
}
