using NUnit.Framework;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    
    [SerializeField] private GameObject projectile;
    [SerializeField] private UIManager ui;
    [SerializeField] private float JumpHeight = 2.4f;
    [SerializeField]private int lives = 3;
    [SerializeField]private AudioClip takeDmg;
    [SerializeField]private AudioClip jumpSound;
    [SerializeField]private AudioClip getFish;

    public float speed = 2;
    private int direction = 1;
    private int jumpCount = 0;
    private int score = 0;
    private Vector2 lastCheckpoint;
    private AudioSource audio;
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 move;

    //for invincibility
    private float invincibilityTime = 3.0f;
    private float invincibilityCount = 3.0f;
    public bool isInvin = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        lastCheckpoint = rb.transform.position;
    }

    void Update()
    {
        //move
        move = InputSystem.actions["Move"].ReadValue<Vector2>();
        if (move.x != 0)
        {
            direction = move.x < 0 ? -1 : 1;
        }

        //jump
        if (InputSystem.actions["Jump"].IsPressed() && jumpCount < 1)
        {
            InputSystem.actions["Jump"].Reset();
            rb.linearVelocity = new Vector2(0,0);
            rb.AddForce(new Vector2(0, Mathf.Sqrt(-2*Physics.gravity.y*JumpHeight)), 
            ForceMode2D.Impulse);
            audio.PlayOneShot(jumpSound);
            jumpCount++;
        }
        
        //attack
        if (InputSystem.actions["Attack"].IsPressed())
        {
            InputSystem.actions["Attack"].Reset();
            Fire();
        }
        
        //invincibility
        if (isInvin)
        {
            invincibilityCount += Time.deltaTime;
            if(invincibilityCount > invincibilityTime)
            {
                isInvin = false;
                invincibilityCount = 0;
                this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            }
        }

        //win condition
        if (score == 10)
        {
            ui.SetWin();
        }
    }

    void FixedUpdate()
    {
        //animation
        animator.SetInteger("Direction", direction);
        animator.SetFloat("Move", move.x);

        //movement
        Vector2 position = rb.position;
        position.x += move.x * speed * Time.fixedDeltaTime * 2;
        rb.position = position;
    }

        //jump
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpCount = 0;
        }
        //enemy hitting player
        else if(collision.gameObject.GetComponent<Enemy>() && !isInvin)
        {
            rb.linearVelocity = Vector2.zero;
            PlayerHit();
        }
        //falling out of the floor
        else if (collision.gameObject.tag == "death")
        {
            rb.position=lastCheckpoint;
            PlayerHit();
        }
    }
    //player taking damage
    private void PlayerHit()
    {
        lives--;
        ui.SetLives(lives);
        audio.PlayOneShot(takeDmg);
        if (lives == 0)
        {
            SceneManager.LoadScene("Whalehaul");
        }
        rb.transform.position = lastCheckpoint;
        isInvin = true;
        this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
    }
    //score
    public void AddFish()
    {
        score++;
        audio.PlayOneShot(getFish);
        ui.SetScore(score);
    }
    //projectile
    private void Fire()
    {
        GameObject sb = Instantiate(projectile, rb.position, Quaternion.identity);
        Projectile pro = sb.GetComponent<Projectile>();
        pro.Launch(new Vector2(direction, 0), 300);
    }
    
}