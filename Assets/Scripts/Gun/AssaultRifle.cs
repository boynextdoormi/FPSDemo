using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AssaultRifle : MonoBehaviour {
    private AssaultRifleView m_AssaultRifleView;

    private AudioClip audio;
    private GameObject effect;
    //通过射线方法生成子弹
    private Ray ray;
    private RaycastHit hit;

    //字段
    private int id;
    private int damage;
    private int durable;
    private GunType gunWeaponType;

    #region 属性
    //属性
    public int Id
    { 
        get { return id; }
        set { id = value; }
    }
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    public int Durable
    {
        get { return durable; }
        set { durable = value; }
    }
    public GunType GunWeaponType
    {
        get { return gunWeaponType; }
        set { gunWeaponType = value; }
    }
    public AudioClip Audio
    {
        get { return audio; }
        set { audio = value; }
    }
    public GameObject Effect
    {
        get { return effect; }
        set { effect = value; }
    }
    #endregion

    void Start () {
        Init();
    }
	
	void Update () {
        ShootReady();
        MouseControll();
	}
    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        m_AssaultRifleView = gameObject.GetComponent<AssaultRifleView>();
        audio = Resources.Load<AudioClip>("Audios/Gun/AssaultRifle_Fire");
        effect = Resources.Load<GameObject>("Effects/Gun/AssaultRifle_GunPoint_Effect");
    }

    /// <summary>
    /// 鼠标控制
    /// </summary>
    private void MouseControll()
    {
        if (Input.GetMouseButtonDown(0))//点击鼠标左键射击
        {
            Shoot();
            PlayAudio();
            PlayEffect();
            m_AssaultRifleView.M_Animator.SetTrigger("Fire");
        }
        if (Input.GetMouseButton(1))   //按住鼠标右键进入瞄准状态
        {
            m_AssaultRifleView.M_Animator.SetBool("HoldPose", true);
            m_AssaultRifleView.EnterHoldPose();
            m_AssaultRifleView.M_GunStar.gameObject.SetActive(false);
        }
        if (Input.GetMouseButtonUp(1)) //松开鼠标右键退出瞄准状态
        {
            m_AssaultRifleView.M_Animator.SetBool("HoldPose", false);
            m_AssaultRifleView.ExitHoldPose();
            m_AssaultRifleView.M_GunStar.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    private void PlayAudio()
    {
        AudioSource.PlayClipAtPoint(audio,m_AssaultRifleView.M_GunPoint.position);
        Debug.Log("播放音效");
    }

    /// <summary>
    /// 播放特效
    /// </summary>
    private void PlayEffect()
    {
        //枪口特效
        GameObject.Instantiate<GameObject>(effect,m_AssaultRifleView.M_GunPoint.position,Quaternion.identity).GetComponent<ParticleSystem>().Play();
        //弹壳特效
        Rigidbody shell = GameObject.Instantiate<GameObject>(m_AssaultRifleView.M_Shell, m_AssaultRifleView.M_EffectPos.position, Quaternion.identity).GetComponent<Rigidbody>();
        shell.AddForce(m_AssaultRifleView.M_EffectPos.up *50);
    }

    /// <summary>
    /// 射击准备
    /// </summary>
    private void ShootReady()
    {
        ray = new Ray(m_AssaultRifleView.M_GunPoint.position,m_AssaultRifleView.M_GunPoint.forward);
        if (Physics.Raycast(ray, out hit))
        {
            Vector2 uiPos = RectTransformUtility.WorldToScreenPoint(m_AssaultRifleView.M_EnvCamera, hit.point);
            m_AssaultRifleView.M_GunStar.position = uiPos;
            Debug.Log("有碰撞信息");
        }
        else
        {
            Debug.Log("没有碰撞信息");
            hit.point = Vector3.zero;
        }
    }
    /// <summary>
    /// 射击
    /// </summary>
    private void Shoot()
    {
        if (hit.point != Vector3.zero)
        {
            GameObject.Instantiate<GameObject>(m_AssaultRifleView.M_Bullet, hit.point, Quaternion.identity);
        }
        else
        {
            Debug.Log("无子弹");
        }
    }


}
