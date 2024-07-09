using UnityEngine;

public class JellyClickReceiver : MonoBehaviour
{

    RaycastHit2D hit;
    Ray clickRay;

    Renderer modelRenderer;
    float controlTime;

    // Use this for initialization
    void Start()
    {
        modelRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        controlTime += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            hit = Physics2D.Raycast(clickRay.origin, clickRay.direction);

            if (hit.collider)
            {
                controlTime = 0;

                Vector3 pos = new Vector3(transform.position.x, transform.position.y, 1);
                Vector3 npos = new Vector3(hit.point.x, hit.point.y, 1);    
                modelRenderer.material.SetVector("_ModelOrigin", transform.position);
                modelRenderer.material.SetVector("_ImpactOrigin", npos);
            }

        }

        modelRenderer.material.SetFloat("_ControlTime", controlTime);
    }
}
