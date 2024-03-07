using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_change_direction : MonoBehaviour
{
    public direction_player direction_in;
    public direction_player direction_ou;
    public GameObject obj_arrow;

    private void Start()
    {
        this.name = "obj_rotate";
        this.check_direction_out();
    }

    private void check_direction_out()
    {
        if (direction_ou == direction_player.top) obj_arrow.transform.localRotation = Quaternion.Euler(0, 90, 0);
        if (direction_ou == direction_player.bottom) obj_arrow.transform.localRotation = Quaternion.Euler(0, -90, 0);
        if (direction_ou == direction_player.right) obj_arrow.transform.localRotation = Quaternion.Euler(0, -180, 0);
        if (direction_ou == direction_player.left) obj_arrow.transform.localRotation = Quaternion.Euler(0, 0, 0);

    }

    public void show_arrow()
    {
        this.obj_arrow.SetActive(true);
    }

    public void hide_arrow()
    {
        this.obj_arrow.SetActive(false);
    }

    public void change_direction()
    {
        direction_player d_temp = this.direction_in;
        this.direction_in = this.direction_ou;
        this.direction_ou = d_temp;
        this.check_direction_out();
    }
}
