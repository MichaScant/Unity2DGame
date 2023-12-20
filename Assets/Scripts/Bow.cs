using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public GameObject arrowManager;

    private ArrowManager arrowSpawn;

    void Start() {
        arrowSpawn = arrowManager.GetComponent<ArrowManager>();
    }

    public void Fire(float xRotation, float yRotation) { 
        arrowSpawn.FireArrow(xRotation, yRotation);
    }
}