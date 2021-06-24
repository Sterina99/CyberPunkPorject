using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int pointPerChip = 10;
    public int numberOfChips = 0;
    public LayerMask blockingLayer;
    public LayerMask floorLayer;
    public float restartLevelDelay = 1f;

    private float moveSpeed;
    float horizontal = 0;
    float vertical = 0;
    [SerializeField] float jumpForce;
    [SerializeField] Transform groundCheck;
   public  bool isGrounded;

    [SerializeField] Transform attackPos;
    [SerializeField] float attackRange = .5f;
    private int hp = 20;
    public int wallDamage = 1;
    public int enemyDamage = 20;

    private Animator anim;
    public BoxCollider2D boxCollider;
    public Rigidbody2D rb;
    // Start is called before the first frame update
     void Start()
    {
        anim = GetComponent<Animator>();

        numberOfChips = GameManager.instance.chipInstalled;
    
       
        moveSpeed = 20f;
        jumpForce = 5f;
    }


    private void OnDisable()
    {
        GameManager.instance.chipInstalled = numberOfChips;
    }
    private void LateUpdate()
    {
      
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        horizontal = Input.GetAxis("Horizontal");


        if (Physics2D.Linecast(transform.position, groundCheck.position, 1<< LayerMask.NameToLayer("BlockingLayer")))
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
        }else
        {
            anim.SetBool("isGrounded", false);
           
            isGrounded = false;
        }
            




        if (horizontal != 0 )
        {
            horizontal *= 2f;
            if(isGrounded)
            anim.SetBool("isRunning", true);
            //          transform.rotation = Quaternion.LookRotation(new Vector3(horizontal*2, 0,0));
            if (horizontal < -0.1f)
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            else
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            }


            rb.velocity = new Vector2(horizontal, rb.velocity.y);
            //    Debug.Log((int)vertical);
            //  moveSpeed *= Time.deltaTime;        
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
         
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.Play("Jump");
       
        }

        if (Input.GetMouseButton(0))
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
        else if (collision.tag == "Trampoline")
        {
            jumpForce = 8f;
            
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Trampoline")
        {
            jumpForce =5f;

        }
    }
    //   private void OnCollisionEnter2D(Collision2D collision)
    //  {
    //      if (collision.gameObject.tag == "Floor") anim.SetBool("isGrounded", true);
    // }
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
                obj.GetComponent<EnemyController>().GetDamage(enemyDamage);
            }if (obj.tag == "Wall")
            {
                obj.GetComponent<Wall>().DamageWall(wallDamage);
            }
           
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
