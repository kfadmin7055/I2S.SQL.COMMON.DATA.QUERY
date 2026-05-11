namespace I2S.SQL.COMMON.DATA.OraData.ProductInfo
{
    #region :: I2S.SQL.COMMON.DATA.OraData.ProductInfo.Q_Scale ::


    /// <summary>
    /// Scale 테이블 쿼리
    /// </summary>
    public static class Q_Scale
    {
        static string queryText = string.Empty;

        #region :: Scale 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
           SELECT PLANT_CODE
                , PROCESSCODE
                , LINECODE
                , SCALE_CODE
                , SCALE_NAME
                , SCALE_TYPE
                , MAX_CAPA
                , MIN_CAPA
                , TOLERANCE
                , UNIT
                , PLCID
                , PLCADDRESS
                , SORT_SEQ
                , I_DTTM
                , U_DTTM
                , I_USER
                , U_USER
                , USE_YN
            FROM SCALE
            ";

            return queryText;
        }

        public static string ScaleCombo()
        {
            queryText = string.Empty;

            queryText = @" /* ScaleCombo() */
                        SELECT SCALE_CODE AS CODEVALUE
                            , SCALE_CODE || ': ' || SCALE_NAME AS DISPLAYVALUE
                        FROM SCALE
                        ORDER BY SCALE_CODE
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

            queryText = @"
            MERGE INTO SCALE d
            USING (
                SELECT
                      :PLANT_CODE     AS PLANT_CODE
                    , :SCALE_CODE     AS SCALE_CODE
                    , :SCALE_NAME     AS SCALE_NAME
                    , :SCALE_TYPE     AS SCALE_TYPE
                    , :MAX_CAPA       AS MAX_CAPA
                    , :MIN_CAPA       AS MIN_CAPA
                    , :TOLERANCE      AS TOLERANCE
                    , :UNIT           AS UNIT
                    , :PLCID          AS PLCID
                    , :PLCADDRESS     AS PLCADDRESS
                    , :SORT_SEQ       AS SORT_SEQ
                    , :USE_YN         AS USE_YN
                    , SYSDATE         AS CHANGEDTTM
                    , :USER_ID        AS CHANGEBY
                FROM DUAL
            ) s
            ON (
                   d.PLANT_CODE = s.PLANT_CODE
               AND d.SCALE_CODE = s.SCALE_CODE
            )

            WHEN MATCHED THEN
            UPDATE SET
                  d.SCALE_NAME  = s.SCALE_NAME
                , d.SCALE_TYPE  = s.SCALE_TYPE
                , d.MAX_CAPA    = s.MAX_CAPA
                , d.MIN_CAPA    = s.MIN_CAPA
                , d.TOLERANCE   = s.TOLERANCE
                , d.UNIT        = s.UNIT
                , d.PLCID       = s.PLCID
                , d.PLCADDRESS  = s.PLCADDRESS
                , d.SORT_SEQ    = s.SORT_SEQ
                , d.USE_YN      = s.USE_YN
                , d.U_DTTM      = s.CHANGEDTTM
                , d.U_USER      = s.CHANGEBY

            WHEN NOT MATCHED THEN
            INSERT (
                  PLANT_CODE
                , SCALE_CODE
                , SCALE_NAME
                , SCALE_TYPE
                , MAX_CAPA
                , MIN_CAPA
                , TOLERANCE
                , UNIT
                , PLCID
                , PLCADDRESS
                , SORT_SEQ
                , USE_YN
                , I_DTTM
                , I_USER
                , U_DTTM
                , U_USER
            )
            VALUES (
                  s.PLANT_CODE
                , s.SCALE_CODE
                , s.SCALE_NAME
                , s.SCALE_TYPE
                , s.MAX_CAPA
                , s.MIN_CAPA
                , s.TOLERANCE
                , s.UNIT
                , s.PLCID
                , s.PLCADDRESS
                , s.SORT_SEQ
                , s.USE_YN
                , s.CHANGEDTTM
                , s.CHANGEBY
                , s.CHANGEDTTM
                , s.CHANGEBY
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
            DELETE FROM SCALE
            ";

            return queryText;
        }
        #endregion
    }

    #endregion
}
