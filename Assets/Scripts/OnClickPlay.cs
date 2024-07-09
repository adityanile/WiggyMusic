using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickPlay : MonoBehaviour
{
    private Animation anim;
    
    private void Start()
    {
        anim = transform.parent.GetComponent<Animation>();
    }

    private void OnMouseDown()
    {
        anim.Play();
    }
}
