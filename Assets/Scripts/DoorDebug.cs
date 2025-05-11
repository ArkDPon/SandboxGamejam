using TMPro;
using UnityEngine;

public class DoorDebug : MonoBehaviour, AiInteractable
{
    public string animationOnReach { get; set; }
    public string reachAnim = "OPEN";
    public GameObject floatTextObj { get; set; }
    public string floatText { get; set; }
    public string notifText = "“олько помощник может выполн€ть “я∆≈Ћ≈…Ў»≈ пространственные манипул€ции (открытие дверей)";
    public Vector3 extend;
    public Vector3 rot;

    bool opened;
    public void Interact()
    {
        if (!opened)
        {
            opened = true;
            Debug.Log("extended");
            GameObject.Find("GENERATE").GetComponent<RoomGeneration>().ExtendGeneration(extend, rot, transform.parent.transform.parent.gameObject);
            Destroy(this);
        }
    }
    void Start()
    {
        animationOnReach = reachAnim;
        floatTextObj = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerData>().notifText;
        floatText = notifText;
    }
    public void PlayerInteract()
    {
        floatTextObj.SetActive(false);
        floatTextObj.GetComponent<TMP_Text>().SetText(floatText);
        floatTextObj.SetActive(true);
    }
}
