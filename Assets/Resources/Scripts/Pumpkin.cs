using UnityEngine;
using System.Collections;

public class Pumpkin : MonoBehaviour
{
    public AudioClip explosion, chaChing, oops;
    public GameObject child, text;
    private bool canCollide = true;
    public int mailboxScore = 2, carScore = 1;
    public float mailboxTimer = 1.5f, carTime = 1;


    void OnCollisionEnter(Collision collision)
    {
        if (!canCollide) return;
        canCollide = false;
        child.audio.PlayOneShot(explosion, 0.25f);

        child.particleSystem.Emit(200);

        //Points * Combo
        if (collision.gameObject.tag == "Car")
        {
            PumpkinCannon.timeLeft += carTime;
            int temp = (carScore * PumpkinCannon.combo);
            PumpkinCannon.score += temp;
            PumpkinCannon.combo += 1;
            text.GetComponent<TextMesh>().text = "+" + temp.ToString() + "p\n+" + carTime + "s";
            text.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform, Vector3.up);
            text.transform.parent = GameObject.FindGameObjectWithTag("MainCamera").transform;
            GameObject.FindGameObjectWithTag("Player").GetComponent<GoCarGo>().DestroyMe(text, 0.75f);
        }
        if (collision.gameObject.tag == "Mailbox")
        {
            PumpkinCannon.timeLeft += mailboxTimer;
            int temp = (mailboxScore * PumpkinCannon.combo);
            PumpkinCannon.score += temp;
            PumpkinCannon.combo += 2;
            text.GetComponent<TextMesh>().text = "+" + temp.ToString() + "p\n+" + mailboxTimer + "s";
            text.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform, Vector3.up);
            text.transform.parent = GameObject.FindGameObjectWithTag("MainCamera").transform;
            GameObject.FindGameObjectWithTag("Player").GetComponent<GoCarGo>().DestroyMe(text, 0.75f);
        }

        //Check Combo gain / sound
        if (collision.gameObject.tag == "Car" || collision.gameObject.tag == "Mailbox")
        {
           // collision.gameObject.tag = "House";
            child.audio.PlayOneShot(chaChing, 0.35f);

            if(collision.transform.parent != null) collision.transform.parent.gameObject.SetActive(false);
            else collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "TrashCan")
        {
            child.audio.PlayOneShot(oops, 0.4f);
            int temp = (int)(PumpkinCannon.timeLeft * 0.25f);
            PumpkinCannon.timeLeft *= 0.75f;
            PumpkinCannon.combo = 1;
            text.GetComponent<TextMesh>().color = Color.red;
            text.GetComponent<TextMesh>().text = "-" + temp.ToString() + "s\n-Combo";
            text.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform, Vector3.up);
            text.transform.parent = GameObject.FindGameObjectWithTag("MainCamera").transform;
            GameObject.FindGameObjectWithTag("Player").GetComponent<GoCarGo>().DestroyMe(text, 0.75f);
        }
        else
        {
            PumpkinCannon.combo /= 2;
            if (PumpkinCannon.combo < 1) PumpkinCannon.combo = 1;
        }

        child.transform.parent = null;

        

        GameObject.Destroy(gameObject);
    }
}
