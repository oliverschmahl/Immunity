using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*THIS SCRIPT CAN BE APPLIED TO ANY GAMEOBJECT AND WILL ENSURE THAT IT DOES NOT LEAVE THE MAINCAMERAS GAME VIEW*/
public class boundaries : MonoBehaviour
{
    private Vector2 screenBounds;

    private float objectWidth;
    private float objectHeight;
    
    // Start is called before the first frame update
    void Start()
    {
        /*
         * screenBounds: Finds the boundry of the main camera view for the x and y axis
         * objectWidth: Finds the width of a sprite in pixels and divides it by 2 to find half the width
         * objectHeight Finds the height of a sprite in pixels and divides it by 2 to find half the height
         */
        screenBounds =
            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 
                                                                    Screen.height, 
                                                                    Camera.main.transform.position.z));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    // Update is called once per frame
    void LateUpdate() {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x + objectWidth, screenBounds.x * -1 - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y + objectHeight, screenBounds.y * -1 - objectHeight);
        transform.position = viewPos;
    }
}
