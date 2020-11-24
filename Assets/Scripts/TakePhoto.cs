using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePhoto : MonoBehaviour
{
    public GameObject[] disableOnPicture;

    public void TakePicture()
    {
        StartCoroutine(PictureDelay());
    }

    IEnumerator PictureDelay()
    {
        foreach (var obj in disableOnPicture)
        {
            obj.SetActive(false);
        }
        yield return new WaitForSecondsRealtime(0.1f);
        Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();
        yield return new WaitForSecondsRealtime(0.1f);
        foreach (var obj in disableOnPicture)
        {
            obj.SetActive(true);
        }
        
    }
    
}
