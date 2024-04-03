using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TailControl : MonoBehaviour
{
    public GameObject tailNext;
    public bool isTail = false;

    private float speed = 1.0f;
    private Vector2 direction;
    private Vector2 preDirection;
    private Vector2 nextDirection;
    
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.zero;
        preDirection = Vector2.zero;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setDirection(Vector2 dir)
    {
        direction = dir;
    }
    public void setNextDirection(Vector2 dir)
    {
        nextDirection = dir;
    }
    public Vector2 getDirection()
    {
        return direction;
    }
    public void setTailNextDirection()
    {
        if(tailNext != null)
        {
            tailNext.GetComponent<TailControl>().setDirection(preDirection);
            tailNext.GetComponent<TailControl>().setNextDirection(direction);
            tailNext.GetComponent<TailControl>().setAnimator();
        }
    }
    public Vector3 calNewTail()
    {
        return transform.position - new Vector3(preDirection.x, preDirection.y, 0);
    }
    public void moveTail()
    {
        transform.Translate(direction * speed);
        if (tailNext != null)
        {
            tailNext.GetComponent<TailControl>().setDirection(preDirection);
            tailNext.GetComponent<TailControl>().setNextDirection(direction);
            tailNext.GetComponent<TailControl>().moveTail();
        }
        setAnimator();
        preDirection = direction;
    }
    
    public void setAnimator()
    {
        //如果是尾巴，直接设置尾巴动画
        if(isTail)
        {
            setTailAnimator();
            return;
        }
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if ((nextDirection == Vector2.left && direction == Vector2.left)||
            (nextDirection == Vector2.right && direction == Vector2.right))
        {
            animator.SetBool("Horizontal", true);
            animator.SetBool("Vertical", false);
            animator.SetBool("IsTurn", false);
        }
        else if((nextDirection == Vector2.up && direction == Vector2.up) ||
            (nextDirection == Vector2.down && direction == Vector2.down))
        {
            animator.SetBool("Vertical", true);
            animator.SetBool("Horizontal", false);
            animator.SetBool("IsTurn", false);
        }
        else
        {
            animator.SetBool("IsTurn", true);
            animator.SetBool("Horizontal", false);
            animator.SetBool("Vertical", false);
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            //通过判断方向来确定旋转角度
            if (direction == Vector2.left && nextDirection == Vector2.up)
            {
                sr.flipX = true;
                sr.flipY = true;
            }
            else if(direction == Vector2.left && nextDirection == Vector2.down)
            {
                sr.flipX = true;
                sr.flipY = false;
            }
            else if(direction == Vector2.right && nextDirection == Vector2.up)
            {
                sr.flipX = false;
                sr.flipY = true;
            }
            else if(direction == Vector2.right && nextDirection == Vector2.down)
            {
                sr.flipX = false;
                sr.flipY = false;
            }
            else if(direction == Vector2.up && nextDirection == Vector2.left)
            {
                sr.flipX = false;
                sr.flipY = false;
            }
            else if(direction == Vector2.up && nextDirection == Vector2.right)
            {
                sr.flipX = true;
                sr.flipY = false;
            }
            else if(direction == Vector2.down && nextDirection == Vector2.left)
            {
                sr.flipX = false;
                sr.flipY = true;
            }
            else if(direction == Vector2.down && nextDirection == Vector2.right)
            {
                sr.flipX = true;
                sr.flipY = true;
            }

        }
    }
    public void setTailAnimator()
    {
        
        if (nextDirection == Vector2.up)
        {
            animator.SetBool("Up", true);
            animator.SetBool("Down", false);
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
        }
        else if (nextDirection == Vector2.down)
        {
            animator.SetBool("Up", false);
            animator.SetBool("Down", true);
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
        }
        else if (nextDirection == Vector2.left)
        {
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
            animator.SetBool("Left", true);
            animator.SetBool("Right", false);
        }
        else if (nextDirection == Vector2.right)
        {
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
            animator.SetBool("Left", false);
            animator.SetBool("Right", true);
        }
    }
    public void IsTail(bool inbool)
    {
        isTail = inbool;
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animator.SetBool("IsTail", isTail);
    }
}
