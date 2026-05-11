using I2S.SQL.COMMON.DATA.OraData.COMMON;
using System.Data;

namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    /// <summary>
    /// CodeMaster 테이블 쿼리
    /// </summary>
    public static class Q_CodeMaster
    {
        static string queryText = string.Empty;

        static string USERID = string.Empty;

        #region :: MenuLog 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            DataSet ds = new DataSet();
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT a.SITE
	                        , a.CODEVALUE
	                        , a.DISPLAYVALUE
	                        , a.PCODEVALUE
	                        , a.MEMO
	                        , a.IDX
	                        , a.REF1
	                        , a.REF2
	                        , a.REF3
	                        , a.USEFLAG
	                        , NVL(a.UPDTTM, a.INITDTTM) CHANGEDTTM 
	                        , NVL(a.UPBY, a.INITBY) CHANGEBY 
                        FROM CodeMaster a 
                        WHERE 1 = 1
                            AND (:SITE IS NULL OR A.SITE = :SITE)
                            AND (:CODEVALUE IS NULL OR A.CODEVALUE = :CODEVALUE)
                            AND (:DISPLAYVALUE IS NULL OR A.DISPLAYVALUE LIKE '%' || :DISPLAYVALUE || '%')
                            AND A.PCODEVALUE = :PCODEVALUE
                        ";

            return queryText;
        }

        /// <summary>
        /// CodeMaster 테이블 조회 쿼리
        /// 특정 조인 쿼리는 Select_[화면명] 등으로 새로 만든다.
        /// </summary>
        /// <param name="param">where 문</param>
        /// <returns></returns>
        public static string ComboList(string[] paramList, object[] valueList)
        {
            queryText = string.Empty;

            queryText += CommonQuery.GetCombo("CodeMaster", "CODEVALUE", "DISPLAYVALUE", "IDX", paramList, valueList, "N");

            return queryText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CodeMaster_Get(string includedisplay = "N")
        {
            queryText = string.Empty;

            string selectText = includedisplay == "N" ? "RTRIM(A.DISPLAYVALUE) AS DISPLAYVALUE" : "RTRIM(A.CODEVALUE) || ':' || RTRIM(A.DISPLAYVALUE) AS DISPLAYVALUE";

            queryText = $@"/* CodeMaster_Get(string includedisplay = ""N"") */
                        SELECT A.CODEVALUE, {selectText}	
	                    FROM CodeMaster A
	                    WHERE A.USEFLAG = 'Y'
		                    AND ((:PCODEVALUE IS NULL) OR (A.PCODEVALUE = :PCODEVALUE))
                            AND ((:CODEVALUE IS NULL) OR (A.CODEVALUE = :CODEVALUE))
	                    ORDER BY A.IDX 
                        ";

            return queryText;
        }

        /// <summary>
        /// 공통코드에서 참조값을 가져오기 위함
        /// </summary>
        /// <returns></returns>
        public static string CodeMaster_GetRef(string REFCol, string PCODEVALUE, string CODEVALUE)
        {
            queryText = string.Empty;

            queryText = $@"/* CodeMaster_GetRef(string REFCol) */
                        SELECT A.{REFCol}
	                    FROM CodeMaster A
	                    WHERE A.USEFLAG = 'Y'
		                    AND A.PCODEVALUE = {PCODEVALUE}
                            AND A.CODEVALUE = {CODEVALUE}
	                    ORDER BY A.IDX 
                        ";

            return queryText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetVendorCode(string userId)
        {
            queryText = string.Empty;

            queryText = $@"/* GetVendorCode(string userId) */
                        SELECT A.CODEVALUE
	                            , A.DISPLAYVALUE
                        FROM CodeMaster A
                        WHERE 1 = 1
                            AND A.USEFLAG = 'Y'
                            AND A.PCODEVALUE = 'C$VENDORCODE'
                            AND A.CODEVALUE IN ( SELECT VENDORCODE FROM VendorAuth WHERE USERID = :USERID  )
                        ORDER BY A.IDX
                        ";

            return queryText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetPlantCode(string userId)
        {
            queryText = string.Empty;

            queryText = $@"/* GetPlantCode(string userId) */
                        SELECT A.CODEVALUE
	                            , A.DISPLAYVALUE
                        FROM CodeMaster A
                        WHERE 1 = 1
                            AND A.USEFLAG = 'Y'
                            AND A.PCODEVALUE =  'C$PLANTCODE'
                            AND A.CODEVALUE IN ( SELECT PLANTCODE FROM PlantAuth WHERE USERID = :USERID  )
                        ORDER BY A.IDX
                        ";

            return queryText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetUseVendor()
        {
            queryText = string.Empty;

            queryText = $@"/* GetUseVendor() */
                           SELECT A.CODEVALUE
	                            , A.DISPLAYVALUE
	                            , A.IDX
	                            , A.USEFLAG 
                            FROM CodeMaster A
                            WHERE 1 = 1
                                AND A.PCODEVALUE = 'C$VENDORCODE'
                            ORDER BY A.IDX
                            ";

            return queryText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetUsePlant()
        {
            queryText = string.Empty;

            queryText = $@"/* GetUsePlant() */
                           SELECT A.CODEVALUE
	                            , A.DISPLAYVALUE
	                            , A.IDX
	                            , A.USEFLAG 
                            FROM CodeMaster A
                            WHERE 1 = 1
                                AND A.PCODEVALUE =  'C$PLANTCODE'
                            ORDER BY A.IDX
                            ";

            return queryText;
        }

        /// <summary>
        /// 공통코드 가져오기
        /// </summary>
        /// /// <param name="PCODEVALUE"></param>
        /// <returns></returns>
        public static string GetCodeValue(string PCODEVALUE)
        {
            queryText = string.Empty;

            queryText = $@"/* GetCodeValue(string PCODEVALUE) */
                        SELECT CODEVALUE
                        FROM CodeMaster
                        WHERE PCODEVALUE = '{PCODEVALUE}'
                            ";

            return queryText;
        }

        /// <summary>
        /// 공통코드 상세코드로 참조값 가져오기
        /// </summary>
        /// <param name="RefNumber"></param>
        /// <param name="PCODEVALUE"></param>
        /// <param name="CODEVALUE"></param>
        /// <returns></returns>
        public static string GetRefValue(string RefNumber, string PCODEVALUE, string CODEVALUE)
        {
            queryText = string.Empty;

            queryText = $@"/* GetRefValue(string RefNumber, string PCODEVALUE, string CODEVALUE) */
                        SELECT {RefNumber}
                        FROM CodeMaster
                        WHERE PCODEVALUE = '{PCODEVALUE}'
                            AND CODEVALUE = '{CODEVALUE}'
                            ";

            return queryText;
        }

        #endregion

        public static string Merge()
        { 
            queryText = string.Empty;

            queryText = $@"
                        MERGE INTO KFMETA.CODEMASTER D
                        USING (
                          SELECT
                            :SITE                     AS SITE,
                            :CODEVALUE                AS CODEVALUE,
                            :DISPLAYVALUE             AS DISPLAYVALUE,
                            NVL(:PCODEVALUE, 'P')     AS PCODEVALUE,
                            :MEMO                     AS MEMO,
                            :IDX                      AS IDX,
                            :REF1                     AS REF1,
                            :REF2                     AS REF2,
                            :REF3                     AS REF3,
                            :USEFLAG                  AS USEFLAG,
                            :CHANGEBY                 AS INITBY,
                            SYSDATE                   AS INITDTTM,
                            :CHANGEBY                 AS UPBY,
                            SYSDATE                   AS UPDTTM
                          FROM Dual) s
                        ON
                          (d.CODEVALUE = s.CODEVALUE AND 
                          d.PCODEVALUE = s.PCODEVALUE AND 
                          d.IDX = s.IDX )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.SITE = s.SITE,
                          d.DISPLAYVALUE = s.DISPLAYVALUE,
                          d.MEMO = s.MEMO,
                          d.REF1 = s.REF1,
                          d.REF2 = s.REF2,
                          d.REF3 = s.REF3,
                          d.USEFLAG = s.USEFLAG,
                          d.UPBY = s.UPBY,
                          d.UPDTTM = s.UPDTTM
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          SITE, CODEVALUE, DISPLAYVALUE,
                          PCODEVALUE, MEMO, IDX,
                          REF1, REF2, REF3,
                          USEFLAG, INITBY, INITDTTM)
                        VALUES (
                          s.SITE, s.CODEVALUE, s.DISPLAYVALUE,
                          s.PCODEVALUE, s.MEMO, s.IDX,
                          s.REF1, s.REF2, s.REF3,
                          s.USEFLAG, s.INITBY, s.INITDTTM)
                        ";

            return queryText;
        }

        /// <summary>
        /// CodeMaster 테이블 삭제 쿼리
        /// </summary>
        /// <param name="param">where 문</param>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = $@"
                            DELETE FROM CodeMaster
                            WHERE (CODEVALUE = :CODEVALUE OR PCODEVALUE = :CODEVALUE)
                            ";

            return queryText;
        }

        /// <summary>
        /// CodeMaster 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string DeleteDetail()
        {
            queryText = string.Empty;

            queryText = $@"
                            DELETE FROM CodeMaster
		                    WHERE CODEVALUE = :CODEVALUE AND PCODEVALUE = :PCODEVALUE
                            ";

            return queryText;
        }
    }
}
