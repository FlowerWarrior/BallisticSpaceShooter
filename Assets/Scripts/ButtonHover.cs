using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour
{
    [SerializeField] Vector3 sizeHover = new Vector3(1F, 1F, 1F);
    [SerializeField] Vector3 sizeIdle = new Vector3(0.9F, 0.9F, 0.9F); 
    [SerializeField] AudioClip audioHover;
    RectTransform m_RectT;
    // Start is called before the first frame update
    void Start()
    {
        m_RectT = GetComponent<RectTransform>();
        MouseExited();

        EventTrigger eventTrigger1 = transform.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry( );
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener( ( data ) => { MouseEntered(); } );
        eventTrigger1.triggers.Add( entry );

        EventTrigger.Entry entry2 = new EventTrigger.Entry( );
        entry2.eventID = EventTriggerType.PointerExit;
        entry2.callback.AddListener( ( data ) => { MouseExited(); } );
        eventTrigger1.triggers.Add( entry2 );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void AddExitListener()
    {
        EventTrigger eventTrigger1 = transform.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry( );
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener( ( data ) => { MouseExited(); } );
        eventTrigger1.triggers.Add( entry );
    }

    public void MouseEntered()
    {
        m_RectT.localScale = sizeHover;
        AudioSource.PlayClipAtPoint(audioHover, Camera.main.gameObject.transform.position - new Vector3(0, 0, -14));
    }

    // ...and the mesh finally turns white when the mouse moves away.
    public void MouseExited()
    {
        m_RectT.localScale = sizeIdle;
    }
}
