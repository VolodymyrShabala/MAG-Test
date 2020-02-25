using UnityEngine;

public class TouchController : MonoBehaviour {
    private Camera myCamera;
    private GridController gridController;
    private bool isSwiping;
    
    private void Start() {
        myCamera = Camera.main;
        gridController = FindObjectOfType<GridController>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            isSwiping = true;
        }

        if (Input.GetMouseButtonUp(0)) {
            isSwiping = false;
            SwipeEnd();
        }

        if (isSwiping) {
            Swiping();
        }
    }

    private void Swiping() {
        Vector3 mousePosition = myCamera.ScreenToWorldPoint(Input.mousePosition);
        gridController.SweepOverBlock(mousePosition);
    }

    private void SwipeEnd() {
        gridController.SweepEnd();
    }
}