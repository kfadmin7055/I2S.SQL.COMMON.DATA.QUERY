namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    #region :: EBAP.UI.ADM._0.Query.Q_HtmlContentsMaster ::


    /// <summary>
    /// TEMPTABLE 테이블 쿼리
    /// </summary>
    public static class Q_HtmlContentsMaster
    {
        static string queryText = string.Empty;

        #region :: TEMPTABLE 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT A.HTMLID
	                            , A.GBN
	                            , A.CONTENTS
	                            , NVL(A.UPDTTM, A.INITDTTM) CHANGEDTTM 
	                            , NVL(A.UPBY, A.INITBY) CHANGEBY 
                        FROM HtmlContentsMaster A 
                        WHERE 1 = 1 
                            AND A.HTMLID = NVL(:HTMLID, A.HTMLID)
                            AND A.GBN = NVL(:GBN, A.GBN)
                        ";

            return queryText;
        }

        #endregion

        #region :: TEMPTABLE 테이블 Usert 쿼리
        /// <summary>
        /// TEMPTABLE 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"MERGE INTO KFMETA.HTMLCONTENTSMASTER d
                        USING (
                          Select
                            :HTMLID     as HTMLID,
                            :GBN        as GBN,
                            :CONTENTS   as CONTENTS,
                            :INITDTTM   as INITDTTM,
                            :INITBY     as INITBY,
                            :UPDTTM     as UPDTTM,
                            :UPBY       as UPBY
                          From Dual) s
                        ON
                          (d.HTMLID = s.HTMLID )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.GBN = s.GBN,
                          d.CONTENTS = s.CONTENTS,
                          d.INITDTTM = s.INITDTTM,
                          d.INITBY = s.INITBY,
                          d.UPDTTM = s.UPDTTM,
                          d.UPBY = s.UPBY
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          HTMLID, GBN, CONTENTS,
                          INITDTTM, INITBY, UPDTTM,
                          UPBY)
                        VALUES (
                          s.HTMLID, s.GBN, s.CONTENTS,
                          s.INITDTTM, s.INITBY, s.UPDTTM,
                          s.UPBY);
                        ";

            return queryText;
        }
        #endregion

        #region :: TEMPTABLE 테이블 Delete 쿼리
        /// <summary>
        /// TEMPTABLE 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM HtmlContentsMaster
		                WHERE HTMLID = :HTMLID
                            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
