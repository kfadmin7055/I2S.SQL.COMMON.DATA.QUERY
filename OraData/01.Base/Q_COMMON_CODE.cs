using System.Data;

namespace I2S.SQL.COMMON.DATA.OraData.Base
{
    #region :: I2S.SQL.COMMON.DATA.OraData.Base.Q_COMMON_CODE ::


    /// <summary>
    /// COMMON_CODE 테이블 쿼리
    /// </summary>
    public static class Q_COMMON_CODE
    {
        static string queryText = string.Empty;

        #region :: COMMON_CODE 테이블 Select 쿼리

        /// <summary>
        /// COMMON_CODE 테이블 그룹 조회
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static string SelectMainQuery(string reference)
        {
            queryText = string.Empty;
            
            queryText = $@"
            /* {reference} */
            SELECT CODEVALUE
                , DISPLAYVALUE
                , PCODEVALUE
                , SORT_NUM
                , REF1
                , REF1_NAME
                , REF2
                , REF2_NAME
                , REF3
                , REF3_NAME
                , INITBY
                , UPDTTM
                , UPBY
                , INITDTTM
                , USEFLAG
            FROM COMMON_CODE
            WHERE CODEVALUE = :CODEVALUE
                AND (PCODEVALUE LIKE '%' || :DISPLAYVALUE || '%' 
                OR DISPLAYVALUE LIKE '%' || :DISPLAYVALUE || '%')
                AND (:USEFLAG IS NULL OR USEFLAG = :USEFLAG)
            ";

            return queryText;
        }

        /// <summary>
        /// COMMON_CODE 테이블 상세 조회
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static string SelectSubQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
            SELECT CODEVALUE
                , DISPLAYVALUE
                , PCODEVALUE
                , SORT_NUM
                , REF1
                , REF1_NAME
                , REF2
                , REF2_NAME
                , REF3
                , REF3_NAME
                , INITBY
                , UPDTTM
                , UPBY
                , INITDTTM
                , USEFLAG
            FROM COMMON_CODE
            WHERE (PCODEVALUE = :PCODEVALUE AND CODEVALUE <> '$COMMON')
                AND (CODEVALUE LIKE '%' || :DISPLAYVALUE || '%' 
                OR DISPLAYVALUE LIKE '%' || :DISPLAYVALUE || '%')
                AND (:USEFLAG IS NULL OR USEFLAG = :USEFLAG)
            ORDER BY SORT_NUM ASC
            ";

            return queryText;
        }
        #endregion

        #region :: COMMON_CODE 테이블 Merge 쿼리
        /// <summary>
        /// COMMON_CODE 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string MergeMain(string reference)
        {
            queryText = string.Empty;

            queryText = $@"            
            /* {reference} */
            MERGE INTO COMMON_CODE d
            USING (
                SELECT
                      :CODEVALUE      AS CODEVALUE
                    , :DISPLAYVALUE   AS DISPLAYVALUE
                    , :PCODEVALUE     AS PCODEVALUE
                    , :USEFLAG        AS USEFLAG
                    , SYSDATE         AS CHANGEDTTM
                    , :CHANGEBY        AS CHANGEBY
                FROM DUAL
            ) s
            ON (
                   d.CODEVALUE  = s.CODEVALUE
               AND d.PCODEVALUE = s.PCODEVALUE
            )

            WHEN MATCHED THEN
            UPDATE SET
                  d.DISPLAYVALUE = s.DISPLAYVALUE
                , d.USEFLAG      = s.USEFLAG
                , d.UPBY         = s.CHANGEBY
                , d.UPDTTM       = s.CHANGEDTTM

            WHEN NOT MATCHED THEN
            INSERT (
                  CODEVALUE
                , DISPLAYVALUE
                , PCODEVALUE
                , USEFLAG
                , INITBY
                , INITDTTM
            )
            VALUES (
                  s.CODEVALUE
                , s.DISPLAYVALUE
                , s.PCODEVALUE
                , s.USEFLAG
                , s.CHANGEBY
                , s.CHANGEDTTM
            )
            ";

            return queryText;
        }

