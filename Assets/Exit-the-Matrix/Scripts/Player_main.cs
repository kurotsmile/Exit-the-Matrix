using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum direction_player
{
    top,
    bottom,
    left,
    right
}

public class Player_main : MonoBehaviour
{
    public GameObject cam;
    public Rigidbody rig;
    public Animator anim;
    public GameObject obj_3d;
    public Transform tr_pos_start;

    public void load()
    {
        this.anim.Play("idle");
    }

    public void ready_play()
    {
        this.transform.rotation = Quaternion.identity;
        this.obj_3d.transform.localRotation = Quaternion.Euler(0f, -180f, 0f);
        this.cam.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        this.cam.transform.localPosition = new Vector3(0f, 0.72f, -2.92f);
        this.transform.position = this.tr_pos_start.position;
        this.anim.Play("idle");
    }

    public void start_play()
    {
        this.transform.rotation = Quaternion.identity;
        this.obj_3d.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        this.cam.transform.localRotation = Quaternion.Euler(21.23f, 0f, 0f);
        this.cam.transform.localPosition = new Vector3(0f, 2.53f, -2.92f);
        this.transform.position = this.tr_pos_start.position;
        this.GetComponent<PlayerMoveController>().enabled = true;
        this.anim.Play("walk");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Chest")
        {
            GameObject.Find("Game").GetComponent<Game_Handle>().show_victory();
        }

        if(other.gameObject.name== "Apple")
        {
            GameObject.Find("Game").GetComponent<Game_Handle>().add_fruit();
            GameObject.Find("Game").GetComponent<Game_Handle>().create_effect(new Vector3(other.transform.position.x, other.transform.position.y+0.2f, other.transform.position.z));
            Destroy(other.gameObject);
        }
    }

    public void stop_anim()
    {
        this.anim.enabled = false;
    }
}
