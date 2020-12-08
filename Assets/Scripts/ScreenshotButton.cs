using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotButton : MonoBehaviour
{
    private Image ButtonImage;
    public Image[] disableObjs;
    public Text ButtonText;
    public GameObject[] disableObjs2;
    public void OnOff(bool boolSwitch)
    {
        if (boolSwitch)
        {GetComponent<Image>().color = new Color(0,0,0,136);}
        else
        {
            GetComponent<Image>().color = new Color(0,0,0,0);
        }
        foreach (var VARIABLE in disableObjs)
        {
            VARIABLE.enabled = boolSwitch;
        }
        foreach (var VARIABLE in disableObjs2)
        {
            VARIABLE.SetActive(boolSwitch);
        }

        ButtonText.enabled = boolSwitch;
    }
    
    public void TakePicture()
    {
        OnOff(false);
        StartCoroutine(TakeScreenshotAndSave());
    }

    private IEnumerator TakeScreenshotAndSave()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D( Screen.width, Screen.height, TextureFormat.RGB24, false );
        ss.ReadPixels( new Rect( 0, 0, Screen.width, Screen.height ), 0, 0 );
        ss.Apply();

        // Save the screenshot to Gallery/Photos
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery( ss, "GalleryTest", "Image.png", ( success, path ) => Debug.Log( "Media save result: " + success + " " + path ) );

        Debug.Log( "Permission result: " + permission );

        // To avoid memory leaks
        Destroy( ss );
        OnOff(true);
    }
}
