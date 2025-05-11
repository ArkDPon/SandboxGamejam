using UnityEngine;

public class ItemData : MonoBehaviour
{
    public float placeOffset = .25f;
    public bool collectable;
    public bool canBreak;
    public float breakPoint = 5;
    public GameObject brokenPrefab;
    public AudioSource destroySound;

    bool broken;

    public void OnCollisionEnter(Collision collision)
    {
        if(!broken && collision.relativeVelocity.magnitude >= breakPoint && canBreak)
        {
            broken = true;
            GameObject inst = Instantiate(brokenPrefab);
            inst.transform.position = gameObject.transform.position;
            inst.transform.rotation = gameObject.transform.rotation;
            PlaySoundStatic.InstSound(destroySound, inst);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (gameObject.GetComponent<Rigidbody>() == null)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
