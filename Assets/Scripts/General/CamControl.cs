using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    public Transform Target;
    public float Speed;
    public Vector3 Adapt, StartAdapt, CurrAdapt;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(Target.position.x + (Adapt.x * Target.localScale.x), Target.position.y + Adapt.y, -10), Speed * Time.deltaTime);
        }
    }
}
