using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailControl : MonoBehaviour
{
    public GameObject tailNext;
    private float speed = 1.0f;
    private Vector2 direction;
    private Vector2 preDirection;
    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.zero;
        preDirection = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setDirection(Vector2 dir)
    {
        direction = dir;
    }
    public Vector2 getDirection()
    {
        return direction;
    }
    public Vector3 calNewTail()
    {
        return transform.position - new Vector3(preDirection.x, preDirection.y, 0);
    }
    public void moveTail()
    {
        transform.Translate(direction * speed);
        if(tailNext != null)
        {
            tailNext.GetComponent<TailControl>().setDirection(preDirection);
            tailNext.GetComponent<TailControl>().moveTail();
        }
        preDirection = direction;
    }

}
