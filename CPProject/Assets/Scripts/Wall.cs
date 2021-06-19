using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public int hp = 4;


    // public Sprite dmgSprite;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    public void DamageWall (int loss)
    {
        //visual feedback that u hit the wall, it becomes redish
        spriteRenderer.color = new Color(1, 0, 0, 0.4f);
        hp -= loss;
        if (hp < 0) Destroy(gameObject);

    }
}
