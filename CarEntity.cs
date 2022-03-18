using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//hi today is a good day
public class CarEntity : MonoBehaviour
{
    public GameObject wheelFrontRight;
    public GameObject wheelFrontLeft;
    public GameObject wheelBackRight;
    public GameObject wheelBackLeft;
   
    public float Velocity { get { return m_Velocity;} }

    float m_FrontWheelAngle = 0;
    const float WHEEL_ANGLE_LIMIT = 40f;
    public float turnAngularVelocity = 20f;

    float m_Velocity = 0;
    public float acceleration = 3f;
    public float deceleration = 10f;
    public float maxVelocity = 60f;
    public float carLength = 1.14f;
    float m_DeltaMovement;


    [SerializeField] SpriteRenderer[] m_Renderers = new SpriteRenderer[5];


    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.UpArrow))  //Speed up 
        {
            m_Velocity = Mathf.Min(maxVelocity, m_Velocity + Time.fixedDeltaTime * acceleration);
        }

        if (Input.GetKey(KeyCode.DownArrow))  //Break
        {
            m_Velocity = Mathf.Max(-20 , m_Velocity - Time.fixedDeltaTime * deceleration);
        }

        m_DeltaMovement = m_Velocity * Time.fixedDeltaTime;
        
        if (Input.GetKey(KeyCode.LeftArrow))  //Turn left
        {
            m_FrontWheelAngle = Mathf.Clamp(m_FrontWheelAngle + Time.fixedDeltaTime * turnAngularVelocity, -WHEEL_ANGLE_LIMIT, WHEEL_ANGLE_LIMIT);
            UpdateWheels();
        }

        if (Input.GetKey(KeyCode.RightArrow))  //Turn right
        {
            m_FrontWheelAngle = Mathf.Clamp(m_FrontWheelAngle - Time.fixedDeltaTime * turnAngularVelocity, -WHEEL_ANGLE_LIMIT, WHEEL_ANGLE_LIMIT);
            UpdateWheels();
        }

        this.transform.Rotate(0f, 0f, 1 / carLength * Mathf.Tan(Mathf.Deg2Rad * m_FrontWheelAngle) * m_DeltaMovement * Mathf.Rad2Deg);
        this.transform.Translate(Vector3.up * m_DeltaMovement);

    }


    // test
    void UpdateWheels()
    {
        Vector3 localEulerAngles = new Vector3(0f, 0f, m_FrontWheelAngle);
        wheelFrontLeft.transform.localEulerAngles = localEulerAngles;
        wheelFrontRight.transform.localEulerAngles = localEulerAngles;
    }

    private void Stop()
    {
        m_Velocity = 0;
    }

    void ChangeColor (Color color)
    {
        foreach (SpriteRenderer s in m_Renderers)
        {
            s.color = color;
        }
    }
    void ResetColor()
    {
        ChangeColor(Color.white);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Stop();
        ChangeColor(Color.red);
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        Stop();
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        ResetColor();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        CheckPoint checkPoint = other.gameObject.GetComponent<CheckPoint>();

        if(checkPoint != null)
        {
            ChangeColor(Color.green);
            this.Invoke("ResetColor", 0.5f);
        }
    }
}
