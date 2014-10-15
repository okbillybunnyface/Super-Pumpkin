using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LotGen : MonoBehaviour {

    public static float maxDist = 250f, treeChance = 0.75f, shedChance = 0.75f, carChance, mailboxChance, trashcanChance, startCarChance = 0.9f, startMailboxChance = 1f, startTrashcanChance = 0f;
    public GameObject fence;
    public Transform rearLeft, rearRight, frontLeft, frontRight;

    private GameObject playerCar, house, car, mailbox, drive0, drive1, tree, shed, trashcan;

	// Use this for initialization
	void Start () 
    {
        playerCar = GameObject.FindGameObjectWithTag("Player");
        carChance = startCarChance;
        mailboxChance = startMailboxChance;
        trashcanChance = startTrashcanChance;
	}

    void OnEnable()
    {
        
    }

	// Update is called once per frame
	void Update () 
    {
        
        float dist = (playerCar.transform.position - transform.position).magnitude;
        if (dist > maxDist)
        {
            if (house != null) house.SetActive(false);
            if (car != null) car.SetActive(false);
            if (mailbox != null) mailbox.SetActive(false);
            if (trashcan != null) trashcan.SetActive(false);
            if (drive0 != null) drive0.SetActive(false);
            if (drive1 != null) drive1.SetActive(false);
            if (tree != null) tree.SetActive(false);
            this.gameObject.SetActive(false);
        }

        //#if DEBUG
        if (Input.GetKeyDown(KeyCode.G))
        {
            Generate();
        }
        //#endif
	}

    public void Generate()
    {
        if (house != null) house.SetActive(false);
        if (car != null) car.SetActive(false);
        if (mailbox != null) mailbox.SetActive(false);
        if (trashcan != null) trashcan.SetActive(false);
        if (drive0 != null) drive0.SetActive(false);
        if (drive1 != null) drive1.SetActive(false);
        if (tree != null) tree.SetActive(false);

        Stack<Transform> spotStack = new Stack<Transform>(4);
        if (Random.value >= 0.5f)
        {
            spotStack.Push(frontLeft);
            spotStack.Push(frontRight);
        }
        else
        {
            spotStack.Push(frontRight);
            spotStack.Push(frontLeft);
        }

        if (Random.value >= 0.5f)
        {
            spotStack.Push(rearLeft);
            spotStack.Push(rearRight);
        }
        else
        {
            spotStack.Push(rearRight);
            spotStack.Push(rearLeft);
        }

        //Decides whether there is a fence or not
        fence.SetActive(Random.value >= 0.5f);

        //Create house
        GenerateHouse(spotStack.Pop());

        //Create car
        if (Random.value < carChance)
        {
            if (Random.value >= 0.5f)//Determine whether the car is in front or back
            {
                GenerateCar(spotStack.Pop(), true);
                spotStack.Pop();
            }
            else
            {
                if (Random.value < treeChance) GenerateTree(spotStack.Pop());
                else spotStack.Pop();
                GenerateCar(spotStack.Pop(), false);
            }
        }
        else
        {
            if (Random.value < treeChance) GenerateTree(spotStack.Pop());
            else spotStack.Pop();
            spotStack.Pop();
        }

        //Create mailbox
        if (Random.value < trashcanChance) GenerateTrashcan(spotStack.Pop());
        else if(Random.value < mailboxChance) GenerateMailbox(spotStack.Pop());
    }

    void GenerateHouse(Transform spot)
    {
        house = ObjectHandler.houses[Random.Range(0, 12)].Forward();
        house.SetActive(true);
        house.transform.position = spot.position;
        house.transform.rotation = transform.rotation;
        //flips house
        if (Random.value >= 0.5f)
        {
            house.transform.localScale = new Vector3(house.transform.localScale.x, house.transform.localScale.y, -house.transform.localScale.z);
        }
    }

    void GenerateTree(Transform spot)
    {
        tree = ObjectHandler.trees[Random.Range(0, 2)].Forward();
        tree.SetActive(true);
        tree.transform.position = spot.position;
        tree.transform.Rotate(Vector3.up, Random.value * 360);
    }

    void GenerateShed(Transform spot)
    {
        shed = (GameObject)GameObject.Instantiate(Resources.Load("Art/3D Models/SimpleZincGarage/Prefab/SimpleZincGarageA" + Random.Range(1, 13).ToString("D2")));
        shed.transform.position = spot.position;
        shed.transform.rotation = house.transform.parent.rotation;
    }

    void GenerateTrashcan(Transform spot)
    {
        trashcan = ObjectHandler.trashcans.Forward();
        trashcan.SetActive(true);
        trashcan.transform.position = spot.position;
        trashcan.transform.parent = this.transform;
        float num = 0.2f;
        if (Random.value >= 0.5f) num *= -1;
        trashcan.transform.localPosition = new Vector3(trashcan.transform.localPosition.x - Random.value / 3, trashcan.transform.localPosition.y, trashcan.transform.localPosition.z + num);
    }

    void GenerateMailbox(Transform spot)
    {
        mailbox = ObjectHandler.mailboxes[Random.Range(0, 2)].Forward();
        mailbox.SetActive(true);
        mailbox.transform.position = spot.position;
        mailbox.transform.parent = this.transform;
        float num = 0.2f;
        if (Random.value >= 0.5f) num *= -1;
        mailbox.transform.localPosition = new Vector3(mailbox.transform.localPosition.x - Random.value / 3, mailbox.transform.localPosition.y, mailbox.transform.localPosition.z + num);
        mailbox.transform.parent = null;
        mailbox.tag = "Mailbox";

    }

    void GenerateCar(Transform spot, bool longDrive)
    {
        car = ObjectHandler.cars[Random.Range(0, 8)].Forward();
        car.SetActive(true);
        car.transform.position = spot.position;
        drive0 = ObjectHandler.driveways.Forward();
        drive0.SetActive(true);
        drive0.transform.position = car.transform.position;
        drive0.transform.parent = this.transform;
        drive0.transform.rotation = drive0.transform.parent.rotation;
        if (longDrive)
        {
            drive1 = ObjectHandler.driveways.Forward();
            drive1.SetActive(true);
            drive1.transform.position = drive0.transform.position - drive0.transform.right * 8f;
            drive1.transform.parent = this.transform;
            drive1.transform.rotation = drive0.transform.rotation;
        }
        car.transform.rotation = transform.rotation;
        car.tag = "Car";
    }
}
