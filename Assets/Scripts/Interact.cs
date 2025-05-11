using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
interface Interactable
{
    public void Interact();
}
public class Interact : MonoBehaviour
{
    public float maxInteract;
    // Start is called before the first frame update
    void Update()
    {
        Ray ray = gameObject.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxInteract) && Input.GetKeyDown(KeyCode.E))
                {
                    if (hitInfo.transform.gameObject.TryGetComponent(out Interactable interactObj))
                    {
                        interactObj.Interact();
                    }
                    else if (hitInfo.transform.gameObject.TryGetComponent(out AiInteractable interactObj2))
            {
                interactObj2.PlayerInteract();
            }
        }
    }
}
