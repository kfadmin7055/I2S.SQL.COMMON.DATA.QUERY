using I2S.SQL.COMMON.DATA.OraData.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace I2S.SQL.COMMON.DATA.OraData.COMMON
{
    /// <summary>
    /// 쿼리 추가 메서드
    /// </summary>
    public static class CommonQuery
    {
        #region :: 전역변수 ::
        static string queryText = string.Empty;
        static string setText = string.Empty;
        static string whereText = string.Empty;


        #endregion

        /// <summary>
        /// where 절 문자 만들기
        /// </summary>
        /// <param name="selectText"></param>
        /// <param name="paramList"></param>
        /// <param name="dt"></param>
        /// <param name="rightLikeColumn"></param>
        /// <param name="fullLikeColumn"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static string GetWhereText(this string selectText, string[] paramList, DataTable dt, string[] rightLikeColumn = null, string[] fullLikeColumn = null, string alias = null)
        {
            string whereText = string.Empty;
            string valueText = string.Empty;

            if (!selectText.Contains("WHERE"))
                selectText += @"WHERE 1 = 1
                ";

            for (int i = 0; i < paramList.Length; i++)
            {
                if (!paramList[i].Equals(""))
                {
                    valueText = !paramList[i].ToString().Contains(":") ? $":{paramList[i].ToString()}" : paramList[i].ToString();

                    if (rightLikeColumn != null && rightLikeColumn.Any(value => paramList[i].ToString().Contains(value)))
                    {
                        selectText += $@"AND {alias}.{paramList[i].ToString().Replace("@", "").Replace(":", "")} LIKE {valueText}%
                            ";
                    }
                    else if (fullLikeColumn != null && rightLikeColumn.Any(value => paramList[i].ToString().Contains(value)))
                    {
                        selectText += $@"AND {alias}.{paramList[i].ToString().Replace("@", "").Replace(":", "")} LIKE %{valueText}%
                            ";
                    }
                    else
                        selectText += $@"AND {alias}.{paramList[i].ToString().Replace("@", "").Replace(":", "")} = {valueText}
                            ";
                }
            }

            return selectText;
        }

        /// <summary>
        /// where 절 문자 만들기
        /// </summary>
        /// <param name="selectText"></param>
        /// <param name="param"></param>
        /// <param name="rightLikeColumn"></param>
        /// <param name="fullLikeColumn"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static string GetWhereText(this string selectText, ParamCollection param, string[] rightLikeColumn = null, string[] fullLikeColumn = null, string alias = null)
        {
            List<string> paramList = new List<string>();
            List<object> valueList = new List<object>();

            if (param != null)
            {
                foreach (DictionaryEntry dParam in param)
                {
                    paramList.Add(dParam.Key.ToString());
                    valueList.Add(dParam.Value);
                }
            }

            return selectText.GetWhereText(paramList.ToArray(), valueList.ToArray(), rightLikeColumn, fullLikeColumn, alias);
        }

        /// <summary>
        /// where 절 문자 만들기
        /// </summary>
        /// <param name="selectText"></param>
        /// <param name="paramList"></param>
        /// <param name="valueList"></param>
        /// <param name="rightLikeColumn"></param>
        /// <param name="fullLikeColumn"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static string GetWhereText(this string selectText, string[] paramList, object[] valueList, string[] rightLikeColumn = null, string[] fullLikeColumn = null, string alias = null)
        {
            string whereText = string.Empty;
            string valueText = string.Empty;

            if (!selectText.Contains("WHERE"))
                selectText += @"WHERE 1 = 1
                ";

            for (int i = 0; i < paramList.Length; i++)
            {
                if (!paramList[i].Equals("") && !valueList[i].Equals(""))
                {
                    valueText = !paramList[i].ToString().Contains(":") ? $":{paramList[i].ToString()}" : paramList[i].ToString();

                    if (rightLikeColumn != null && rightLikeColumn.Any(value => paramList[i].ToString().Contains(value)))
                    {
                        selectText += $@"AND {alias}.{paramList[i].ToString().Replace("@", "").Replace(":", "")} LIKE {valueText} || '%'
                            ";
                    }
                    else if (fullLikeColumn != null && rightLikeColumn.Any(value => paramList[i].ToString().Contains(value)))
                    {
                        selectText += $@"AND {alias}.{paramList[i].ToString().Replace("@", "").Replace(":", "")} LIKE '%' || {valueText} || '%'
                            ";
                    }
                    else
                        selectText += $@"AND {alias}.{paramList[i].ToString().Replace("@", "").Replace(":", "")} = {valueText}
                            ";
                }
            }

            return selectText;
        }



        #region 쿼리 관련 확장메서드
        /// <summary>
        /// 콤보리스트 쿼리
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="codeColumn"></param>
        /// <param name="displayColumn"></param>
        /// <param name="orderByColumn"></param>
        /// <param name="paramList"></param>
        /// <param name="valueList"></param>
        /// <param name="includedisplay"></param>
        /// <returns></returns>
        public static string GetCombo(string tableName, string codeColumn, string displayColumn, string orderByColumn, string[] paramList, object[] valueList, string includedisplay = "N")
        {
            string queryText = string.Empty;
            string displayValue = string.Empty;

            if (includedisplay == "Y")
                displayValue = $@"RTRIM(A.{codeColumn}) + '' : '' + RTRIM(A.{displayColumn}) AS DISPLAYVALUE
                                ";
            else
                displayValue = $@"RTRIM(a.{displayColumn}) AS DISPLAYVALUE
                                ";

            queryText = $@"
                           SELECT a.{codeColumn}
	                            , {displayValue}
                           FROM CodeMaster a 
                            ";

            queryText = queryText.GetWhereText(paramList, valueList, null, null, "A");

            queryText += $@"ORDER BY A.{orderByColumn}";

            return queryText;
        }

        public static string CompleteQuery(this string query, ParamCollection dbParams)
        {
            List<string> paramList = new List<string>();
            List<object> valueList = new List<object>();

            if (dbParams != null)
            {
                foreach (DictionaryEntry dParam in dbParams)
                {
                    paramList.Add(dParam.Key.ToString());
                    valueList.Add(dParam.Value);
                }
            }

            CompleteQuery(query, paramList, valueList);

            return query;
        }

        public static string CompleteQuery(this string query, string[] paramList, DataTable dt)
        {
            for (int k = 0; k < paramList.Length; k++)
            {
                query = query.Replace(paramList[k].ToString(), "'" + dt.Rows[0][paramList[k].Replace(":", "")].ToString() + "'");
            }

            return query;
        }

        public static string CompleteQuery(this string query, List<string> paramList, List<object> valueList)
        {
            for (int i = 0; i < paramList.Count; i++)
            {
                query = query.Replace(paramList[i], "'" + valueList[i] + "'");
            }

            return query;
        }

        #endregion

        #region 기타 쿼리
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string InitUserMenu()
        {
            queryText = string.Empty;

            queryText = $@"
                           SELECT A.MENUID
	                             , A.MENUNAME
	                             , A.LINKURL
	                             , A.LINKTYPE
	                             , A.ISBEGINGROUP
                            FROM LinkMenu A
                            WHERE 1 = 1 
                                AND A.USEFLAG = 'Y''
                            ";

            queryText += whereText;

            return queryText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string InitLinkMenu()
        {
            queryText = string.Empty;

            queryText = $@"
                           SELECT A.MENUID
	                             , A.MENUNAME
	                             , A.LINKURL
	                             , A.LINKTYPE
	                             , A.ISBEGINGROUP
                            FROM LinkMenu A
                            WHERE 1 = 1 
                                AND A.USEFLAG = 'Y'
                            ";

            queryText += whereText;

            return queryText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string InitFavoritesMenu(string USERID, string GUBUN)
        {
            queryText = string.Empty;

            queryText = $@"
                           SELECT DISTINCT A.USERID
	                             , A.MENUID
	                             , B.MENUNAME
	                             , C.DLLNAME
	                             , C.CLASSNAME
	                             , A.GUBUN
	                             , CASE WHEN '{GUBUN}' = 'F' THEN '' ELSE RTRIM(A.IPADDRESS) END IPADDRESS
                            FROM MyMenu A
                            JOIN MenuMaster B
                            ON A.MENUID = B.MENUID
                            JOIN ScreenMaster C
                            ON B.SCREENID = C.SCREENID
                            WHERE A.USERID = '{USERID}' AND A.GUBUN = '{GUBUN}'
                            ";

            queryText += whereText;

            return queryText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string SaveMenuExecuteLog()
        {
            queryText = string.Empty;

            queryText = $@"  /* SaveMenuExecuteLog(string menuId) */
		                        INSERT INTO MenuExecuteLog@KFLOG
				                        ( VENDORCODE,
				                          PLANTCODE,
				                          MENUID, 
				                          EXETIME, 
				                          SIGNINSEQ )
		                        VALUES
				                        ( :VENDORCODE,
				                          :PLANTCODE,
				                          :MENUID,
				                          :EXETIME,
				                          :SIGNINSEQ )
                            ";

            //queryText += whereText;

            return queryText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetDatabaseID(int server_id)
        {
            queryText = string.Empty;

            queryText = $@"
                        SELECT USER_ID AS CODEVALUE, USERNAME AS DISPLAYVALUE
                        FROM all_users
                        ";

            whereText = server_id > 0 ? $"WHERE USER_ID > :USER_ID" : null;

            queryText += whereText;

            return queryText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetServerID()
        {
            queryText = string.Empty;

            queryText = $@"
                       SELECT DBID, 'KFI2' AS DISPLAYVALUE
                        FROM v$DATABASE
                        ";

            return queryText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string DatabaseLogEvent_Get()
        {
            queryText = string.Empty;

            queryText = $@"SELECT  DISTINCT 
                                Event AS CODEVALUE, 
                                Event AS DISPLAYVALUE
                        FROM DatabaseLog@KFLOG
                        WHERE 1 = 1
                            AND DATABASE = :DATABASE
                        ";

            return queryText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string DatabaseLogObject_Get()
        {
            queryText = string.Empty;

            queryText = $@"SELECT  DISTINCT 
                                OBJECT AS CODEVALUE, 
                                OBJECT AS DISPLAYVALUE
                        FROM DatabaseLog@KFLOG
                        WHERE 1 = 1
                            AND DATABASE = :DATABASE
                        ";

            return queryText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string DatabaseLogObjectType_Get()
        {
            queryText = string.Empty;

            queryText = $@"SELECT  DISTINCT 
                                ObjectType AS CODEVALUE, 
                                ObjectType AS DISPLAYVALUE 
                        FROM DatabaseLog@KFLOG
                        WHERE 1 = 1
                            AND DATABASE = :DATABASE
                        ";

            return queryText;
        }

        #endregion
    }

}

