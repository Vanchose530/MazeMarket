using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSign : MonoBehaviour
{
    public Portal portal { private get; set; }

    private bool _visible;
    public bool visible
    {
        get { return _visible; }
        set
        {
            SetVisible(value);
            _visible = value;
        }
    }

    public void TeleportPlayer()
    {
        Debug.Log("TP");
        Player.instance.transform.position = new Vector3(portal.transform.position.x, portal.transform.position.y, Player.instance.transform.position.z);
    }

    void SetVisible(bool value)
    {
        gameObject.SetActive(value);
    }
}
