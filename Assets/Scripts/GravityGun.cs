using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GravityGun : MonoBehaviour
{

    [SerializeField] Camera cam;
    [SerializeField] float maxGrabDistance = 15f, throwForce = 20f, lerpSpeed = 10f;
    [SerializeField] Transform objectHolder;

    public Rigidbody grabbedRB;

    public float mCorrectionForce = 50.0f;

    public float rotationFactor = 2f;

    public List<GameObject> tempBlacklist;

    public GameObject requestedObject;

    public AIBehaviour aib;

    public AudioSource collectedSound;

    public IEnumerator ThrowCooldown(GameObject obj)
    {
        tempBlacklist.Add(obj);
        yield return new WaitForSeconds(1f);
        if(tempBlacklist.Contains(obj) == true)
        {
            tempBlacklist.Remove(obj);
        }
    }

    public IEnumerator AiPickupRequest()
    {
        yield return new WaitForSeconds(1f);
        requestedObject = null;
    }

    public void PickupItem()
    {
        RaycastHit hit;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out hit, maxGrabDistance))
        {
            if (hit.transform.GetComponent<Rigidbody>() != null && tempBlacklist.Contains(hit.transform.gameObject) == false)
            {
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("AI"))
                {
                    if (aib.knockedDown && !aib.bonesTransition)
                    {
                        if (hit.transform.GetComponent<Rigidbody>().isKinematic == true)
                        {
                            hit.transform.GetComponent<Rigidbody>().isKinematic = false;
                        }
                        if (grabbedRB != null && grabbedRB != hit.transform.GetComponent<Rigidbody>())
                        {
                            grabbedRB.useGravity = true;
                            grabbedRB.constraints = RigidbodyConstraints.None;
                            grabbedRB = null;
                        }
                        grabbedRB = hit.transform.gameObject.GetComponent<Rigidbody>();
                    }
                }
                else
                {
                    if(aib.grabbedItem == hit.transform.gameObject)
                    {
                        aib.StopCoroutine(aib.HoldItem());
                        StartCoroutine(aib.PickupCooldown());
                        aib.grabbedItem.transform.parent = null;
                        aib.grabbedItem.GetComponent<Rigidbody>().isKinematic = false;
                        aib.grabbedItem = null;
                    }
                    if (hit.transform.GetComponent<ItemData>() != null && hit.transform.GetComponent<ItemData>().collectable)
                    {
                        Destroy(hit.transform.gameObject);
                        gameObject.GetComponent<PlayerData>().resourcesCollected++;
                        gameObject.GetComponent<PlayerData>().txt.SetText(gameObject.GetComponent<PlayerData>().resourcesCollected + "/20");
                        PlaySoundStatic.InstSound(collectedSound, gameObject);
                        if(gameObject.GetComponent<PlayerData>().resourcesCollected >= 20)
                        {
                            SceneManager.LoadScene(2);
                        }
                    }
                    else 
                    {
                        if (hit.transform.GetComponent<Rigidbody>().isKinematic == true)
                        {
                            hit.transform.GetComponent<Rigidbody>().isKinematic = false;
                        }
                        if (grabbedRB != null && grabbedRB != hit.transform.GetComponent<Rigidbody>())
                        {
                            grabbedRB.useGravity = true;
                            grabbedRB.constraints = RigidbodyConstraints.None;
                            grabbedRB = null;
                        }
                        grabbedRB = hit.transform.gameObject.GetComponent<Rigidbody>();
                    }
                }
            }
        }
    }

    public void DetachItem()
    {
        tempBlacklist.Clear();
        if (grabbedRB)
        {
            grabbedRB.useGravity = true;
            grabbedRB.constraints = RigidbodyConstraints.None;
            requestedObject = grabbedRB.gameObject;
            grabbedRB = null;
        }
    }

    public void ThrowItem()
    {
        StartCoroutine(ThrowCooldown(grabbedRB.gameObject));
        grabbedRB.useGravity = true;
        grabbedRB.constraints = RigidbodyConstraints.None;
        grabbedRB.AddForce(cam.transform.forward * throwForce, ForceMode.Impulse);
        grabbedRB = null;
    }

    void FixedUpdate()
    {
        if (grabbedRB)
        {
            if (grabbedRB.useGravity)
                grabbedRB.useGravity = false;

            Vector3 targetPoint = objectHolder.transform.position;
            Vector3 force = targetPoint - grabbedRB.transform.position;

            grabbedRB.linearVelocity = force.normalized * grabbedRB.linearVelocity.magnitude;
            grabbedRB.AddForce(force * mCorrectionForce);

            grabbedRB.linearVelocity *= Mathf.Min(1.0f, force.magnitude / 2);

            if (Vector3.Distance(objectHolder.transform.position, grabbedRB.gameObject.transform.position) > maxGrabDistance+1)
            {
                DetachItem();
            }
            else if (Input.GetMouseButtonDown(1)) //бросить предмет
            {
                ThrowItem();
            }
        }

        if (Input.GetMouseButton(0)) //взять предмет
        {
            PickupItem();
        }
        else
        {
            DetachItem();
        }
    }
}