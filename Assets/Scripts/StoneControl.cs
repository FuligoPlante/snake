using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class StoneControl : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isWood = false;
    void Start()
    {
        if(isWood)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("graphics/1x/sprite-21-3");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Fire")
        {
            Destroy(gameObject);
        }
    }
}
