using UnityEngine;

public class GetTouchPos : MonoBehaviour
{
    public Vector2 touchPos;

    public bool getNewTouchPos;
    
    private Vector2 screenRes;

    public Vector2 textureOffset;

    public float pinchDist;
    public float startingPinchDist;
    public float lastPinchDist;

    private bool getStartPinchDist = true;
    
    void Awake()
    {
        pinchDist = 1;
        lastPinchDist = 1;
        screenRes = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
    }
    
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (getNewTouchPos == false)
                getNewTouchPos = true;
            
            touchPos = Input.GetTouch(0).position;
        }
        
        textureOffset = new Vector2(-touchPos.x, touchPos.y) / screenRes;
        
        if (Input.touchCount <= 0 && getNewTouchPos)
        {
            lastPinchDist = pinchDist;
            getNewTouchPos = false;
            getStartPinchDist = true;
        }
        
        if (Input.touchCount <= 1) return;
        if (getStartPinchDist)
        {
            getStartPinchDist = false;
            startingPinchDist = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
        }
        pinchDist = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position) - startingPinchDist;

        pinchDist = pinchDist/screenRes.x * 2.5f + lastPinchDist;
    }
}
