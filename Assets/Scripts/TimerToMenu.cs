using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerToMenu : MonoBehaviour
{
    public int scene;
    public float num;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Count(num));
    }
    public IEnumerator Count(float num)
    {
        yield return new WaitForSeconds(num);
        SceneManager.LoadScene(scene);
    }
}
