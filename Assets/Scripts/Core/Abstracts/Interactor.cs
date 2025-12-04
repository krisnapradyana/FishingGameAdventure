using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactor : MonoBehaviour, IInteractor
{
    [Header("Interaction Settings")]
    [field: SerializeField]public int MaxColliders { get; protected set; } = 20;
    [field: SerializeField]public float InteractionRange { get; protected set; } = 5f;

    public Action OnInteract { get; protected set; }

    public Action<Collider> OnInRange { get; set; }

    public Action<Collider> OnExitRange { get; set; }

    [field:SerializeField] public string InteractionTag { get ; protected set; } = "Interactable";

    public InputSystem_Actions Input { get; protected set; }

    public Collider InteractedObject { get; protected set; }
    //-------------------------------------------------------------
    // Private Parameters
    private Collider[] _collidersBuffer;
    private HashSet<Collider> _currentSet;
    private HashSet<Collider> _previousSet;

    //-------------------------------------------------------------
    // Debug
    public bool isDebug;

    public virtual void OnDestroy()
    {
        OnExitRange -= ExitRange;
        OnInRange -= InRange;
    }

    public virtual void Start()
    {
        _collidersBuffer = new Collider[MaxColliders];
        _currentSet = new HashSet<Collider>();
        _previousSet = new HashSet<Collider>();

        OnInRange += InRange;
        OnExitRange += ExitRange;
    }

    public virtual void FixedUpdate()
    {
        ProcessOverlap();
    }

    public virtual void Interact(Collider col)
    {
        try
        {
            Debug.LogFormat($"Interacted With : <color=blue>{col.gameObject}</color>");
        }
        catch (Exception e)
        {
            Debug.LogError($"Interaction Failed: {e.Message} | There is probably no Interaction Object in Range");
        }
    }

    public virtual void ProcessOverlap()
    {
        //Debug.Log("Sensing Interaction Range");
        _currentSet.Clear();

        int numColliders = Physics.OverlapSphereNonAlloc(
            transform.position,
            InteractionRange,
            _collidersBuffer
        );

        for (int i = 0; i < numColliders; i++)
        {
            Collider col = _collidersBuffer[i];

            if (col.CompareTag(InteractionTag))
            {
                _currentSet.Add(col);
            }
        }

        foreach (var col in _currentSet)
        {
            if(!_previousSet.Contains(col))
            { 
                OnInRange?.Invoke(col);
            }
        }

        foreach (var col in _previousSet)
        {
            if (!_currentSet.Contains(col))
            {
                OnExitRange?.Invoke(col);
            }
        }

        HashSet<Collider> temp = _previousSet;
        _previousSet = _currentSet;
        _currentSet = temp;
    }

    public virtual void InRange(Collider other)
    {
        InteractedObject = other;
#if UNITY_EDITOR
        Debug.Log($"<color=green>ENTER:</color> {other.name}");
#endif
    }
    public virtual void ExitRange(Collider other)
    {
        InteractedObject = null;   
#if UNITY_EDITOR
        Debug.Log($"<color=red>EXIT:</color> {other.name}");
#endif
    }

    void OnDrawGizmosSelected()
    {
        if (!isDebug) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, InteractionRange);
    }
}
