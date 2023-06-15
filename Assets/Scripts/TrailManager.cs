using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrailManager : MonoBehaviour
{
    [SerializeField] private GameObject explosionSFX;
    [SerializeField] private GameObject explosionVFX;

    private BoxCollider2D myCollider;

    // Everything here is to make sure the player isn't destroyed by it's own trail
    private void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        myCollider.enabled = false;
        Invoke(nameof(ActivateCollider), 0.3f);
    }

    private void ActivateCollider()
    {
        myCollider.enabled = true;
    }
}
