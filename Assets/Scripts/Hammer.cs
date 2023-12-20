using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public int damage = 1;
    public bool enabled = false;

    private int hitCount = 0;

    void Update() {
        if (!enabled) {
            hitCount = 0;
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Slime" && enabled && hitCount == 0) {
            other.gameObject.GetComponent<Damage>().kill();
            hitCount = 1;
        }

        if (other.gameObject.tag == "Wizard" && enabled && hitCount == 0) {
            other.gameObject.GetComponent<WizardController>().takeDamage(damage);
            hitCount = 1;
        }
    }

}
