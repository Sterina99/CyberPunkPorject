using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject
{
    public int wallDamage = 1;
    public int enemyDamage = 20;
    public int pointPerChip = 10;
    public int numberOfChips = 0;
    public float restartLevelDelay = 1f;
    float horizontal = 0;
    float vertical = 0;
    Transform attackPos;
    [SerializeField] float attackRange = 1f;
    private int hp = 20;
    private Animator anim;
    // Start is called before the first frame update
    protected override void Start()
    {
        anim = GetComponent<Animator>();

        numberOfChips = GameManager.instance.chipInstalled;
        base.Start();
        attackPos = GetComponentInChildren<Transform>();
    }


    private void OnDisable()
    {
        GameManager.instance.chipInstalled = numberOfChips;
    }
    // Update is called once per frame
    void Update()
    {
        

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (horizontal != 0)
            vertical = 0;
        if (horizontal != 0 || vertical != 0)
        {
            horizontal *= 1.8f;
            vertical *= 1.8f;

            //          transform.rotation = Quaternion.LookRotation(new Vector3(horizontal*2, 0,0));
            if(horizontal<0)
            transform.rotation = Quaternion.AngleAxis(180*(int)horizontal,Vector3.up);         
      //      Debug.Log((int)horizontal);
        //    Debug.Log((int)vertical);
            AttemptMove<Wall>((int)horizontal, (int)vertical);
        }
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("mcAttack");
            
            
        }
    }
    
    private void CheckIfGameOver()
    {
      
    }
    public void LoseHp(int damage)
    {
        hp -= damage;
    }
    protected override void OnCantMove<T>(T component)
    {
      /*  Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        anim.SetTrigger("mcAttack"); */
    }
    protected override void AttemptMove<T>(int dirX, int dirY)
    {
        base.AttemptMove<T>(dirX, dirY);
        RaycastHit2D hit;

        
    }
    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (collision.tag == "Chip")
        {
            numberOfChips++;
            collision.gameObject.SetActive(false);
        }
    }

    public void EnableCombo()
    {
       
        anim.SetBool("canCombo",true);
    }
    public void FinalCombo()
    {
        anim.SetTrigger("finalCombo");
    }

    public void ApplyDamage()
    {
        Debug.Log("Applydamage");
        //detect enemy
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPos.position, attackRange, blockingLayer);

        //apply dmg to enemies and objects

        foreach (Collider2D obj in hitObjects)
        {
            if (obj.tag == "Enemy")
            {
                obj.GetComponent<Enemy>().GetDamage(enemyDamage);
            }if (obj.tag == "Wall")
            {
                obj.GetComponent<Wall>().DamageWall(wallDamage);
            }
           
        }
    }
}
