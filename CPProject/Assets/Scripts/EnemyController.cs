using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    public Transform attackPos;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;
   public  float distance;
   public  float height;
    float attackRange = 1f;
    float range = 5f;
    [SerializeField] float speed = 0.4f;
    public int hp;
    public int damage;
    public LayerMask blockingLayer;
    private float damageRange = 0.3f;

    public bool hasChip;
    [SerializeField] GameObject chipPrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        hp = 40;
        damage = 15;
      //  hasChip = false;
    }   

    // Update is called once per frame
    void Update()
    {
        distance = target.position.x - transform.position.x;
        height= target.position.y - transform.position.y;
        if (Mathf.Abs(distance) < range && Mathf.Abs(height) < 1 )
        {
            anim.SetBool("isMoving", true);
            if (Mathf.Abs(distance) < attackRange)
            { 
                anim.SetTrigger("enemyAttack");
                rb.velocity = new Vector2(0, 0);
            }
           // Debug.Log("Chase");
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
    public void CheckDeath()
    {
        if (hp <= 0)
        {
            Death();
        }
    }
    public void Death()
    {
        Destroy(gameObject);
        if (hasChip)
            Instantiate(chipPrefab, transform.position, Quaternion.identity);
    }
    public void ApplyDamage()
    {
        Debug.Log("Applydamage");
        //detect enemy
        attackPos.position = transform.position;
        attackPos.position += new Vector3(distance, 0, 0);
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPos.position, damageRange);

        //apply dmg to enemies and objects

        foreach (Collider2D obj in hitObjects)
        {
            if (obj.tag == "Player")
            {
                Debug.Log("damage playe");
                obj.GetComponent<Player>().LoseHp(damage);
            }
           

        }
    }
    public void AssignChip()
    {
       // Debug.Log("assign chip");
        hasChip = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPos.position, damageRange);
    }
}
