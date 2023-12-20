using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
public class hazard : MonoBehaviour
{
    public int damage = 1;
    private void OnTriggerEnter2D(Collider2D other) {
        GameObject intruder = other.gameObject;
        
        if (intruder.tag == "Player") {
            intruder.GetComponent<PlayerController>().takeDamage(damage);
        }
        
    }
}
}
