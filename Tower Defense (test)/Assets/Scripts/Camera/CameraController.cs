using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    /*
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(minX, 1f);
            Gizmos.DrawWireSphere(minY, 1f);
            Gizmos.DrawWireSphere(maxX, 1f);
            Gizmos.DrawWireSphere(maxY, 1f);
        }*/


    public Transform uiToScale;
    public Vector3 originalScale;

    private bool canMove = true;
    public float camSpeed = 30f;
    public float camBorderThickness = 10f;
    [Space]
    public float scrollSpeed = 15f;
    public float bordermaxX = 240f;
    public float bordermaxZ = 129f ;
    public float borderminX = -106f ;
    public float borderminZ = -145f;
    [Space]
    public float zoomInMax = 50f;
    public float zoomOutMax = 250f ;
    public float RotationSpeed = 1;
    public float niveauRotCam = 90f;
    public float niveauRotMax = 70f;
    public float niveauRotMin = 40f;
    [Space]
    public bool useMouseToMove = false;
    public KeyCode[] touchesUnlockMouse;


    private void Start()
    {
        originalScale = uiToScale.localScale;
    }

    void Update() {

        foreach (KeyCode key in touchesUnlockMouse)
        {
            if (Input.GetKeyDown(key))
            {
                useMouseToMove = !useMouseToMove;
            }
        }


        if (GameLogic.instance.isGameOver || GameLogic.instance.isGamePaused || GameLogic.instance.isGameWon)
        {
            return;
        }


        //Disable onMenuPause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canMove = !canMove;
        }
        if (!canMove)
            return;


        if (useMouseToMove)
        {
            //Movement ZQSD + mouseScreen
            if ((Input.GetAxisRaw("Vertical") > 0f || Input.mousePosition.y >= Screen.height - camBorderThickness) && transform.position.x >= borderminX)
            {
                transform.Translate(Vector3.left * camSpeed * Time.deltaTime, Space.World);
            }
            if ((Input.GetAxisRaw("Vertical") < 0f || Input.mousePosition.y <= camBorderThickness) && transform.position.x <= bordermaxX)
            {
                transform.Translate(Vector3.right * camSpeed * Time.deltaTime, Space.World);
            }
            if ((Input.GetAxisRaw("Horizontal") > 0f || Input.mousePosition.x >= Screen.width - camBorderThickness) && transform.position.z <= bordermaxZ)
            {
                transform.Translate(Vector3.forward * camSpeed * Time.deltaTime, Space.World);
            }
            if ((Input.GetAxisRaw("Horizontal") < 0f || Input.mousePosition.x <= camBorderThickness) && transform.position.z >= borderminZ)
            {
                transform.Translate(Vector3.back * camSpeed * Time.deltaTime, Space.World);
            }
        }
        else
        {
            //Movement ZQSD
            if (Input.GetAxisRaw("Vertical") > 0f  && transform.position.x >= borderminX)
            {
                transform.Translate(Vector3.left * camSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetAxisRaw("Vertical") < 0f && transform.position.x <= bordermaxX)
            {
                transform.Translate(Vector3.right * camSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetAxisRaw("Horizontal") > 0f && transform.position.z <= bordermaxZ)
            {
                transform.Translate(Vector3.forward * camSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetAxisRaw("Horizontal") < 0f && transform.position.z >= borderminZ)
            {
                transform.Translate(Vector3.back * camSpeed * Time.deltaTime, Space.World);
            }
        }



        //Zoom
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float scrollRaw = Input.GetAxisRaw("Mouse ScrollWheel");
        Vector3 pos = transform.position;

        
        if ((scroll < 0 && zoomOutMax >= pos.y) || (scroll > 0 && zoomInMax <= pos.y))
        {
            //Position
            pos.y += scroll >= 0 ? -scrollSpeed : scrollSpeed;
            pos.y = Mathf.Clamp(pos.y, zoomInMax, zoomOutMax);
            transform.position = pos;
            
            //Rotation
            if (transform.position.y < niveauRotCam && transform.position.y >= zoomInMax)
            {
                //Debug.Log(RotationSpeed);
                transform.Rotate(new Vector3(1, 0, 0), scroll < 0 ? RotationSpeed : -RotationSpeed);
                transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.rotation.eulerAngles.x, niveauRotMin, niveauRotMax), -90, transform.rotation.z);
                if (transform.position.y == zoomInMax)
                {
                    transform.rotation = Quaternion.Euler(niveauRotMin, -90, transform.rotation.z);
                }
            }
            else
            {
                if (uiToScale.transform.parent.GetComponent<NodeUi>().targetedNode == null)
                    return;

                uiToScale.transform.parent.GetComponent<NodeUi>().uiPosition = new Vector3(uiToScale.transform.parent.GetComponent<NodeUi>().uiPosition.x + (scroll * 20f), 
                                                                                            uiToScale.transform.parent.GetComponent<NodeUi>().uiPosition.y + (-scroll * 20f), 
                                                                                            uiToScale.transform.parent.GetComponent<NodeUi>().uiPosition.z);

                uiToScale.transform.parent.position = uiToScale.transform.parent.GetComponent<NodeUi>().GetUiPosition();

                uiToScale.transform.parent.GetComponent<NodeUi>().uiPosition = new Vector3(
                            Mathf.Clamp(uiToScale.transform.parent.GetComponent<NodeUi>().uiPosition.x, -10f, -10f),
                            Mathf.Clamp(uiToScale.transform.parent.GetComponent<NodeUi>().uiPosition.y, 17f, 30f),
                            uiToScale.transform.parent.GetComponent<NodeUi>().uiPosition.z);

                uiToScale.transform.localScale -= (Vector3.one * scroll) /5f;
                uiToScale.transform.localScale = new Vector3(
                            Mathf.Clamp(uiToScale.transform.localScale.x, 0.1f, .2f),
                            Mathf.Clamp(uiToScale.transform.localScale.y, 0.1f, .2f),
                            Mathf.Clamp(uiToScale.transform.localScale.z, 0.1f, .2f)
                        );
            }

        }

    }
}
