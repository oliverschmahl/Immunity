using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray;


    // Start is called before the first frame update
    void Start()
    {
        // Get the SpriteRenderer of the GameObject
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Changes the sprite of the SpriteRenderer to the specified index in the Sprite array
    public void changeSprite(int index)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = spriteArray[index];
        }
    }
}
