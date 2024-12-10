using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public Texture2D[] cursors;

    // Start is called before the first frame update
    void Start()
    {
        DefaultCursor();
    }

    // Update is called once per frame
    void DefaultCursor()
    {
        Cursor.SetCursor(cursors[0], Vector2.zero, CursorMode.ForceSoftware);
    }
}
