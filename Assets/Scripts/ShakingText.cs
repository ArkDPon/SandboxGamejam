using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShakingText : MonoBehaviour
{
    public TMP_Text txt;
    public float varx;
    public float vartime;
    public float multip;
    //ublic SettingsApply st;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        txt.ForceMeshUpdate();
        var ti = txt.textInfo;

        for(int i = 0; i<ti.characterCount; i++)
        {
            var charInfo = ti.characterInfo[i];

            if(!charInfo.isVisible) continue;

            var verts = ti.meshInfo[charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; j++)
            {
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time*vartime + orig.x*varx) * multip, 0);
            }
        }

        for(int i =0; i<ti.meshInfo.Length; i++)
        {
            var meshInfo = ti.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            txt.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
