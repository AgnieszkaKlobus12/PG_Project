using System;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject orc;
    public GameObject human;
    private Camera _camera;

    private void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        if (Math.Abs(Math.Abs(orc.transform.position.x) - Math.Abs(human.transform.position.x)) >
            (2f * _camera.orthographicSize * _camera.aspect) - 0.3f)
        {
            if (orc.transform.position.x < human.transform.position.x)
            {
                orc.transform.position = new Vector3(
                    orc.transform.position.x + (_camera.orthographicSize * _camera.aspect) / 200,
                    orc.transform.position.y);
                human.transform.position = new Vector3(
                    human.transform.position.x - (_camera.orthographicSize * _camera.aspect) / 200,
                    human.transform.position.y);
            }
            else
            {
                orc.transform.position = new Vector3(
                    orc.transform.position.x - (_camera.orthographicSize * _camera.aspect) / 200,
                    orc.transform.position.y);
                human.transform.position = new Vector3(
                    human.transform.position.x + (_camera.orthographicSize * _camera.aspect) / 200,
                    human.transform.position.y);
            }
        }

        gameObject.transform.position = new Vector3((orc.transform.position.x + human.transform.position.x) / 2,
            (orc.transform.position.y + human.transform.position.y) / 2);
    }
}