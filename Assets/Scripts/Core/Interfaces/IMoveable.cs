using UnityEngine;
public interface IMoveable
{
    float MoveSpeed { get; }
    void Move(Vector2 direction);
    void Jump();
}
