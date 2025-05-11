using UnityEngine;

public class SelectInterior : MonoBehaviour
{
    
    public GameObject[] interior;
    public GameObject predeterminedVariant = null;
    GameObject lastObj;

    private void Start()
    {
        if(predeterminedVariant != null)
        {
            predeterminedVariant.SetActive(true);
        }
        else
        {
            int selectedPattern = Random.Range(0, interior.Length);
            interior[selectedPattern].SetActive(true);
        }
    }
}
