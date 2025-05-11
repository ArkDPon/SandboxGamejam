
using UnityEngine;

interface AiInteractable
{
    public void Interact();
    public void PlayerInteract();
    public string animationOnReach { get; set; }

    public GameObject floatTextObj { get; set; }
    public string floatText { get; set; }
}
public class NavigateAI : MonoBehaviour
{
    public AIBehaviour aib;
    void Update()
    {
        RaycastHit hit;
        Ray ray = gameObject.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f));
        //Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward*10, Color.red);
        if (Input.GetKeyDown(KeyCode.C) && Physics.Raycast(ray, out hit) && !aib.knockedDown && !aib.ragdollCooldown)
        {
            if(hit.transform.gameObject.TryGetComponent(out AiInteractable interactObj))
            {
                aib.SetDestination(hit.transform.Find("InteractDestination").position, hit.transform.gameObject, interactObj.animationOnReach);
            }
            else
            {
                aib.SetDestination(hit.point);
            }
        }
    }
}
