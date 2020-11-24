using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class GetImage : MonoBehaviour
{
    public Texture2D getPicture;

    public Material faceMat;

    public MeshRenderer faceMesh;

    public Slider transparencySlider;
    public Button screenshotButton;
    public GetTouchPos GetTouchPos_script;
    
    
    public Vector2 currentOffset, oldOffset, startingOffset;
    public bool getFirstTouch;
    
    [SerializeField]
    bool testGetTexture;

    
    private EventSystem _eventSystem;

    [SerializeField]private MemorizePhotos _memorizePhotos;

    public string previousPath;

    public PanelSpawner PanelSpawner_script;
    void Awake()
    {
        _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        _memorizePhotos = GetComponent<MemorizePhotos>();
    }
    // Update is called once per frame

    void Update()
    {
        if (faceMesh != null)
        {
            faceMesh.material.color = new Color(faceMesh.material.color.r,faceMesh.material.color.g,faceMesh.material.color.b,transparencySlider.value);
        }
    }

    void LateUpdate()
    {
        // Get the touch distance from newest starting point to current starting point, then add upon to last touch point
        // so it starts at the image's current position.
        if (faceMesh != null)
        {
            if (EventSystem.current.currentSelectedGameObject != transparencySlider.gameObject && GetTouchPos_script.getnewTouchPos)
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

                faceMesh.material.mainTextureOffset = finalOffset;
            }

            if (GetTouchPos_script.getnewTouchPos == false)
            {
                getFirstTouch = false;
                oldOffset = faceMesh.material.mainTextureOffset;
            }
        }



        if (testGetTexture)
        {
            testGetTexture = false;
            SetFaceTexture();
        }
    }
    
    public void PickImage( int maxSize )
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery( ( path ) =>
        {
            Debug.Log( "Image path: " + path );
            if( path != null )
            {
                // Create Texture from selected image
                getPicture = NativeGallery.LoadImageAtPath( path, maxSize );
                if( getPicture == null )
                {
                    Debug.Log( "Couldn't load texture from " + path );
                    return;
                }
                

                _memorizePhotos.newPath = path;
                _memorizePhotos.beginArrayAdd = true;

                //var thumbnail = getPicture.GetRawTextureData();
                
                /*
                byte[] tmp = getPicture.GetRawTextureData();
                Texture2D tmpTexture = new Texture2D(getPicture.width,getPicture.height);
                tmpTexture.LoadRawTextureData(tmp);
                TextureScale.Bilinear(tmpTexture, 50, 50);
                */
                
                // CREDIT: https://support.unity.com/hc/en-us/articles/206486626-How-can-I-get-pixels-from-unreadable-textures-
                RenderTexture tmp = RenderTexture.GetTemporary( 
                    getPicture.width,
                    getPicture.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);
                Graphics.Blit(getPicture, tmp);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = tmp;
                Texture2D myTexture2D = new Texture2D(getPicture.width, getPicture.height);
                myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
                myTexture2D.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(tmp);
                
                TextureScale.Bilinear(myTexture2D, 300, 300);
                _memorizePhotos.thumbnailList.Add(myTexture2D);
                _memorizePhotos.thumbnailIteration++;

                Debug.Log("Width: " + _memorizePhotos.thumbnailList[_memorizePhotos.thumbnailIteration].width +
                          " Height: " + _memorizePhotos.thumbnailList[_memorizePhotos.thumbnailIteration].height);
                

                Sprite tempSprite = Sprite.Create(myTexture2D,new Rect(0,0, 300,300), new Vector2(0.5f, 0.5f));

                PanelSpawner_script.thumbnailSlot = tempSprite;
                PanelSpawner_script.AddPanel();
                /*
                if (_memorizePhotos.thumbnailList[_memorizePhotos.thumbnailIteration] != null)
                {
                    Debug.Log("Picture " + _memorizePhotos.thumbnailIteration + " is not null");
                }
                else
                {
                    Debug.Log("Picture " + _memorizePhotos.thumbnailIteration + " null");
                }

                Debug.Log("IMAGE: " + _memorizePhotos.thumbnailList[_memorizePhotos.thumbnailIteration]);
                */
                previousPath = path;
                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
                //Destroy( getPicture, 5f );
                Debug.Log(getPicture.name);
                GetTouchPos_script.textureOffset = Vector2.zero;                
                SetFaceTexture();
            }
        }, "Select a PNG image", "image/png" );

        Debug.Log( "Permission result: " + permission );
    }
    
    public void SetFaceTexture()
    {
        faceMat.SetTexture("_MainTex", getPicture);
    }


}
