//Team No Man's Pie
//Alex Freeman, Allen Chen, Maharshi Patel, Kriti Nelavelli, Ravikiran Ramaswamy

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class WorldShift : MonoBehaviour
{
    public AudioSource horrorAmbient;
    public AudioSource parallelAmbient;
    public AudioClip switchSound;
    public GameObject player;
    public GameObject parallelWorld;
    public GameObject horrorWorld;
    public GameObject parallelObjects;
    public GameObject horrorObjects;
	public GameObject flashlight;
    public GameObject enemy;
    private Vector3 worldOffset; // The difference in position of the parallel worlds. 

    public Material parallelSkybox;
    public Material horrorSkybox;

    public GameObject parallelLight;
    public GameObject horrorLight;

    public Image transitionImage;
    public Color flashColour = new Color(1f, 0f, 0f, 1f);
	public float flashSpeed = 2f;
	public ColorCorrectionCurves colorCurves;
	public float maxSaturation = 3f; 

	public GlobalFog fogScript; 
	public DollyCam dolly;

    public bool parallel = false;

    public float timerMax = 3f; // Amount of seconds you will stay in the parallel world. 
    public float timeToRecharge = 8f;
    private float timer = 0f;
    private float rechargeRate;
    private bool isSwitchRecharging;
    private bool plays;
	private float fogDistance;

    // Use this for initialization
    void Start()
    {
        timer = timerMax;
		fogDistance = fogScript.startDistance;
        RenderSettings.skybox = horrorSkybox;
//        RenderSettings.fog = true; // Uses old fog method. Uncomment fog references to re-enable. 
        worldOffset = parallelWorld.transform.position - horrorWorld.transform.position;
        dolly.maxTime = timerMax; // Make sure that the dolly effect lasts as long as the world shift. 
        transitionImage.color = Color.clear;
        plays = false;
        isSwitchRecharging = false;


		ToggleVisibility(parallel);
    }

    // Update is called once per frame
    void Update()
    {
        rechargeRate = timerMax / timeToRecharge;
        if (plays)
        {
            AudioSource.PlayClipAtPoint(switchSound, player.transform.position, 1.0f);
            enemy.SetActive(true);
            plays = !plays;
        }
        // The following set of behavior is: Press shift to enter parallel world for {Timer} seconds. 
        if (!globalPauseManager.isPaused())
        {
            if (!parallel && Input.GetButtonDown("WorldShift") && !isSwitchRecharging)
            {
                transitionImage.color = flashColour;
                parallel = true;
                enemy.SetActive(false);
                player.GetComponent<NavMeshAgent>().enabled = !(player.GetComponent<NavMeshAgent>().enabled);
                player.transform.Translate(worldOffset);
                player.GetComponent<NavMeshAgent>().enabled = !(player.GetComponent<NavMeshAgent>().enabled);
                RenderSettings.skybox = parallelSkybox;
//                RenderSettings.fog = false;

                horrorAmbient.mute = !horrorAmbient.mute;
                parallelAmbient.mute = !parallelAmbient.mute;
                AudioSource.PlayClipAtPoint(switchSound, player.transform.position, 1.0f);
                dolly.startDolly();
            }
            if (parallel)
            {
                timer -= Time.deltaTime;
                dolly.renderDolly();
            }
            if (parallel && timer <= 0f)
            {
                parallel = false;
                transitionImage.color = flashColour;
                dolly.endDolly();

                dolly.endDolly();
                player.GetComponent<NavMeshAgent>().enabled = !(player.GetComponent<NavMeshAgent>().enabled);
                player.transform.Translate(-worldOffset);
                player.GetComponent<NavMeshAgent>().enabled = !(player.GetComponent<NavMeshAgent>().enabled);
                RenderSettings.skybox = horrorSkybox;
				colorCurves.saturation = maxSaturation;
//                RenderSettings.fog = true;

                horrorAmbient.mute = !horrorAmbient.mute;
                parallelAmbient.mute = !parallelAmbient.mute;
                //timer = timerMax;
                timer = 0f;
                isSwitchRecharging = true;
                plays = true;
            }
            if (isSwitchRecharging)
            {
                timer += rechargeRate * Time.deltaTime;

				float percent = timer / timerMax;
				percent = percent * percent * percent; 
				colorCurves.saturation = Mathf.Lerp (maxSaturation, 1f, timer / timerMax);

                if (timer > timerMax)
                {
                    timer = timerMax;
                    isSwitchRecharging = false;
                }
            }
            transitionImage.color = Color.Lerp(transitionImage.color, Color.clear, flashSpeed * Time.deltaTime);
            ToggleVisibility(parallel);
        }

    }

    void ToggleVisibility(bool isParallel)
    {
        parallelWorld.SetActive(isParallel);
        horrorWorld.SetActive(!isParallel);
        parallelLight.SetActive(isParallel);
        horrorLight.SetActive(!isParallel);
        parallelObjects.SetActive(isParallel);
        horrorObjects.SetActive(!isParallel);
		flashlight.SetActive (!isParallel);
		if (isParallel) {
			fogScript.startDistance = 10000f;
		} else {
			fogScript.startDistance = fogDistance;
		}
    }

}
