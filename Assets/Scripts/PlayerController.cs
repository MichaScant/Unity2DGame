using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
    
public class PlayerController: MonoBehaviour
{
    Vector2 lookDirection = new Vector2(1,0);
    Rigidbody2D rigidbody2D;
    Animator animator;
    public float speed = 5f;
    public int health = 4;
    public int numOfHearts;
    public Image[] hearts;
    public Sprite full;
    public Sprite empty;
    Vector2 movement;
    public GameObject placeableTower;
    public GameObject otherBodyParts;
    private SpriteRenderer bodyPart;

    public Vector2 respawnPoint = new Vector2(50, -50);
    private bool isInvincible;
    private bool dying = false;
    private bool attacking = false;
    private bool takingDamage = false;
    
    [Range(1,2)]
    public int weaponChosen = 1;
    private IDictionary<int, KeyValuePair<GameObject, String>> inventory;
    public GameObject[] inventoryItems;
    public String[] inventoryItemScriptNames;
    private GameObject currentWeapon;
    public GameObject ArrowManager;

    [SerializeField]
    private float invincibleDuration = 2f;
    public TerrainManager terrainManager;
    private GameObject currentTower;
    private bool inTowerRange = false;

    void Start() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyPart = otherBodyParts.GetComponent<SpriteRenderer>();

        inventory = new Dictionary<int, KeyValuePair<GameObject, String>>();

        for (int i = 0; i < inventoryItems.Length; i++) {
            inventory.Add(i + 1, new KeyValuePair<GameObject, String>(inventoryItems[i], inventoryItemScriptNames[i]));
        }

        currentWeapon = inventory[weaponChosen].Key;
    }
    
    
    private void OnTriggerEnter2D(Collider2D other) {
        GameObject intruder = other.gameObject;
        
        if (intruder.tag == "Tower") {
            inTowerRange = true;
            currentTower = intruder;
        }
    }
        private void OnTriggerExit2D(Collider2D other) {
        GameObject intruder = other.gameObject;
        
        if (intruder.tag == "Tower") {
            inTowerRange = false;
        }
    }
    
    void Update() {

        //Physics2D.IgnoreLayerCollision(gameObject.layer);

        if (Input.GetKeyDown("1") && weaponChosen != 1) {
            weaponChosen = 1;
            onWeaponChange();
        } 
        
        if (Input.GetKeyDown("2") && weaponChosen != 2) {
            
            weaponChosen = 2;
            onWeaponChange();
        } 

        if (Input.GetKeyDown("f")) {
            Instantiate(placeableTower, new Vector3(transform.position.x + lookDirection.x, transform.position.y + lookDirection.y, transform.position.z), Quaternion.identity);
        }

        if (Input.GetKeyDown("g") && inTowerRange) {
            terrainManager.RemoveTower(currentTower);
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movement = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(movement.x, 0.0f) || !Mathf.Approximately(movement.y, 0.0f))
        {
            lookDirection.Set(movement.x, movement.y);

            lookDirection.Normalize();
        }

        if (!attacking && Input.GetMouseButtonDown(0) && !takingDamage) {
            animator.SetFloat("Speed", 0);
            attacking = true;
            rigidbody2D.velocity = Vector2.zero;

            if (weaponChosen == 1) {
                animator.SetTrigger("hammer");
                currentWeapon.GetComponent<Hammer>().enabled = true;
            } else {
                animator.SetTrigger("bow");
                //string scriptName = inventory[weaponChosen].Value;
                currentWeapon.GetComponent<Bow>().Fire(lookDirection.x, lookDirection.y);
            }

            if (lookDirection.x != 0 || lookDirection.y != 1) {
                bodyPart.enabled = true;
            }

            currentWeapon.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (!dying && !attacking && !takingDamage) {

            animator.SetFloat("lookX", lookDirection.x);
            animator.SetFloat("lookY", lookDirection.y); 
            animator.SetFloat("Speed", movement.magnitude);
            
            //position.x = position.x + movement.x * 5.0f * Time.deltaTime;
            //position.y = position.y + movement.y * 5.0f * Time.deltaTime;

            Vector2 newPostion = movement * speed;
        
            rigidbody2D.velocity = newPostion;
            
        }

    }

    private void onWeaponChange() {
        currentWeapon = inventory[weaponChosen].Key;
    }

    private void onAttackEnd() {
        
        if (attacking) {
            currentWeapon.GetComponent<SpriteRenderer>().enabled = false;
            attacking = false;
            bodyPart.enabled = false;
            if (weaponChosen == 1) {
                currentWeapon.GetComponent<Hammer>().enabled = false;
            }
        }
    }
    public void takeDamage(int amount) {
        
        
        if (isInvincible) {
            return;
        }

        rigidbody2D.velocity = Vector2.zero;

        takingDamage = true;

        isInvincible = true;

        if (amount > health) {
            health = 0;
        } else {
            health -= amount;
        }


        if (health != 0) {
            animator.SetTrigger("isHit");
            StartCoroutine(temporaryInvincible());
        }
        updateHealth();
    }

    public void takeDamageEnd() {
        takingDamage = false;
    }

    void updateHealth() {

        for (int i = 0; i < hearts.Length; i++) {
            if (i < health) {
                hearts[i].sprite = full;
            } else {
                hearts[i].sprite = empty;
            }
        }


        if (health <= 0) {
            dying = true;
            killCharacter();
            StartCoroutine(temporaryInvincible());
        }
    }

    private IEnumerator temporaryInvincible() {

        yield return new WaitForSeconds(invincibleDuration);

        isInvincible = false;
    }

    private void killCharacter() {
        rigidbody2D.velocity = Vector2.zero;
        animator.SetFloat("Speed", 0f);
        animator.SetBool("dying", true);  
        isInvincible = true;
    }

    private void onDeathEnd() {
        animator.SetBool("dying", false);
        rigidbody2D.position = respawnPoint;
        health = 4;
        dying = false;
        updateHealth();
        isInvincible = false;
        takingDamage = false;
    }
}   
