//Team No Man's Pie
//Alex Freeman, Allen Chen, Maharshi Patel, Kriti Nelavelli, Ravikiran Ramaswamy

using UnityEngine;
using System.Collections;

public class OpeningScript : MonoBehaviour {

    public Animator anim;
    private int m_OpenParameterId;
    const string k_OpenTransitionName = "Open";
    const string k_ClosedStateName = "Closed";
    private IEnumerator coroutine;
    void Start()
    {
        m_OpenParameterId = Animator.StringToHash(k_OpenTransitionName);
        anim.gameObject.SetActive(true);
        anim.transform.SetAsLastSibling();
        anim.SetBool(m_OpenParameterId, true);

    }

    // Update is called once per frame
    void Update () {
        if (Time.time > 5)
            anim.SetBool(m_OpenParameterId, false);
    }
}
