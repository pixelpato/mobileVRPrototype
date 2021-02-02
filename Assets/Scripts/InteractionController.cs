using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [HideInInspector] public GameObject lastHitObj;


    // Update is called once per frame
    void Update()
    {       
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity)) {
            if(hit.collider.tag == "Interactable") {
                // change old obj mat when looking at new obj
                if (lastHitObj != hit.collider.gameObject && lastHitObj != null) {
                    lastHitObj.GetComponent<Renderer>().material = lastHitObj.GetComponent<Interactable>().normMat;
                    lastHitObj.GetComponent<Outline>().OutlineWidth = 0;
                }

                // save last hitted obj in var
                lastHitObj = hit.collider.gameObject;

                // change hitted obj material
                lastHitObj.GetComponent<Renderer>().material = lastHitObj.GetComponent<Interactable>().hoverMat;
                lastHitObj.GetComponent<Outline>().OutlineWidth = 8;
            }
            else {
                lastHitObj.GetComponent<Renderer>().material = lastHitObj.GetComponent<Interactable>().normMat; // if ray hits non interactable obj, change prev mat back
                lastHitObj.GetComponent<Outline>().OutlineWidth = 0;
                lastHitObj = null; // clear reference
            }
        }
        else {
            if (hit.collider.gameObject != null) {
                if (lastHitObj != null)
                    lastHitObj.GetComponent<Renderer>().material = lastHitObj.GetComponent<Interactable>().normMat; // if ray hits nothing, change prev mat back
                lastHitObj.GetComponent<Outline>().OutlineWidth = 0;
                lastHitObj = null; // clear reference
            }
        }                 
    }
}
