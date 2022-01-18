using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMovement : MonoBehaviour
{
    //Cameras
    public GameObject _1stPersonCam;
    public GameObject _3rdPersonCam;
    private CinemachineVirtualCamera _1stCam;
    private CinemachineFreeLook _3rdCam;
    bool _3rdCamOn = true;

    public UnityEngine.CharacterController controller;
    public Transform cam;
    public Animator _animator;
    private Vector3 velocity;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundStructure;
    public LayerMask groundFurniture;
    public LayerMask groundProps;

    int oldHorizontal = 0;
    int oldVertical = 0;
    //Speed
    public float defaultSpeed = 5.0f;
    public float sprintSpeed = 10.0f;
    private float speed;
    //Turn
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    //Gravity
    private float gravity = -9.81f;
    private bool isGrounded;
    public float jumpHeight = 10.0f;
    //Sprint
    private bool sprint = false;
    public float animationSpeed = 1;
    private int timeStill = 0;

    // Start is called before the first frame update
    void Start()
    {
        _1stCam = _1stPersonCam.GetComponent<CinemachineVirtualCamera>();
        _3rdCam = _3rdPersonCam.GetComponent<CinemachineFreeLook>();
        speed = defaultSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        cameraSwitch();

        //Check if can jump
        _animator.speed = speed / 2;
        if (Physics.CheckSphere(groundCheck.position, groundDistance, groundStructure))
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundStructure);
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, groundFurniture))
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundFurniture);
        else if (Physics.CheckSphere(groundCheck.position, groundDistance, groundStructure))
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundStructure);
        else
            isGrounded = false;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2.0f;

        //Get movement direction
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (oldHorizontal == 0 && horizontal != 0)
            vertical = 0;
        if (oldVertical == 0 && vertical != 0)
            horizontal = 0;
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        sprintCheck();
        transform.rotation = Quaternion.Euler(0.0f, cam.eulerAngles.y, 0.0f);
        if (direction.magnitude >= 0.1f)
        {
            _animator.SetBool("movement", true);
            //3rd camera movement
            if (_3rdCamOn)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                Vector3 moveDir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
                //Edit move to go only in for direction but based on the camera
                Vector3 move = new Vector3(Mathf.Round(moveDir.x), 0, Mathf.Round(moveDir.z));
                if (move.x != 0 && move.z != 0)
                    move.x = 0;

                controller.Move(move * speed * Time.deltaTime);//moveDir.normalized
            }
            //1st camera movement
            else
            {
                Vector3 moveDir = transform.right * horizontal + transform.forward * vertical;
                Vector3 move = new Vector3(Mathf.Round(moveDir.x), 0, Mathf.Round(moveDir.z));
                if (move.x != 0 && move.z != 0)
                    move.x = 0;
                controller.Move(move * speed * Time.deltaTime);
            }
        }
        else
        {
            timeStill++;
            //Check if is still or only changing direction
            if (timeStill > 5)
            {
                _animator.SetBool("movement", false);
                speed = defaultSpeed;
                sprint = false;
                timeStill = 0;
            }
        }

        //Jump and gravity
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    //If left control pressed sprint or run normally
    void sprintCheck()
    {
        if (Input.GetButtonDown("LCTRL") && !sprint)
        {
            speed = sprintSpeed;
            sprint = true;

        }
        else if (Input.GetButtonDown("LCTRL") && sprint)
        {
            speed = defaultSpeed;
            sprint = false;
        }
        _animator.speed = speed / 2;
    }

    //Switch camera
    void cameraSwitch()
    {
        if (Input.GetKey("f") && _3rdCamOn)
        {
            _3rdCamOn = false;
            _3rdCam.gameObject.SetActive(false);
            _1stCam.gameObject.SetActive(true);
        }
        else if (Input.GetKey("f") && !_3rdCamOn)
        {
            _3rdCamOn = true;
            _1stCam.gameObject.SetActive(false);
            _3rdCam.gameObject.SetActive(true);
        }
    }
}
