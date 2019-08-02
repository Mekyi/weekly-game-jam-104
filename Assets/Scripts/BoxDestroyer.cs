using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDestroyer : MonoBehaviour
{
    BoxCollider2D destroyerCollider;

    // Start is called before the first frame update
    void Start()
    {
        destroyerCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destroyerCollider.IsTouchingLayers(LayerMask.GetMask("Box")))
        {
            Destroy(collision.gameObject);
        }
    }
}
