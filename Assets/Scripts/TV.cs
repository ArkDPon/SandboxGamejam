using Unity.VisualScripting;
using UnityEngine;

public class TV : MonoBehaviour, Interactable
{

    public bool isOn;

    public GameObject vid;

    public AudioSource snd;
    public void Interact()
    {
        Switch();
        PlaySoundStatic.InstSound(snd, gameObject);
    }
    public void Switch()
    {
        if (isOn == false)
        {
            isOn = true;
            vid.SetActive(true);

        }
        else
        {
            isOn = false;
            vid.SetActive(false);
        }
    }
}
