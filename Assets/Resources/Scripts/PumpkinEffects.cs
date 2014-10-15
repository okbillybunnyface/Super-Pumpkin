using UnityEngine;
using System.Collections;

public class PumpkinEffects : MonoBehaviour 
{
   
	// Use this for initialization
	void Start () 
    {
        StartCoroutine(destroyEffectObject());
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    IEnumerator destroyEffectObject()
    {
        yield return new WaitForSeconds(3f);
        GameObject.Destroy(this.gameObject);
    }

}
