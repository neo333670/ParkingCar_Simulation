using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarEntity : MonoBehaviour
{
    public GameObject wheelFrontLeft;
    public GameObject wheelFrontRight;
    public GameObject wheelBackLeft;
    public GameObject wheelBackRight;
    //Car Steering
    float m_FrontWheelAngle = 0;
    const float WHEEL_ANGLE_LIMIT = 40f;
    public float turnAngulurVelocity = 20f;
    //Accelerate and deceleration
    float m_Velocity = 0;
    public float Velocity { get { return m_Velocity; } }
    public float maxVelocity = 40f;
    public float acceleration = 3f;
    public float deceleration = 10f;

    float m_DeltaMovement = 0;
    float CarLength = 0.96f;

    void Start()
    {

    }
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)) { 
            // Speed up
            m_Velocity = Mathf.Min(maxVelocity, m_Velocity + Time.deltaTime * deceleration);
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            m_Velocity = Mathf.Max(0, m_Velocity - Time.deltaTime * deceleration);
        }

        m_DeltaMovement = m_Velocity * Time.deltaTime;
        //Update car transform 
        this.transform.Translate(Vector3.right * m_DeltaMovement);
        this.transform.Rotate(0, 0, 1/CarLength *
            Mathf.Tan(Mathf.Deg2Rad *m_FrontWheelAngle) 
            *m_DeltaMovement *Mathf.Rad2Deg);

        if (Input.GetKey(KeyCode.LeftArrow)) {
            m_FrontWheelAngle = Mathf.Clamp(
                m_FrontWheelAngle + Time.deltaTime * turnAngulurVelocity,
                -WHEEL_ANGLE_LIMIT,
                WHEEL_ANGLE_LIMIT );

            UpdateWheels();
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            m_FrontWheelAngle = Mathf.Clamp(
               m_FrontWheelAngle - Time.deltaTime * turnAngulurVelocity,
               -WHEEL_ANGLE_LIMIT,
               WHEEL_ANGLE_LIMIT );

            UpdateWheels();
        }
    }

    void UpdateWheels() {
        Vector3 localEulerAngles = new Vector3 (0f, 0f, m_FrontWheelAngle);
        wheelFrontLeft.transform.localEulerAngles = localEulerAngles;
        wheelFrontRight.transform.localEulerAngles = localEulerAngles;
    }
}