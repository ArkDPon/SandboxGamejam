using UnityEngine;

public class BoneBehaviour : MonoBehaviour
{

    public AIBehaviour aib;
    GravityGun gg;

    private void Start()
    {
        gg = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GravityGun>();
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > 12 && !aib.knockedDown)
        {
            Vector3 hitVelocity = Vector3.zero;
            if(collision.gameObject.GetComponent<Rigidbody>() != null)
            {
                hitVelocity = collision.gameObject.GetComponent<Rigidbody>().linearVelocity;
            }
            aib.KnockDown(gameObject.transform, hitVelocity);
        }
    }
    private void Update()
    {
        if(gg.grabbedRB == gameObject.GetComponent<Rigidbody>())
        {
            aib.KnockDown(gameObject.transform, Vector3.zero);
        }
    }
}
