using System;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    [SerializeField]private float walkTime;
    [SerializeField]private float idleTime;
    [SerializeField]private float redTime;
    [SerializeField]private AudioClip enemyDmg;

    private int strength = 3;
    private float elapsedWalkTime;
    private float elapsedRedTime;
    private int direction = 1;
    private bool isWalking = false;
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audio;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        animator.SetInteger("Direction",direction);
    }

    //enemy walking back and forth
    void Update()
    {
        if(isWalking && elapsedWalkTime < walkTime)
        {
            Vector2 position = rb.transform.position;
            position.x += direction * Time.deltaTime * 2;
            rb.transform.position = position;
            animator.SetInteger("Direction", direction);
        }
        else if(!isWalking && elapsedWalkTime > idleTime)
        {
            isWalking = true;
            elapsedWalkTime = 0;
            direction *= -1;
            animator.SetInteger("Direction", direction);
        }
        else if(isWalking && elapsedWalkTime > walkTime)
        {
            isWalking = false;
            elapsedWalkTime = 0;
            animator.SetInteger("Direction", direction);
        }
        //enemy turning red when taking damage
        if(elapsedRedTime > redTime)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        }

        elapsedRedTime += Time.deltaTime;
        elapsedWalkTime += Time.deltaTime;
    }

    //if projectile hits enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {  
        if(collision.gameObject.GetComponent<Projectile>() != null)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(255, 1, 1, 1f);
            elapsedRedTime = 0.0f;
            audio.PlayOneShot(enemyDmg);
            rb.linearVelocity = new Vector2(0,0);
            hit();
        }
    }

    //times player has to hit the enemy
    private void hit()
    {
        strength--;
        if(strength <= 0)
        {
            Destroy(this.gameObject);
            return;
        }
    }

}
