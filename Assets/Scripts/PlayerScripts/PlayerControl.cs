using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{

    public float walkSpeed = 0.15f;
    public float maxSprintSpeed = 1.5f;
    public float maxStamina = 4.0f;
    public float sprintRechargeRate = 0.5f;
    public float slowdownThreshold = 1.0f;
    public float slowdownRate = 0.000000005f;

    public float turnSmoothing = 3.0f;
    public float aimTurnSmoothing = 15.0f;
    public float speedDampTime = 0.1f;

    public float jumpHeight = 5.0f;
    public float jumpCooldown = 1.0f;
    public bool hasChild;
    public GameObject child;

    private float timeToNextJump = 0;

    private float speed;

    private Vector3 lastDirection;

    private Animator anim;
    private int speedFloat;
    private int jumpBool;
    private int hFloat;
    private int vFloat;
    private int groundedBool;
    private Transform cameraTransform;

    private float h;
    private float v;

    private bool sprint;
    private bool fire;
    private bool pick;
    private float stamina;
    private bool isMoving;
    private float sprintSpeed;
    private float distToGround;

    void Awake()
    {
        anim = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;

        speedFloat = Animator.StringToHash("Speed");
        jumpBool = Animator.StringToHash("Jump");
        hFloat = Animator.StringToHash("H");
        vFloat = Animator.StringToHash("V");
        groundedBool = Animator.StringToHash("Grounded");
        distToGround = GetComponent<Collider>().bounds.extents.y;
        hasChild = false;
        stamina = maxStamina;
        sprintSpeed = maxSprintSpeed;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        sprint = Input.GetButton("Sprint");
        isMoving = Mathf.Abs(h) > 0.1 || Mathf.Abs(v) > 0.1;
        fire = Input.GetButtonDown("Fire1");
        pick = Input.GetButtonDown("Fire2");
        if (isSprinting())
        {
            stamina -= Time.deltaTime;
            if (stamina < 0)
            {
                stamina = 0;
            }
            if (sprintSpeed < maxSprintSpeed && stamina >= slowdownThreshold)
            {
                sprintSpeed = maxSprintSpeed;
            }
            if (stamina < slowdownThreshold)
            {
                float slowdownPercent = (slowdownThreshold - stamina) / slowdownThreshold;
                slowdownPercent = (slowdownPercent * slowdownPercent); // Makes you slowdown suddenly. 
                sprintSpeed = Mathf.Lerp(maxSprintSpeed, walkSpeed, slowdownPercent);
            }
        }
        else
        {
            stamina += Time.deltaTime * sprintRechargeRate;
            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }
        }
    }

    void FixedUpdate()
    {
        if (!globalPauseManager.isPaused())
        {
            anim.SetFloat(hFloat, h);
            anim.SetFloat(vFloat, v);

            anim.SetBool(groundedBool, IsGrounded());
            MovementManagement(h, v, sprint);
            JumpManagement();
        }
    }

    void JumpManagement()
    {
        if (GetComponent<Rigidbody>().velocity.y < 10) // already jumped
        {
            anim.SetBool(jumpBool, false);
            if (timeToNextJump > 0)
                timeToNextJump -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump"))
        {
            anim.SetBool(jumpBool, true);
            if (speed > 0 && timeToNextJump <= 0)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(0, jumpHeight, 0);
                timeToNextJump = jumpCooldown;
            }
        }
    }

    void MovementManagement(float horizontal, float vertical, bool sprinting)
    {
        Rotating(horizontal, vertical);

        if (isMoving)
        {
            if (sprinting)
            {
                speed = sprintSpeed;
            }
            else
            {
                speed = walkSpeed;
            }
            anim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
        }
        else
        {
            speed = 0f;
            anim.SetFloat(speedFloat, 0f);
        }
        GetComponent<Rigidbody>().AddForce(Vector3.forward * speed);
    }

    Vector3 Rotating(float horizontal, float vertical)
    {
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        Vector3 targetDirection;

        float finalTurnSmoothing;

        targetDirection = forward * vertical + right * horizontal;
        finalTurnSmoothing = turnSmoothing;


        if ((isMoving && targetDirection != Vector3.zero))
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            Quaternion newRotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, targetRotation, finalTurnSmoothing * Time.deltaTime);
            GetComponent<Rigidbody>().MoveRotation(newRotation);
            lastDirection = targetDirection;
        }
        if (!(Mathf.Abs(h) > 0.9 || Mathf.Abs(v) > 0.9))
        {
            Repositioning();
        }

        return targetDirection;
    }

    private void Repositioning()
    {
        Vector3 repositioning = lastDirection;
        if (repositioning != Vector3.zero)
        {
            repositioning.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(repositioning, Vector3.up);
            Quaternion newRotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);
            GetComponent<Rigidbody>().MoveRotation(newRotation);
        }
    }

    public bool IsFiring()
    {
        return fire;
    }

    public bool IsPicking()
    {
        return pick;
    }

    public bool isSprinting()
    {
        return sprint && (isMoving);
    }
}
