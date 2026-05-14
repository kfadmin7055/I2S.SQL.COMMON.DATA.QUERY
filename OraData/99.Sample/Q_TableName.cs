namespace I2S.SQL.COMMON.DATA.OraData.Sample
{
    #region :: I2S.SQL.COMMON.DATA.OraData.Sample.Q_TableName ::


    /// <summary>
    /// TEMPTABLE 테이블 쿼리
    /// </summary>
    public static class Q_TableName
    {
        static string queryText = string.Empty;

        #region :: TEMPTABLE 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
            SELECT E.ID
                , E.SMALL_NUM
                , E.LARGE_NUM
                , E.FIXED_CHAR
                , E.VARIABLE_CHAR
                , E.COMBO_TEST
                , DBMS_LOB.SUBSTR(E.LONG_STRING, 4000, 1) AS LONG_STRING
                , E.DATE_COL
                , E.TIMESTAMP_COL
                , E.TIMESTAMPLTZ_COL
                , E.TIMESTAMPTZ_COL
                , E.RAW_COL
                , E.BLOB_COL
                , E.XML_COL
                , E.JSON_COL
                , E.USEFLAG
                , E.INITDTTM
                , E.INITBY
                , E.UPDTTM
                , E.UPBY
            FROM EXAMTEMP E
            ";

            return queryText;
        }

        #endregion

        #region :: TEMPTABLE 테이블 Merge 쿼리
        /// <summary>
        /// TEMPTABLE 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
            MERGE INTO EXAMTEMP d
            USING
            (
                SELECT        :ID AS ID
                        ,       :SMALL_NUM AS SMALL_NUM
                        ,       :LARGE_NUM AS LARGE_NUM
                        ,       :FIXED_CHAR AS FIXED_CHAR
                        ,       :VARIABLE_CHAR AS VARIABLE_CHAR
                        ,       :COMBO_TEST AS COMBO_TEST
                        ,       :LONG_STRING AS LONG_STRING
                        ,       :DATE_COL AS DATE_COL
                        ,       :TIMESTAMP_COL AS TIMESTAMP_COL
                        ,       :TIMESTAMPLTZ_COL AS TIMESTAMPLTZ_COL
                        ,       :TIMESTAMPTZ_COL AS TIMESTAMPTZ_COL
                        ,       :RAW_COL AS RAW_COL
                        ,       :BLOB_COL AS BLOB_COL
                        ,       :XML_COL AS XML_COL
                        ,       :JSON_COL AS JSON_COL
                        ,       :USEFLAG AS USEFLAG
                        ,       SYSDATE AS CHANGEDTTM
                        ,       :CHANGEBY AS CHANGEBY
                    FROM DUAL
            ) s
            ON
            (
                    d.ID = s.ID
            )
            WHEN MATCHED
            THEN
                UPDATE
                    SET d.SMALL_NUM = s.SMALL_NUM
                        , d.LARGE_NUM = s.LARGE_NUM
                        , d.FIXED_CHAR = s.FIXED_CHAR
                        , d.VARIABLE_CHAR = s.VARIABLE_CHAR
                        , d.COMBO_TEST = s.COMBO_TEST
                        , d.LONG_STRING = s.LONG_STRING
                        , d.DATE_COL = s.DATE_COL
                        , d.TIMESTAMP_COL = s.TIMESTAMP_COL
                        , d.TIMESTAMPLTZ_COL = s.TIMESTAMPLTZ_COL
                        , d.TIMESTAMPTZ_COL = s.TIMESTAMPTZ_COL
                        , d.RAW_COL = s.RAW_COL
                        , d.BLOB_COL = s.BLOB_COL
                        , d.XML_COL = s.XML_COL
                        , d.JSON_COL = s.JSON_COL
                        , d.USEFLAG = s.USEFLAG
                        , d.UPDTTM = s.CHANGEDTTM
                        , d.UPBY = s.CHANGEBY
            WHEN NOT MATCHED
            THEN
                INSERT
                (
                        ID
                    , SMALL_NUM
                    , LARGE_NUM
                    , FIXED_CHAR
                    , VARIABLE_CHAR
                    , COMBO_TEST
                    , LONG_STRING
                    , DATE_COL
                    , TIMESTAMP_COL
                    , TIMESTAMPLTZ_COL
                    , TIMESTAMPTZ_COL
                    , RAW_COL
                    , BLOB_COL
                    , XML_COL
                    , JSON_COL
                    , USEFLAG
                    , INITDTTM
                    , INITBY
                    , UPDTTM
                    , UPBY
                )
                VALUES
                (
                        s.ID
                    , s.SMALL_NUM
                    , s.LARGE_NUM
                    , s.FIXED_CHAR
                    , s.VARIABLE_CHAR
                    , s.COMBO_TEST
                    , s.LONG_STRING
                    , s.DATE_COL
                    , s.TIMESTAMP_COL
                    , s.TIMESTAMPLTZ_COL
                    , s.TIMESTAMPTZ_COL
                    , s.RAW_COL
                    , s.BLOB_COL
                    , s.XML_COL
                    , s.JSON_COL
                    , s.USEFLAG
                    , s.CHANGEDTTM
                    , s.CHANGEBY
                    , s.CHANGEDTTM
                    , s.CHANGEBY
                )
            ";

            return queryText;
        }
        #endregion

        #region :: TEMPTABLE 테이블 Delete 쿼리
        /// <summary>
        /// TEMPTABLE 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete(string reference)
        {
            queryText = string.Empty;

            queryText = @"
            DELETE FROM EXAMTEMP
            WHERE 1 = 1
                AND ID = :ID
            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
