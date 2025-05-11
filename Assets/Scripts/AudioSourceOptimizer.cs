using System.Collections;
using UnityEngine;

public class AudioSourceOptimizer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameObject.transform.parent.GetComponent<SoundOnCollide>() != null)
        {
            StartCoroutine("Count");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<AudioSource>().isPlaying == false)
        {
            StopAllCoroutines();
            if(gameObject.transform.parent.GetComponent<SoundOnCollide>() != null)
            {
                gameObject.transform.parent.GetComponent<SoundOnCollide>().instanceExist = false;
            }
            Destroy(gameObject);
        }
    }
    public IEnumerator Count() //prevent audio sources from stacking
    {
        yield return new WaitForSeconds(.25f);
        if (gameObject.transform.parent.GetComponent<SoundOnCollide>() != null)
        {
            gameObject.transform.parent.GetComponent<SoundOnCollide>().instanceExist = false;
        }
    }
}
