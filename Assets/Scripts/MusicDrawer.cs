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
    public bool spawnNote = true;

    // Touch Controls
    private Vector3 lp;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0); // get the touch

            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                lp = touch.position;

                if (spawnNote)
                {
                    Vector3 pos = GetWorldPositionOnPlane(lp, zOffset);
                    GameObject inst = Instantiate(note, pos, Quaternion.identity, gameObject.transform);
                    inst.name = count.ToString();
                    inst.AddComponent<JellyClickReceiver>();
                    allNotes.Add(inst);

                    count++;
                }
            }
        }   
    }

    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);

        return ray.GetPoint(distance);
    }

}
