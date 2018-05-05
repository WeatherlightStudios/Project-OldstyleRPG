using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseRotation : MonoBehaviour {
	
	public float rotSpeed;
	
	[System.Serializable]
    public class MouseCursor
    {
        public bool GamePaused;
        public CursorLockMode lockedMode;
        public CursorLockMode unlockedMode;
    }
    public MouseCursor cursor;
	
	// Update is called once per frame
	void Update () {
		
		//pause game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursor.GamePaused = !cursor.GamePaused;
        }

        //cursor lock on game pause
        if (!cursor.GamePaused)
        {
            Cursor.lockState = cursor.lockedMode;
            Cursor.visible = false;
        }
        else if (cursor.GamePaused)
        {
            Cursor.lockState = cursor.unlockedMode;
            Cursor.visible = true;
        }
		
		if(!cursor.GamePaused){
			float mouseInput = Input.GetAxis("Mouse X");
			transform.Rotate(Vector3.up, mouseInput * rotSpeed * 2 * Mathf.PI * Time.deltaTime);
		}
	}
}
