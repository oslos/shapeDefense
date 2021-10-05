using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCaster : MonoBehaviour
{
    public static event SkillEvent SkillFreezeStarted;
    public static event SkillEvent SkillFreezeEnded;
    public static event SkillEvent SkillTornadoStarted;
    public static event SkillEvent SkillTornadoEnded;
    public delegate void SkillEvent();

    private bool _freezeInProgress;
    private bool _tornadoInProgress;

    private static SkillCaster _instance;

    public static SkillCaster Instance()
    {
        return _instance;
    }

    private void Awake()
    {
        if (!_instance)
            _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            CastFreeze();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            CastTornado();
        }

        if (Input.GetKeyDown(KeyCode.P)) 
        {
            CastShield();
        }
    }

    public void CastFreeze() {
        StartCoroutine(DoCastFreeze());
    }

    public void CastTornado() {
        StartCoroutine(DoCastTornado());
    }

    public void CastShield() {
        GameManager.Instance().GetShield().Deploy();
    }

    private IEnumerator DoCastFreeze()
    {
        if (_freezeInProgress) {
            yield break;
        }

        _freezeInProgress = true;

        if (SkillFreezeStarted != null)
            SkillFreezeStarted();

        yield return new WaitForSeconds(5);
   
        if (SkillFreezeEnded != null)
            SkillFreezeEnded();

        _freezeInProgress = false;
    }

    private IEnumerator DoCastTornado() {
        
        if (_tornadoInProgress) {
            yield break;
        }
        
        _tornadoInProgress = true;

        if (SkillTornadoStarted != null)
            SkillTornadoStarted();

        yield return new WaitForSeconds(2);

        if (SkillTornadoEnded != null)
            SkillTornadoEnded();

        _tornadoInProgress = false;
    }
}
