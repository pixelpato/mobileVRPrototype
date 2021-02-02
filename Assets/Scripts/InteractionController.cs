using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [HideInInspector] public GameObject target;
    private Material objMat;
    private Material passiveMat;
    private float outlineWidth;

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
                    objMat = passiveMat;
                    outlineWidth = 0;
                }

                // save last ray hitted target in var
                target = hit.collider.gameObject;
                objMat = target.GetComponent<Renderer>().material;
                passiveMat = target.GetComponent<Interactable>().normMat;
                outlineWidth = target.GetComponent<Outline>().OutlineWidth;

                // change target material
                objMat = target.GetComponent<Interactable>().hoverMat;
                outlineWidth = 8;
            }
            else {
                ClearMat();
            }
        }
        else {
            if (hit.collider.gameObject != null) {
                ClearMat();
            }
        }
    }

    // clear hover state on passive obj
    void ClearMat() {
        if (target != null)
            objMat = passiveMat;
        outlineWidth = 0;
        target = null;
    }
}
