using UnityEngine;

public class TouchController : MonoBehaviour {
    private Camera myCamera;
    private GameController gameController;
    private bool isSweeping;

    private void Start() {
        myCamera = Camera.main;
        // gameController = FindObjectOfType<GameController>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            isSweeping = true;
        }

        if (Input.GetMouseButtonUp(0)) {
            isSweeping = false;
            SweepEnd();
        }

        if (isSweeping) {
            Swiping();
        }
    }

    private void Swiping() {
        Vector3 mousePosition = myCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (!hit.collider) {
            return;
        }

        Block block = hit.collider.GetComponent<Block>();

        if (!block) {
            return;
        }

        gameController.SweepOverBlock(block);
    }

    private void SweepEnd() {
        gameController.SweepEnd();
    }
}