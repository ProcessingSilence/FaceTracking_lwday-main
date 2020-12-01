using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSpawner : MonoBehaviour
{
    public List<GameObject> panelList;

    public Sprite thumbnailSlot;
    
    public GameObject panelObj;

    private GameObject currentPanel;
    public int panelCounter = -1;

    public bool testAddPanel;
    public bool testAddManyPanels;

    public Transform panelParent;

    [SerializeField] private int instantiateDist = -150;

    private int currentDist;
    // Start is called before the first frame update
    void Start()
    {
        if (testAddManyPanels)
        {
            for (int i = 0; i < 10; i++)
            {
                AddPanel();
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (testAddPanel)
        {
            testAddPanel = false;
            AddPanel();
        }
    }

    public void AddPanel()
    {
        GameObject tempPanel = Instantiate(panelObj, Vector3.zero, Quaternion.identity);
        panelCounter++;
        tempPanel.transform.SetParent(panelParent);
        var tempRectTrans = tempPanel.transform as RectTransform;
        tempRectTrans.SetBottom(-1060);
        tempRectTrans.SetTop(1060);
        tempRectTrans.SetRight(panelCounter * -150);
        tempRectTrans.SetLeft(panelCounter * 150);
        tempPanel.GetComponent<Image>().sprite = thumbnailSlot;
        panelList.Add(tempPanel);
    }
}
