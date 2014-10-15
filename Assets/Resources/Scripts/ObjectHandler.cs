using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ObjectHandler : MonoBehaviour
{
    //Stores the objects for reuse
    public static CircularQueue<GameObject> lots;
    public static CircularQueue<GameObject> driveways;
    public static CircularQueue<GameObject> trashcans;
    public static CircularQueue<GameObject>[] trees = new CircularQueue<GameObject>[2];
    public static CircularQueue<GameObject>[] houses = new CircularQueue<GameObject>[12];
    public static CircularQueue<GameObject>[] cars = new CircularQueue<GameObject>[8];
    public static CircularQueue<GameObject>[] mailboxes = new CircularQueue<GameObject>[2];

    void Awake()
    {
        lots = new CircularQueue<GameObject>();
        for (int i = 0; i < LotGen.maxDist / 6 + 10; i++)
        {
            GameObject temp = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/Lot"));
            temp.SetActive(false);
            lots.EnQueue(temp);
        }

        driveways = new CircularQueue<GameObject>();
        for (int i = 0; i < LotGen.maxDist / 3; i++)
        {
            GameObject temp = (GameObject)GameObject.Instantiate(Resources.Load("Art/3D Models/Cars/Driveway"));
            temp.SetActive(false);
            driveways.EnQueue(temp);
        }

        trashcans = new CircularQueue<GameObject>();
        for (int i = 0; i < LotGen.maxDist / 3; i++)
        {
            GameObject temp = (GameObject)GameObject.Instantiate(Resources.Load("Art/3D Models/trash_can/TrashCan"));
            temp.SetActive(false);
            trashcans.EnQueue(temp);
        }

        //Fill each tree queue with its tree type
        trees[0] = new CircularQueue<GameObject>();
        trees[1] = new CircularQueue<GameObject>();
        for ( int j = 0; j < LotGen.maxDist / 6; j++ )
        {
            GameObject temp;

            temp = (GameObject)GameObject.Instantiate(Resources.Load("Art/3D Models/Trees/ScotsPineTypeA"));
            temp.SetActive(false);
            trees[0].EnQueue(temp);

            temp = (GameObject)GameObject.Instantiate(Resources.Load("Art/3D Models/Trees/ScotsPineTypeB"));
            temp.SetActive(false);
            trees[1].EnQueue(temp);
        }

		//Fill each house queue with the its house type
		for (int i = 0; i < houses.Length; i++)
		{
			houses[i] = new CircularQueue<GameObject>();
			for (int j = 0; j < LotGen.maxDist / 12; j++)
			{
				GameObject temp = (GameObject)GameObject.Instantiate(Resources.Load("Art/3D Models/Houses/Prefabs/House" + i.ToString("D2")));
				temp.SetActive(false);
				houses[i].EnQueue(temp);
			}
		}
        //Fill each car queue with the its car type
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i] = new CircularQueue<GameObject>();
            for (int j = 0; j < LotGen.maxDist / 12; j++)
            {
                GameObject temp = (GameObject)GameObject.Instantiate(Resources.Load("Art/3D Models/Cars/Car" + i));
                temp.SetActive(false);
                cars[i].EnQueue(temp);
            }
        }
        //Fill each mailbox queue with the its mailbox type
        for (int i = 0; i < mailboxes.Length; i++)
        {
            mailboxes[i] = new CircularQueue<GameObject>();
            for (int j = 0; j < LotGen.maxDist / 3; j++)
            {
                GameObject temp = (GameObject)GameObject.Instantiate(Resources.Load("Art/3D Models/Mailbox/Prefabs/Mailbox" + i));
                temp.SetActive(false);
                mailboxes[i].EnQueue(temp);
            }
        }
    }
}
