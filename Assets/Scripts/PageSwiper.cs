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
    private float originalAdditionThing;

    private RectTransform _rt;

    // Start is called before the first frame update
    void Start()
    {
        _rt = GetComponent<RectTransform>();
        panelLocation = _rt.anchoredPosition;
        if (isWholeScreen)
        {
            widthAmt = Screen.width;
        }
    }

    // Update is called once per frame
    void Update()
    {
        mainPercentage = -_rt.anchoredPosition.x / widthAmt;
        if (additionThing <  PanelSpawner_script.panelCounter && additionThing > 0 )
        additionThing = Mathf.Round(mainPercentage);
        debugText.text = "Panel: " + additionThing;
        
    }

    public void OnDrag(PointerEventData data)
    {
        if (SmoothMoveC != null)
        {
            StopCoroutine(SmoothMoveC);
        }
        float difference = (data.pressPosition.x - data.position.x);
        _rt.anchoredPosition = panelLocation - new Vector3(difference, 0, 0);
        _rt.anchoredPosition = new Vector3(_rt.anchoredPosition.x, 0, 0);
        
        //mainPercentage = ((data.pressPosition.x - data.position.x) / widthAmt);


        if (mainPercentage * widthAmt > .5 )
        {
            //newLocationMain += new Vector3(-widthAmt*additionThing, 0 ,0);
            if (additionThing <  PanelSpawner_script.panelCounter)
                additionThing++;
        }
        else if (-mainPercentage * widthAmt < -.5)
        {
            //newLocationMain += new Vector3(widthAmt*additionThing, 0, 0);
            if (additionThing > 0)
                additionThing--;
        }
        //Debug.Log(data.pressPosition - data.position);
        
    }

    public void OnEndDrag(PointerEventData data)
    {
        //mainPercentage = (data.pressPosition.x - data.position.x) / widthAmt;
        newLocationMain = new Vector3(additionThing * widthAmt,0,0);

        SmoothMoveC = StartCoroutine(SmoothMove(_rt.anchoredPosition, newLocationMain, easing));
        panelLocation = -newLocationMain;
        originalAdditionThing = additionThing;

    }
    IEnumerator SmoothMove(Vector3 startPos, Vector3 endPos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime / seconds;
            _rt.anchoredPosition = Vector3.Lerp(startPos, -newLocationMain, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

    private Coroutine SmoothMoveC;
}
