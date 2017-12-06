
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;
namespace V5_WinLibs.Core {
    /// <summary>
    /// ȫ�ֻ�����
    /// </summary>
    /// <example><code>
    /// ʹ��ʾ����
    /// ʵ������ CacheManage cache=CacheManage.Instance;
    /// ��ӣ�   cache.Add("123",new MDataTable);
    /// �жϣ�   if(cache.Contains("123"))
    ///          {
    /// ��ȡ��       MDataTable table=cache.Get("123") as MDataTable;
    ///          }
    /// </code></example>
    public class CacheManageHelper {
        private readonly System.Web.HttpContext H = new System.Web.HttpContext(new System.Web.HttpRequest("Null.File", "http://cyq1162.cnblogs.com", String.Empty), new System.Web.HttpResponse(null));
        private System.Web.Caching.Cache theCache = null;
        private Dictionary<string, CacheDependencyInfo> cacheState = new Dictionary<string, CacheDependencyInfo>();
        private CacheManageHelper() {
            theCache = H.Cache;
        }
        /// <summary>
        /// ��ͻ�������
        /// </summary>
        public int Count {
            get {
                return theCache == null ? 0 : theCache.Count;
            }
        }
        /// <summary>
        /// ����Ψһʵ��
        /// </summary>
        public static CacheManageHelper Instance {
            get {
                return Shell.instance;
            }
        }

        class Shell {
            internal static readonly CacheManageHelper instance = new CacheManageHelper();
        }

        /// <summary>
        /// ���һ��Cache����
        /// </summary>
        /// <param name="key">��ʶ</param>
        /// <returns></returns>
        public object Get(string key) {
            if (Contains(key)) {
                return theCache[key];
            }
            return null;
        }
        /// <summary>
        /// �Ƿ���ڻ���
        /// </summary>
        /// <param name="key">��ʶ</param>
        /// <returns></returns>
        public bool Contains(string key) {
            return theCache != null && theCache[key] != null;
        }
        /// <summary>
        /// ���һ��Cache����
        /// </summary>
        /// <param name="key">��ʶ</param>
        /// <param name="value">����ֵ</param>
        public void Add(string key, object value) {
            Add(key, value, null);
        }
        public void Add(string key, object value, string filePath) {
            if (!Contains(key)) {
                Add(key, value, filePath, 0);
            }
        }
        public void Add(string key, object value, string filePath, int cacheTimeMinutes) {
            if (!Contains(key)) {
                Insert(key, value, filePath, cacheTimeMinutes);
            }
        }
        /// <summary>
        /// ��Եײ�Cache��ӷ���,���һ��Cache����Add����
        /// </summary>
        /// <param name="key">��ʶ</param>
        /// <param name="value">����ֵ</param>
        /// <param name="filePath">�ļ�����·��</param>
        private void Insert(string key, object value, string filePath, int cacheTimeMinutes) {
            CacheDependency theCacheDependency = null;
            if (!string.IsNullOrEmpty(filePath)) {
                theCacheDependency = new CacheDependency(filePath);
            }
            int cacheTime = cacheTimeMinutes;
            if (cacheTimeMinutes == 0) {
                cacheTime = 1000;
            }
            theCache.Insert(key, value, theCacheDependency, DateTime.Now.AddMinutes(cacheTime == 0 ? 20 : cacheTime), TimeSpan.Zero, CacheItemPriority.Default, null);
            if (cacheState.ContainsKey(key)) {
                cacheState.Remove(key);
            }
            CacheDependencyInfo info = new CacheDependencyInfo(theCacheDependency);
            cacheState.Add(key, info);
        }

        /// <summary>
        /// ɾ��һ��Cache����
        /// </summary>
        /// <param name="key">��ʶ</param>
        public void Remove(string key) {
            if (Contains(key)) {
                theCache.Remove(key);
                cacheState.Remove(key);
            }
        }

        /// <summary>
        /// �û��ֶ����û�������Ѹ���
        /// </summary>
        /// <param name="key"></param>
        /// <param name="change"></param>
        public void SetChange(string key, bool change) {
            if (cacheState.ContainsKey(key) && Contains(key)) {
                CacheDependencyInfo info = cacheState[key];
                if (info != null) {
                    info.UserSetChange(change);
                }
            }
        }
        /// <summary>
        /// ��ȡ��������Ƿ��Ѹ���
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetHasChanged(string key) {
            if (cacheState.ContainsKey(key) && Contains(key)) {
                CacheDependencyInfo info = cacheState[key];
                if (info != null) {
                    return info.IsChanged ? false : info.UserChange;
                }
            }
            return false;
        }
    }
    /// <summary>
    /// ����������Ϣ
    /// </summary>
    internal class CacheDependencyInfo {
        public CacheDependencyInfo(CacheDependency dependency) {
            if (dependency != null) {
                FileDependency = dependency;
                CacheChangeTime = FileDependency.UtcLastModified;
            }
        }
        /// <summary>
        /// ϵͳ�ļ������Ƿ����ı�
        /// </summary>
        public bool IsChanged {
            get {
                if (FileDependency != null && (FileDependency.HasChanged || CacheChangeTime != FileDependency.UtcLastModified)) {
                    CacheChangeTime = FileDependency.UtcLastModified;
                    return true;
                }
                return false;
            }
        }
        CacheDependency FileDependency = null;
        public bool UserChange = false;
        DateTime CacheChangeTime = DateTime.MinValue;
        public void UserSetChange(bool change) {
            UserChange = IsChanged ? false : change;
        }
    }
}
