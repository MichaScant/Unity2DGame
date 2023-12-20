using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private bool hidden = false;
    private Rigidbody2D rigidbody2D;

    public int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void setHidden(bool newValue) {
        hidden = newValue;
        
        if (hidden) {
            gameObject.SetActive(false);

        } else {
            gameObject.SetActive(true);
            StartCoroutine(wait());
        }
    }

    public bool getHidden() {
        return hidden;
    }

     IEnumerator wait() {
        yield return new WaitForSeconds(5);
        if (!hidden) {
            setHidden(true);
        }
     }

    private void OnTriggerEnter2D(Collider2D other) { 

        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<PlayerController>().takeDamage(damage);
            setHidden(true);
        }
    }
}
