using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

    public enum TypeBath
    {
        MAN,
        WOMAN
    }

    public TypeBath bathType;

    public GameObject characterPrefab;

    public Transform mainPoint;

    float characterTimeBath = 0;
    int charactersCountInScene = 0;
    int maxCountWave = 2;
    int waveCount = 0;

    int numberOfWave = 0;
    float countDown = 3;
    bool startCountdown = false;
    bool waveChange = false;
    bool gameEnd = false;

    [HideInInspector]
    public float timeBetweenSpawn = 2f;

    List<GameObject> bathList = new List<GameObject>();

    public Text waveText;
    public GameObject nextWaveText;
    public GameObject theEnd;

	void Start ()
    {
        gameEnd = false;
        FillBathList();
        SetValues(400, 7, 5, 3);
        StartCoroutine(Delay(2.80f));
	}
    
    void FillBathList()
    {
        switch(bathType)
        {
            case TypeBath.MAN:
                GameObject[] bm = GameObject.FindGameObjectsWithTag("manbath");
                for(int i = 0; i < bm.Length; i++)
                {
                    bathList.Add(bm[i]);
                    bathList[i].GetComponent<Bath>().IsOcuped = false;
                }
                break;
            case TypeBath.WOMAN:
                GameObject[] bw = GameObject.FindGameObjectsWithTag("womanbath");
                for (int i = 0; i < bw.Length; i++)
                {
                    bathList.Add(bw[i]);
                    bathList[i].GetComponent<Bath>().IsOcuped = false;
                }
                break;
        }
    }
    
	void Update ()
    {
        if(waveText != null)
        {
            waveText.text = "Wave: " + (numberOfWave + 1);
        }

        if (nextWaveText != null)
        {
            if (startCountdown)
            {
                nextWaveText.SetActive(true);
                nextWaveText.transform.GetChild(0).GetComponent<Text>().text = ((int)countDown + 1).ToString();
                countDown -= Time.deltaTime;
                if (countDown <= 0)
                {
                    countDown = 3;
                }
            }
            else
            {
                nextWaveText.SetActive(false);
            }
        }
        
        if (gameEnd)
        {
            StopAllCoroutines();
            if(theEnd != null)
            theEnd.SetActive(true);
        }        
    }
    

    void InstantiateCharacter()
    {   
        // TOMAMOS UN BATH NO OCUPADO DE FORMA RANDOM
        int rand = Random.Range(0, bathList.Count);
        while (bathList[rand].GetComponent<Bath>().IsOcuped)
        {
            rand = Random.Range(0, bathList.Count);
        }

        // ASIGNAMOS AL PERSONAJE INSTANCIADO EL BATH AL QUE TIENE QUE IR Y SU TOILET
        GameObject go = Instantiate(characterPrefab, transform.position, Quaternion.identity);
        bathList[rand].GetComponent<Bath>().IsOcuped = true;
        go.GetComponent<Character>().AddPathPos(transform);
        go.GetComponent<Character>().AddPathPos(mainPoint);
        go.GetComponent<Character>().AddPathPos(bathList[rand].GetComponent<Bath>().GetBathPos());
        go.GetComponent<Character>().AddToiletPos(bathList[rand].GetComponent<Bath>().GetToiletPos());
        go.GetComponent<Character>().AddAssignedBath(bathList[rand]);
        go.GetComponent<Character>().SetTimeInBath(characterTimeBath);
        go.GetComponent<Character>().SetSpeed(Random.Range(3, 7));
        go.GetComponent<Character>().spawnScript = this;
        AddCharacterCountInScene(1);
    }

    public void AddCharacterCountInScene(int mod)
    {
        if(charactersCountInScene < 0)
        {
            charactersCountInScene = 0;
        }
        else
        {
            charactersCountInScene += mod;
        }
    }

    void SetValues(float lightDuration, float characterTimeInBath, int waveMax, float timeToSpawn)
    {
        foreach(GameObject go in bathList)
        {
            go.GetComponent<Bath>().SetLightDurability(lightDuration);
        }

        characterTimeBath = characterTimeInBath;
        waveCount = 0;
        maxCountWave = waveMax;
        charactersCountInScene = 0;
        timeBetweenSpawn = timeToSpawn;
    }

    IEnumerator Delay(float time)
    {
        countDown = 3;
        startCountdown = true;        
        yield return new WaitForSeconds(time);
        startCountdown = false;
        StartCoroutine(SpawnCharacter());
    }

    IEnumerator SpawnCharacter()
    {        
        if (waveCount < maxCountWave)
        {
            yield return new WaitForSeconds(0.5f);
            if (charactersCountInScene < bathList.Count)
            {
                InstantiateCharacter();
            }
            waveCount++;
        }
        else
        {
            if (numberOfWave < 4)
            {
                GameObject[] go = GameObject.FindGameObjectsWithTag("character");
                if (go.Length == 0)
                {
                    numberOfWave++;
                    //temporizador hasta la siguiente wave
                    countDown = 3;
                    startCountdown = true;
                    waveChange = true;
                    yield return new WaitForSeconds(2.8f);
                    startCountdown = false;
                    // seteamos la nueva wave
                    switch (numberOfWave)
                    {
                        case 1:
                            SetValues(400, 7, 5, 3);
                            break;
                        case 2:
                            SetValues(300, 9, 10, 2);
                            break;
                        case 3:
                            SetValues(250, 12, 15, 1);
                            break;
                        case 4:
                            SetValues(200, 15, 25, 0.5f);
                            break;
                    }                    
                }
            }
            else
            {
                GameObject[] go = GameObject.FindGameObjectsWithTag("character");
                if (go.Length == 0)
                {
                    gameEnd = true;
                }
            }
        }
        
        if(waveChange)
        {
            yield return new WaitForSeconds(0.01f);
            waveChange = false;
        }
        else
        {
            yield return new WaitForSeconds(timeBetweenSpawn);
        }

        StartCoroutine(SpawnCharacter());
    }    
}
