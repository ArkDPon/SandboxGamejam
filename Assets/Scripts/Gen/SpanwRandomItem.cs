using UnityEngine;

public class SpanwRandomItem : MonoBehaviour
{
    public GameObject[] prefs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Random.Range(0, 1) == 0)
        {
            int selectedPref = Random.Range(0, prefs.Length);
            GameObject inst = Instantiate(prefs[selectedPref]);
            inst.transform.parent = gameObject.transform;
            inst.transform.localRotation = prefs[selectedPref].transform.localRotation;
            if(inst.GetComponent<ItemData>() != null)
            {
                inst.transform.localPosition = new Vector3(0, 0 + inst.GetComponent<ItemData>().placeOffset, 0);
            }
            else
            {
                inst.transform.localPosition = Vector3.zero;
            }
            for(int i = 0; i < inst.transform.GetComponentsInChildren<Transform>().Length; i++)
            {
                if (inst.transform.GetComponentsInChildren<Transform>()[i].GetComponent<MeshRenderer>() != null)
                {
                    inst.transform.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<MeshOptimizer>();
                }
            }
        }
    }
}
