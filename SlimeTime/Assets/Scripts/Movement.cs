using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Movement : MonoBehaviour
{
    //Cameras
    [SerializeField] private GameObject _1stPersonCam;
    [SerializeField] private GameObject _3rdPersonCam;
    private CinemachineVirtualCamera _1stCam;
    private CinemachineFreeLook _3rdCam;
    bool _3rdCamOn = true;

    [SerializeField] private UnityEngine.CharacterController controller;
    [SerializeField] private Transform cam;
    [SerializeField] private Animator _animator;
    private Vector3 velocity;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;

    int oldHorizontal = 0;
    int oldVertical = 0;
    //Speed
    [SerializeField] private float defaultSpeed = 5.0f;
    [SerializeField] private float sprintSpeed = 10.0f;
    private float speed;
    //Turn
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    //Gravity
    private float gravity = -9.81f;
    private bool isGrounded;
    [SerializeField] private float jumpHeight = 10.0f;
    //Sprint
    private bool sprint = false;
    private int timeStill = 0;
    //Audio
    [SerializeField] private AudioSource shotAudio;

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
		RaycastHit raycastHit = new RaycastHit();
		isGrounded = Physics.Raycast(
			groundCheck.position,
			new Vector3(0.0f, -1.0f, 0.0f),
			out raycastHit,
			groundDistance
		);

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
        if (direction.magnitude >= 0.1f)
        {
            if (!shotAudio.isPlaying) {
                shotAudio.PlayOneShot(shotAudio.clip);
            }
            _animator.SetBool("movement", true);
            _animator.speed = speed / 2;
            //3rd camera movement
            if (_3rdCamOn)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
                Vector3 moveDir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;

                controller.Move(moveDir.normalized * speed * Time.deltaTime);//
            }
            //1st camera movement
            else
            {
                Vector3 move = transform.right * horizontal + transform.forward * vertical;
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
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
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
    }

    //Switch camera
    void cameraSwitch()
    {
        if (Input.GetKeyUp("f") && _3rdCamOn)
        {
            _3rdCamOn = false;
            _3rdCam.gameObject.SetActive(false);
            _1stCam.gameObject.SetActive(true);
        }
        else if (Input.GetKeyUp("f") && !_3rdCamOn)
        {
            _3rdCamOn = true;
            _1stCam.gameObject.SetActive(false);
            _3rdCam.gameObject.SetActive(true);
        }
    }
}