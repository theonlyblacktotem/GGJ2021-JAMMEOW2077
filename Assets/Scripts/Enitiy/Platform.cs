using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxHeight;
    public float minHeight;
    public float platformSpeed;

    private Rigidbody2D obj_Rb;
    private void Start()
    {
        obj_Rb = GetComponent<Rigidbody2D>();
    }
    public void MoveUp(bool isPull)
    {
        if (isPull)
        {
            if (transform.position.y <= maxHeight)
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
            if (transform.position.y >= minHeight)
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
