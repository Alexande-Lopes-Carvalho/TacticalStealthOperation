using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorPointer : MonoBehaviour
{
    
    [SerializeField] private Texture2D cursorTexture;

    public void SetNewCursor(){
        Cursor.SetCursor(cursorTexture,Vector2.zero,CursorMode.Auto);
    }

    public void SetClassicCursor(){
        Cursor.SetCursor(null,Vector2.zero,CursorMode.Auto);
    }

}
