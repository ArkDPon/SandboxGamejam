using Unity.VisualScripting;
using UnityEngine;

public class Flashlight : MonoBehaviour, Interactable
{
    public GameObject glass;

    public GameObject lightsource;
    public Material notGlowing;
    public Material glowing;

    public bool isActive;

    public AudioSource click;
    public void Interact()
    {
        if(isActive == false)
        {
            glass.GetComponent<Renderer>().material = glowing;
            lightsource.SetActive(true);
            isActive = true;
        }
        else
        {
            glass.GetComponent<Renderer>().material = notGlowing;
            lightsource.SetActive(false);
            isActive = false;
        }
        PlaySoundStatic.InstSound(click, gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
