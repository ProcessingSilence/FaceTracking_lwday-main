using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PanelSpawner : MonoBehaviour
{
    public List<GameObject> panelList;

    public Sprite thumbnailSlot;
    
    public GameObject panelObj;

    private GameObject currentPanel;
    public int panelCounter = -1;
    
    public Transform panelParent;

    [SerializeField] private int instantiateDist = -150;

    private int currentDist;
    
    private PageSwiper pageSwiper;

    private void Awake()
    {
        pageSwiper = GetComponent<PageSwiper>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
            AddPanel();
    }

    public void AddPanel()
    {
        GameObject tempPanel = Instantiate(panelObj, transform.position, Quaternion.identity);
        panelCounter++;
        tempPanel.transform.SetParent(panelParent);
        RectTransform tempRectTrans = tempPanel.transform as RectTransform;
        tempRectTrans.SetBottom(-1060);
        tempRectTrans.SetTop(1060);
        tempRectTrans.SetRight(panelCounter * -150);
        tempRectTrans.SetLeft(panelCounter * 150);
        tempPanel.GetComponent<Image>().sprite = thumbnailSlot;
        panelList.Add(tempPanel);
        
        pageSwiper.smoothMoveCoroutine = null;
        Vector3 newPos = pageSwiper.rt.anchoredPosition;
        newPos.x = -panelList.Count * pageSwiper.widthAmt + pageSwiper.widthAmt;
        pageSwiper.rt.anchoredPosition = newPos;
        pageSwiper.additionThing = panelList.Count - 1;
        pageSwiper.newLocationMain = pageSwiper.currentPanelLocation = new Vector3(-pageSwiper.widthAmt * pageSwiper.additionThing,newPos.y,newPos.z);
    }
}
