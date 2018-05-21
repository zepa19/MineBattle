using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Character/Character Motor")]
[RequireComponent(typeof(CharacterController))]
public class CharacterMotor : MonoBehaviour {

    public bool
    canControl = true,
    useFixedUpdate = true;

    [System.NonSerialized]
    public Vector3 inputMoveDirection = Vector3.zero;

    [System.NonSerialized]
    public bool inputJump = false;

    [System.Serializable]
    public class CharacterMotorMovement
    {
        public float
            maxForwardSpeed = 10f,
            maxSidewaysSpeed = 10f,
            maxBackwardsSpeed = 10f;

        public AnimationCurve slopeSpeedMultipier = new AnimationCurve(new Keyframe(-90, 1), new Keyframe(0, 1), new Keyframe(90, 0));

        public float
            maxGroundAcceleration = 30f,
            maxAirAcceleration = 20f;

        public float
            gravity = 10f,
            maxFallSpeed = 20f;

        [System.NonSerialized]
        public CollisionFlags collisionFlags;

        [System.NonSerialized]
        public Vector3 velocity;

        [System.NonSerialized]
        public Vector3 frameVelocity = Vector3.zero;

        [System.NonSerialized]
        public Vector3 hitPoint = Vector3.zero;

        [System.NonSerialized]
        public Vector3 lastHitPoint = new Vector3(Mathf.Infinity, 0, 0);
    }
    public enum MovementTransferOnJump
    {
        None,
        InitTransfer,
        PermaTransfer,
        PermaLocked
    }

    [System.Serializable]
    public class CharacterMotorJumping
    {
        public bool enabled = true;

        public float 
            baseHeight = 1f,
            extraHeight = 4.1f;

        public float 
            perpAmount = 0f,
            steepPerpAmount = 0.5f;

        [System.NonSerialized]
        public bool jumping = false;

        [System.NonSerialized]
        public bool holdingJumpButton = false;

        [System.NonSerialized]
        public float lastStartTime = 0f;

        [System.NonSerialized]
        public float lastButtonDownTime = -100f;

        [System.NonSerialized]
        public Vector3 jumpDir = Vector3.up;
    }

    [System.Serializable]
    public class CharacterMotorMovingPlatform
    {
        public bool enabled = true;

        public MovementTransferOnJump movementTransfer = MovementTransferOnJump.PermaTransfer;

        [System.NonSerialized]
        public Transform hitPlatform;

        [System.NonSerialized]
        public Transform activePlatform;

        [System.NonSerialized]
        public Vector3 activeLocalPoint;

        [System.NonSerialized]
        public Vector3 activeGlobalPoint;

        [System.NonSerialized]
        public Quaternion activeLocalRotation;

        [System.NonSerialized]
        public Quaternion activeGlobalRotation;

        [System.NonSerialized]
        public Matrix4x4 lastMatrix;

        [System.NonSerialized]
        public Vector3 platformVelocity;

        [System.NonSerialized]
        public bool newPlatform;
    }

    [System.Serializable]
    public class CharacterMotorSliding
    {
        public bool enabled = true;

        public float 
            slidingSpeed = 15f,
            sidewaysControl = 1f,
            speedControl = 0.4f;
    }

    public CharacterMotorMovement movement = new CharacterMotorMovement();
    public CharacterMotorJumping jumping = new CharacterMotorJumping();
    public CharacterMotorMovingPlatform movingPlatform = new CharacterMotorMovingPlatform();
    public CharacterMotorSliding sliding = new CharacterMotorSliding();

    [System.NonSerialized]
    public bool grounded = true;

    [System.NonSerialized]
    public Vector3 groundNormal = Vector3.zero;

    private Vector3 lastGroundNormal = Vector3.zero;
    private CharacterController controller;
    private Transform tr;
    public static CharacterMotor _Instance;

    void Awake()
    {
        _Instance = this;
        controller = GetComponent<CharacterController>();
        tr = transform;
    }

    void FixedUpdate()
    {
        if (GameManager._Instance.StateOfTheGame == GameManager.GameState.RUNNING)
        {
            if (movingPlatform.enabled)
            {
                if (movingPlatform.activePlatform != null)
                {
                    if (!movingPlatform.newPlatform)
                    {
                        movingPlatform.platformVelocity = (
                            movingPlatform.activePlatform.localToWorldMatrix.MultiplyPoint3x4(movingPlatform.activeLocalPoint)
                            - movingPlatform.lastMatrix.MultiplyPoint3x4(movingPlatform.activeLocalPoint)
                        ) / Time.deltaTime;
                    }
                    movingPlatform.lastMatrix = movingPlatform.activePlatform.localToWorldMatrix;
                    movingPlatform.newPlatform = false;
                }
                else
                {
                    movingPlatform.platformVelocity = Vector3.zero;
                }
            }

            if (useFixedUpdate)
            {
                UpdateFunction();
            }
        }
    }

