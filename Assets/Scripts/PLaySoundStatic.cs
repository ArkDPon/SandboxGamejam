using UnityEngine;

public class PlaySoundStatic : MonoBehaviour, Interactable
{
    public AudioSource snd;
    public void Interact()
    {
        InstSound(snd, gameObject);
    }
    public static void InstSound(AudioSource audioSource, GameObject parent)
    {
        GameObject inst = new GameObject();
        inst.transform.parent = parent.transform;
        inst.transform.localPosition = Vector3.zero;
        inst.AddComponent<AudioSource>();
        inst.GetComponent<AudioSource>().playOnAwake = false;
        inst.GetComponent<AudioSource>().loop = false;
        inst.GetComponent<AudioSource>().spatialBlend = 1;
        inst.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Custom;
        inst.GetComponent<AudioSource>().maxDistance = 10;
        inst.GetComponent<AudioSource>().volume = audioSource.volume;
        inst.GetComponent<AudioSource>().pitch = audioSource.pitch;
        inst.GetComponent<AudioSource>().clip = audioSource.clip;
        inst.GetComponent<AudioSource>().Play();
        inst.AddComponent<AudioSourceOptimizer>();
    }
}
