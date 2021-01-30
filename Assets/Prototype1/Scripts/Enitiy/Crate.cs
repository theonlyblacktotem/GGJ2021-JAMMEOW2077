using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : InteractableObject
{
    // Start is called before the first frame update
    private Vector3 offset = new Vector3(0.4f, 0,0);
    // left,right,up
    public bool isActive;
    [SerializeField] private GameObject[] Boarder;
    [SerializeField] private RaycastHit2D[] Ray;
    void Awake()
    {
        Boarder = new GameObject[3];
        Ray = new RaycastHit2D[3];
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        Ray[0] = Physics2D.Raycast(transform.position - offset, Vector2.left, 0.25f, LayerMask.GetMask("Crate"));
        Ray[1] = Physics2D.Raycast(transform.position + offset, Vector2.right, 0.25f, LayerMask.GetMask("Crate"));
        Ray[2] = Physics2D.Raycast(transform.position + new Vector3(0, 0.4f, 0), Vector2.up, 0.25f, LayerMask.GetMask("Crate"));
        for (int i = 0; i < 3; i++)
        {
            // If there is something in Ray[i] and Boarder[i] and they ara not the same thing deactive it
            if(Ray[i] && Boarder[i] && Ray[i].transform.gameObject != Boarder[i])
            {
                Boarder[i].GetComponent<Crate>().DeActiveCrate();
                Boarder[i] = null;
            }
            if (CheckCrate())
            {
                if (Ray[i] && Ray[i].transform.gameObject != this.gameObject && Ray[i].collider.tag == "Crate")
                {
                    GameObject currentbox = Ray[i].collider.transform.gameObject;
                    RaycastHit2D currentRay = currentbox.GetComponent<Crate>().getRay(i);
                    Boarder[i] = currentbox;
                    currentbox.GetComponent<Crate>().ActiveCrate();
                    while (currentRay && currentbox != currentRay.transform.gameObject && currentRay.collider.tag == "Crate")
                    {
                        currentbox = currentRay.collider.gameObject;
                        currentRay = currentbox.GetComponent<Crate>().getRay(i);
                        Boarder[i] = currentbox;
                        currentbox.GetComponent<Crate>().ActiveCrate();
                    }
                }
                if (Ray[2])
                {
                    Ray[2].transform.position = new Vector2(transform.position.x, Ray[2].transform.position.y);
                }
            }
        }
        Debug.DrawRay(transform.position - offset, Vector2.left * 0.25f, Color.red);
        Debug.DrawRay(transform.position + offset, Vector2.right * 0.25f, Color.red);
        Debug.DrawRay(transform.position + new Vector3(0, 0.4f, 0), Vector2.up * 0.25f, Color.red);
    }
    RaycastHit2D getRay(int idx)
    {
        return Ray[idx];
    }
    public void ActiveCrate()
    {
        isActive = true;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        GetComponent<SpriteRenderer>().color = Color.red;
    }
    public void DeActiveCrate()
    {
        isActive = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        GetComponent<SpriteRenderer>().color = Color.white;

    }
    public bool CheckCrate()
    {
        return isActive;
    }
}
