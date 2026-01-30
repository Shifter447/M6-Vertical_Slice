"# M6-Vertical_Slice" 
# visual sheet, Movement
<img width="966" height="542" alt="image" src="https://github.com/user-attachments/assets/09458b19-4484-4fbb-8263-22d113b3c575" />

# visual sheet, Dragable Object
<img width="968" height="543" alt="image" src="https://github.com/user-attachments/assets/c0c28620-923a-47db-9482-88b1fab99fb5" />

# visual sheet, object carry
<img width="973" height="547" alt="image" src="https://github.com/user-attachments/assets/2c730d98-b763-4490-8fe1-6d7137ec816b" />

# visual sheet, Camera Zoom
<img width="966" height="541" alt="image" src="https://github.com/user-attachments/assets/791f7904-2795-4c5b-8c64-1a99a9054304" />

```Mermaid
classDiagram
    direction LR

    %% ===== Core Player / Goose =====
    class GooseMovement {
        +moveSpeed
        +runMultiplier
        +OnGroundTouch(Vector3)
    }

    class GooseDrag {
        +TryGrab()
        +Release()
        +IsDragging() bool
        +GetGrabbedRigidbody() Rigidbody
        +GetDraggedWeight() float
    }

    class GooseHeadLook {
        +turnSpeed
        +maxYaw
        +maxPitch
    }

    GooseMovement --> GooseDrag : reads drag weight
    GooseHeadLook --> GooseDrag : queries dragged object

    %% ===== Dragging System =====
    class DraggableObject {
        +dragWeight
        +maxDragDistance
        +StartDragging()
        +StopDragging()
    }

    GooseDrag --> DraggableObject : controls
    DraggableObject --> ContinuousInteractionAudio : plays loop

    %% ===== Carry System =====
    class PlayerCarry {
        +TryPickUp()
        +Drop()
    }

    class CarryableObject {
        +carryWeight
        +OnPickup()
        +OnDrop()
    }

    PlayerCarry --> CarryableObject : picks up / drops
    CarryableObject --> AudioSource : carry audio loop

    %% ===== Audio Components =====
    class AudioPlayer {
        +PlaySound()
    }

    class CarryAudio {
        +StartCarryAudio()
        +StopCarryAudio()
    }

    class ConstantAudio {
        +Play()
    }

    class ContinuousInteractionAudio {
        +StartLoop()
        +StopLoop()
    }

    class InteractionAudioCue {
        +PlayOnce()
    }

    class MovementAudio {
        +PlayMovementSound()
        +StopMovementSound()
    }

    MovementAudio --> NavMeshAgent : checks velocity
    MovementAudio --> AudioSource

    %% ===== Camera =====
    class CameraFollow {
        +positionOffset
        +followSpeed
    }

    class CameraFollowAnchor {
        +smoothTime
    }

    class CameraZoom {
        +minZoom
        +maxZoom
    }

    class FixedAngleCamera {
        +offset
        +zoomSpeed
    }

    CameraFollow --> Transform : follows player
    CameraFollowAnchor --> NavMeshAgent
    FixedAngleCamera --> Transform : looks at player

    %% ===== Unity Components =====
    class AudioSource
    class NavMeshAgent
    class Animator
    class Rigidbody

    GooseMovement --> NavMeshAgent
    GooseMovement --> Animator
    GooseDrag --> Rigidbody
    DraggableObject --> Rigidbody

