using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListEntities : MonoBehaviour
{
    public List<GameObject> List = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "MoveCam")
        {
            List.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MoveCam")
        {
            List.Remove(collision.gameObject);
        }
    }

}
