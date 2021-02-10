using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionController : MonoBehaviour
{
    [HideInInspector] public GameObject target;
    private Material objMat;
    private Material passiveMat;
    private float outlineWidth;


    private void Awake () {
        target = null;
    }

    void Update()
    {
        Raycast();         
    }

    void Raycast() {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity)) {
            if (hit.collider.tag == "Interactable") {
                // change old target mat when looking at new target object
                if (target != hit.collider.gameObject && target != null) {
                    target.GetComponent<Renderer>().material = target.GetComponent<Interactable>().normMat;
                    target.GetComponent<Outline>().OutlineWidth = 0;
                }

                // save last ray hitted target in var
                target = hit.collider.gameObject;

                // change target material
                target.GetComponent<Renderer>().material = target.GetComponent<Interactable>().hoverMat;
                target.GetComponent<Outline>().OutlineWidth = 8;

                // collect obj when hitting the hmd-button
                if (Input.GetButton("Fire1")) {
                    target.GetComponent<AudioSource>().Play();
                    Destroy(target, 0.5f);                
                }
            }
            else {
                ClearMaterial();
            }
        }
        else {
            ClearMaterial();
        }
    }

    // clear hover state on passive obj
    void ClearMaterial() {
        if (target != null) {
            target.GetComponent<Renderer>().material = target.GetComponent<Interactable>().normMat;
            target.GetComponent<Outline>().OutlineWidth = 0;
        }           
        target = null;
    }
}
