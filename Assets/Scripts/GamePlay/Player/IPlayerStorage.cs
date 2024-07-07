using System;

public interface IPlayerStorage 
{
    public int GetBalance();

    public void AddBalance(int amount);

    public bool SubtractBalance(int amount);
    public void ResetBalance();
}
