using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform sideMovementRoot;
    [SerializeField] private Transform leftLimit, rightLimit;

    [SerializeField] private float playerSpeed, sideMovementSensitivity, sideMovementLerpSpeed;

    [SerializeField] private Animator playerAnimator;

    private Vector2 inputDrag;

    private Vector2 previousMousePosition;

    private float leftLimitX => leftLimit.localPosition.x;

    private float rightLimitX => rightLimit.localPosition.x;

    private float sideMovementTarget;

    private Vector2 mousePositionCM
    {
        get
        {
            Vector2 pixels = Input.mousePosition;
            var inches = pixels / Screen.dpi;
            var centimeters = inches * 2.54f;

            return centimeters;
        }
    }

    void Update()
    {
        HandleForwardMovement();
        HandleInput();
        HandleSideMovement();
    }

    private void HandleForwardMovement()
    {
        if (GameManager.Instance.IsGameStart() == true && GameManager.Instance.IsGameFinish() == false)
        {
            transform.position += transform.forward * Time.deltaTime * playerSpeed;

            playerAnimator.SetBool("IsGameStart", true);
        }      
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = mousePositionCM;

            GameManager.Instance.OnGameStarted();
        }

        if (Input.GetMouseButton(0))
        {
            var deltaMouse = mousePositionCM - previousMousePosition;
            inputDrag = deltaMouse;
            previousMousePosition = mousePositionCM;
        }
        else
        {
            inputDrag = Vector2.zero;
        }
    }

    private void HandleSideMovement()
    {
        sideMovementTarget += inputDrag.x * sideMovementSensitivity;
        sideMovementTarget = Mathf.Clamp(sideMovementTarget, leftLimitX, rightLimitX);

        var localPosition = sideMovementRoot.localPosition;

        localPosition.x = Mathf.Lerp(localPosition.x, sideMovementTarget, Time.deltaTime * sideMovementLerpSpeed);

        sideMovementRoot.localPosition = localPosition;
    }
}
