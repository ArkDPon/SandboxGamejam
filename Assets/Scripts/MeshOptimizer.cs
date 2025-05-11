using Unity.VisualScripting;
using UnityEngine;

public class MeshOptimizer : MonoBehaviour
{
    public Transform player;
    MeshRenderer rend;

    private void Start()
    {
        if(GameObject.FindGameObjectWithTag("MainCamera") != null)
        {
            player = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
        rend = GetComponent<MeshRenderer>();
    }
    public void Update()
    {
        if(player != null && Vector3.Distance(player.position, transform.position) > 50)
        {
            rend.enabled = false;
        }
        else if(player != null)
        {
            rend.enabled = true;
        }
    }
}
