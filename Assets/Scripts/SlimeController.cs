using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class SlimeController : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;
	Rigidbody2D rigidbody2D;
    private bool disableDmg = false;
    Animator animator;
    private bool chasing = false;
    private GameObject target;
    private BoxCollider2D collider;
    private UnityEngine.AI.NavMeshAgent agent;
    public enum AIState { Chase, Idle, Dead }
    public AIState currentState = AIState.Idle;
    private LevelManager lvlManager;
    public float bounceForce;
    private bool bounce;
    private Collider2D collisionPoint;

    void Start() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        lvlManager = GameObject.Find("SceneManager").GetComponent<LevelManager>();
    }

    public void intruderDetected(Collider2D other) {
        
        GameObject intruder = other.gameObject;
        
        if ((intruder.tag == "Player" || intruder.tag == "Tower") && !disableDmg) {
            target = intruder;
            chasing = true;

            /*Rigidbody2D rbIntruder = target.GetComponent<Rigidbody2D>();
            Vector2 intruderPosition = rbIntruder.position;

            //currentState = AIState.Chase;
            //agent.SetDestination(intruderPosition);
            
            Vector2 direction = intruderPosition - rigidbody2D.position;

            rigidbody2D.MovePosition(rigidbody2D.position + direction * speed * Time.deltaTime);*/
        }
    }

    
    void OnTriggerEnter2D(Collider2D other) {
        bounce = true;
        collisionPoint = other;
    }

    public void applyBounce(Collider2D collisionPoints) {
        Vector2 collisionPos = collisionPoint.gameObject.transform.position;
        collisionPos = -collisionPos.normalized;
        Vector2 direction = collisionPos - rigidbody2D.position;

        rigidbody2D.AddForce(rigidbody2D.position + direction * bounceForce * Time.deltaTime);
    }

    public void intruderFled(Collider2D other) {
         GameObject intruder = other.gameObject;
        
        if (intruder.tag == "Player") {
            chasing = false;
        }
        
    }

    public void applyDamage(Collider2D other) { 
               
        if (other.gameObject.tag == "Player" && !disableDmg) {
            other.gameObject.GetComponent<PlayerController>().takeDamage(damage);
        }

        if (other.gameObject.tag == "Tower" && !disableDmg) {
            other.gameObject.GetComponent<Tower>().takeDamage(damage);
        }
    }

    void LateUpdate() {
        if (chasing) {
            //Rigidbody2D rbIntruder = target.GetComponent<Rigidbody2D>();
            Vector2 intruderPosition = target.transform.position;

            //currentState = AIState.Chase;
            //agent.SetDestination(intruderPosition);
            
            Vector2 direction = intruderPosition - rigidbody2D.position;
            
            if (bounce) {
                
                Vector2 collisionPos = collisionPoint.gameObject.transform.position;
                //collisionPos = -collisionPos.normalized;
                Vector2 direction2 = rigidbody2D.position - collisionPos;
                
                rigidbody2D.AddForce(direction2 * bounceForce);
                bounce = false;

            } else {
                rigidbody2D.MovePosition(rigidbody2D.position + direction * speed * Time.deltaTime);
            }

        }
    }

    public void kill() {
        animator.SetTrigger("isDying");
        disableDmg = true;
    }
    public void remove() {
        lvlManager.mobKilled();
        gameObject.SetActive(false);
    }

}
