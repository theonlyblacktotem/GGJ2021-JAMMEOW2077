using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxHeight;

    public float platformSpeed;

    private float curHeight;
    private Rigidbody2D obj_Rb;
    private void Start()
    {
        obj_Rb = GetComponent<Rigidbody2D>();
        curHeight = transform.position.y;
    }
    public void MoveUp(bool isPull)
    {
        if (isPull)
        {
            if (transform.position.y <= curHeight + maxHeight)
            {
                obj_Rb.velocity = Vector2.up * platformSpeed * Time.fixedDeltaTime;
            }
            else
            {
                obj_Rb.velocity = Vector2.zero;
            }
        }
        else
        {
            if (transform.position.y >= curHeight)
            {
                obj_Rb.velocity = Vector2.up * -1* platformSpeed * Time.fixedDeltaTime;
            }
            else
            {
                obj_Rb.velocity = Vector2.zero;
            }
        }
    }
}
