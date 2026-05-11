using System.Collections;
using System.Collections.Generic;

namespace I2S.SQL.COMMON.DATA.OraData.Collections
{
    /// <summary>
    /// System의 Parameter 를 생성하는 Class 입니다.
    /// </summary>
    /// <remarks>
    /// 2023-02-01 최초생성 : 오인봉
    /// 변경내역
    /// 
    /// </remarks>
    public class ParamCollection : DictionaryBase
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////////////////////////////

        #region :: Index :: DbParams의 Index를 설정합니다.

        /// <summary>
        /// DbParams의 Index를 설정합니다.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[object key]
        {
            get { return ((object)Dictionary[key]); }
            set { Dictionary[key] = value; }
        }

        #endregion

        #region :: Keys ::  DbParams의 Key를 가져옵니다.

        /// <summary>
        /// DbParams의 Key를 가져옵니다.
        /// </summary>
        public ICollection Keys
        {
            get { return (Dictionary.Keys); }
        }

        #endregion

        #region :: Values :: DbParams의 값을 가져옵니다.

        /// <summary>
        /// DbParams의 값을 가져옵니다.
        /// </summary>
        public ICollection Values
        {
            get { return (Dictionary.Values); }
        }

        #endregion

        #region :: Add :: DbParams의 값을 추가 합니다.

        /// <summary>
        /// DbParams의 값을 추가 합니다.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, object value)
        {
            if (Contains(key))
                Dictionary[key] = value;
            else
                Dictionary.Add(key, value);
        }

        /// <summary>
        /// DbParams의 값을 추가 합니다.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(int key, object value)
        {
            if (Contains(key))
                Dictionary[key] = value;
            else
                Dictionary.Add(key, value);
        }

        #endregion

        #region :: Remove :: DbParams의 값을 제거 합니다.

        /// <summary>
        /// DbParams의 값을 제거 합니다.
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            Dictionary.Remove(key);
        }

        /// <summary>
        /// DbParams의 값을 제거 합니다.
        /// </summary>
        /// <param name="key"></param>
        public void Remove(int key)
        {
            Dictionary.Remove(key);
        }

        #endregion

        #region :: Contains :: DbParams의 값이 있는지 Check 합니다.

        /// <summary>
        /// DbParams의 값이 있는지 Check 합니다.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return (Dictionary.Contains(key));
        }

        /// <summary>
        /// DbParams의 값이 있는지 Check 합니다.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(int key)
        {
            return (Dictionary.Contains(key));
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Method
        ///////////////////////////////////////////////////////////////////////////////////////////////

        #region :: GetParamList() ::

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string[] GetParamList()
        {
            List<string> paramList = new List<string>();

            foreach (DictionaryEntry dParam in this)
            {
                paramList.Add(dParam.Key.ToString());
            }
            return paramList.ToArray();
        }

        #endregion

        #region :: GetValueList() ::

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object[] GetValueList()
        {
            List<object> valueList = new List<object>();

            foreach (DictionaryEntry dParam in this)
            {
                valueList.Add(dParam.Value);
            }
            return valueList.ToArray();
        }

        #endregion
    }
}
