using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float walkSpeed;
    [SerializeField] float climbSpeed;
    public bool climbing;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (climbing)
        {
            transform.Translate(Vector2.up * y * climbSpeed * Time.deltaTime);
        } else
        {
            transform.Translate(Vector2.right * x * walkSpeed * Time.deltaTime);
        }
    }
}
