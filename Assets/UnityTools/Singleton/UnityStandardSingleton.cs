using System.Collections;
using UnityEngine;


/// <summary>
/// unity 标准双锁线程安全单例类
/// 无需挂载，需要用到时自动生成GameObject
/// 并且在场景切换时不会被删除
/// </summary>
/// <typeparam name="T"></typeparam>
public class UnityStandardSingleton<T> : MonoBehaviour
where T : MonoBehaviour
{
    private static T instance;

    private static readonly object syncLock;

    private static bool applicationIsQutting;

    static UnityStandardSingleton()
    {
        syncLock = new object();
    }
    
    public static T Instance
    {
        get
        {
            if (applicationIsQutting)
            {
                return null;
            }

            if (instance == null)
            {
                lock (syncLock)
                {
                    if (instance == null)
                    {
                        instance = FindObjectOfType<T>();
                        if (FindObjectsOfType<T>().Length > 1)
                        {
                            return instance;
                        }

                        if (instance == null)
                        {
                            // 创建gameobject物体
                            GameObject singleton = new GameObject();
                            // 在物体上加组件T
                            instance = singleton.AddComponent<T>();
                            // 给该物体重命名
                            singleton.name = "(singleton)" + typeof(T).Name;
                            // 设置场景切换时不销毁该物体
                            DontDestroyOnLoad(singleton);
                        }
                    }
                }
            }

            return instance;
        }
    }
	
	public void OnDestory()
	{
		applicationIsQutting = true;
	}
}

// 注意：typeof(T).Name和nameof(T)不同！！！！！！！！
/*
using System.Collections;
using UnityEngine;


/// <summary>
/// unity标准单例模式
/// </summary>
/// <remarks>
/// <para>介绍：</para>
/// <para>该单例模式无需挂载到物体，在运行时自动生成物体并自动挂载，且物体不会因为场景改变而销毁</para>
/// <para>格式：物体名称为：singleton + 类型名</para>
/// <para></para>
/// </remarks>
/// <typeparam name="T"></typeparam>
public class UnityStandardSingleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    /// <summary>
    /// 单例中的instance
    /// </summary>
    private static T instance;

    /// <summary>
    /// 线程锁
    /// </summary>
    private static object syncLock;

    /// <summary>
    /// 应用是否退出
    /// </summary>
    private static bool applicationIsQutting;

    // 事实证明，unity需要构造函数
    /// <summary>
    /// 将构造函数变为不可访问，防止外部new
    /// </summary>
    //private UnityStandardSingleton()
    //{

    //}

    /// <summary>
    /// 对static的成员进行必要的初始化
    /// </summary>
    static UnityStandardSingleton()
    {
        syncLock = new object();
        instance = null;
        applicationIsQutting = false;
    }

    /// <summary>
    /// 对单例instance的访问器
    /// </summary>
    /// <remarks>
    /// <para>特点：</para>
    /// <para>实现了应用退出</para>
    /// </remarks>
    /// <example>
    /// <code lang="Csharp">
    /// <![CDATA[
    /// UnityStandardSingleton.Instance.xxx
    /// ]]>
    /// </code>
    /// </example>
    public static T Instance
    {
        get
        {
            // 如果应用退出，则给出报错，并弹出异常
            if (applicationIsQutting)
            {
                Debug.LogError($"{nameof(T)}:{nameof(UnityStandardSingleton<T>)}.Instance:" +
                    $"application is quttiong! can not find this singleton object");
                //throw new System.Exception();
                return null;
            }
            // 双锁实现单例模式
            // 新的unity单例实例化的步骤：
            // 创建新物体，设置新物体的相关参数，并给该物体挂载脚本，设置该物体为场景切换不销毁的物体
            if (instance == null)
            {
                lock (syncLock)
                {
                    if (instance == null)
                    {
                        GameObject gameObject = new GameObject();
                        instance = gameObject.AddComponent<T>();
                        gameObject.tag = Tags.Controller;
                        gameObject.name = $"Singleton{nameof(T)}";

                        DontDestroyOnLoad(gameObject);
                    }
                }
            }
            return instance;
        }

        private set => instance = value;
    }

    /// <summary>
    /// 当游戏物体习销毁时，添加销毁标记
    /// </summary>
    public virtual void OnDestroy()
    {
        // 将脚本中国的退出标记改为true，表示物体消失
        applicationIsQutting = true;
        // 释放资源
        Instance = null;
    }
}

*/