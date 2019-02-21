using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AssaultRifleView : MonoBehaviour {

    //组件字段
    private Transform m_Transform;
    private Animator m_Animator;
    private Camera m_EnvCamera;
    //位置
    private Transform m_gunPoint; //枪口位置
    private Transform m_gunStar;  //UI准星
    private Transform effectPos;  //弹壳特效挂点

    private GameObject m_Bullet;  //临时子弹模型
    private GameObject m_shell;   //子弹壳模型

    //优化开镜动作
    private Vector3 startPos;
    private Vector3 startRot;
    private Vector3 endPos;
    private Vector3 endRot;
    //组件属性

    public Transform M_Transform { get { return m_Transform; } }
    public Animator M_Animator { get { return m_Animator; } }
    public Camera M_EnvCamera { get { return m_EnvCamera; } }
    public Transform M_GunPoint { get { return m_gunPoint; } }
    public GameObject M_Bullet { get { return m_Bullet; } }
    public GameObject M_Shell { get { return m_shell; } }
    public Transform M_GunStar { get { return m_gunStar; } }
    public Transform M_EffectPos { get { return effectPos; } }
	void Awake ()
    {
        //组件查找
        m_Transform = gameObject.GetComponent<Transform>();
        m_Animator = gameObject.GetComponent<Animator>();
        m_EnvCamera = GameObject.Find("ENVCamera").GetComponent<Camera>();
        
        //优化开镜动作
        startPos = m_Transform.localPosition;
        startRot = m_Transform.localRotation.eulerAngles;
        endPos = new Vector3(-0.065f, -1.85f, 0.25f);
        endRot = new Vector3(2.8f, 1.3f, 0.08f);

        m_gunPoint = m_Transform.Find("Assault_Rifle/EffectPos_A");
        m_gunStar = GameObject.Find("GunStar").GetComponent<Transform>();
        effectPos = m_Transform.Find("Assault_Rifle/EffectPos_B");

        m_Bullet = Resources.Load<GameObject>("Gun/Bullet");
        m_shell = Resources.Load<GameObject>("Gun/Shell");
	}
    /// <summary>
    /// 进入开镜状态--动画优化
    /// </summary>
    public void EnterHoldPose()
    {
        m_Transform.DOLocalMove(endPos, 0.2f);
        m_Transform.DOLocalRotate(endRot, 0.2f);
        m_EnvCamera.DOFieldOfView(40, 0.2f);
    }
    /// <summary>
    /// 退出开镜状态--动画优化
    /// </summary>
    public void ExitHoldPose()
    {
        m_Transform.DOLocalMove(startPos, 0.2f);
        m_Transform.DOLocalRotate(startRot, 0.2f);
        m_EnvCamera.DOFieldOfView(60, 0.2f);
    }

	
}
