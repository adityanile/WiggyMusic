using System.Collections.Generic;
using UnityEngine;

public class MusicDrawer : MonoBehaviour
{
    [SerializeField]
    List<GameObject> allNotes = new List<GameObject>();

    public List<AudioClip> electricBass;
    public List<AudioClip> beepBoop;
    public List<AudioClip> drumGuitar;

    public Color enclosedColor;
    public Color[] Cat;

    public GameObject note;
    private int count = 1;

    public float zOffset = -0.1f;

    public float offset = 0.1f;
    private bool spawnNote = false;

    Vector3 prevPos;
    Ray ray;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            spawnNote = true;

            Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zOffset);
            prevPos = Camera.main.ScreenToWorldPoint(pos);
            prevPos.z = zOffset;
        }


        if (spawnNote)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zOffset);
                Vector3 currPos = Camera.main.ScreenToWorldPoint(pos);
                currPos.z = zOffset;

                float dist = Vector3.Distance(prevPos, currPos);

                if (dist > offset)
                {
                    spawnNote = false;

                    GameObject inst = Instantiate(note, prevPos, Quaternion.identity, gameObject.transform);
                    inst.name = count.ToString();
                    inst.AddComponent<JellyClickReceiver>();
                    allNotes.Add(inst);

                    count++;
                }

                prevPos = currPos;
            }
        }
    }


}
