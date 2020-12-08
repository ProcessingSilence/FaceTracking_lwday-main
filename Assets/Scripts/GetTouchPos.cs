using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GetTouchPos : MonoBehaviour
{
    public Vector2 touchPos;
    
    public Vector2 newTouchPos;
    public Vector2 lastTouchPos;
    [HideInInspector]public bool getnewTouchPos;

    //[SerializeField] private Camera camera;

    [SerializeField] private Material faceMat;

    private Vector2 screenRes;

    public Vector2 textureOffset;

    public float pinchDist;
    public float startingPinchDist;
    public float lastPinchDist;

    private bool getStartPinchDist = true;

    public Text debugPinchDistText;

    private int numOne = 1;

    private bool hasHadTwoFingers;
    //public bool testOffset;
    // Start is called before the first frame update
    void Start()
    {
        pinchDist = 1;
        lastPinchDist = 1;
        faceMat.mainTextureOffset = Vector2.zero;
        screenRes = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        Debug.Log("Resolution: Width- " + screenRes.x + " Height- " + screenRes.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 /*&& Input.touchCount < 2*/)
        {
            if (getnewTouchPos == false)
            {
                getnewTouchPos = true;
                newTouchPos = Input.GetTouch(0).position;             
            }


            touchPos = Input.GetTouch(0).position;

            Debug.Log(textureOffset);   
        }
        
        textureOffset = new Vector2(((-touchPos.x) / screenRes.x), touchPos.y / screenRes.y);
        
        if (Input.touchCount <= 0 && getnewTouchPos)
        {
            lastTouchPos = touchPos;
            lastPinchDist = pinchDist;
            getnewTouchPos = false;
            getStartPinchDist = true;
        }

        

        
        if (Input.touchCount > 1)
        {
            hasHadTwoFingers = true;
            if (getStartPinchDist)
            {
                getStartPinchDist = false;
                startingPinchDist = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

            }
            var tempDist = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position) - startingPinchDist;

            tempDist = (tempDist/screenRes.x)*2.5f;
            /*
            if (tempDist <= 0)
            {
                tempDist = 0;
            }
            */

            pinchDist = (tempDist + lastPinchDist) * numOne;
            
            Debug.Log("Distance: " + Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position));

            if (debugPinchDistText)
            {
                if (debugPinchDistText.IsActive())
                {
                    debugPinchDistText.text = "pinchDist: " + pinchDist;
                }
            }
        }                  
    }
}
