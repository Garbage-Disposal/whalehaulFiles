using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]private Player player;

    public int direction = 1;
    private float height= 1;
    private float timePassed = 0;


    //moving up and down

    void Update()
    {
        Vector2 position =  transform.position;
        position.y += (height * Time.deltaTime * direction) * 0.5f;
        transform.position = position; 

        timePassed += Time.deltaTime;
        if (timePassed > 0.3f)
        {
            timePassed = 0;
            direction *= -1;
        }
    }
    //being collected
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.AddFish();
            Destroy(this.gameObject);
        }
    }
}