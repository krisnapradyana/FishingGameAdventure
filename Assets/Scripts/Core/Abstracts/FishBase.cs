using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class FishBase : MonoBehaviour, ISwimmable, INonPlayable, IInventoryItem, ICatchable
{
    [Header("Entitiy Parameters")]
    public int ID { get; protected set; }
    [field:SerializeField] public string Name { get; protected set; }
    [field: SerializeField] public string Description { get; protected set; }

    [Header("Inventory Parameters")]
    [field: SerializeField] public float Weight { get; protected set; }
    public int BuyPrice { get; }
    [field: SerializeField] public int SellPrice { get; }


    [Header("Fish Parameters")]
    [field:SerializeField] public FishState CurrentState { get; set; }
    public float CatchDifficulty { get; set; }
    [field: SerializeField] public float Stamina { get; set; }
    public float PullResistence { get; }
    [field:SerializeField] public float StruggleStrength { get; }
    [SerializeField] public GameObject Player { get; protected set; }

    [field:SerializeField]  public float MoveSpeed { get; protected set; }
    [field: SerializeField] public float TurnSpeed => 50f;
    [field:SerializeField]  public float HomeRadius { get; protected set; }
    [field: SerializeField] public LayerMask ObstacleLayer { get; protected set; }
    public float SensorLength => 5f;

    private Vector3 _currentDirection;
    private Vector3 _swimAnchorPosition;
    private float _maxStamina;
    [SerializeField] private bool _recovering = false;

    public FishBase(string name, string description, float moveSpeed, float homeRadius,float pullResistance, float StruggleStrenght, float catchDifficulty, float stamina, float weight, int sellPrice)
    {
        Name = name;
        Description = description;
        MoveSpeed = moveSpeed;
        HomeRadius = homeRadius;
        PullResistence = pullResistance;
        StruggleStrength = StruggleStrenght;
        CatchDifficulty = catchDifficulty;
        Stamina = stamina;
        Weight = weight;
        BuyPrice = 0;
        SellPrice = sellPrice;
    }

    public virtual void Start()
    {
        ID = GetInstanceID();
        gameObject.name = Name;
        _swimAnchorPosition = transform.position;
        _maxStamina = Stamina;

        // Randomize direction
        Vector3 randomDir = Random.onUnitSphere;
        randomDir.y = 0;
        _currentDirection = randomDir.normalized;
        // Apply Randomized direction
        if (_currentDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_currentDirection);
        }
    }

    public virtual void Update()
    {
        Swim();
    }

    public virtual void SetPlayer(Player player)
    {
        Player = player.gameObject;
    }

    public virtual void SetFishState(FishState fishState)
    {
        CurrentState = fishState;
    }

    public virtual float GetPullResistence()
    {
        return 10; //Hard coded for testing
    }

    public virtual void OnHook()
    {
        Debug.Log("A fish is on hook");
        CurrentState = FishState.Hooked;
    }

    public virtual void ReduceStamina(float amount)
    {
        if (CurrentState == FishState.Hooked)
        {
            Stamina -= amount + Time.deltaTime;

            if (Stamina <= 0)
            {
                Stamina = 0;
               
                return;
            }
            Debug.LogFormat("Reducing Stamina {0}", Stamina);
        }
    }

    public virtual void OnCaptured()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) < 5f)
        {
            Debug.Log("Fish Captured");
            Destroy(this.gameObject);
        }
    }

    public virtual void Swim()
    {
        switch(CurrentState)
        {
            case FishState.Swimming:
                HandleSwiming();
                break;

            case FishState.Hooked:
                HandleHooked();
                break;
            default:
                return;
        }
    }

    public virtual void SetSwimAnchor(Vector3 swimPosition)
    {
        _swimAnchorPosition = swimPosition;
    }

    public void ReelIn(float reelSpeed)
    {
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, reelSpeed * Time.deltaTime);
        Debug.Log("Reeling In Fish");
    }

    private void HandleSwiming()
    {
        // Decide direction
        Vector3 targetDir = _currentDirection;

        // Fire rays forward, check obstacles
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, SensorLength, ObstacleLayer))
        {
            // Counter direction on detected obstacle
            Vector3 reflectDir = Vector3.Reflect(transform.forward, hit.normal);

            // Debugging visual
            Debug.DrawLine(transform.position, reflectDir, Color.red);
            Debug.DrawRay(hit.point, reflectDir, Color.blue);

            targetDir = reflectDir;
        }
        else if (Vector3.Distance(transform.position, _swimAnchorPosition) > HomeRadius)
        {
            // Return to center point if wander too far
            Vector3 directionToCenter = (_swimAnchorPosition - transform.position).normalized;
            targetDir = directionToCenter;
        }
      
        else
        {
            // Random chance to change direction
            if (Random.value < 0.01f)
            {
                targetDir = Quaternion.Euler(0, Random.Range(-45, 45), 0) * _currentDirection;
            }
        }
        
        // Smooth turning using Slerp
        if (targetDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, TurnSpeed * Time.deltaTime);
        }

        // Update direction
        _currentDirection = transform.forward;

        // Apply movement
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        //Debug.Log("Swimming");
    }

    private void HandleHooked()
    {
        Vector3 flatPlayerPos = Player.transform.position;
        flatPlayerPos.y = transform.position.y;

        var finalStruggleStrength = StruggleStrength;

        if (Stamina <= 0)
        {
            finalStruggleStrength = StruggleStrength / 5f;
            Debug.Log("Fish Tired");
        }

        // Calculate flee direction. Move further from Player
        Vector3 fleeDirection = (transform.position - flatPlayerPos).normalized;

        Vector2 struggle2D = Random.insideUnitCircle * (finalStruggleStrength * 0.1f);
        Vector3 struggleDir = new Vector3(struggle2D.x, 0, struggle2D.y);

        // Raw direction 
        Vector3 rawMoveDir = (fleeDirection + struggleDir).normalized;
        Vector3 finalMoveDir = rawMoveDir;
        float bodyRadius = 0.5f; 
       

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, bodyRadius, rawMoveDir, out hit, SensorLength, ObstacleLayer))
        {
            Debug.DrawRay(transform.position, rawMoveDir * SensorLength, Color.red); // Visualisasi arah tabrakan
            Vector3 slideDir = Vector3.ProjectOnPlane(rawMoveDir, hit.normal).normalized;
            Vector3 pushAwayDir = hit.normal * 1.5f;
            finalMoveDir = (slideDir + pushAwayDir).normalized;

            Debug.DrawRay(hit.point, finalMoveDir * 2f, Color.green); // Visualisasi jalan keluar
        }
        else
        {
            //Debug on normal direction
            Debug.DrawRay(transform.position, rawMoveDir * SensorLength, Color.white);
        }

        
        if (finalMoveDir != Vector3.zero)
        {
            // Emergency Rotation on proximity obstacle detected
            float turnSpeed = Physics.SphereCastAll(transform.position, bodyRadius, rawMoveDir, SensorLength, ObstacleLayer).Length > 0 ? 20f : 5f;

            Quaternion lookRot = Quaternion.LookRotation(finalMoveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * turnSpeed);
        }

        transform.position += finalMoveDir * PullResistence * Time.deltaTime;

        ReduceStamina(Time.deltaTime);
        OnCaptured();
        Debug.Log("Fish Hooked");
    }

    //IEnumerator RecoverStamina(float recoveryTime)
    //{
    //    Debug.Log("Recovering");
    //    if (_recovering)
    //    {
    //        yield break;
    //    }
    //    _recovering = true;
    //    yield return new WaitForSeconds(recoveryTime);
    //    Stamina = _maxStamina;
    //    _recovering = false;
    //}

    void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.cyan;
        //Gizmos.DrawWireSphere(_swimAnchorPosition == Vector3.zero ? transform.position : _swimAnchorPosition, HomeRadius);
        //
        //// Draw line to target position
        //if (Application.isPlaying)
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawLine(transform.position, _wanderTarget);
        //}
    }

}
