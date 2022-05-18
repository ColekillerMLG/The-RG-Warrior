using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject Player;
    private Transform cam;
    private GameObject Triggers;
    private ListEntities Script;
    public float speed = 10;

    private void Start()
    {
        Player = GameObject.Find("Player");
        cam = this.gameObject.transform;
        Script = Player.GetComponent<ListEntities>();
        Triggers = GameObject.Find("Main Square");
    }
    private void Update()
    {
        activateLerp();
        Triggers = Script.List[Script.List.Count - 1];
    }
    void activateLerp()
    {
        cam.position = Vector2.Lerp(cam.position, Triggers.transform.position, Time.deltaTime * speed);
        cam.position = new Vector3(cam.position.x, cam.position.y, -10f);
    }
}