        public static string MergeSub(string reference)
        {
            queryText = string.Empty;

            queryText = $@"            
            /* {reference} */
            MERGE INTO COMMON_CODE d
            USING (
                SELECT
                      :CODEVALUE      AS CODEVALUE
                    , :DISPLAYVALUE   AS DISPLAYVALUE
                    , :PCODEVALUE     AS PCODEVALUE
                    , :SORT_NUM       AS SORT_NUM
                    , :REF1           AS REF1
                    , :REF1_NAME      AS REF1_NAME
                    , :REF2           AS REF2
                    , :REF2_NAME      AS REF2_NAME
                    , :REF3           AS REF3
                    , :REF3_NAME      AS REF3_NAME
                    , :USEFLAG        AS USEFLAG
                    , SYSDATE         AS CHANGEDTTM
                    , :CHANGEBY        AS CHANGEBY
                FROM DUAL
            ) s
            ON (
                   d.CODEVALUE  = s.CODEVALUE
               AND d.PCODEVALUE = s.PCODEVALUE
            )

            WHEN MATCHED THEN
            UPDATE SET
                  d.DISPLAYVALUE = s.DISPLAYVALUE
                , d.SORT_NUM     = s.SORT_NUM
                , d.REF1         = s.REF1
                , d.REF1_NAME    = s.REF1_NAME
                , d.REF2         = s.REF2
                , d.REF2_NAME    = s.REF2_NAME
                , d.REF3         = s.REF3
                , d.REF3_NAME    = s.REF3_NAME
                , d.USEFLAG      = s.USEFLAG
                , d.UPBY         = s.CHANGEBY
                , d.UPDTTM       = s.CHANGEDTTM

            WHEN NOT MATCHED THEN
            INSERT (
                  CODEVALUE
                , DISPLAYVALUE
                , PCODEVALUE
                , SORT_NUM
                , REF1
                , REF1_NAME
                , REF2
                , REF2_NAME
                , REF3
                , REF3_NAME
                , USEFLAG
                , INITBY
                , INITDTTM
            )
            VALUES (
                  s.CODEVALUE
                , s.DISPLAYVALUE
                , s.PCODEVALUE
                , s.SORT_NUM
                , s.REF1
                , s.REF1_NAME
                , s.REF2
                , s.REF2_NAME
                , s.REF3
                , s.REF3_NAME
                , s.USEFLAG
                , s.CHANGEBY
                , s.CHANGEDTTM
            )
            ";

            return queryText;
        }
        #endregion

        #region :: COMMON_CODE 테이블 Delete 쿼리
        /// <summary>
        /// COMMON_CODE 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string DeleteMain(string reference)
        {
            queryText = string.Empty;

            queryText = @"
            /* {reference} */
            DELETE FROM COMMON_CODE
            WHERE 1 = 1
                AND PCODEVALUE = :PCODEVALUE
            ";
            return queryText;
        }

        /// <summary>
        /// COMMON_CODE 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string DeleteSub(string reference)
        {
            queryText = string.Empty;

            queryText = @"
            /* {reference} */
            DELETE FROM COMMON_CODE
            WHERE 1 = 1
                AND PCODEVALUE = :PCODEVALUE
                AND (:CODEVALUE IS NULL OR CODEVALUE = :CODEVALUE)
            ";
            return queryText;
        }
        #endregion

        /// <summary>
        /// 공통코드의 PCODEVALUE 를 기준으로 (CODEVALUE, DISPLAYVALUE)필드를 가져옵니다.
        /// </summary>
        /// <param name="reference">호출하는 곳</param>
        /// <returns></returns>
        public static string GetCommonCombo(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
            SELECT
                CODEVALUE
                , DISPLAYVALUE
            FROM COMMON_CODE
            WHERE PCODEVALUE = :PCODEVALUE
                AND CODEVALUE != '$COMMON'
                AND USEFLAG = '1'
            ORDER BY SORT_NUM ASC
            ";

            return queryText;
        }
    }

    #endregion
}
