using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> Fireballs;
    public GameObject fireBall;
    private bool detection = false;
    private GameObject target;
    Animator animator;
    private bool inAnimation = false;
    public float launchForce = 8.0f;
    public int health = 2;
    private bool cooldown = false;
    public LevelManager lvlManager;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        lvlManager = GameObject.Find("SceneManager").GetComponent<LevelManager>();
    }   

    // Update is called once per frame
    void Update()
    {
        if (detection && !inAnimation && !cooldown) {
            Fire();
        }

        if (health <= 0) {
            lvlManager.mobKilled();
            gameObject.SetActive(false);
        }
    }

    public void intruderDetected(Collider2D other) {
    GameObject intruder = other.gameObject;
        
        if (intruder.tag == "Player" || intruder.tag == "Tower") {
            detection = true;
            target = intruder;
        }
    }

    public void intruderFled(Collider2D other) {
          GameObject intruder = other.gameObject;
        
        if (intruder.tag == "Player") {
            detection = false;
        }   else if (intruder.tag == "Tower") {
            detection = false;
        } 
    }

    private void Fire() {   
        Vector3 rotation = Vector3.forward;
        if (target.transform.position.x > transform.position.x) {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        } else {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        GameObject FireballToLaunch = null;
        
        if (Fireballs.Count != 0) {
            foreach (GameObject fire in Fireballs) {
                Fireball script = fire.GetComponent<Fireball>();
                if (script.getHidden()) {
                    script.setHidden(false);
                    fire.transform.position = transform.parent.gameObject.transform.position;
                    FireballToLaunch = fire;
                    break;
                }
            }
        }
        
        if (Fireballs.Count == 0 || FireballToLaunch == null) {
            GameObject newFireball = Instantiate(fireBall, transform.parent.gameObject.transform.position, Quaternion.identity);
            Fireballs.Add(newFireball);
            FireballToLaunch = newFireball;
        } 

        if (FireballToLaunch != null) {
            Fireball script = FireballToLaunch.GetComponent<Fireball>();
            script.setHidden(false);
            Vector2 moveDirection = (target.transform.position - FireballToLaunch.transform.position).normalized;
            FireballToLaunch.GetComponent<Rigidbody2D>().velocity = new Vector2(moveDirection.x, moveDirection.y).normalized * launchForce;
        }

        animator.SetTrigger("attack");
        inAnimation = true;
        StartCoroutine(waitOnCooldown());
    }

    IEnumerator waitOnCooldown() {
        cooldown = true;
        yield return new WaitForSeconds(4);
        cooldown = false;
     }

    void animationEnd() {
        inAnimation = false;
    }

    public void takeDamage(int damage) {
        animator.SetTrigger("damage");
        health -= damage;
    }
}
