using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Structure for Player Input Data from Arduino
/// </summary>
[System.Serializable]
public struct PlayerData
{
    public int tankRotation;
    public int modeSwitch;
    public int slideInput;
    public int headRotation;
    public int fireButton;
    
    public PlayerData(int[] values)
    {
        tankRotation = values.Length > 0 ? values[0] : 0;
        modeSwitch = values.Length > 1 ? values[1] : 0;
        slideInput = values.Length > 2 ? values[2] : 0;
        headRotation = values.Length > 3 ? values[3] : 0;
        fireButton = values.Length > 4 ? values[4] : 0;
    }
}

public class TankController : MonoBehaviour
{
    [Header("Public Variables")] 
    public int bodyRotSpeed;
    public int headRotSpeed;
    public int maxHeadRot;
    public float maxSpeed;
    public AnimationCurve speedCurve;
    public float fireCooldown;
    
    [Header("Component References")]
    public GameObject head;
    public GameObject body;
    public GameObject projectilePrefab;
    private Rigidbody rb;
    
    [Header("Faux Input")] 
    public int rotInput = 0;                    //Rotary Encoder for body rotation
    [Range(0, 1024)] public int potInput;       //Potentiometer for head rotation
    [Range(0, 128)] public int throttleInput;   //Slide-Potentiometer for forward/backward movement
    [Range(0, 128)] public int barrelInput;     //Slide-Potentiometer for determining barrel-height
    public bool fireButton;                     //Simulates whether the fire-button is pressed or not pressed

    public PlayerData playerInputData;
    public int playerNumber;

    [SerializeField] private int lastThrottle;
    [SerializeField] private int lastBarrel;
    
    //Member Variables
    private bool isFiring = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

     void Update()
    {
        //Decide which set of Input data to use for this tank
        if (playerNumber == 1)
        {
            playerInputData = SerialInputReader.Instance.p1Data;
        }
        else if (playerNumber == 2)
        {
            playerInputData = SerialInputReader.Instance.p2Data;
        }
        
        //Update only the variable currently selected with the switch
        if (playerInputData.modeSwitch == 0)
        {
            lastThrottle = playerInputData.slideInput;
        }
        else
        {
            lastBarrel = playerInputData.slideInput;
        }
    }

    private void FixedUpdate()
    {
        RotateTank();
        RotateHead();
        MoveTank();

        if (playerInputData.fireButton == 0 && !isFiring)   //For some reason the button variable is always "1" when NOT pressed
        {
            StartCoroutine(Fire());
        }
    }

    private void RotateTank()
    {
        var desiredAngle = playerInputData.tankRotation * 18;
        Quaternion targetRotation = Quaternion.Euler(0, 0, desiredAngle);
        Quaternion rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, bodyRotSpeed);
        rb.MoveRotation(rotation);
    }
    
    private void RotateHead()
    {
        var desiredAngle = Mathf.Lerp(-maxHeadRot, maxHeadRot, Mathf.InverseLerp(1024, 0, playerInputData.headRotation));
        var currentAngle = Mathf.Lerp(maxHeadRot, -maxHeadRot, Mathf.InverseLerp(-maxHeadRot, maxHeadRot, NormalizeAngle(head.transform.localEulerAngles.z)));
        var angleDiff = desiredAngle - currentAngle;
        //Debug.Log("Desired angle: " + desiredAngle + "\nangleDiff: " + angleDiff + "\nCurrent angle: " + currentAngle);
        if (Mathf.Abs(angleDiff) <= headRotSpeed)
        {
            head.transform.localEulerAngles = new Vector3(0, 0, -desiredAngle);
        }
        else
        {
            var rotationStep = Mathf.Sign(angleDiff) * headRotSpeed;    //
            head.transform.localRotation *= Quaternion.Euler(0,0,-rotationStep);
        }

    }

    private void MoveTank()
    {
        var t = lastThrottle / 1024f;
        var speed = speedCurve.Evaluate(t);
        Vector3 move = transform.up * speed * maxSpeed;
        rb.MovePosition(rb.position + move * Time.fixedDeltaTime);
        rb.linearVelocity = Vector3.zero;
    }

    private IEnumerator Fire()
    {
        isFiring = true;
        
        var projectile = Instantiate(projectilePrefab, transform.position, head.transform.rotation);
        projectile.GetComponent<Projectile>().Initialize(lastBarrel, this.gameObject);
            
        yield return new WaitForSeconds(fireCooldown);
        isFiring = false;
    }
    
    float NormalizeAngle(float angle)
    {
        angle = Mathf.Repeat(angle + 180f, 360f) - 180f;
        return angle;
    }
}
