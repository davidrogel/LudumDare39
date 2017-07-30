using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public enum CharacterState
    {
        GoToBath,
        Bathing,
        BackToExit
    }
    [HideInInspector]
    public CharacterState state;

    private float speed = 3;

    Transform toiletPos = null;

    List<Transform> path = new List<Transform>();
    int pathCountStep = 0;

    bool inBath = false;
    float timeInBath = 10;
    float timer = 0;

    bool outToilet = true;

    [HideInInspector]
    public Spawner spawnScript;

    GameObject assignedBath;
    GameController controllerScript;

    void Start ()
    {
        state = CharacterState.GoToBath;
        controllerScript = GameObject.Find("GameManager").GetComponent<GameController>();
	}
	
	void Update ()
    {
		switch(state)
        {
            case CharacterState.GoToBath:
                GoToBath();
            break;

            case CharacterState.Bathing:
                Bathing();
            break;

            case CharacterState.BackToExit:
                BackToExit();
            break;
        }
	}

    void GoToBath()
    {
        if (pathCountStep >= path.Count)
        {
            pathCountStep = 0;
            inBath = true;
            assignedBath.GetComponent<Bath>().SetInBath(true);
            assignedBath.GetComponent<Bath>().LightON();
            state = CharacterState.Bathing;
        }
        else
        {
            float z = Mathf.Atan2((path[pathCountStep].position.y - transform.position.y), (path[pathCountStep].position.x - transform.position.x)) * Mathf.Rad2Deg - 90;
            transform.eulerAngles = new Vector3(0, 0, z);

            transform.position = Vector2.MoveTowards(transform.position, path[pathCountStep].position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, path[pathCountStep].position) < 0.1f)
            {
                pathCountStep++;
            }
        }
    }

    void Bathing()
    {
        if(inBath)
        {
            transform.position = toiletPos.position;
            transform.parent = toiletPos;
            transform.eulerAngles = toiletPos.eulerAngles - new Vector3(0, 0, 90);
            
            timer += Time.deltaTime;
            if(timer >= timeInBath)
            {
                controllerScript.AddPoint();
                timer = 0;
                inBath = false;
            }
        }
        else
        {
            // sale del bath
            LeaveBath();
        }
        
    }
   
    void BackToExit()
    {
        if(outToilet)
        {
            transform.parent = null;
            transform.position = path[2].position;
            pathCountStep = path.Count - 1;
            outToilet = false;
        }

        if (pathCountStep < 0)
        {
            pathCountStep = 0;
            GoDestroy();
        }
        else
        {
            float z = Mathf.Atan2((path[pathCountStep].position.y - transform.position.y), (path[pathCountStep].position.x - transform.position.x)) * Mathf.Rad2Deg - 90;
            transform.eulerAngles = new Vector3(0, 0, z);

            transform.position = Vector2.MoveTowards(transform.position, path[pathCountStep].position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, path[pathCountStep].position) < 0.1f)
            {
                pathCountStep--;
            }
        }
    }

    void GoDestroy()
    {        
        spawnScript.AddCharacterCountInScene(-1);
        Destroy(this.gameObject);
    }

    public void AddPathPos(Transform pos)
    {
        path.Add(pos);
    }

    public void AddToiletPos(Transform pos)
    {
        toiletPos = pos;
    }

    public void AddAssignedBath(GameObject bath)
    {
        assignedBath = bath;
    }

    public void SetTimeInBath(float time)
    {
        timeInBath = time;
    }

    public bool CheckIsInBath()
    {
        return inBath;
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }

    public void LeaveBath()
    {
        assignedBath.GetComponent<Bath>().LightOFF();
        assignedBath.GetComponent<Bath>().IsOcuped = false;
        assignedBath.GetComponent<Bath>().SetInBath(false);
        state = CharacterState.BackToExit;
    }
}
