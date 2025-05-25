using System;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

[RequireComponent(typeof(Rigidbody))]
//projectile based.
internal class Bullet : MonoBehaviour
{
    //[SerializeField] private float forwardForce;
    //[SerializeField] private float upForce;
    //[SerializeField] private float leftForce;
    [SerializeField] private float force;
    [SerializeField] private Rigidbody _rb;
    private Type _opponentType;
    private float _damage;
    //Vector3 point;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Fire(Transform wParent, Type opponentType, float damage)
    {
        //rb.AddForce(FPCamera.transform.forward * forwardForce, ForceMode.Impulse);
        //rb.AddForce(FPCamera.transform.up * upForce, ForceMode.Impulse);
        //rb.AddForce(-FPCamera.transform.right * leftForce, ForceMode.Impulse);

        _rb.AddForce(wParent.transform.forward * force * Time.deltaTime, ForceMode.Impulse);

        _opponentType = opponentType;
        _damage = damage;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Enemy hitCharacterA = collision.gameObject.GetComponent<Enemy>();

        if (!CheckCollision(collision, hitCharacterA, typeof(Enemy)))
        {
            Player hitCharacterB = collision.gameObject.GetComponent<Player>();
            CheckCollision(collision, hitCharacterB, typeof(Player));
        }

        Destroy(this.gameObject);
    }

    private bool CheckCollision(Collider collision, Character hitCharacter, Type intendedType)
    {
        if (hitCharacter != null && hitCharacter.GetType() == intendedType)
        {
            hitCharacter.TakeDamage(_damage);
            Debug.Log("Shot " + hitCharacter.name + " for " + _damage + " damage");
            return true;
        }
        return false;
    }
}