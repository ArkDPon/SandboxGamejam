  using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    public Material mat;

    // Update is called once per frame
    void FixedUpdate()
    {
        mat.SetFloat("_Rotation", mat.GetFloat("_Rotation") + .5f);
    }
}
