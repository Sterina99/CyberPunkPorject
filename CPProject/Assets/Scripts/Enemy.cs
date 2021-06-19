using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public int playerDamage;
    private Animator anim;
    private Transform target;
    public int hp = 20;
    // Start is called before the first frame update
    protected override void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
        InvokeRepeating("MoveEnemy", 1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void AttemptMove<T>(int dirX, int dirY)
    {
        
        base.AttemptMove<T>(dirX, dirY);
    }

    public void MoveEnemy()
    {
        int dirX = 0;
        int dirY = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < 0.1f)
        {
            dirY = target.position.y > transform.position.y ? 1 : -1;
            Debug.Log(dirY);

        }
        else
        {
            dirX = target.position.x > transform.position.x ? 1 : -1;
        }
        
        if(gameObject.name== "Guard(Clone)")
        {
            anim.SetFloat("vertical", dirY);
            anim.SetFloat("horizontal", dirX);


        }else
            anim.SetBool("isMoving", true);
        AttemptMove<Player>(dirX, dirY);
    }
    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        anim.SetTrigger("enemyAttack");
        hitPlayer.LoseHp(playerDamage);
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
