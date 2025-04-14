using UnityEngine;
/// <summary>
/// Represents a force that can be applied to a rigidbody. 
/// No hereda de monobehaviour ya que no es un componente, es nuestra propia implementacion.
/// </summary>
public class ForceRequest
{
    public Vector3 direction;
    public float acceleration;
    public float speed;
    public ForceMode forceMode;
}
