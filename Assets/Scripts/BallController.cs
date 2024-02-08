using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallController : MonoBehaviour
{
    public float initialSpeed = 6f;
    private float currentSpeed;
    private Vector3 startPosition;
    private int leftcount = 0;
    private int rightcount = 0;

    

    public TextMeshProUGUI countText;

    public GameObject LeftWinTextObject;
    public GameObject RightWinTextObject;

    private float timeUntilSurprise = 0f;
    private bool surpriseActive = false;
    public GameObject surpriseText;
    public AudioSource audioSource;


    private float stationaryTimer = 0f;

    void Start()
    {
        surpriseText = GameObject.Find("SurpriseText"); 
        if (surpriseText != null)
        {
            surpriseText.SetActive(false);
        }
        
        startPosition = transform.position;
        currentSpeed = initialSpeed;

        LaunchBall();

        audioSource = GetComponent<AudioSource>();

        leftcount = 0;
        rightcount = 0;

        SetCountText();

        LeftWinTextObject.SetActive(false);
        RightWinTextObject.SetActive(false);

       

    
    }

   void Update()
{
    CheckGoals();

    if (!surpriseActive && Time.time >= timeUntilSurprise)
    {
        ActivateSurpriseEvent();
    }

    if (GetComponent<Rigidbody>().velocity.magnitude >= 2f && surpriseActive)
    {
        surpriseActive = false;
        surpriseText.SetActive(false);
        ResetSurpriseTimer();
    }

    if (GetComponent<Rigidbody>().velocity.magnitude < 2f)
    {
        stationaryTimer += Time.deltaTime;

        if (stationaryTimer >= 0.5f && stationaryTimer < 1.5f)
        {
            if (!surpriseActive)
            {
                ActivateSurpriseEvent();
            }
        }
        else if (stationaryTimer >= 1.5f)
        {
            surpriseActive = false;
            surpriseText.SetActive(false);
            ResetSurpriseTimer();
        }
    }
    else
    {
        stationaryTimer = 0f;
    }
}

    void ActivateSurpriseEvent()
{
    surpriseActive = true;
    surpriseText.SetActive(true);
    audioSource.Play();

    GetComponent<Rigidbody>().velocity = Vector3.zero;
    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

    StartCoroutine(RestartBallAfterDelay());
}


IEnumerator RestartBallAfterDelay()
{
   
    yield return new WaitForSeconds(1.5f);

    if (transform.position.x < 0)
    {
        GetComponent<Rigidbody>().AddForce(Vector3.right * 10f, ForceMode.VelocityChange);
    }
    else
    {
        GetComponent<Rigidbody>().AddForce(Vector3.left * 10f, ForceMode.VelocityChange);
    }

    
    surpriseActive = false;
    surpriseText.SetActive(false);

}

void ResetSurpriseTimer()
{
    timeUntilSurprise = Time.time + Random.Range(20f, 40f);
}

            
    

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Panel") )
        {
            BounceAndAccelerate(collision.contacts[0].normal);
        }
        if (collision.gameObject.CompareTag("Border"))
        {
            Bounce(collision.contacts[0].normal);
        }
    }

    void CheckGoals()
    {
        if (transform.position.x < -11f)
        {
            ScorePoint("RightPlayer");
            
        }
        else if (transform.position.x > 10.5f) 
        {
            ScorePoint("LeftPlayer");
        }
    }

    void ScorePoint(string playerTag)
    {
        
        transform.position = startPosition;
        currentSpeed = initialSpeed;

        Debug.Log(playerTag + " scores a point!");

        if (playerTag == "LeftPlayer")
        {           
            leftcount = leftcount + 1;
        }
        else
        {
            rightcount = rightcount + 1;
        }

        SetCountText();

        LaunchBall();
    }

    void BounceAndAccelerate(Vector3 collisionNormal)
    {
        float maxAngle = 10f;
        float randomAngle = Random.Range(1f, maxAngle);
    
        Vector3 reflectedVelocity = Vector3.Reflect(GetComponent<Rigidbody>().velocity, collisionNormal);

        
        Quaternion rotation = Quaternion.AngleAxis(randomAngle, Vector3.forward);
        reflectedVelocity = rotation * reflectedVelocity;

        GetComponent<Rigidbody>().velocity = reflectedVelocity.normalized * currentSpeed;

        currentSpeed += 1.5f; //
    }

    void Bounce(Vector3 collisionNormal)
    {
        float maxAngle = 10f;
        float randomAngle = Random.Range(1f, maxAngle);
    
        Vector3 reflectedVelocity = Vector3.Reflect(GetComponent<Rigidbody>().velocity, collisionNormal);
       
        Quaternion rotation = Quaternion.AngleAxis(randomAngle, Vector3.forward);
        reflectedVelocity = rotation * reflectedVelocity;

        GetComponent<Rigidbody>().velocity = reflectedVelocity.normalized * currentSpeed;

    }




    void LaunchBall()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 initialDirection = new Vector3(Random.Range(0.5f, 1f), Random.Range(-1f, 1f), 0f).normalized;

        rb.velocity = initialDirection * currentSpeed;
        rb.angularVelocity = Vector3.zero;
        rb.freezeRotation = true;
    }

    void SetCountText() 
    {
        countText.text = leftcount.ToString() + "            " + rightcount;

        if (leftcount >= 11)
        {
            LeftWinTextObject.SetActive(true);
            currentSpeed = 0f;
        }
        if(rightcount >= 11)
        {
            RightWinTextObject.SetActive(true);
            currentSpeed = 0f;
        }
    }
    
}
