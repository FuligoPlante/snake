using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caveControl : MonoBehaviour
{ 
    public int foodCount = 0;
    public bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setFoodCount(int count)
    {
        foodCount = count;
    }
    public void minusFoodCount()
    {
        foodCount--;
        if(foodCount == 0)
        {
            openCave();
        }
    }

    public void openCave()
    {
        isOpen = true;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("graphics/1x/sprite-19-1");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(isOpen)
            {
                collision.GetComponent<Control>().OpenSuccessMenu();
            }
        }
    }
}
