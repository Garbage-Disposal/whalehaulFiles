using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
   public int direction = 1;
    private float height= 2;
    private float timePassed = 0;
    
    //moving up and down
    void Update()
    {
        Vector2 position =  transform.position;
        position.y += (height * Time.deltaTime * direction);
        transform.position = position; 

        timePassed += Time.deltaTime;
        if (timePassed > 2)
        {
            timePassed = 0;
            direction *= -1;
        }
    }
}
