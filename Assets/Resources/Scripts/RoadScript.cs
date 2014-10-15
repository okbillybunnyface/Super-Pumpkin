using UnityEngine;
using System.Collections.Generic;

public class RoadScript : MonoBehaviour {

    public Transform prefab;
    public int numberOfObjects;
    public float recycleOffset;
    public Vector3 startPosition;
    public float spawnWidth = 90;  //90 for Roads, 2000 for terrain

    private Vector3 nextPosition;
    private Queue<Transform> objectQueue;

    // Use this for initialization
    void Start()
    {

        objectQueue = new Queue<Transform>(numberOfObjects);

        for (int i = 0; i < numberOfObjects; i++)
        {
            objectQueue.Enqueue((Transform)Instantiate(prefab));
        }

        nextPosition = startPosition;

        for (int i = 0; i < numberOfObjects; i++)
        {
            Recycle();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //peek looks at first element w/o removing
        if (objectQueue.Peek().localPosition.x + recycleOffset < GoCarGo.distanceTraveled)
        {
            Recycle();
        }
    }

    private void Recycle()
    {
        
        Transform o = objectQueue.Dequeue();
        o.localPosition = nextPosition;
        //nextPosition.x += o.localScale.x;
        nextPosition.x += spawnWidth;
        objectQueue.Enqueue(o);

    

    }
}
