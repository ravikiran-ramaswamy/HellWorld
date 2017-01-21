//Team No Man's Pie
//Alex Freeman, Allen Chen, Maharshi Patel, Kriti Nelavelli, Ravikiran Ramaswamy

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreditsScreen : MonoBehaviour {

    Animator anim;
    // Use this for initialization

    void Start () {
        anim = GetComponent<Animator>();
        anim.SetBool("Open", true);
        GameObject go = FindFirstEnabledSelectable(this.gameObject);
        Debug.Log(go.name);
        SetSelected(go);

    }


    void OnEnable() {
        GameObject go = FindFirstEnabledSelectable(this.gameObject);
        Debug.Log(go.name);
        SetSelected(go);
    }


    private void SetSelected(GameObject go)
    {
        //Select the GameObject.
        EventSystem.current.SetSelectedGameObject(go);

        //If we are using the keyboard right now, that's all we need to do.
        var standaloneInputModule = EventSystem.current.currentInputModule as StandaloneInputModule;
        if (standaloneInputModule != null && standaloneInputModule.inputMode == StandaloneInputModule.InputMode.Buttons)
            return;

        //Since we are using a pointer device, we don't want anything selected. 
        //But if the user switches to the keyboard, we want to start the navigation from the provided game object.
        //So here we set the current Selected to null, so the provided gameObject becomes the Last Selected in the EventSystem.
        //EventSystem.current.SetSelectedGameObject(null);
    }

    private GameObject FindFirstEnabledSelectable(GameObject gameObject)
    {
        GameObject go = null;
        var selectables = gameObject.GetComponentsInChildren<Selectable>(true);
        foreach (var selectable in selectables)
        {
            if (selectable.IsActive() && selectable.IsInteractable())
            {
                go = selectable.gameObject;
                break;
            }
        }
        return go;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
