using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;

    public LayerMask blockingLayer;


    public BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private float inverseMoveTime;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1 / moveTime;
    }

    protected bool Move (int dirX, int dirY, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(dirX, dirY);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if(hit.transform==null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;

        }
        return false;
    }
   protected IEnumerator SmoothMovement (Vector3 end)
    {
        float sqRemainigDistance = (transform.position - end).sqrMagnitude;
        while(sqRemainigDistance > float.Epsilon)
        {
            Vector3 newPos = Vector3.MoveTowards(rb.position, end, inverseMoveTime * Time.deltaTime);
            rb.MovePosition(newPos);
            sqRemainigDistance = (transform.position - end).sqrMagnitude;
            yield return new WaitForEndOfFrame();
        }
    }
    protected abstract void OnCantMove<T>(T component) where T : Component;
    protected virtual void AttemptMove <T> (int dirX, int dirY) where T: Component
    {
        RaycastHit2D hit;
        bool canMove = Move(dirX, dirY, out hit);
        if (hit.transform == null) return;

        T hitComponent = hit.transform.GetComponent<T>();
        if(!canMove && hitComponent!= null)
        {
            OnCantMove(hitComponent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
