using UnityEngine;
using UnityEngine.UI;

public class SettingsEvent : MonoBehaviour
{
    public GameObject panel;
    public Toggle tog;
    public Slider slider;
    public CameraMovement cam;
    public void SetIntensity()
    {
        float value = slider.value;
        cam.sensitivity = new Vector2(value, value);
    }
    public void SetLowQuality()
    {
        bool native = tog.isOn;
        if (native)
        {
            Screen.SetResolution(1280, 720, true);
        }
        else
        {
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
                cam.enabled = true;
            }
            else
            {
                panel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
                cam.enabled = false;
            }
        }
    }
}
