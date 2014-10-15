using UnityEngine;
using System.Collections;

public class GoCarGo : MonoBehaviour {

    public static float distanceTraveled;
    public static float speed, accelPerMin = 10f, startSpeed = 5f;
    public AudioSource engine;
    public AudioClip horn;
    public static bool canPlay = true;
    public static float actualSpeed;

	// Use this for initialization
	void Start () {
	
	}

    void OnEnable()
    {
        speed = startSpeed;
        actualSpeed = speed;
        StartCoroutine(StartSound());
        StartCoroutine(HonkHonk());
    }

	// Update is called once per frame
	void Update () {
        speed += accelPerMin / 60 * Time.deltaTime;
        actualSpeed = Lerp(actualSpeed, speed + PumpkinCannon.combo / 2, 1f * Time.deltaTime);

        LotGen.carChance -= 0.005f * Time.deltaTime;
        if (LotGen.carChance < 0.5f) LotGen.carChance = 0.5f;
        LotGen.mailboxChance -= 0.002f * Time.deltaTime;
        if (LotGen.mailboxChance < 0.6f) LotGen.mailboxChance = 0.6f;
        LotGen.trashcanChance += 0.004f * Time.deltaTime;
        if (LotGen.trashcanChance > 0.4f) LotGen.trashcanChance = 0.4f;
        LotSpawn.houseChance -= 0.001f * Time.deltaTime;
        if (LotSpawn.houseChance < 0.8f) LotSpawn.houseChance = 0.8f;

        //Controls the engine pitch
        engine.pitch += 0.01f * Time.deltaTime;
        if (engine.pitch > 0.5f) engine.pitch = 0.3f;

        distanceTraveled = transform.localPosition.x;
        moveItCar();
	}

    public void moveItCar()
    {
        transform.Translate(Vector3.right * (actualSpeed) * Time.deltaTime);
    }

    IEnumerator HonkHonk()
    {
        while (true)
        {
            if (canPlay) this.audio.PlayOneShot(horn, 2f);
            yield return new WaitForSeconds(Random.Range(10f, 30f));
        }
    }

    private float Lerp(float a, float b, float f)
    {
        if (f < 0f) f = 0f;
        if (f > 1.0f) f = 1.0f;
        return a * (1.0f - f) + b * f;
    }

    IEnumerator StartSound()
    {
        engine.Play();
        audio.Play();

        while (canPlay)
        {
            yield return null;
        }

        StartCoroutine(StopSound());
    }

    IEnumerator StopSound()
    {
        engine.Stop();
        audio.Pause();

        while (!canPlay)
        {
            yield return null;
        }

        StartCoroutine(StartSound());
    }

    public void DestroyMe(GameObject doomed, float time)
    {
        StartCoroutine(DestroyMeCoroutine(doomed, time));
    }

    public static IEnumerator DestroyMeCoroutine(GameObject doomed, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject.DestroyObject(doomed);
    }
}
