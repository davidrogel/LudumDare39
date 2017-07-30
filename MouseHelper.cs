using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHelper : MonoBehaviour {

    void Start()
    {
        DontDestroyOnLoad(this);
    }

	void FixedUpdate()
    {
        MouseHitOnBath();
    }

    void MouseHitOnBath()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if(hit)
            {
                if (hit.collider.gameObject.layer == 8)
                {
                    var bath = hit.collider.gameObject.transform.parent.GetComponent<Bath>();
                    if (bath.GetInBath())
                    {
                        bath.LightON();
                    }
                }
            }
        }
    }
}
