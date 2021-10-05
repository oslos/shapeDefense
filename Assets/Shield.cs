using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float _size = 1;
    [SerializeField] private float _duration = 10f;

    private Vector3 _targetScale;
    private Vector3 _velocity = Vector3.zero;
    private bool _isDeployed;
    private bool _isReadyToDeploy;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
        _targetScale = new Vector3(_size, _size, 1f);
        _isReadyToDeploy = true;
    }

    public void Deploy() {
        StartCoroutine(DoDeploy());
    }

    private void ResetShield() {
        transform.localScale = Vector3.zero;
        _isReadyToDeploy = true;
    }

    private IEnumerator DoDeploy() {

        if (!_isReadyToDeploy) {
            yield break;
        }
        
        _velocity = Vector3.zero;
        _isReadyToDeploy = false;

        do {

            Vector3 newScale = Vector3.SmoothDamp(transform.localScale, _targetScale, ref _velocity, Time.deltaTime * 15f, 10f);
            transform.localScale = newScale;

            if (Vector3.Distance(newScale, _targetScale) < 0.02f) {
                Debug.Log("Shield deploy finished.");
                _isDeployed = true;
            }

            yield return null;

        } while (!_isDeployed);

        yield return new WaitForSeconds(_duration);

        var zeroScale = Vector3.zero;

        do {

            Vector3 newScale = Vector3.SmoothDamp(transform.localScale, zeroScale, ref _velocity, Time.deltaTime * 15f, 10f);
            transform.localScale = newScale;

            if (Vector3.Distance(newScale, zeroScale) < 0.02f) {
                Debug.Log("Shield removing finished.");
                _isDeployed = false;
            }

            yield return null;

        } while (_isDeployed);

        ResetShield();
    }
}
