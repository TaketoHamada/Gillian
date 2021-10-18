using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// シーン遷移を管理する唯一のクラス
/// </summary>
public class SceneManager : MonoBehaviour
{
    private static SceneManager m_instance = null;

    [SerializeField,Tooltip("一つ前のシーン名")]
    private string m_beforScene = "";

    [SerializeField,Tooltip("現在のシーン名")]
    private string m_nowScene = "";

    [SerializeField,Tooltip("ロード中のシーン名")]
    private string m_loadingScene = "";

    [SerializeField,Tooltip("新しくロードを受け付けるか")]
    private bool m_allowload = true;

    [SerializeField, Tooltip("ロードの状態")]
    private AsyncOperation m_loadAsync = null;

    [SerializeField, Tooltip("遷移できる状態かどうか")]
    private bool m_allowActive = false;

    void Awake()
    {
        //初めてインスタンスが生成された場合のみ、登録する
        if (m_instance == null)
        {
            m_instance = this;
            //シーンを跨げるようにする
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    /// <summary>
    /// シーンを非同期で読み込む
    /// </summary>
    /// <returns></returns>
    private IEnumerator ILoadScene()
    {
        //シーンのロードを非同期で始める
        m_loadAsync = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(m_loadingScene);

        //読み込み後直ぐに遷移するかしないかを設定する
        m_loadAsync.allowSceneActivation = m_allowActive;

        //遷移を許可されるまで待機
        while (m_allowActive == false)
        {
            yield return null;
        }

        //シーンを読み込む
        m_loadAsync.allowSceneActivation = true;

        //現在のシーンを前のシーン名として保存
        m_beforScene = m_nowScene;
        //現在のシーンにロード中だったシーン名を保存
        m_nowScene = m_loadingScene;
        //ロード中のシーン名を空白にする
        m_loadingScene = "";
        
        //ロード情報を開放する
        m_loadAsync = null;

        //次のロードを受け付けるようにする
        m_allowload = true;

    }

    /// <summary>
    /// 指定した名前のシーンが存在するかどうか
    /// </summary>
    /// <param name="sceneName_"></param>
    /// <returns></returns>
    private bool IsExistScene(string sceneName_)
    {
        return UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName_) != null ;
    }


    //======================================================================
    //シーン取得用
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
    //唯一のインスタンスを取得する
    //======================================================================
    public static SceneManager Instance
    {
        get { return m_instance; }
    }


    [ContextMenu("シーンのロードを開始する")]
    /// <summary>
    /// ロードを開始する　waitActive_==trueなら手動で遷移させる
    /// </summary>
    /// <param name="waitActive_"></param>
    /// <returns></returns>
    public bool LoadScene(bool waitActive_,string sceneName_)
    {
        //すでに読み込み中、またはsceneが存在しないならfalseを返す
        if (m_allowload == false || !IsExistScene(sceneName_)) { return false; }

        //遷移タイミングのオプションを設定
        m_allowActive = !waitActive_;

        //遷移先を設定する
        m_loadingScene = sceneName_;

        //シーンのロードを開始する
        StartCoroutine(ILoadScene());
        return true;
    }

    /// <summary>
    /// 遷移を許可する
    /// </summary>
    public void AllowActive()
    {
        m_allowActive = true;
    }

}
