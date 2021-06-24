using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;
   public  float distance;
    float attackRange = 0.2f;
    float range = 5f;
    [SerializeField] float speed = 0.4f;
    public int hp;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        hp = 40;
    }   

    // Update is called once per frame
    void Update()
    {
        distance = target.position.x - transform.position.x;

        if (Mathf.Abs(distance) < range)
        {
            anim.SetBool("isMoving", true);
            if (Mathf.Abs(distance) < attackRange)
            { 
                anim.SetTrigger("enemyAttack");
            }
            Debug.Log("Chase");
            rb.velocity = new Vector2(distance*speed, 0);
            
            if (distance < 0)
                sprite.flipX=true;
            else
                sprite.flipX=false;
                
           
        }
    }
     public void GetDamage(int damage)
    {
        //PUT DAMAGE
        hp -= damage;

        if (hp <= 0) anim.SetBool("isDead", true);
        anim.SetTrigger("hitted");
    }
    public void Death()
    {
        Destroy(gameObject);
    }
}
