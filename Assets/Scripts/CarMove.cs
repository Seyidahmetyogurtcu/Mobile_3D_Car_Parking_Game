using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CarMove : MonoBehaviour
{
    bool isStoped;
    float input_x, input_y, temp_y;


    public WheelCollider leftFrontWheelCollider, rightFrontWheelCollider, leftBackWheelCollider, rightBackWheelCollider;
    public Transform leftFrontWheel, rightFrontWheel, leftBackWheel, rightBackWheel;
    public float wheelTorque = 150, breakTorque = 75, maxTurnAngle = 60;
    private Rigidbody rb;
    private Vector3 centerOfMass = new Vector3(0, 0, -3f);
    //public Slider fuelTankFill;
    private float realTimeInSeconds;
    private float timer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject wonParticles, getOutOfTheGardenPartiles;
    [SerializeField] private GameObject door;
    private Vector3 vector3=new Vector3(0, 0, -0.001f);

    #region steering wheel
    //private bool wheelBeingHeld = false;
    //public RectTransform wheel;
    //private float wheelAngle = 0;
    //private float lastWheelAngle = 0;
    //private Vector2 center;
    //public float maxSteerAngle = 200f;
    //public float releaseSpeed = 300f;
    //public float output;


    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    wheelBeingHeld = true;
    //    center = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, wheel.position);
    //    lastWheelAngle = Vector2.Angle(Vector2.up, eventData.position - center);
    //}
    //public void OnDrag(PointerEventData eventData)
    //{
    //    float newAngle = Vector2.Angle(Vector2.up, eventData.position - center);
    //    if (eventData.position.x > center.x)
    //    {
    //        wheelAngle += newAngle - lastWheelAngle;
    //    }
    //    else
    //    {
    //        wheelAngle -= newAngle - lastWheelAngle;
    //    }
    //    wheelAngle = Mathf.Clamp(wheelAngle, -maxSteerAngle, maxSteerAngle);
    //    lastWheelAngle = newAngle;
    //}
    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    OnDrag(eventData);
    //    wheelBeingHeld = false;
    //}

    //private void Update()
    //{
    //    if (!wheelBeingHeld && wheelAngle != 0f)
    //    {
    //        float deltaAngle = releaseSpeed * Time.deltaTime;
    //        if (Mathf.Abs(deltaAngle) > Mathf.Abs(wheelAngle))
    //        {
    //            wheelAngle = 0;
    //        }
    //        else if (wheelAngle > 0f)
    //        {
    //            wheelAngle -= deltaAngle;
    //        }
    //        else
    //        {
    //            wheelAngle += deltaAngle;
    //        }
    //    }
    //    wheel.localEulerAngles = new Vector3(0, 0, -wheelAngle);
    //    output = wheelAngle / maxSteerAngle;
    //}
    #endregion


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
    }

    void FixedUpdate() //it spends much CPU usage ,fix it
    {
        Timer();
        GetUIMovements();
        MoveFortlift();
        turnWheelsMeshes();
        GameOver();
    }

    #region EventTrigger 
    ////Gaz Pedal
    //public void GazPedalDown()
    //{
    //    isGasSteppedOn = true;
    //}
    //public void GazPedalExit()
    //{
    //    isGasSteppedOn = false;
    //}
    //public void GazPedalEnter()
    //{
    //    isGasSteppedOn = true;
    //}
    //public void GazPedalUp()
    //{
    //    isGasSteppedOn = false;
    //}

    ////Reverse Pedal
    //public void ReversePedalDown()
    //{
    //    isReverseGasSteppedOn = true;
    //}
    //public void ReversePedalExit()
    //{
    //    isReverseGasSteppedOn = false;
    //}
    //public void ReversePedalEnter()
    //{
    //    isReverseGasSteppedOn = true;
    //}
    //public void ReversePedalUp()
    //{
    //    isReverseGasSteppedOn = false;
    //}

    ////Brake
    //public void BrakeDown()
    //{
    //    isBrakeSteppedOn = true;
    //}
    //public void BrakeExit()
    //{
    //    isBrakeSteppedOn = false;
    //}
    //public void BrakeEnter()
    //{
    //    isBrakeSteppedOn = true;
    //}
    //public void BrakeUp()
    //{
    //    isBrakeSteppedOn = false;
    //}

    ////LiftUp
    //public void LiftUpDown()
    //{
    //    isLiftUpSteppedOn = true;
    //}
    //public void LiftUpExit()
    //{
    //    isLiftUpSteppedOn = false;
    //}
    //public void LiftUpEnter()
    //{
    //    isLiftUpSteppedOn = true;
    //}
    //public void LiftUpUp()
    //{
    //    isLiftUpSteppedOn = false;
    //}

    ////LiftDown
    //public void LiftDownDown()
    //{
    //    isLiftDownSteppedOn = true;
    //}
    //public void LiftDownExit()
    //{
    //    isLiftDownSteppedOn = false;
    //}
    //public void LiftDownEnter()
    //{
    //    isLiftDownSteppedOn = true;
    //}
    //public void LiftDownUp()
    //{
    //    isLiftDownSteppedOn = false;
    //}
    #endregion

    void GetUIMovements()
    {
        //#if (UNITY_EDITOR)
        isStoped = Input.GetKey(KeyCode.Q);
        input_y = -Input.GetAxis("Vertical"); //I made my car opposite direction 
        input_x = Input.GetAxis("Horizontal");
        //#endif
#if UNITY_ANDROID
        isStoped = UIManager.singleton.isStoped;
        input_y = UIManager.singleton.input_y;
        input_x = UIManager.singleton.input_x;
#endif

    }

    //    public void Quit()
    //    {
    //#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
    //        Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
    //#endif
    //#if (UNITY_EDITOR)
    //        UnityEditor.EditorApplication.isPlaying = false;
    //#elif (UNITY_STANDALONE)
    //    Application.Quit();
    //#elif (UNITY_WEBGL)
    //    Application.OpenURL("about:blank");
    //#endif
    //    }

    void turnWheelsMeshes()
    {
        //get Colliders possition and quaternion adn set Meshs position and Quaternion
        leftFrontWheelCollider.GetWorldPose(out Vector3 pos1, out Quaternion quat1);
        leftFrontWheel.position = pos1;
        leftFrontWheel.rotation = quat1;

        rightFrontWheelCollider.GetWorldPose(out Vector3 pos2, out Quaternion quat2);
        rightFrontWheel.position = pos2;
        rightFrontWheel.rotation = quat2;

        leftBackWheelCollider.GetWorldPose(out Vector3 pos3, out Quaternion quat3);
        leftBackWheel.position = pos3;
        leftBackWheel.rotation = quat3;

        rightBackWheelCollider.GetWorldPose(out Vector3 pos4, out Quaternion quat4);
        rightBackWheel.position = pos4;
        rightBackWheel.rotation = quat4;
    }


    private float maxTime = 0.1f;
    private float minGasPress = 0.1f;
    private float fuelConsumptionPerMaxTimeSeconds = 0.001f;
    void Timer()
    {
        //Time.timeSinceLevelLoad;
        //realTimeInSeconds = Time.realtimeSinceStartup;
        //timer += Time.fixedDeltaTime;
        //if (timer >= maxTime || Mathf.Abs(input_y) > minGasPress)
        //{
        //    timer -= maxTime;
        //}

        Debug.Log((int)timer);
    }

    void MoveFortlift()
    {
        //move each whells
        leftFrontWheelCollider.motorTorque = input_y * wheelTorque;
        rightFrontWheelCollider.motorTorque = input_y * wheelTorque;
        leftBackWheelCollider.motorTorque = input_y * wheelTorque;
        rightBackWheelCollider.motorTorque = input_y * wheelTorque;

        //stop
        if (isStoped)
        {
            breakTorque = 50f;
        }
        else if (!isStoped)
        {
            breakTorque = 0f;
        }
        leftFrontWheelCollider.brakeTorque = breakTorque;
        rightFrontWheelCollider.brakeTorque = breakTorque;
        leftBackWheelCollider.brakeTorque = breakTorque;
        rightBackWheelCollider.brakeTorque = breakTorque;

        //turn front_wheels
        leftFrontWheelCollider.steerAngle = maxTurnAngle * input_x;
        rightFrontWheelCollider.steerAngle = maxTurnAngle * input_x;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("OtherCar"))
        {
                StartCoroutine(WaitForHalfSecond());
                Debug.LogWarning("hited to :" + collision.gameObject.name);
     
            StartCoroutine(RestartInThreeSecond());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //crashed,failed

        if (other.gameObject.CompareTag("GetOutOfTheGarden"))
        {
            getOutOfTheGardenPartiles.SetActive(true); 

            for (int j = 0; j < 5; j++)
            {  

                    door.transform.position += new Vector3(0,0,1f);

            }
        } 

        if (other.gameObject.CompareTag("Won"))
        {
            StartCoroutine(NextLevel());
        }
    }
    void GameOver()
    {
        Debug.Log("rb.velocity.magnitude" + rb.velocity.magnitude);
        if (transform.position.y < -5)
        {
            StartCoroutine(RestartInThreeSecond());
        }
        if (Mathf.Abs(transform.rotation.z) >= 85 && Mathf.Abs(transform.rotation.x) >= 85)
        {
            transform.rotation = Quaternion.identity;
        }
    }

    IEnumerator WaitForHalfSecond()
{
    for (int i = 0; i < 5; i++)
    {
        audioSource.Play();
        yield return new WaitForSeconds(0.2f);
        }
    }
    IEnumerator NextLevel()
    {
        wonParticles.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);//next level

    }
    IEnumerator RestartInThreeSecond()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//restart
        //GameLoader.singleton.Loading(SceneManager.GetActiveScene().buildIndex);//restart asynchronously
    }
}

