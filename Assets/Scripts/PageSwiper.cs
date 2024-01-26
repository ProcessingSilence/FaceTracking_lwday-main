using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private float easing = 0.5f;
    [SerializeField] private float widthAmount;

    private int panelCount;
    private float mainPercentage;
    private float currentPanelTracker;
    private Vector3 newLocationMain;
    private Vector3 currentPanelLocation;

    private Coroutine smoothMoveCoroutine;
    private RectTransform rt;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        currentPanelLocation = rt.anchoredPosition;
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

            rt.anchoredPosition = newPos;
            currentPanelTracker = panelCount;
            newLocationMain = currentPanelLocation = new Vector3(0,newPos.y,newPos.z);
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (smoothMoveCoroutine != null)
            StopCoroutine(smoothMoveCoroutine);
        
        float difference = (data.pressPosition.x - data.position.x);
        Vector3 newPos = rt.anchoredPosition;
        newPos.x = currentPanelLocation.x - difference;
        rt.anchoredPosition = newPos;

        if (mainPercentage * widthAmount > .5 )
        {
            if (currentPanelTracker <  panelCount)
                currentPanelTracker++;
        }
        else if (-mainPercentage * widthAmount < -.5)
        {
            if (currentPanelTracker > 0)
                currentPanelTracker--;
        }
        mainPercentage = -rt.anchoredPosition.x / widthAmount;
        if (mainPercentage > panelCount)
            currentPanelTracker = panelCount;
        else if (mainPercentage < 0)
            currentPanelTracker = 0;
        else
            currentPanelTracker = Mathf.Round(mainPercentage);
    }

    public void OnEndDrag(PointerEventData data)
    {
        Vector2 anchoredPosition = rt.anchoredPosition;
        newLocationMain = new Vector3(currentPanelTracker * widthAmount,anchoredPosition.y);

        smoothMoveCoroutine = StartCoroutine(SmoothMove(anchoredPosition, -newLocationMain, easing));
        currentPanelLocation = -newLocationMain;
    }
    
    // todo: refactor code to be better written.
    IEnumerator SmoothMove(Vector3 startPos, Vector3 endPos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime / seconds;
            Vector3 newPos = startPos;
            newPos.x = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t)).x;
            
            rt.anchoredPosition = newPos;
            yield return null;
        }
    }
}