    void Update()
    {
        if (GameManager._Instance.StateOfTheGame == GameManager.GameState.RUNNING)
        {
            if (!useFixedUpdate)
            {
                UpdateFunction();
            }
        }
    }

    private void UpdateFunction()
    {
        Vector3 velocity = movement.velocity;

        velocity = ApplyInputVelocityChange(velocity);

        velocity = ApplyGravityAndJumping(velocity);

        Vector3 moveDistance = Vector3.zero;
        if (MoveWithPlatform())
        {
            Vector3 newGlobalPoint = movingPlatform.activePlatform.TransformPoint(movingPlatform.activeLocalPoint);
            moveDistance = (newGlobalPoint - movingPlatform.activeGlobalPoint);
            if (moveDistance != Vector3.zero)
                controller.Move(moveDistance);

            Quaternion newGlobalRotation = movingPlatform.activePlatform.rotation * movingPlatform.activeLocalRotation;
            Quaternion rotationDiff = newGlobalRotation * Quaternion.Inverse(movingPlatform.activeGlobalRotation);

            var yRotation = rotationDiff.eulerAngles.y;
            if (yRotation != 0)
            {
                tr.Rotate(0, yRotation, 0);
            }
        }

        Vector3 lastPosition = tr.position;

        Vector3 currentMovementOffset = velocity * Time.deltaTime;

        float pushDownOffset = Mathf.Max(controller.stepOffset, new Vector3(currentMovementOffset.x, 0, currentMovementOffset.z).magnitude);
        if (grounded)
            currentMovementOffset -= pushDownOffset * Vector3.up;
        
        movingPlatform.hitPlatform = null;
        groundNormal = Vector3.zero;
        
        movement.collisionFlags = controller.Move(currentMovementOffset);

        movement.lastHitPoint = movement.hitPoint;
        lastGroundNormal = groundNormal;

        if (movingPlatform.enabled && movingPlatform.activePlatform != movingPlatform.hitPlatform)
        {
            if (movingPlatform.hitPlatform != null)
            {
                movingPlatform.activePlatform = movingPlatform.hitPlatform;
                movingPlatform.lastMatrix = movingPlatform.hitPlatform.localToWorldMatrix;
                movingPlatform.newPlatform = true;
            }
        }

        Vector3 oldHVelocity = new Vector3(velocity.x, 0, velocity.z);
        movement.velocity = (tr.position - lastPosition) / Time.deltaTime;
        Vector3 newHVelocity = new Vector3(movement.velocity.x, 0, movement.velocity.z);
        
        if (oldHVelocity == Vector3.zero)
        {
            movement.velocity = new Vector3(0, movement.velocity.y, 0);
        }
        else
        {
            float projectedNewVelocity = Vector3.Dot(newHVelocity, oldHVelocity) / oldHVelocity.sqrMagnitude;
            movement.velocity = oldHVelocity * Mathf.Clamp01(projectedNewVelocity) + movement.velocity.y * Vector3.up;
        }

        if (movement.velocity.y < velocity.y - 0.001)
        {
            if (movement.velocity.y < 0)
            {
                movement.velocity.y = velocity.y;
            }
            else
            {
                jumping.holdingJumpButton = false;
            }
        }

        if (grounded && !IsGroundedTest())
        {
            grounded = false;
            
            if (movingPlatform.enabled &&
                (movingPlatform.movementTransfer == MovementTransferOnJump.InitTransfer ||
                movingPlatform.movementTransfer == MovementTransferOnJump.PermaTransfer)
            )
            {
                movement.frameVelocity = movingPlatform.platformVelocity;
                movement.velocity += movingPlatform.platformVelocity;
            }

            SendMessage("OnFall", SendMessageOptions.DontRequireReceiver);
            tr.position += pushDownOffset * Vector3.up;
        }
        else if (!grounded && IsGroundedTest())
        {
            grounded = true;
            jumping.jumping = false;
            SubtractNewPlatformVelocity();

            SendMessage("OnLand", SendMessageOptions.DontRequireReceiver);
        }

        if (MoveWithPlatform())
        {
            movingPlatform.activeGlobalPoint = tr.position + Vector3.up * (controller.center.y - controller.height * 0.5f + controller.radius);
            movingPlatform.activeLocalPoint = movingPlatform.activePlatform.InverseTransformPoint(movingPlatform.activeGlobalPoint);

            movingPlatform.activeGlobalRotation = tr.rotation;
            movingPlatform.activeLocalRotation = Quaternion.Inverse(movingPlatform.activePlatform.rotation) * movingPlatform.activeGlobalRotation;
        }
    }

