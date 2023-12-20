using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public GameObject user;
    private bool wait = false;

    void OnTriggerStay2D(Collider2D other) {
        if (user.tag == "Slime") {
            user.GetComponent<SlimeController>().applyDamage(other);;
        }
    }

    public void kill() {
        if (user.tag == "Slime") {
            user.GetComponent<SlimeController>().kill();;
        }
    }

    /*
    private IEnumerator stop() {
        wait = true;

        yield return new WaitForSeconds(5.0f);

        wait = false;
    }*/
}
