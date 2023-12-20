using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public GameObject user;
    void OnTriggerStay2D(Collider2D other) {
        if (user.tag == "Slime") {
            user.GetComponent<SlimeController>().intruderDetected(other);
        }

        if (user.tag == "Wizard") {
            user.GetComponent<WizardController>().intruderDetected(other);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (user.tag == "Slime") {
            user.GetComponent<SlimeController>().intruderFled(other);
        }

        if (user.tag == "Wizard") {
            user.GetComponent<WizardController>().intruderFled(other);
        }
    }
}
