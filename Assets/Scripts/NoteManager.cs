using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    private LineRenderer line;
    Vector3 prevPos;

    [SerializeField]
    float minDist = 0.1f;

    [SerializeField]
    float zOffset = 10;

    [SerializeField]
    private bool draw = false;

    EdgeCollider2D edgeCollider;

    public bool isEnclosed = false;

    Vector2 startPos;

    AudioSource audioSource;

    [SerializeField]
    AudioClip clip;

    public bool played = false;

    public float collisionOffset = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

        line = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();

        audioSource = GetComponent<AudioSource>();

        prevPos = transform.position;
        prevPos.z = zOffset;
        line.positionCount = 1;

        draw = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (draw)
        {
            Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zOffset);
            Vector3 currPos = Camera.main.ScreenToWorldPoint(pos);
            currPos.z = zOffset;


            if (Vector3.Distance(prevPos, currPos) > minDist)
            {
                if (prevPos == transform.position)
                {
                    line.SetPosition(0, prevPos);
                }
                else
                {
                    line.positionCount++;
                    line.SetPosition(line.positionCount - 1, currPos);
                }
                prevPos = currPos;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            draw = false;
            SetColliders();

            if (!played)
            {
                played = true;

                AllocateSound();
                PlaySound();
            }
        }
    }

    bool CheckIfEnclossed()
    {
        RaycastHit2D hit;

        Vector3[] points = new Vector3[line.positionCount];
        line.GetPositions(points);

        Vector3 startPoint = points[points.Length - 1];
        Vector3 endpos = points[0];

        Vector3 dir = (endpos - startPoint).normalized;

        Debug.DrawRay(startPoint, dir * collisionOffset, Color.red, 5);

        hit = Physics2D.Raycast(startPoint, dir, collisionOffset);

        if (hit.collider)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void AllocateSound()
    {
        MusicDrawer drwr = transform.parent.GetComponent<MusicDrawer>();

        if (CheckIfEnclossed())
        {
            isEnclosed = true;
            line.material.color = drwr.enclosedColor;
        }

        if (isEnclosed)
        {
            int index = Random.Range(0, drwr.beepBoop.Count);
            clip = drwr.beepBoop[index];

        }
        else
        {
            int cIndex = Random.Range(0, drwr.Cat.Length);
            int set = Random.Range(0, 2);

            if (set == 0)
            {
                int index = Random.Range(0, drwr.electricBass.Count);
                clip = drwr.electricBass[index];

            }
            else
            {
                int index = Random.Range(0, drwr.drumGuitar.Count);
                clip = drwr.drumGuitar[index];
            }

            line.material.color = drwr.Cat[cIndex];
        }
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(clip, 1);

    }

    void SetColliders()
    {
        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i < line.positionCount; i++)
        {
            Vector2 temp = line.GetPosition(i);
            Vector2 pos = temp - startPos;

            points.Add(pos);
        }
        edgeCollider.SetPoints(points);
    }

    private void OnMouseDown()
    {
        PlaySound();
    }


}
