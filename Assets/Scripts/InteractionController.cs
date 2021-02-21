using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip [] instructionSFX;
    public Material material1;
    public Material material2;
    public Material material3;

    [HideInInspector] public GameObject target;
    private Material objMat;
    private Material passiveMat;
    private float outlineWidth;
    private int itemsCollected = 0;
    private bool fireActive;


    private void Awake () {
        target = null;

        // "Hier sollte sich irgendwo eine gute Stelle für ein Lagerfeuer finden."
        PlayInstructions(0);
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
                if (Input.GetButton("Fire1") && hit.collider.name != "Campfire") {
                    target.GetComponent<AudioSource>().Play();
                    itemsCollected++;
                }
                else {
                    if (itemsCollected == 11) {
                        // materialize & end sequence
                        itemsCollected = 0;
                        fireActive = true;
                        target.GetComponent<AudioSource>().Play();
                        target.transform.GetChild(0).gameObject.SetActive(true);

                        target.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material = material1;
                        target.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material = material2;

                        // "Das Lagerfeuer ist fertig!"
                        PlayInstructions(3);
                    }
                    else if (itemsCollected < 11 && itemsCollected > 0) {
                        // "Ich habe noch nicht genügend Zweige und Steine für das Lagerfeuer gesammelt."
                        PlayInstructions(2);

                    }
                    else if (itemsCollected == 0 && !fireActive) {
                        // "Das ist ein guter Platz für ein Lagerfeuer, ich sollte Zweige und Steine sammeln."
                        PlayInstructions(1);
                    }
                    else {
                        // do nothing
                    }
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

    // play audio voice for player instructions
    void PlayInstructions(int clip) {
        audioSource.clip = instructionSFX[clip];
        
        if(!audioSource.isPlaying)
            audioSource.Play();
    }
}
