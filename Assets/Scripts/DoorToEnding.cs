using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToEnding : MonoBehaviour, Interactable
{
    public GameObject fade;
    public void Interact()
    {
        fade.SetActive(true);
        Invoke("LoadScene", 3);
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(4);
    }
}
