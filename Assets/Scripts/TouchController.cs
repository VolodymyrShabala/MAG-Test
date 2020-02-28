using UnityEngine;

public class TouchController : MonoBehaviour {
    private Camera myCamera;
    private GameController gameController;
    private bool isSwiping;
    
    private void Start() {
        myCamera = Camera.main;
        gameController = FindObjectOfType<GameController>();
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
        gameController.SweepOverBlock(mousePosition);
    }

    private void SwipeEnd() {
        gameController.SweepEnd();
    }
}