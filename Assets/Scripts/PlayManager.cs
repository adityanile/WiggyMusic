using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        NoteManager nM = collision.GetComponent<NoteManager>();
        nM.PlaySound();
    }
}
