using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelLocation;

    public float percentThreshold = 0.2f;

    public float easing = 0.5f;

    public bool isWholeScreen;

    public float widthAmt;

    public PanelSpawner PanelSpawner_script;

    public int currentPanel;

    public float mainPercentage;

    public Text debugText;

    public Vector3 newLocationMain;

    public float additionThing;

    private RectTransform _rt;
    // Start is called before the first frame update
    void Start()
    {
        _rt = GetComponent<RectTransform>();
        panelLocation = _rt.localPosition;
        if (isWholeScreen)
        {
            widthAmt = Screen.width;
        }
    }

    // Update is called once per frame
    void Update()
    {
        debugText.text = "Panel: " + additionThing;
    }

    public void OnDrag(PointerEventData data)
    {
        float difference = (data.pressPosition.x - data.position.x);
        _rt.localPosition = panelLocation - new Vector3(difference, 0, 0);
        _rt.localPosition = new Vector3(_rt.localPosition.x, 0, 0);
        
        mainPercentage = ((data.pressPosition.x - data.position.x) / widthAmt);

        if (mainPercentage > .5 + additionThing)
        {
            //newLocationMain += new Vector3(-widthAmt*additionThing, 0 ,0);
            additionThing++;
        }
        else if (mainPercentage < -.5 + additionThing)
        {
            //newLocationMain += new Vector3(widthAmt*additionThing, 0, 0);
            additionThing--;
        }
        //Debug.Log(data.pressPosition - data.position);
    }

    public void OnEndDrag(PointerEventData data)
    {
        //mainPercentage = (data.pressPosition.x - data.position.x) / widthAmt;
            newLocationMain = new Vector3(widthAmt * additionThing,0,0);
            StartCoroutine(SmoothMove(_rt.localPosition, newLocationMain, easing));
            panelLocation = -newLocationMain;

    }
    IEnumerator SmoothMove(Vector3 startPos, Vector3 endPos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime / seconds;
            _rt.localPosition = Vector3.Lerp(startPos, -newLocationMain, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}
