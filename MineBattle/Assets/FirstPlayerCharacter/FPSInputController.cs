using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Character/FPS Input Controller")]
[RequireComponent(typeof(CharacterMotor))]
public class FPSInputController : MonoBehaviour {

    CharacterMotor motor;
    public static FPSInputController _Instance;

    void Start()
    {
        _Instance = this;
    }

    void Awake()
    {
        motor = GetComponent<CharacterMotor>();
    }

	void Update ()
    {
        if (GameManager._Instance.StateOfTheGame == GameManager.GameState.RUNNING)
        {
            motor.enabled = true;
            Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (directionVector != Vector3.zero)
            {
                var directionLength = directionVector.magnitude;
                directionVector = directionVector / directionLength;

                directionLength = Mathf.Min(1, directionLength);

                directionLength = directionLength * directionLength;

                directionVector = directionVector * directionLength;
            }

            motor.inputMoveDirection = transform.rotation * directionVector;
            motor.inputJump = Input.GetButton("Jump");
        }
        else
        {
            motor.inputMoveDirection = Vector3.zero;
            motor.inputJump = false;
            motor.enabled = false;
        }
    }

}