    private Vector3 ApplyInputVelocityChange(Vector3 velocity)
    {
        if (!canControl)
            inputMoveDirection = Vector3.zero;

        Vector3 desiredVelocity;
        if(grounded && TooSteep())
        {
            desiredVelocity = new Vector3(groundNormal.x, 0, groundNormal.z).normalized;
            Vector3 projectedMoveDir = Vector3.Project(inputMoveDirection, desiredVelocity);
            desiredVelocity = desiredVelocity + projectedMoveDir * sliding.speedControl + (inputMoveDirection - projectedMoveDir) * sliding.sidewaysControl;
            desiredVelocity *= sliding.slidingSpeed;
        }
        else
        {
            desiredVelocity = GetDesiredHorizontalVelocity();
        }

        if (movingPlatform.enabled && movingPlatform.movementTransfer == MovementTransferOnJump.PermaTransfer)
        {
            desiredVelocity += movement.frameVelocity;
            desiredVelocity.y = 0;
        }

        if (grounded)
            desiredVelocity = AdjustGroundVelocityToNormal(desiredVelocity, groundNormal);
        else
            velocity.y = 0;

        float maxVelocityChange = GetMaxAcceleration(grounded) * Time.deltaTime;
        Vector3 velocityChangeVector = (desiredVelocity - velocity);

        if (velocityChangeVector.sqrMagnitude > maxVelocityChange * maxVelocityChange)
        {
            velocityChangeVector = velocityChangeVector.normalized * maxVelocityChange;
        }

        if (grounded || canControl)
            velocity += velocityChangeVector;

        if (grounded)
        {
            velocity.y = Mathf.Min(velocity.y, 0);
        }

        return velocity;
    }

    private Vector3 ApplyGravityAndJumping(Vector3 velocity)
    {
        if (!inputJump || !canControl)
        {
            jumping.holdingJumpButton = false;
            jumping.lastButtonDownTime = -100;
        }

        if (inputJump && jumping.lastButtonDownTime < 0 && canControl)
            jumping.lastButtonDownTime = Time.time;

        if (grounded)
            velocity.y = Mathf.Min(0, velocity.y) - movement.gravity * Time.deltaTime;
        else
        {
            velocity.y = movement.velocity.y - movement.gravity * Time.deltaTime;

            if (jumping.jumping && jumping.holdingJumpButton)
            {
                if (Time.time < jumping.lastStartTime + jumping.extraHeight / CalculateJumpVerticalSpeed(jumping.baseHeight))
                {
                    velocity += jumping.jumpDir * movement.gravity * Time.deltaTime;
                }
            }
            
            velocity.y = Mathf.Max(velocity.y, -movement.maxFallSpeed);
        }

        if (grounded)
        {

            if (jumping.enabled && canControl && (Time.time - jumping.lastButtonDownTime < 0.2))
            {
                grounded = false;
                jumping.jumping = true;
                jumping.lastStartTime = Time.time;
                jumping.lastButtonDownTime = -100;
                jumping.holdingJumpButton = true;
                
                if (TooSteep())
                    jumping.jumpDir = Vector3.Slerp(Vector3.up, groundNormal, jumping.steepPerpAmount);
                else
                    jumping.jumpDir = Vector3.Slerp(Vector3.up, groundNormal, jumping.perpAmount);
                
                velocity.y = 0;
                velocity += jumping.jumpDir * CalculateJumpVerticalSpeed(jumping.baseHeight);
                
                if (movingPlatform.enabled &&
                    (movingPlatform.movementTransfer == MovementTransferOnJump.InitTransfer ||
                    movingPlatform.movementTransfer == MovementTransferOnJump.PermaTransfer)
                )
                {
                    movement.frameVelocity = movingPlatform.platformVelocity;
                    velocity += movingPlatform.platformVelocity;
                }

                SendMessage("OnJump", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                jumping.holdingJumpButton = false;
            }
        }

        return velocity;
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y > 0 && hit.normal.y > groundNormal.y && hit.moveDirection.y < 0)
        {
            if ((hit.point - movement.lastHitPoint).sqrMagnitude > 0.001 || lastGroundNormal == Vector3.zero)
                groundNormal = hit.normal;
            else
                groundNormal = lastGroundNormal;

            movingPlatform.hitPlatform = hit.collider.transform;
            movement.hitPoint = hit.point;
            movement.frameVelocity = Vector3.zero;
        }
    }

