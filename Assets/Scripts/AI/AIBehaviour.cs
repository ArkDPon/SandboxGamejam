
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AIBehaviour : MonoBehaviour
{

    GameObject player;
    NavMeshAgent ai;
    public Animator animator;

    public LineRenderer line;
    public GameObject lineEndPoint;

    public int happyPoints;

    public GameObject interactObj;
    public string animOnReach;
    public NavMeshPath interactPath;

    public Transform hips;
    public List<Transform> ragdollBones;

    public List<Vector3> bonePos;
    public List<Quaternion> boneRot;
    public List<Vector3> standUpAnimBonesPos;
    public List<Quaternion> standUpAnimBonesRot;


    public bool knockedDown;

    public bool bonesTransition;
    public float bonesTransitionTime = 1f;
    float elapsedBonesTransitionTime;

    public GameObject grabbedItem;
    public GameObject previousItem;
    public Transform hand;
    public bool pickupCooldown;

    bool boredCountdown;
    public bool ragdollCooldown;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("MainCamera");
        ai = GetComponent<NavMeshAgent>();
        SetupBones();
        for(int i = 0; i < ragdollBones.Count; i++)
        {
            bonePos.Add(ragdollBones[i].localPosition);
            boneRot.Add(ragdollBones[i].localRotation);
            standUpAnimBonesPos.Add(ragdollBones[i].localPosition);
            standUpAnimBonesRot.Add(ragdollBones[i].localRotation);
        }
        PopulateAnimationStartBoneTransforms("STAND_UP");
    }
    public IEnumerator BoredCountDown(float count)
    {
        boredCountdown = true;
        yield return new WaitForSeconds(count);
        if(happyPoints > -10)
        {
            if (Random.Range(0, 2) == 0)
            {
                animator.Play("LOOKING_AROUND");
            }
            else
            {
                animator.Play("BORED");
            }
        }
        boredCountdown = false;

    }
    public IEnumerator RagdollCooldown()
    {
        yield return new WaitForSeconds(2f);
        ragdollCooldown = false;
    }
    private void Update()
    {
        if (happyPoints <= -50)
        {
            SceneManager.LoadScene(6);
        }
        else if (happyPoints >= 50)
        {
            SceneManager.LoadScene(7);
        }
        if(ai.enabled && interactObj != null && Vector3.Distance(gameObject.transform.position, interactObj.transform.Find("InteractDestination").position) < .5f)
        {
            //animator.Play(animOnReach);
            interactObj.GetComponent<AiInteractable>().Interact();
            interactPath = null;
            interactObj = null;
            animOnReach = "";
        }
        if(ai.enabled == true && ai.velocity.magnitude > 0.15f)
        {
            line.enabled = true;
            lineEndPoint.SetActive(true);
            DrawPath(ai.path);
            animator.SetBool("walking", true);
            StopCoroutine("BoredCountdown");
            boredCountdown = false;
        }
        else if(ai.enabled)
        {
            animator.SetBool("walking", false);
            line.enabled = false;
            lineEndPoint.SetActive(false);
            if (!boredCountdown && happyPoints > -10)
            {
                StartCoroutine(BoredCountDown(Random.Range(15, 30)));
            }
        }

        if (bonesTransition)
        {
            elapsedBonesTransitionTime += Time.deltaTime;
            float elapsedPercentage = elapsedBonesTransitionTime / bonesTransitionTime;

            for (int i = 0; i < ragdollBones.Count; i++)
            {
                ragdollBones[i].localPosition = new Vector3(Vector3.Lerp(
                    ragdollBones[i].localPosition,
                    standUpAnimBonesPos[i],
                    elapsedPercentage).x, ragdollBones[i].localPosition.y, Vector3.Lerp(
                    ragdollBones[i].localPosition,
                    standUpAnimBonesPos[i],
                    elapsedPercentage).z);

                ragdollBones[i].localRotation = Quaternion.Lerp(
                    ragdollBones[i].localRotation,
                    standUpAnimBonesRot[i],
                    elapsedPercentage);
            }

            if (elapsedPercentage >= 1)
            {
                knockedDown = false;
                for(int i = 0; i < ragdollBones.Count;i++)
                {
                    ragdollBones[i].GetComponent<Rigidbody>().isKinematic = true;
                }
                animator.enabled = true;
                ragdollCooldown = true;
                StartCoroutine(RagdollCooldown());
                animator.Play("STAND_UP");
                bonesTransition = false;
                ai.enabled = true;
            }
        }

    }
    public void SetDestination(Vector3 destination, GameObject interObj = null, string animationOnReach = null)
    {
        ai.isStopped = true;
        ai.velocity = Vector3.zero;
        line.enabled = false;
        interactObj = interObj;
        animOnReach = animationOnReach;
        //Debug.Log(interactObj.name);
        //Debug.Log(animOnReach);
        lineEndPoint.SetActive(false);
        NavMeshPath navMeshPath = new NavMeshPath();
        ai.path = navMeshPath;
        interactPath = ai.path;
        if (ai.CalculatePath(destination, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete && !knockedDown)
        {
            ai.destination = destination;
        }
        else
        {
            animator.Play("SHRUGGING");
        }
    }

    public void DrawPath(NavMeshPath path)
    {
        if (path.corners.Length < 2)
            return;

        line.positionCount = path.corners.Length;

        for (int i = 0; i < path.corners.Length; i++)
        {
            line.SetPosition(i, path.corners[i] + new Vector3(0, .05f, 0));
        }
        lineEndPoint.transform.position = path.corners[path.corners.Length-1];
    }

    public void SetupBones()
    {
        for(int i = 0; i < hips.GetComponentsInChildren<Transform>().Length; i++)
        {
            if (hips.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody>())
            {
                hips.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody>().gameObject.layer = LayerMask.NameToLayer("AI");
                hips.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody>().isKinematic = true;
                ragdollBones.Add(hips.GetComponentsInChildren<Transform>()[i]);
                hips.GetComponentsInChildren<Transform>()[i].AddComponent<BoneBehaviour>();
                hips.GetComponentsInChildren<Transform>()[i].GetComponent<BoneBehaviour>().aib = this;
            }
        }
    }
    public void KnockDown(Transform hitBone, Vector3 hitVelocity)
    {
        line.enabled = false;
        lineEndPoint.SetActive(false);
        if (!knockedDown)
        {
            ai.isStopped = true;
            happyPoints -= 5;
        }
        knockedDown = true;
        ai.enabled = false;
        animator.enabled = false;
        for(int i = 0; i < ragdollBones.Count; i++)
        {
            ragdollBones[i].GetComponent<Rigidbody>().isKinematic = false;
            if (ragdollBones[i] == hitBone)
                ragdollBones[i].GetComponent<Rigidbody>().linearVelocity += hitVelocity;
        }
        if(grabbedItem != null)
        {
            ReleaseItem();
            grabbedItem = null;
        }
        StopAllCoroutines();
        pickupCooldown = false;
        StartCoroutine(RagdollCountDown(Random.Range(5, 10)));
    }
    public IEnumerator RagdollCountDown(float count)
    {
        yield return new WaitForSeconds(count);
        AlignHips();
        PopulateBoneTransforms();
        //PopulateAnimationStartBoneTransforms("STAND_UP");
        elapsedBonesTransitionTime = 0;
        bonesTransition = true;
    }
    public void AlignHips()
    {
        Vector3 originalHipsPosition = hips.position;

        transform.position = hips.position;
        hips.position = originalHipsPosition;

        Quaternion originalHipsRotation = hips.rotation;
        Vector3 direction = hips.up * -1;
        direction.y = 0;
        direction.Normalize();
        Quaternion fromToRotation = Quaternion.FromToRotation(transform.forward, direction);
        transform.rotation *= fromToRotation;
        hips.rotation = originalHipsRotation;

        //Debug.LogError("test");
    }
    private void PopulateBoneTransforms()
    {
        for (int i = 0; i < ragdollBones.Count; i++)
        {
            bonePos[i] = ragdollBones[i].localPosition;
            boneRot[i] = ragdollBones[i].localRotation;
        }
    }

    private void PopulateAnimationStartBoneTransforms(string clipName)
    {
        Vector3 positionBeforeSampling = transform.position;
        Quaternion rotationBeforeSampling = transform.rotation;

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                List<Transform> ogPos = ragdollBones;
                clip.SampleAnimation(animator.gameObject, 0);
                for (int i = 0; i < ragdollBones.Count; i++)
                {
                    standUpAnimBonesPos[i] = new Vector3(ragdollBones[i].localPosition.x, ogPos[i].transform.localPosition.y, ragdollBones[i].localPosition.z);
                    standUpAnimBonesRot[i] = ragdollBones[i].localRotation;
                }
                break;
            }
        }

        transform.position = positionBeforeSampling;
        transform.rotation = rotationBeforeSampling;
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if(other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.layer != LayerMask.NameToLayer("AI") && !knockedDown && grabbedItem == null && !pickupCooldown && (Random.Range(0, 2) == 0 || player.GetComponent<GravityGun>().requestedObject == other.gameObject))
        {
            PickupItem(other.gameObject);
        }
        else if (other.transform.parent != null && other.transform.parent.gameObject.GetComponent<Rigidbody>() && other.transform.parent.gameObject.layer != LayerMask.NameToLayer("AI") && !knockedDown && grabbedItem == null && !pickupCooldown && (Random.Range(0, 2) == 0 || player.GetComponent<GravityGun>().requestedObject == other.transform.parent.gameObject))
        {
            PickupItem(other.transform.parent.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == previousItem)
        {
            previousItem = null;
        }
    }
    public IEnumerator HoldItem()
    {
        //Debug.Log("started corot");
        yield return new WaitForSeconds(1);
        if(happyPoints > -10)
        {
            if (grabbedItem != null && grabbedItem.TryGetComponent(out Interactable interactObj))
            {
                happyPoints += 10;
                interactObj.Interact();
            }
            if (grabbedItem != null)
            {
                //Debug.Log("waiting");
                yield return new WaitForSeconds(Random.Range(5, 6));
                Debug.Log("waited");
                if (grabbedItem != null)
                {
                    ReleaseItem();
                }
            }
        }
        else
        {
            if (grabbedItem != null)
            {
                ThrowItem((player.transform.position - grabbedItem.transform.position).normalized);
            }
        }


    }
    public IEnumerator PickupCooldown()
    {
        pickupCooldown = true;
        yield return new WaitForSeconds(5);
        pickupCooldown = false;
    }
    public void PickupItem(GameObject item)
    {
        item.GetComponent<Rigidbody>().isKinematic = true;
        grabbedItem = item;
        previousItem = item;
        item.transform.parent = hand.transform;
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        StartCoroutine(HoldItem());
    }
    public void ReleaseItem()
    {
        StartCoroutine(PickupCooldown());
        if(grabbedItem != null)
        {
            grabbedItem.transform.parent = null;
            grabbedItem.GetComponent<Rigidbody>().isKinematic = false;
            grabbedItem = null;
        }
    }
    public void ThrowItem(Vector3 direction)
    {
        StartCoroutine(PickupCooldown());
        if (grabbedItem != null)
        {
            grabbedItem.transform.parent = null;
            grabbedItem.GetComponent<Rigidbody>().isKinematic = false;
            grabbedItem.GetComponent<Rigidbody>().AddForce(direction * 30, ForceMode.Impulse);
            grabbedItem = null;
        }
    }
}
