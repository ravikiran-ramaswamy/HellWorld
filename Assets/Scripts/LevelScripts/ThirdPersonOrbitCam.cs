//Team No Man's Pie
//Alex Freeman, Allen Chen, Maharshi Patel, Kriti Nelavelli, Ravikiran Ramaswamy

using UnityEngine;
using System.Collections;

public class ThirdPersonOrbitCam : MonoBehaviour
{
    public Transform player;
    public Texture2D crosshair;
    public DollyCam dolly;
    public float minPickupDistance = 3.0f;
    public float dropDistance = 0.5f;
    public float dropSpeed = 1.0f;
    public Vector3 pivotOffset = new Vector3(0.0f, 1.0f, 0.0f);
    public Vector3 camOffset = new Vector3(0.0f, 0.7f, -3.0f);

    public float smooth = 10f;

    public Vector3 aimPivotOffset = new Vector3(0.0f, 1.7f, -0.3f);
    public Vector3 aimCamOffset = new Vector3(0.8f, 0.0f, -1.0f);

    public float horizontalAimingSpeed = 400f;
    public float verticalAimingSpeed = 400f;
    public float maxVerticalAngle = 30f;
    public float flyMaxVerticalAngle = 60f;
    public float minVerticalAngle = -60f;

    public float mouseSensitivity = 0.3f;

    public float sprintFOV = 100f;
    private int playerLayer;
    private PlayerControl playerControl;
    private float angleH = 0;
    private float angleV = 0;
    private Transform cam;

    private Vector3 relCameraPos;
    private float relCameraPosMag;

    private Vector3 smoothPivotOffset;
    private Vector3 smoothCamOffset;
    private Vector3 targetPivotOffset;
    private Vector3 targetCamOffset;

    private float defaultFOV;
    private float targetFOV;

    private GameObject lookedAt;
    private Rect crosshairRect;
    private GUITexture crosshairTexture;

    void Awake()
    {
        cam = transform;
        playerControl = player.GetComponent<PlayerControl>();
        dolly = gameObject.GetComponent<DollyCam>();

        playerLayer = LayerMask.NameToLayer("Player");

        relCameraPos = transform.position - player.position;
        relCameraPosMag = relCameraPos.magnitude - 0.5f;

        smoothPivotOffset = pivotOffset;
        smoothCamOffset = camOffset;

        defaultFOV = cam.GetComponent<Camera>().fieldOfView;

        lookedAt = new GameObject();
        crosshairRect = new Rect(Screen.width / 2 - (crosshair.width * 0.5f),
                                     Screen.height / 2 - (crosshair.height * 0.5f),
                                     crosshair.width, crosshair.height);
    }

