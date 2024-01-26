using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public float widthAmount;
    [FormerlySerializedAs("panelLocation")] public Vector3 currentPanelLocation;
    
    public float easing = 0.5f;

    public bool isWholeScreen;

    public float widthAmt;

    // public PanelSpawner PanelSpawner_script;

    public float mainPercentage;

    public Text debugText;

    public Vector3 newLocationMain;

    public float additionThing;
    private int panelCount;
    private float originalAdditionThing = -10;

    public Coroutine smoothMoveCoroutine;

    [HideInInspector] public RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        currentPanelLocation = rt.anchoredPosition;
        if (isWholeScreen)
            widthAmt = Screen.width;
        foreach (Transform child in transform)
        {
            RectTransform childRT = child.GetComponent<RectTransform>();
            if (!childRT) continue;
            
            panelCount++;
            childRT.SetBottom(-1060);
            childRT.SetTop(1060);
            childRT.SetRight(panelCount * -150);
            childRT.SetLeft(panelCount * 150);
                
            smoothMoveCoroutine = null;
            Vector3 newPos = rt.anchoredPosition;
            newPos.x = panelCount * widthAmount + widthAmount;
            rt.anchoredPosition = newPos;
            additionThing = panelCount;
            newLocationMain = currentPanelLocation = new Vector3(-widthAmount * additionThing,newPos.y,newPos.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        mainPercentage = -rt.anchoredPosition.x / widthAmt;
        if (mainPercentage > panelCount)
            additionThing = panelCount;
        else if (mainPercentage < 0)
            additionThing = 0;
        else
            additionThing = Mathf.Round(mainPercentage);
    }

    public void OnDrag(PointerEventData data)
    {
        if (smoothMoveCoroutine != null)
        {
            StopCoroutine(smoothMoveCoroutine);
        }
        float difference = (data.pressPosition.x - data.position.x);
        Vector3 newPos = rt.anchoredPosition;
        newPos.x = currentPanelLocation.x - difference;
        rt.anchoredPosition = newPos;
        
        //mainPercentage = ((data.pressPosition.x - data.position.x) / widthAmt);


        if (mainPercentage * widthAmt > .5 )
        {
            //newLocationMain += new Vector3(-widthAmt*additionThing, 0 ,0);
            if (additionThing <  panelCount)
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
        Vector2 anchoredPosition = rt.anchoredPosition;
        newLocationMain = new Vector3(additionThing * widthAmt,anchoredPosition.y);

        smoothMoveCoroutine = StartCoroutine(SmoothMove(anchoredPosition, newLocationMain, easing));
        currentPanelLocation = -newLocationMain;
        
        originalAdditionThing = additionThing;
            

    }
    IEnumerator SmoothMove(Vector3 startPos, Vector3 endPos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime / seconds;
            Vector3 newPos = rt.anchoredPosition;
            newPos.x = Vector3.Lerp(startPos, -newLocationMain, Mathf.SmoothStep(0f, 1f, t)).x;
            
            rt.anchoredPosition = newPos;
            yield return null;
        }
    }
}
