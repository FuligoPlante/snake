using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class foodControl : MonoBehaviour
{
    public float speed = 10.0f;
    public bool isPepper = false;
    public bool isIce = false;
    public bool isFlying = false;
    private Vector2 targetDirection;
    private bool isDrop = false;

    // Start is called before the first frame update
    void Start()
    {
        if(isPepper)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("graphics/1x/sprite-15-0");
        }
        if(isIce)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("graphics/1x/sprite-21-4");
            spriteRenderer.sortingLayerName = "Stone";
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isDrop)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
            if(transform.position.y < targetDirection.y)
            {
                Destroy(gameObject);
            }
        }
    }
    public bool canMove(Vector2 direction)
    {
        if(isDrop)
        {
            return true;
        }
        RaycastHit2D hitObjects = Physics2D.Raycast(transform.position + new Vector3(direction.x/2,direction.y/2,0), direction, 0.5f, LayerMask.GetMask("Obstacle"));
        if (hitObjects.collider == null)
        {
            return true;
        }
        //Debug.Log(hitObjects.collider.tag);
        if (hitObjects.collider.tag == "Stone")
        {
            return false;
        }
        if (hitObjects.collider.tag == "food")
        {
            if(hitObjects.collider.GetComponent<foodControl>().canMove(direction))
            {
                hitObjects.collider.GetComponent<foodControl>().move(direction);
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public bool canMove2(Vector2 direction)
    {
        if (isDrop)
        {
            return true;
        }
        RaycastHit2D hitObjects = Physics2D.Raycast(transform.position + new Vector3(direction.x / 2, direction.y / 2, 0), direction, 0.01f, LayerMask.GetMask("Obstacle"));
        if (hitObjects.collider == null)
        {
            return true;
        }
        //Debug.Log(hitObjects.collider.tag);
        if (hitObjects.collider.tag == "Stone")
        {
            return false;
        }
        if (hitObjects.collider.tag == "food")
        {
            if (hitObjects.collider.GetComponent<foodControl>().canMove2(direction))
            {
                hitObjects.collider.GetComponent<foodControl>().move(direction * Time.deltaTime * 10.0f);
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    public void move(Vector2 direction)
    {
        transform.Translate(direction.x,direction.y,0);
        if(!isFlying)
        {
            isDropping();
        }
    }

    private void isDropping()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Empty"));
        //Debug.Log(hit.collider);
        // 如果射线检测到地面，则返回 true
        if (hit.collider != null)
        {
            targetDirection.y = transform.position.y - 0.5f;
            isDrop = true;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = SortingLayer.GetLayerValueFromName("DyingPlayer");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isIce)
        {
            if (collision.tag == "Fire")
            {
                Destroy(gameObject);
            }
        }
    }
}