    private IEnumerator SubtractNewPlatformVelocity()
    {
        if (movingPlatform.enabled &&
            (movingPlatform.movementTransfer == MovementTransferOnJump.InitTransfer ||
            movingPlatform.movementTransfer == MovementTransferOnJump.PermaTransfer)
        )
        {
            if (movingPlatform.newPlatform)
            {
                Transform platform = movingPlatform.activePlatform;
                yield return new WaitForFixedUpdate();
                yield return new WaitForFixedUpdate();
                if (grounded && platform == movingPlatform.activePlatform)
                {
                    yield return 1;
                }
            }
            movement.velocity -= movingPlatform.platformVelocity;
        }
    }

    private bool MoveWithPlatform()
    {
        return (
            movingPlatform.enabled
            && (grounded || movingPlatform.movementTransfer == MovementTransferOnJump.PermaLocked)
            && movingPlatform.activePlatform != null
        );
    }

    private Vector3 GetDesiredHorizontalVelocity()
    {
        Vector3 desiredLocalDirection = tr.InverseTransformDirection(inputMoveDirection);
        float maxSpeed = MaxSpeedInDirection(desiredLocalDirection);
        if(grounded)
        {
            float movementSlopeAngle = Mathf.Asin(movement.velocity.normalized.y) * Mathf.Rad2Deg;
            maxSpeed *= movement.slopeSpeedMultipier.Evaluate(movementSlopeAngle);
        }
        return tr.TransformDirection(desiredLocalDirection * maxSpeed);
    }

    private Vector3 AdjustGroundVelocityToNormal(Vector3 hVelocity, Vector3 groundNormal)
    {
        Vector3 sideways = Vector3.Cross(Vector3.up, hVelocity);
        return Vector3.Cross(sideways, groundNormal).normalized * hVelocity.magnitude;
    }

    private bool IsGroundedTest()
    {
        return (groundNormal.y > 0.01);
    }

    public float GetMaxAcceleration(bool grounded)
    {
        if(grounded)
        {
            return movement.maxGroundAcceleration;
        }
        else
        {
            return movement.maxAirAcceleration;
        }
    }

    public float CalculateJumpVerticalSpeed(float targetJumpHeight)
    {
        return Mathf.Sqrt(2 * targetJumpHeight * movement.gravity);
    }

    public bool IsJumping()
    {
        return jumping.jumping;
    }

    public bool IsSliding()
    {
        return (grounded && sliding.enabled && TooSteep());
    }

    public bool IsTouchingCeiling()
    {
        return (movement.collisionFlags & CollisionFlags.CollidedAbove) != 0;
    }

    public bool IsGrounded()
    {
        return grounded;
    }

    public bool TooSteep()
    {
        return (groundNormal.y <= Mathf.Cos(controller.slopeLimit * Mathf.Deg2Rad));
    }

    public Vector3 GetDirection()
    {
        return inputMoveDirection;
    }

    public void SetControllable(bool controllable)
    {
        canControl = controllable;
    }

    public float MaxSpeedInDirection(Vector3 desiredMovementDirection)
    {
        if(desiredMovementDirection == Vector3.zero)
        {
            return 0;
        }
        else
        {
            float zAxisEllipseMultiplier = (desiredMovementDirection.z > 0 ? movement.maxForwardSpeed : movement.maxBackwardsSpeed) / movement.maxSidewaysSpeed;
            Vector3 temp = new Vector3(desiredMovementDirection.x, 0, desiredMovementDirection.z / zAxisEllipseMultiplier).normalized;
            float length = new Vector3(temp.x, 0, temp.z * zAxisEllipseMultiplier).magnitude * movement.maxSidewaysSpeed;
            return length;
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        grounded = false;
        movement.velocity = velocity;
        movement.frameVelocity = Vector3.zero;
        SendMessage("OnExternalVelocity");
    }
}
