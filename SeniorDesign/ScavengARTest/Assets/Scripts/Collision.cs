using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    public GameObject btn;
    public List<string> collided_names = new List<string>();
    private int currentCollisions = 0;
    //public static List<string> ObjectNames = new List<string>();

    // Use this for initialization
    void Start()
    {
        //btn.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name=="CollisionRadius")
        {
            currentCollisions++;
            collided_names.Add(other.transform.parent.name);
            btn.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.name=="CollisionRadius")
        {
            currentCollisions--;
            collided_names.Remove(other.transform.parent.name);
            if(currentCollisions == 0)
            {
                btn.SetActive(false);
            }
        }
    }

    public List<string> getCurrentCollisions()
    {
        return collided_names;
    }

    public void RemoveCollision(string name)
    {
        collided_names.Remove(name);
        currentCollisions--;
        if(currentCollisions == 0)
        {
            btn.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
