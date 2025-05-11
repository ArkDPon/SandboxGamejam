using OutlineFx;
using UnityEngine;

public class DespawnChance : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Random.Range(0, 3) == 1)
        {
            Destroy(gameObject);
        }
        else
        {
            for (int i = 0; i < gameObject.transform.GetComponentsInChildren<Transform>().Length; i++)
            {
                if (gameObject.transform.GetComponentsInChildren<Transform>()[i].GetComponent<MeshRenderer>() != null)
                {
                    gameObject.transform.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<MeshOptimizer>();
                }
            }
        }
    }
}
