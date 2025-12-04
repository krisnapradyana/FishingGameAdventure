using UnityEngine;

public interface ISwimmable
{
    public FishState CurrentState { get; set; }
    public void Swim();
}
