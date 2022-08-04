using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowWithBarriers : MonoBehaviour
{
    [SerializeField] Transform startBarrier;
    [SerializeField] Transform endBarrier;
    [SerializeField] float speed = 10;
    [SerializeField] Transform followObject;

    void LateUpdate()
    {
        if (!followObject) { return; }

        Vector3 movement = new Vector3(followObject.position.x - transform.position.x, followObject.position.y - transform.position.y);

        if (transform.position.x <= startBarrier.position.x)
        {
            if (movement.x < 0)
            {
                movement.x = 0;
            }
        }
        if (transform.position.y <= startBarrier.position.y)
        {
            if (movement.y < 0)
            {
                movement.y = 0;
            }
        }
        if (transform.position.x >= endBarrier.position.x)
        {
            if (movement.x > 0)
            {
                movement.x = 0;
            }
        }
        if (transform.position.y >= endBarrier.position.y)
        {
            if (movement.y > 0)
            {
                movement.y = 0;
            }
        }

        transform.Translate((movement / 150) * speed);
    }
}
