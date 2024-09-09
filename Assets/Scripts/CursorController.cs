using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CursorController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Texture2D defaultCursor;
    [SerializeField] public Texture2D clickCursor;
    [SerializeField] public InputHandler inputHandler;

    private Texture2D currentCursor;

    private Vector2 hotSpot = new Vector2(0, 0);

    void Start()
    {
    }

    private void Update()
    {
        if (inputHandler.fire && currentCursor != clickCursor)
        {
            currentCursor = clickCursor;
            Cursor.SetCursor(clickCursor, hotSpot, CursorMode.ForceSoftware);
        }
        else if (!inputHandler.fire && currentCursor != defaultCursor)
        {
            Cursor.SetCursor(defaultCursor, hotSpot, CursorMode.ForceSoftware);
            currentCursor = defaultCursor;
        }
    }
}
