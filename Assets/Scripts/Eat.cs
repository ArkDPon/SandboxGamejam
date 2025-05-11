using Unity.VisualScripting;
using UnityEngine;

public class Eat : MonoBehaviour, Interactable
{
    public AudioSource eatSound;
    public void Interact()
    {
        GameObject emp = new GameObject();
        emp.transform.position = gameObject.transform.position;
        PlaySoundStatic.InstSound(eatSound, emp);
        Destroy(gameObject);
    }
}
