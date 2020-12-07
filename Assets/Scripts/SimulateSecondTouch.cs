using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulateSecondTouch : MonoBehaviour
{
    private bool onOffSwitch;

    private Vector3 tap1, tap2;

    private float currentDist;

    private float? lastDist;

    //public RectTransform triangle;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {       
        tap1 = Input.mousePosition;
        Debug.DrawLine(new Vector3(540, 1200, 0), tap1, Color.red);

        tap2 = -tap1 + new Vector3(1080, 2400, 0);
        Debug.DrawLine(new Vector3(540, 1200, 0), tap2, Color.blue);

        currentDist = Vector3.Distance(tap1, tap2);
        Debug.Log("Distance: " + currentDist);


        if (lastDist != null)
        {
            float difference = currentDist - lastDist.Value;
            Debug.Log("Difference: " + difference);
            //triangle.sizeDelta += new Vector2(difference,difference);

        }
        
        lastDist = currentDist;
    }
}
