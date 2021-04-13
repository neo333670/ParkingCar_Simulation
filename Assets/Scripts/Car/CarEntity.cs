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
    const float WHEEL_ANGLE_LIMIT = 30f;
    public float turnAngulurVelocity = 20f;
    //Accelerate and deceleration
    public float m_Velocity = 0;
    private float maxVelocity = 3f;
    private float acceleration = 1f;
    private float deceleration = 3f;
    public float Velocity { get { return m_Velocity; } }

    float m_DeltaMovement = 0;
    float CarLength = 0.96f;
    //GameManeger update score
    private GameManerger m_GM;
    private const int deductscore = 16;
    //gear system
    internal enum Gear {P, R, N, D}
    Gear GearInspector ;
    

    [SerializeField] SpriteRenderer[] m_renders = new SpriteRenderer[5];
    //backwardSensor
    [SerializeField] GameObject m_Radar;

    private void Start() {
        m_GM = GameObject.Find("Game Maneger").GetComponent<GameManerger>();
        //m_Radar = GameObject.Find("Radars").gameObject;
        GearInspector = Gear.P;
    }

    void ChangeColor(Color _color) {
        foreach(SpriteRenderer r in m_renders){
            r.color = _color;
        }
    }

    void ResetColor() {
        ChangeColor(Color.white);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        ChangeColor(Color.red);
        stop();
    }
    private void OnCollisionExit2D(Collision2D collision) {
        ResetColor();
    }
    void stop()
    {
        m_Velocity = 0;
    }

    void OnTriggerEnter2D(Collider2D other){
        Checkpoint checkpoint = other.gameObject.GetComponent<Checkpoint> ();
        if (checkpoint != null) {
            ChangeColor(Color.green);
            this.Invoke("ResetColor", 0.5f);
        }

        if (other.GetType() == typeof(EdgeCollider2D))
        {
            m_GM.UpdateScore(deductscore);
            TriggerEntity triggerLevel = other.gameObject.GetComponentInParent<TriggerEntity>();
            triggerLevel.AlertIconChangeRed();
            triggerLevel.alarm.Play();
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetType() == typeof(EdgeCollider2D))
        {
            TriggerEntity triggerLevel = other.gameObject.GetComponentInParent<TriggerEntity>();
            triggerLevel.AlertIconResetColor();
            triggerLevel.alarm.Stop();
            
        }

    }


    //Change GearBox
    void ChangeGearUP() {
        if((int)GearInspector < 3){
            if (GearInspector == Gear.R){
               m_Radar.gameObject.SetActive(false);
            }
            GearInspector++;
            m_GM.updateGearText(GearInspector.ToString());
            if (GearInspector == Gear.R) {
                m_Radar.gameObject.SetActive(true);
            }
        }
    }
    void ChangeGearDown() {
        if ((int)GearInspector > 0) {
            if (GearInspector == Gear.R){
                m_Radar.gameObject.SetActive(false);

            }
            GearInspector--;
            m_GM.updateGearText(GearInspector.ToString());
            if (GearInspector == Gear.R) {
                m_Radar.gameObject.SetActive(true);
            }
        }
    }

    void MoveUpControl() {
        switch (GearInspector) {
            case Gear.D:
                if (Input.GetKey(KeyCode.UpArrow)){
                    m_Velocity = Mathf.Min(maxVelocity, m_Velocity + Time.fixedDeltaTime * acceleration);
                } else {
                    m_Velocity = Mathf.Max(0, m_Velocity - deceleration * Time.fixedDeltaTime);
                }
                break;
            case Gear.R:
                if (Input.GetKey(KeyCode.UpArrow)){
                    m_Velocity = Mathf.Max(-maxVelocity, m_Velocity - Time.fixedDeltaTime * acceleration);
                } else {
                    m_Velocity = Mathf.Min(0, m_Velocity + deceleration * Time.fixedDeltaTime);
                }
                break;
        }

    }

    void TurnControl() {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            m_FrontWheelAngle = Mathf.Clamp(
                m_FrontWheelAngle + Time.fixedDeltaTime * turnAngulurVelocity,
                -WHEEL_ANGLE_LIMIT,
                WHEEL_ANGLE_LIMIT);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            m_FrontWheelAngle = Mathf.Clamp(
               m_FrontWheelAngle - Time.fixedDeltaTime * turnAngulurVelocity,
               -WHEEL_ANGLE_LIMIT,
               WHEEL_ANGLE_LIMIT);
        }
        else {
            m_FrontWheelAngle = m_FrontWheelAngle > 0 ?
                Mathf.Max(0, m_FrontWheelAngle - turnAngulurVelocity * Time.fixedDeltaTime) :
                Mathf.Min(0, m_FrontWheelAngle + turnAngulurVelocity * Time.fixedDeltaTime);
        }
        UpdateWheels();
    }

    void UpdateWheels() {
        Vector3 localEulerAngles = new Vector3 (0f, 0f, m_FrontWheelAngle);
        wheelFrontLeft.transform.localEulerAngles = localEulerAngles;
        wheelFrontRight.transform.localEulerAngles = localEulerAngles;
    }
    private void Update(){
        if (Input.GetKeyDown(KeyCode.W)){
            ChangeGearUP();
        }
        else if (Input.GetKeyDown(KeyCode.S)){
            ChangeGearDown();
        }
    }

    void FixedUpdate()
    {
        MoveUpControl();
        TurnControl();
        //Update car transform 
        m_DeltaMovement = m_Velocity * Time.deltaTime;

        this.transform.Translate(Vector3.right * m_DeltaMovement);
        this.transform.Rotate(0, 0, 1 / CarLength *
            Mathf.Tan(Mathf.Deg2Rad * m_FrontWheelAngle)
            * m_DeltaMovement * Mathf.Rad2Deg);
    }
}