using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class SoundOnCollide : MonoBehaviour
{

    public bool instanceExist;

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 2 && instanceExist == false)
        {
            instanceExist = true;
            //Debug.Log(collision.relativeVelocity.magnitude);
            GameObject inst = new GameObject();
            inst.transform.parent = gameObject.transform;
            inst.transform.localPosition = Vector3.zero;
            inst.AddComponent<AudioSource>();
            inst.GetComponent<AudioSource>().playOnAwake = false;
            inst.GetComponent<AudioSource>().loop = false;
            inst.GetComponent<AudioSource>().spatialBlend = 1;
            inst.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Custom;
            inst.GetComponent<AudioSource>().maxDistance = 10;
            if (collision.relativeVelocity.magnitude < 5)
            {
                inst.GetComponent<AudioSource>().volume = gameObject.GetComponent<AudioSource>().volume * (collision.relativeVelocity.magnitude - 1) * .5f;
            }
            else
            {
                inst.GetComponent<AudioSource>().volume = gameObject.GetComponent<AudioSource>().volume * 2.5f;
            }
            inst.GetComponent<AudioSource>().pitch = gameObject.GetComponent<AudioSource>().pitch;
            inst.GetComponent<AudioSource>().clip = gameObject.GetComponent<AudioSource>().clip;
            inst.GetComponent<AudioSource>().Play();
            inst.AddComponent<AudioSourceOptimizer>();
        }
    }
}
