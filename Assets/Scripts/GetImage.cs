using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class GetImage : MonoBehaviour
{
    public Texture2D getPicture;

    public Material faceMat;

    public Slider transparencySlider;
    public GetTouchPos GetTouchPos_script;
    
    
    public Vector2 currentOffset, oldOffset, startingOffset;
    public bool getFirstTouch;
    
    [SerializeField]
    bool testGetTexture;


    [SerializeField]private MemorizePhotos _memorizePhotos;

    public string previousPath;

    public PanelSpawner PanelSpawner_script;

    public GameObject arSessionOrigin;

    public bool testSceneReset;

    [SerializeField]private GameObject canvasObj;

    public EventSystem eventSystem;
    public EventSystem currentEventSystem;

    //public GameObject arSystem;
    private ARFaceManager arFaceManager;
    
    //public GameObject[] overlays;

    public GameObject facePrefab;
    private GameObject currentFace;
    
    private int overlayNum;

    private bool hasHadTwoFingers;

    private Vector2 trueFinalOffset;

    private bool lettingFingerGo;
    private bool touchingFirstTime;
    void Awake()
    {
        arFaceManager = gameObject.AddComponent<ARFaceManager>();
        arFaceManager.maximumFaceCount = 1;
        arFaceManager.facePrefab = facePrefab;
        
        _memorizePhotos = GetComponent<MemorizePhotos>();
        overlayNum = -1;
        
        canvasObj.name = "finding";
        if (GameObject.Find("Canvas") == false)
        {
            canvasObj.name = "Canvas";
            //DontDestroyOnLoad(canvasObj);
        }
        else
        {
            Destroy(canvasObj);
        }
    }
    // Update is called once per frame

    private void FixedUpdate()
    {
        /*
        if (currentFace == null)
        {
            currentFace = GameObject.FindWithTag("FaceObject");
        }
        else if (currentFace != null)
        {
            
        }
        */
    }

    void Update()
    {
        
        if (currentEventSystem == null)
        {
            currentEventSystem = Instantiate(eventSystem);
            //DontDestroyOnLoad(currentEventSystem);
        }

        faceMat.color = new Color(faceMat.color.r,faceMat.color.g,faceMat.color.b,transparencySlider.value);
        

        if (testSceneReset)
        {
            testSceneReset = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void LateUpdate()
    {


        // Get the touch distance from newest starting point to current starting point, then add upon to last touch point
        // so it starts at the image's current position.
         
        if (currentEventSystem.currentSelectedGameObject != transparencySlider.gameObject && GetTouchPos_script.getnewTouchPos)
        {
            if (getFirstTouch == false)
            {
                getFirstTouch = true;
                startingOffset = GetTouchPos_script.textureOffset;
            }

            currentOffset = GetTouchPos_script.textureOffset;


            var finalOffset = (startingOffset - currentOffset) + oldOffset;

            // Wrap around number upon going above 1 or under -1, as texture offset number cannot go any further.
            if (finalOffset.x > 1 || finalOffset.x < -1)
            {
                finalOffset = new Vector2(Mathf.Repeat(finalOffset.x, 1.0f), finalOffset.y);
            }
        
            if (finalOffset.y > 1 || finalOffset.y < -1)
            {
                finalOffset = new Vector2(finalOffset.x, Mathf.Repeat(finalOffset.y, 1.0f));
            }


            if (GetTouchPos_script.pinchDist < 0)
                faceMat.mainTextureOffset = finalOffset;
            else
                faceMat.mainTextureOffset = finalOffset;
            
        }

        faceMat.mainTextureScale = (-GetTouchPos_script.pinchDist * Vector2.one) + (2 * Vector2.one);

        if (GetTouchPos_script.getnewTouchPos == false)
        {
            getFirstTouch = false;
            oldOffset = faceMat.mainTextureOffset;
        }       



        if (testGetTexture)
        {
            testGetTexture = false;
            SetFaceTexture(getPicture);
        }
    }
    
    public void PickImage( int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery( ( path ) =>
        {
            Debug.Log( "Image path: " + path );
            if( path != null )
            {
                // Create Texture from selected image
                var tempPic= NativeGallery.LoadImageAtPath( path, maxSize );
                if( tempPic== null )
                {
                    Debug.Log( "Couldn't load texture from " + path );
                    return;
                }

                _memorizePhotos.newPath = path;
                _memorizePhotos.beginArrayAdd = true;
                
                _memorizePhotos.thumbnailList.Add(tempPic);
                _memorizePhotos.thumbnailIteration++;
                SetCurrentImage(path, tempPic, true);

            }
        }, "Select a PNG image", "image/png" );

        Debug.Log( "Permission result: " + permission );
    }

    public void SetCurrentImage(string path, Texture2D facePicture, bool isNewPanel)
    {
        Debug.Log("SetCurrentImage() called");
        previousPath = path;
        // If a procedural texture is not destroyed manually, 
        // it will only be freed after a scene change
        //Destroy( facePicture, 5f );
        Debug.Log("facePicture: " + facePicture);
        GetTouchPos_script.textureOffset = Vector2.zero;                
        SetFaceTexture(facePicture);
        
        if (isNewPanel)
        {
            // CREDIT: https://support.unity.com/hc/en-us/articles/206486626-How-can-I-get-pixels-from-unreadable-textures-
            RenderTexture tmp = RenderTexture.GetTemporary( 
                facePicture.width,
                facePicture.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear);
        
            Graphics.Blit(facePicture, tmp);
        
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = tmp;
        
            Texture2D myTexture2D = new Texture2D(facePicture.width, facePicture.height);
            myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            myTexture2D.Apply();
        
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(tmp);
                
            TextureScale.Bilinear(myTexture2D, 300, 300);
        


            Debug.Log("Width: " + _memorizePhotos.thumbnailList[_memorizePhotos.thumbnailIteration].width +
                      " Height: " + _memorizePhotos.thumbnailList[_memorizePhotos.thumbnailIteration].height);
                

            Sprite tempSprite = Sprite.Create(myTexture2D,new Rect(0,0, 300,300), new Vector2(0.5f, 0.5f));
            
            PanelSpawner_script.thumbnailSlot = tempSprite;
            PanelSpawner_script.AddPanel();
        }


    }

    public void PanelImagePicked(string path)
    {
        // Create Texture from selected image
        var tempPic= NativeGallery.LoadImageAtPath( path, 100000000 );
        Debug.Log("received path: " + path);
        Debug.Log(tempPic);
        if( tempPic == null )
        {
            Debug.Log( "Couldn't load texture from " + path );
        }       
        else
        {
            SetCurrentImage(path, tempPic, false);
        }

    }

    public void SetFaceTexture(Texture2D facePicture)
    {
        //Transform tracked = gameObject.transform.Find("Trackables");

        faceMat.mainTextureScale = Vector2.one;
        
        ARFace[] faces = GameObject.FindObjectsOfType<ARFace>();
        if (faces != null)
        {
            foreach(ARFace face in faces)
            {
                Destroy(face.gameObject);
                Debug.Log(face.gameObject.name);
            }
        }

        DestroyImmediate(arFaceManager);
       
        arFaceManager = gameObject.AddComponent<ARFaceManager>();
        arFaceManager.maximumFaceCount = 1;
        arFaceManager.facePrefab = facePrefab;
        GetTouchPos_script.pinchDist = 1;
        
        faceMat.SetTexture("_MainTex", facePicture);
    }
}
