using UnityEngine;
using System.Collections;

public class logoSplash : MonoBehaviour
{

    public Texture2D logo;
    public float delay = 2;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(showLogo());

    }

    // Update is called once per frame
    void Update()
    {

    }


    IEnumerator showLogo()
    {
        yield return new WaitForSeconds(delay);
        Application.LoadLevel(1);
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(Screen.width / 2 - logo.width / 2, Screen.height / 2 - logo.height / 2, logo.width, logo.height), logo, ScaleMode.ScaleToFit);
    }
}
