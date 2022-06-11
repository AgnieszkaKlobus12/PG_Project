using System;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private GameObject _orc;
    private GameObject _human;
    private Camera _camera;

    private void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
        _orc = GameObject.Find("Orc");
        _human = GameObject.Find("Human");
    }

    void Update()
    {
        if (Math.Abs(Math.Abs(_orc.transform.position.x) - Math.Abs(_human.transform.position.x)) >
            (2f * _camera.orthographicSize * _camera.aspect) - 0.3f)
        {
            if (_orc.transform.position.x < _human.transform.position.x)
            {
                _orc.transform.position = new Vector3(
                    _orc.transform.position.x + (_camera.orthographicSize * _camera.aspect) / 300,
                    _orc.transform.position.y);
                _human.transform.position = new Vector3(
                    _human.transform.position.x - (_camera.orthographicSize * _camera.aspect) / 300,
                    _human.transform.position.y);
            }
            else
            {
                _orc.transform.position = new Vector3(
                    _orc.transform.position.x - (_camera.orthographicSize * _camera.aspect) / 300,
                    _orc.transform.position.y);
                _human.transform.position = new Vector3(
                    _human.transform.position.x + (_camera.orthographicSize * _camera.aspect) / 300,
                    _human.transform.position.y);
            }
        }

        gameObject.transform.position = new Vector3((_orc.transform.position.x + _human.transform.position.x) / 2,
            (_orc.transform.position.y + _human.transform.position.y) / 2);
    }
}