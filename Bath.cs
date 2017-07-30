using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bath : MonoBehaviour {

    Transform bathPos;
    Transform toiletPos;
    SpriteRenderer lightBath;
    float lightDurability = 1500;

    [HideInInspector]
    public bool IsOcuped { get; set; }
    bool characterIsInBath = false;

	void Start ()
    {
        bathPos = transform.GetChild(0);
        toiletPos = transform.GetChild(1);
        lightBath = transform.GetChild(2).GetComponent<SpriteRenderer>();
        IsOcuped = false;
	}

    void Update()
    {        
        if(lightBath != null && lightBath.gameObject.activeSelf && characterIsInBath)
        {
            Color color = lightBath.color;
            if(color.a <= 1)
                color.a +=  1 / lightDurability;
            if(color.a > 1)
            {
                if (toiletPos.childCount != 0)
                {
                    toiletPos.GetChild(0).gameObject.GetComponent<Character>().LeaveBath();
                }
            }
            lightBath.color = color;
        }
    }

    public Transform GetBathPos()
    {
        return bathPos;
    }

    public Transform GetToiletPos()
    {
        return toiletPos;
    }

    public void LightON()
    {
        Color color = lightBath.color;
        color.a = 0;
        lightBath.color = color;
    }

    public void LightOFF()
    {
        Color color = lightBath.color;
        color.a = 1;
        lightBath.color = color;
    }

    public void SetInBath(bool value)
    {
        characterIsInBath = value;
    }

    public bool GetInBath()
    {
        return characterIsInBath;
    }

    public void SetLightDurability(float value)
    {
        lightDurability = value;
    }
}
