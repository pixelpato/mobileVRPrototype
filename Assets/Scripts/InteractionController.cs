using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip [] instructionSFX;
    public GameObject campfire;

    public Light sun;
    public GameObject soundMng;
    public AudioClip nightSFX;

    [HideInInspector] public GameObject target;
    private Material objMat;
    private Material passiveMat;
    private float outlineWidth;
    private int itemsCollected;
    private bool fireActive;
    private float skyBoxFade = 3.75f;
    private float sunFade = 1.25f;


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
                    target.GetComponent<Outline>().OutlineWidth = -0.1f;
                }

                // save last ray hitted target in var
                target = hit.collider.gameObject;

                // change target material
                target.GetComponent<Renderer>().material = target.GetComponent<Interactable>().hoverMat;
                target.GetComponent<Outline>().OutlineWidth = 8;

                // collect obj when hitting the hmd-button
                if (Input.GetButtonDown("Fire1")) {
                    target.GetComponent<AudioSource>().Play();
                    itemsCollected++;
                    Destroy(target, 0.5f);
                }
            }
            else if (hit.collider.tag == "Campfire") {
                ClearMaterial();

                if (Input.GetButtonDown("Fire1")) {
                    if (itemsCollected >= 11) {
                        // "Das Lagerfeuer ist fertig!"
                        PlayInstructions(3);

                        // materialize & end sequence
                        itemsCollected = 0;
                        fireActive = true;

                        GameObject newFire = Instantiate(campfire, hit.collider.transform);
                        newFire.transform.position = hit.collider.transform.position;
                        newFire.transform.rotation = hit.collider.transform.rotation;
                        Destroy(hit.collider);

                        // play end sequence
                        StartCoroutine(EndSequence());
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

    IEnumerator EndSequence () {
        // wait
        yield return new WaitForSeconds(2);

        // set scene ambient
        StartCoroutine(FadeSkybox(3, 0.1f));
        StartCoroutine(FadeSun(1, 0.1f));      
        soundMng.GetComponent<AudioSource>().clip = nightSFX;
        soundMng.GetComponent<AudioSource>().Play();

        // wait
        yield return new WaitForSeconds(6);

        // fade out screen + back to menu
        GameObject.Find("FadeImage").GetComponent<ScreenFading>().FadeOut();
        StartCoroutine(LoadScene("StartScreen", 2));
    }

    public IEnumerator FadeSkybox (float fadeSpeed, float fadeMin) {
        while (skyBoxFade > fadeMin) {
            skyBoxFade -= (fadeSpeed * Time.deltaTime);
            RenderSettings.skybox.SetFloat("_Exposure", skyBoxFade);
            yield return null;
        }
    }

    public IEnumerator FadeSun (float fadeSpeed, float fadeMin) {        
        while (sunFade > fadeMin) {
            sunFade -= (fadeSpeed * Time.deltaTime);
            sun.intensity = sunFade;
            yield return null;
        }
    }  

    IEnumerator LoadScene (string scene, float delay) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }
}
