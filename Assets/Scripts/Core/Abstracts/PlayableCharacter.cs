using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInteractor))]
public abstract class PlayableCharacter : MonoBehaviour, IControllable, IEntity, IMoveable
{
    [Header("Entity Parameters")]
    public int ID  {get; protected set;}
    [field: SerializeField] public string Name { get; protected set; }
    [field: SerializeField] public string Description { get; protected set; }

    [Header("Player Parameters")]
    [field:SerializeField] public float MoveSpeed { get; set; } = 5f;
    public Action OnInteract { get; private set; }
    [field:SerializeField] public InputSystem_Actions Controller { get; protected set; }
    public Animator CharacterAnimator { get; protected set; }
    [field:SerializeField] protected FishingRod fishingRod;
    [field:SerializeField] protected Lure lure;

    [field:SerializeField] public bool IsInteracting { get; protected set; }
    Vector3 _moveDir;
    PlayerInteractor _interactor;

    public PlayableCharacter(string name, string description)
    {
        Name = name;
        Description = description;
    }

    protected virtual void Start()
    {
        ID = GetInstanceID();
        CharacterAnimator = GetComponent<Animator>();
        _interactor = GetComponent<PlayerInteractor>();

        //----Debug Purposes Only----//
        _interactor.OnExitRange += (col) => { IsInteracting = false; };
    }

    protected virtual void Update()
    {
        transform.Translate(_moveDir * MoveSpeed * Time.deltaTime, Space.World);
        transform.RotateAround(transform.position, Vector3.up, 
            Vector3.SignedAngle(transform.forward, _moveDir, Vector3.up) * Time.deltaTime * 10f);
        CharacterAnimator.SetFloat("Move", _moveDir.magnitude);
    }

    public virtual void Jump()
    {
       Debug.Log("Jumping");
    }

    public virtual void Move(Vector2 direction)
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        var calculatedDir = cameraForward.normalized * direction.y + cameraRight.normalized * direction.x;
        _moveDir = new Vector3(calculatedDir.x, 0, calculatedDir.z).normalized;
    }

    public virtual void InitializeControl()
    {
        Controller.Player.Move.performed += ctx =>
        {
            //Debug.Log("Moving");
            Move(ctx.ReadValue<Vector2>());
        };

        Controller.Player.Move.canceled += ctx =>
        {
            //Debug.Log("Stopping");
            Move(Vector2.zero);
        };

        Controller.Player.Attack.started += ctx =>
        {
            if (!fishingRod) return;

            if (fishingRod.HasCasted)
            {
                Debug.Log("Reeling Bait");
            }
            else
            {
                fishingRod.Cast(.5f);
            }
        };

        Controller.Player.Interact.started += ctx =>
        {
            if (IsInteracting) return;
            _interactor.Interact(_interactor.InteractedObject);
            IsInteracting = true;
        };

        Controller.Fishing.ReelIn.started += ctx =>
        {
            if (!fishingRod || !fishingRod.HasCasted) return; //Check if fishing rod is casted or eqiupped
            Debug.Log("Reeling Bait");
        };
    }
}