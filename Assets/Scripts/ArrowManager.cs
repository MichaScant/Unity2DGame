using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> arrows;

    public GameObject arrowModel;
    public float launchForce = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        arrows = new List<GameObject>();
    }

    public void FireArrow(float xRotation, float yRotation) {

        var direction = Quaternion.LookRotation(Vector3.forward, new Vector2(xRotation, yRotation).normalized);
        
        /*if (xRotation == 0) {
            if (yRotation == 1) {
                direction = Vector3.forward * -90;
            } else {
                direction = Vector3.forward * 90;
            }
        } else {
            if (xRotation == 1) {
                direction = Vector3.forward * 180;
            } else {
                direction = Vector3.forward * 0;
            }
        }*/

        GameObject arrowToLaunch = null;
        
        if (arrows.Count != 0) {
            foreach (GameObject arrow in arrows) {
                Arrow script = arrow.GetComponent<Arrow>();
                if (script.getHidden()) {
                    script.setHidden(false);
                    arrow.transform.position = transform.position;
                    arrowToLaunch = arrow;
                    break;
                }
            }
        }
        
        if (arrows.Count == 0 || arrowToLaunch == null) {
            GameObject arrow = Instantiate(arrowModel, transform.position, Quaternion.identity);
            arrows.Add(arrow);
            arrowToLaunch = arrow;
        } 

        if (arrowToLaunch != null) {
            direction *= Quaternion.Euler(0, 0, -90);
            arrowToLaunch.transform.rotation = direction;
            Arrow script = arrowToLaunch.GetComponent<Arrow>();
            script.setHidden(false);
            arrowToLaunch.GetComponent<Rigidbody2D>().velocity = new Vector2(xRotation, yRotation).normalized * launchForce;
        }
    }
}
