namespace I2S.SQL.COMMON.DATA.OraData.ProductInfo
{
    #region :: I2S.SQL.COMMON.DATA.OraData.ProductInfo.Q_Company ::


    /// <summary>
    /// COMPANY 테이블 쿼리
    /// </summary>
    public static class Q_Company
    {
        static string queryText = string.Empty;

        #region :: Scale 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
           SELECT COMPANY_CODE
                , COMPANY_NAME
                , NVL(USEFLAG, '0') AS USEFLAG
                , NVL(U_DTTM, I_DTTM) AS CHANGEDTTM
                , NVL(U_USER, I_USER) AS CHANGEBY
            FROM COMPANY
            WHERE (:COMPANY_NAME IS NULL OR COMPANY_NAME LIKE '%' || :COMPANY_NAME || '%')
            ";

            return queryText;
        }

        public static string ScaleCombo()
        {
            queryText = string.Empty;

            queryText = @" /* ScaleCombo() */
                        SELECT COMPANY_CODE AS CODEVALUE
                            , COMPANY_CODE || ': ' || COMPANY_NAME AS DISPLAYVALUE
                        FROM COMPANY
                        ORDER BY COMPANY_CODE
                        ";

            return queryText;
        }

        #endregion

        #region :: Scale 테이블 Merge 쿼리
        /// <summary>
        /// Scale 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = $@"
            MERGE INTO COMPANY D
            USING (
                SELECT  :COMPANY_CODE   AS COMPANY_CODE
                      , :COMPANY_NAME   AS COMPANY_NAME
                      , :USEFLAG         AS USEFLAG
                      , :CHANGEDTTM         AS CHANGEDTTM
                      , :CHANGEBY         AS CHANGEBY
                FROM DUAL
            ) S
            ON (
                   D.COMPANY_CODE = S.COMPANY_CODE
               )
            WHEN MATCHED THEN
            UPDATE SET
                   D.COMPANY_NAME = S.COMPANY_NAME
                 , D.USEFLAG       = S.USEFLAG
                 , D.U_DTTM       = S.CHANGEDTTM
                 , D.U_USER       = S.CHANGEBY
            WHEN NOT MATCHED THEN
            INSERT (
                   COMPANY_CODE
                 , COMPANY_NAME
                 , USEFLAG
                 , I_DTTM
                 , I_USER
            )
            VALUES (
                   S.COMPANY_CODE   -- 01 회사코드
                 , S.COMPANY_NAME   -- 02 회사명
                 , S.USEFLAG         -- 03 사용여부
                 , S.CHANGEDTTM         -- 04 등록일시
                 , S.CHANGEBY         -- 06 등록자
            )
            ";

            return queryText;
        }
        #endregion

        #region :: Scale 테이블 Delete 쿼리
        /// <summary>
        /// Scale 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"
            DELETE FROM COMPANY WHERE COMPANY_CODE = :COMPANY_CODE
            ";

            return queryText;
        }
        #endregion
    }

    #endregion
}
