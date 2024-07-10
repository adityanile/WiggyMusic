using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    // Touch Controls
    private Vector3 fp;
    private Vector3 lp;
    private float dragDistance;

    private MusicDrawer drwr;

    // Start is called before the first frame update
    void Start()
    {
        dragDistance = Screen.height * 15 / 100;

        startPos = transform.position;

        line = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();

        audioSource = GetComponent<AudioSource>();

        fp = Input.GetTouch(0).position;
        lp = Input.GetTouch(0).position;

        prevPos = GetWorldPositionOnPlane(lp, zOffset);
        
        line.positionCount = 1;
        line.SetPosition(0, prevPos);
        
        draw = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (draw)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    lp = touch.position;
                    prevPos = GetWorldPositionOnPlane(lp, zOffset);

                    line.positionCount++;
                    line.SetPosition(line.positionCount - 1, prevPos);
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    lp = touch.position;

                    if (line.positionCount < 2)
                        Destroy(gameObject);

                    SetColliders();

                    if (!played)
                    {
                        played = true;

                        AllocateSound();
                        PlaySound();
                    }
                    draw = false;
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
        drwr = transform.parent.GetComponent<MusicDrawer>();

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
        drwr.spawnNote = false;
        PlaySound();
    }

    private void OnMouseUp()
    {
        drwr.spawnNote = true;
    }

}
