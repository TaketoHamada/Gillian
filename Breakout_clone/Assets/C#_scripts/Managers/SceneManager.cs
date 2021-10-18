using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �V�[���J�ڂ��Ǘ�����B��̃N���X
/// </summary>
public class SceneManager : MonoBehaviour
{
    private static SceneManager m_instance = null;

    [SerializeField,Tooltip("��O�̃V�[����")]
    private string m_beforScene = "";

    [SerializeField,Tooltip("���݂̃V�[����")]
    private string m_nowScene = "";

    [SerializeField,Tooltip("���[�h���̃V�[����")]
    private string m_loadingScene = "";

    [SerializeField,Tooltip("�V�������[�h���󂯕t���邩")]
    private bool m_allowload = true;

    [SerializeField, Tooltip("���[�h�̏��")]
    private AsyncOperation m_loadAsync = null;

    [SerializeField, Tooltip("�J�ڂł����Ԃ��ǂ���")]
    private bool m_allowActive = false;

    void Awake()
    {
        //���߂ăC���X�^���X���������ꂽ�ꍇ�̂݁A�o�^����
        if (m_instance == null)
        {
            m_instance = this;
            //�V�[�����ׂ���悤�ɂ���
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    /// <summary>
    /// �V�[����񓯊��œǂݍ���
    /// </summary>
    /// <returns></returns>
    private IEnumerator ILoadScene()
    {
        //�V�[���̃��[�h��񓯊��Ŏn�߂�
        m_loadAsync = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(m_loadingScene);

        //�ǂݍ��݌㒼���ɑJ�ڂ��邩���Ȃ�����ݒ肷��
        m_loadAsync.allowSceneActivation = m_allowActive;

        //�J�ڂ��������܂őҋ@
        while (m_allowActive == false)
        {
            yield return null;
        }

        //�V�[����ǂݍ���
        m_loadAsync.allowSceneActivation = true;

        //���݂̃V�[����O�̃V�[�����Ƃ��ĕۑ�
        m_beforScene = m_nowScene;
        //���݂̃V�[���Ƀ��[�h���������V�[������ۑ�
        m_nowScene = m_loadingScene;
        //���[�h���̃V�[�������󔒂ɂ���
        m_loadingScene = "";
        
        //���[�h�����J������
        m_loadAsync = null;

        //���̃��[�h���󂯕t����悤�ɂ���
        m_allowload = true;

    }

    /// <summary>
    /// �w�肵�����O�̃V�[�������݂��邩�ǂ���
    /// </summary>
    /// <param name="sceneName_"></param>
    /// <returns></returns>
    private bool IsExistScene(string sceneName_)
    {
        return UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName_) != null ;
    }


    //======================================================================
    //�V�[���擾�p
    //======================================================================
    public string BeforScene
    {
        get{ return m_beforScene; }
    }

    public string NowScne
    {
        get { return m_nowScene; }
    }

    public string LoadingScene
    {
        get { return m_loadingScene; }
    }


    //======================================================================
    //�B��̃C���X�^���X���擾����
    //======================================================================
    public static SceneManager Instance
    {
        get { return m_instance; }
    }


    [ContextMenu("�V�[���̃��[�h���J�n����")]
    /// <summary>
    /// ���[�h���J�n����@waitActive_==true�Ȃ�蓮�őJ�ڂ�����
    /// </summary>
    /// <param name="waitActive_"></param>
    /// <returns></returns>
    public bool LoadScene(bool waitActive_,string sceneName_)
    {
        //���łɓǂݍ��ݒ��A�܂���scene�����݂��Ȃ��Ȃ�false��Ԃ�
        if (m_allowload == false || !IsExistScene(sceneName_)) { return false; }

        //�J�ڃ^�C�~���O�̃I�v�V������ݒ�
        m_allowActive = !waitActive_;

        //�J�ڐ��ݒ肷��
        m_loadingScene = sceneName_;

        //�V�[���̃��[�h���J�n����
        StartCoroutine(ILoadScene());
        return true;
    }

    /// <summary>
    /// �J�ڂ�������
    /// </summary>
    public void AllowActive()
    {
        m_allowActive = true;
    }

}
