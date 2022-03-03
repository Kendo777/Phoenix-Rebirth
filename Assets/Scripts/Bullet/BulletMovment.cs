using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DEPRECATED, Use BulletMovement instead.
/// </summary>
public class BulletMovment : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 vel = new Vector3(0.0f, 0.0f, 0.0f);
    public float bulletVel = 10.0f;
    void Start()
    {
        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        vel = transform.forward * bulletVel;
        transform.position += vel * dt;

    }

    public void movment(Quaternion rot, float spreadAngle)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, spreadAngle);

        //GetComponent<Rigidbody>().AddForce(transform.forward * bulletVel);
    }
}
