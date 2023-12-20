using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverBodyAttack : MonoBehaviour
{
    private SpriteRenderer otherBodySprite;
    private Animator animator;
    public int damage = 1;
    Rigidbody2D rigidbody2D;
    
    void Start() {
        otherBodySprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void itsSwingingTime(float x, float y) {     
        otherBodySprite.enabled = true;  
        
        animator.SetFloat("lookX", x);
        animator.SetFloat("lookY", y);
        animator.SetTrigger("swing");
    }

    private void onFinish() {
        //otherBodySprite.enabled = false;
    }


}
