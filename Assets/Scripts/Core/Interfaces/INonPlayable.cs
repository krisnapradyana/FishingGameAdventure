using UnityEngine;

public interface INonPlayable : IEntity
{
    float MoveSpeed { get; }
    float TurnSpeed { get;  }
    float SensorLength { get; }
    float HomeRadius { get; }
    LayerMask ObstacleLayer { get; }
}