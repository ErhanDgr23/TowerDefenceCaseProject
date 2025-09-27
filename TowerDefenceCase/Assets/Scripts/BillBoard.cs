using UnityEngine;

public class Billboard : MonoBehaviour {

    [SerializeField] bool OneTime;

    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;

        if(OneTime)
        {
            transform.forward = _cam.transform.forward;
            this.enabled = false;
        }
    }

    void LateUpdate()
    {
        if (_cam == null) return;

        transform.forward = _cam.transform.forward;
    }
}
