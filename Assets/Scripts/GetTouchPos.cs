using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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


    //public bool testOffset;
    // Start is called before the first frame update
    void Start()
    {
        faceMat.mainTextureOffset = Vector2.zero;
        screenRes = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        Debug.Log("Resolution: Width- " + screenRes.x + " Height- " + screenRes.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (getnewTouchPos == false)
            {
                getnewTouchPos = true;
                newTouchPos = Input.GetTouch(0).position;             
            }


            touchPos = Input.GetTouch(0).position;

            textureOffset = new Vector2(-touchPos.x / screenRes.x, touchPos.y / screenRes.y);
            Debug.Log(textureOffset);   
        }
        if (Input.touchCount <= 0 && getnewTouchPos)
        {
            lastTouchPos = touchPos;
            getnewTouchPos = false;
        }
    }
}
