using UnityEngine;

public class MonitorOnOff : MonoBehaviour, Interactable
{
    bool isOn;

    public Material monitorMaterial;
    public AudioSource onSound;
    public AudioSource offSound;
    public void Interact()
    {
        isOn = !isOn;
        Switch();

    }
    public void Switch()
    {
        if (isOn)
        {
            monitorMaterial.color = Color.blue;
            PlaySoundStatic.InstSound(onSound, gameObject);
        }
        else
        {
            monitorMaterial.color = Color.black;
            PlaySoundStatic.InstSound(offSound, gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Switch();
    }
}
