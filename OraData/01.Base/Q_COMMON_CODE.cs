namespace I2S.SQL.COMMON.DATA.OraData._01.Base
{
    #region :: I2S.SQL.COMMON.DATA.OraData._01.Base.Q_COMMON_CODE ::


    /// <summary>
    /// COMMON_CODE 테이블 쿼리
    /// </summary>
    public static class Q_COMMON_CODE
    {
        static string queryText = string.Empty;

        #region :: COMMON_CODE 테이블 Select 쿼리

        public static string SelectQuery(string reference)
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
            WHERE (:PCODEVALUE IS NULL OR PCODEVALUE = :PCODEVALUE)
                AND (CODEVALUE LIKE '%' || :DISPLAYVALUE || '%' 
                OR DISPLAYVALUE LIKE '%' || :DISPLAYVALUE || '%')
                AND (:USEFLAG IS NULL OR USEFLAG = :USEFLAG)
            ";

            return queryText;
        }
        #endregion

        #region :: COMMON_CODE 테이블 Merge 쿼리
        /// <summary>
        /// COMMON_CODE 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge(string reference)
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
                    , :USER_ID        AS CHANGEBY
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
                , UPBY
                , UPDTTM
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
        public static string Delete(string reference)
        {
            queryText = string.Empty;

            queryText = @"
            DELETE FROM COMMON_CODE
            WHERE 1 = 1
                AND GROUPCODE = :GROUPCODE
                AND (:DETAILCODE IS NULL OR DETAILCODE = :DETAILCODE)
            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
