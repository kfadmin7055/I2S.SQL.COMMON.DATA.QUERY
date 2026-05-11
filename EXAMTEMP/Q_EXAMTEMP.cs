namespace I2S.SQL.COMMON.DATA.OraData.EXAMTEMP
{
    #region :: EBAP.Core.OracleQuery.EXAMTEMP.Q_EXAMTEMP ::


    /// <summary>
    /// EXAMTEMP 테이블 쿼리
    /// </summary>
    public static class Q_EXAMTEMP
    {
        static string queryText = string.Empty;

        #region :: EXAMTEMP 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT 
                            E.ID, E.SMALL_NUM, E.LARGE_NUM, 
                            E.FIXED_CHAR, E.VARIABLE_CHAR, E.LONG_STRING, 
                            E.DATE_COL, E.TIMESTAMP_COL, E.TIMESTAMPLTZ_COL, 
                            E.TIMESTAMPTZ_COL, E.RAW_COL, E.BLOB_COL, 
                            E.XML_COL.GetClobVal() as XML_COL, E.JSON_COL, E.USEFLAG
                            , NVL(E.UPDTTM, E.INITDTTM) CHANGEDTTM 
	                        , NVL(E.UPBY, E.INITBY) CHANGEBY
                        FROM KFMETA.EXAMTEMP E
                        WHERE ((:VARIABLE_CHAR IS NULL) OR (E.VARIABLE_CHAR LIKE '%' || :VARIABLE_CHAR || '%'))
                            AND ((:SMALL_NUM IS NULL) OR (E.SMALL_NUM = :SMALL_NUM))
                        ";

            return queryText;
        }

        #endregion

        #region :: EXAMTEMP 테이블 Merge 쿼리
        /// <summary>
        /// EXAMTEMP 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"
                        MERGE INTO KFMETA.EXAMTEMP d
                        USING (
                          Select
                            :ID                 as ID,
                            :SMALL_NUM          as SMALL_NUM,
                            :LARGE_NUM          as LARGE_NUM,
                            :FIXED_CHAR         as FIXED_CHAR,
                            :VARIABLE_CHAR      as VARIABLE_CHAR,
                            :LONG_STRING        as LONG_STRING,
                            :DATE_COL           as DATE_COL,
                            :TIMESTAMP_COL      as TIMESTAMP_COL,
                            :TIMESTAMPLTZ_COL    as TIMESTAMPLTZ_COL,
                            :TIMESTAMPTZ_COL    as TIMESTAMPTZ_COL,
                            :RAW_COL            as RAW_COL,
                            :BLOB_COL           as BLOB_COL,
                            :XML_COL            as XML_COL,
                            :JSON_COL            as JSON_COL,
                            :USEFLAG             as USEFLAG
                          From Dual) s
                        ON
                          (d.ID = s.ID )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.SMALL_NUM           = s.SMALL_NUM,
                          d.LARGE_NUM           = s.LARGE_NUM,
                          d.FIXED_CHAR          = s.FIXED_CHAR,
                          d.VARIABLE_CHAR       = s.VARIABLE_CHAR,
                          d.LONG_STRING         = s.LONG_STRING,
                          d.DATE_COL            = s.DATE_COL,
                          d.TIMESTAMP_COL       = s.TIMESTAMP_COL,
                          d.TIMESTAMPLTZ_COL    = s.TIMESTAMPLTZ_COL,
                          d.TIMESTAMPTZ_COL     = s.TIMESTAMPTZ_COL,
                          d.RAW_COL             = s.RAW_COL,
                          d.BLOB_COL            = s.BLOB_COL,
                          d.XML_COL             = s.XML_COL,
                          d.JSON_COL            = s.JSON_COL,
                          d.USEFLAG             = s.USEFLAG
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          ID, SMALL_NUM, LARGE_NUM,
                          FIXED_CHAR, VARIABLE_CHAR, LONG_STRING,
                          DATE_COL, TIMESTAMP_COL, TIMESTAMPLTZ_COL,
                          TIMESTAMPTZ_COL, RAW_COL, BLOB_COL,
                          XML_COL, JSON_COL)
                        VALUES (
                          s.ID, s.SMALL_NUM, s.LARGE_NUM,
                          s.FIXED_CHAR, s.VARIABLE_CHAR, s.LONG_STRING,
                          s.DATE_COL, s.TIMESTAMP_COL, s.TIMESTAMPLTZ_COL,
                          s.TIMESTAMPTZ_COL, s.RAW_COL, s.BLOB_COL,
                          s.XML_COL, s.JSON_COL)
                        ";

            return queryText;
        }
        #endregion

        #region :: EXAMTEMP 테이블 Delete 쿼리
        /// <summary>
        /// EXAMTEMP 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"
                        DELETE FROM EXAMTEMP
                        WHERE ID = :ID
                            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
