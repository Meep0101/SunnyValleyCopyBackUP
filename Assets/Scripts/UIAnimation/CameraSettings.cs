using UnityEngine;
using UnityEngine.UI;

public class CameraSettings : MonoBehaviour
{
    [Header ("space between menu items")]
    [SerializeField] Vector2 spacing;

    Button camButton;
    CameraSettingsItem[] camItems;
    bool isExpanded = false;

    Vector2 camButtonPosition;
    int itemsCount;
    ScrollAndPinch scrollAndPinch; //Cam Rotation Iso
    PinchZoom pinchZoom; //Top View Zoom
    SwitchCamera switchCamera;

    void Start ()
    {
        itemsCount = transform.childCount - 1;
        camItems = new CameraSettingsItem[itemsCount];
        for (int i = 0; i < itemsCount; i++)
        {
            camItems[i] = transform.GetChild(i+1).GetComponent<CameraSettingsItem>();
        }
        camButton = transform.GetChild(0).GetComponent <Button>();
        camButton.onClick.AddListener(ToggleMenu);
        camButton.transform.SetAsLastSibling();

        camButtonPosition = camButton.transform.position;

        //reset all cam items position to camButtonPosition
        ResetPositions();

        pinchZoom = GetComponent<PinchZoom>();
        scrollAndPinch = GetComponent<ScrollAndPinch>();
    }

    void ResetPositions()
    {
        for (int i = 0; i < itemsCount; i++)
        {
            camItems [i].trans.position = camButtonPosition;
        }
    }

    void ToggleMenu ()
    {
        isExpanded = !isExpanded;

        if(isExpanded)
        {

            //menu open
            for (int i = 0; i < itemsCount; i++)
            {
                camItems [i].trans.position = camButtonPosition + spacing * (i + 1);
            }
        }

        else 
        {
            //menu close
            for (int i = 0; i < itemsCount; i++)
            {
                camItems [i].trans.position = camButtonPosition;
            }
        }
    }

    public void OnItemClick (int index)
    {
        if (index >= 0 && index < camItems.Length)
        {
            
            
           
        
        }
    }

    void OnDestroy ()
    {
        camButton.onClick.RemoveListener(ToggleMenu);
    }
    
}
