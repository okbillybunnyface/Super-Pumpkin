using UnityEngine;
using System.Collections;

public class LotSpawn : MonoBehaviour 
{
    public static float houseChance, startHouseChance = 1f;
    public GameObject playerCar, lot;
	// Use this for initialization
	void Start () 
    {
        houseChance = startHouseChance;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {

        float dist = (playerCar.transform.position - transform.position).magnitude;
        if (dist < LotGen.maxDist)
        {
            if (Random.value < houseChance)
            {
                GameObject newLot = ObjectHandler.lots.Forward();
                newLot.transform.position = transform.position;
                newLot.transform.rotation = transform.rotation;
                newLot.SetActive(true);
                LotGen lotScript = newLot.GetComponent<LotGen>();
                lotScript.Generate();
            }
            transform.position += playerCar.transform.right * 12f;
        }
	}
}