    void Update()
    {
        if (!playerControl.hasChild)
        {
            RaycastHit hit;
            var cameraCenter = cam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, cam.GetComponent<Camera>().nearClipPlane));
            if (Physics.Raycast(cameraCenter, this.transform.forward, out hit, 15, ~(1 << playerLayer)))
            {
                GameObject obj = hit.transform.gameObject;
                if (lookedAt != null)
                {
                    if (lookedAt.GetComponent<Renderer>())
                        lookedAt.GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
                    if (obj.tag == "Pickable" && (obj.transform.position - player.position).magnitude <= minPickupDistance)
                    {
                        obj.GetComponent<Renderer>().material.shader = Shader.Find("Outlined/Silhouetted Diffuse");
                        if (playerControl.IsPicking())
                        {
                            pickupObject(obj);
                        }
                    }
                    if (obj.tag != "VictoryPiece")
                        lookedAt = obj;
                }
            }
        }
        else
        {
            RaycastHit hit;
            var cameraCenter = cam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, cam.GetComponent<Camera>().nearClipPlane));
            if (Physics.Raycast(cameraCenter, this.transform.forward, out hit, 15))
            {

                if (playerControl.IsFiring())
                {
                    dropObject(hit.point);
                }

            }
        }
    }

    void pickupObject(GameObject o)
    {
        o.GetComponent<NavMeshObstacle>().enabled = false;
        if (o.GetComponent<Rigidbody>())
            Destroy(o.GetComponent<Rigidbody>());
        o.transform.SetParent(player);
        o.transform.localPosition = (new Vector3(0, 1.2f, -0.1f));
        o.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        playerControl.hasChild = !playerControl.hasChild;
        playerControl.child = o;
    }

    void dropObject(Vector3 target)
    {
        GameObject objectObject = playerControl.child;
        Transform objectTransform = objectObject.transform;
        objectTransform.parent = null;
        Quaternion targetRotation = Quaternion.LookRotation(target);
        objectTransform.rotation = targetRotation;
        objectTransform.position = player.position + dropDistance * transform.forward;
        Rigidbody objectRigidbody = objectObject.AddComponent<Rigidbody>();
        objectRigidbody.velocity = objectTransform.forward * dropSpeed;
        objectObject.AddComponent<ObjectScript>();
        objectTransform.localScale = new Vector3(1, 1, 1);
        playerControl.hasChild = !playerControl.hasChild;
        objectTransform.GetComponent<NavMeshObstacle>().enabled = true;
    }

    void LateUpdate()
    {
        if (!globalPauseManager.isPaused())
        {
            angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * horizontalAimingSpeed * Time.deltaTime;
            angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * verticalAimingSpeed * Time.deltaTime;


            angleV = Mathf.Clamp(angleV, minVerticalAngle, maxVerticalAngle);



            Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
            Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
            cam.rotation = aimRotation;

            targetPivotOffset = pivotOffset;
            targetCamOffset = camOffset;

            if (playerControl.isSprinting())
            {
                targetFOV = sprintFOV;
            }
            else
            {
                targetFOV = defaultFOV;
            }
            cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(cam.GetComponent<Camera>().fieldOfView - dolly.dollyFovOffset, targetFOV, Time.deltaTime);

            // Test for collision
            Vector3 baseTempPosition = player.position + camYRotation * targetPivotOffset;
            Vector3 tempOffset = targetCamOffset;
            for (float zOffset = targetCamOffset.z; zOffset <= 0; zOffset += 0.5f)
            {
                tempOffset.z = zOffset;
                if (DoubleViewingPosCheck(baseTempPosition + aimRotation * tempOffset) || zOffset == 0)
                {
                    targetCamOffset.z = tempOffset.z;
                    break;
                }
            }



            smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, targetPivotOffset, smooth * Time.deltaTime);
            smoothCamOffset = Vector3.Lerp(smoothCamOffset - dolly.dollyCamOffset, targetCamOffset, smooth * Time.deltaTime);

            // Add in dolly zoom effect.
            // Dolly's render call is made in WorldShift. The values will be computed at this point, and only need to be read. 
            smoothCamOffset += dolly.dollyCamOffset;
            cam.GetComponent<Camera>().fieldOfView += dolly.dollyFovOffset; // I know this is redundant with line 98, but it seems better for organization. 

            cam.position = player.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;

        }

    }

    // concave objects doesn't detect hit from outside, so cast in both directions
    bool DoubleViewingPosCheck(Vector3 checkPos)
    {
        float playerFocusHeight = player.GetComponent<CapsuleCollider>().height * 0.5f;
        return ViewingPosCheck(checkPos, playerFocusHeight) && ReverseViewingPosCheck(checkPos, playerFocusHeight);
    }

    bool ViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight)
    {
        RaycastHit hit;

        // If a raycast from the check position to the player hits something...
        if (Physics.Raycast(checkPos, player.position + (Vector3.up * deltaPlayerHeight) - checkPos, out hit, relCameraPosMag))
        {
            // ... if it is not the player...
            if (hit.transform != player && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                // This position isn't appropriate.
                return false;
            }
        }
        // If we haven't hit anything or we've hit the player, this is an appropriate position.
        return true;
    }

    bool ReverseViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight)
    {
        RaycastHit hit;

        if (Physics.Raycast(player.position + (Vector3.up * deltaPlayerHeight), checkPos - player.position, out hit, relCameraPosMag))
        {
            if (hit.transform != player && hit.transform != transform && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                return false;
            }
        }
        return true;
    }

    void OnGUI()
    {
        GUI.DrawTexture(crosshairRect, crosshair);
    }
}
