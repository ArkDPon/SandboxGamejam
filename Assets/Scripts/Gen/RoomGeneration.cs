
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    public GameObject[] roomPrefs;
    public List<GameObject> rooms;

    public NavMeshSurface surface;


    public AudioSource lockedSound;
    public AudioSource openSound;

    public float increment = .15f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateFirst();
    }
    public void GenerateFirst()
    {
        GameObject inst = Instantiate(roomPrefs[0]);
        inst.transform.Find("Interiors").GetComponent<SelectInterior>().predeterminedVariant = inst.transform.Find("Interiors").GetComponent<SelectInterior>().interior[0];
        rooms.Add(inst);
        surface.BuildNavMesh();
    }
    public void ExtendGeneration(Vector3 direction, Vector3 rot, GameObject doorObj)
    {
        bool canSpawn = true;
        for(int i = 0; i < rooms.Count; i++)
        {
            if (doorObj.transform.parent.position + doorObj.transform.parent.rotation * direction == rooms[i].transform.position)
                canSpawn = false;
        }
        if (canSpawn)
        {
            doorObj.GetComponent<Animator>().SetBool("opened", true);
            PlaySoundStatic.InstSound(openSound, doorObj);
            GameObject inst = Instantiate(roomPrefs[Random.Range(0, roomPrefs.Length)]);
            direction = doorObj.transform.parent.rotation* direction;
            inst.transform.position = doorObj.transform.parent.position + direction;
            Quaternion roomRotation = Quaternion.Euler(rot.x, rot.y, rot.x);
            inst.transform.rotation = roomRotation * doorObj.transform.parent.rotation;
            inst.transform.position = new Vector3(Mathf.Round(inst.transform.position.x / increment) * increment, Mathf.Round(inst.transform.position.y / increment) * increment, Mathf.Round(inst.transform.position.z / increment) * increment);
            /*
            Debug.Log(doorObj.transform.parent.rotation);
            Debug.Log(rot);
            Debug.Log(Quaternion.Euler(roomRotation.x, roomRotation.y, roomRotation.z));
            Debug.Log(doorObj.transform.parent.rotation * roomRotation);
            */
            inst.transform.Find("FRONT_DOOR").gameObject.SetActive(false);
            rooms.Add(inst);
            surface.BuildNavMesh();
        }
        else
        {
            PlaySoundStatic.InstSound(lockedSound, doorObj);
        }

    }
}
