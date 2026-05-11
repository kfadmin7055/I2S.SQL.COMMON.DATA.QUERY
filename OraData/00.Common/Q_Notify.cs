namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    /// <summary>
    /// NOTIFY 테이블 쿼리
    /// </summary>
    public static class Q_Notify
    {
        static string queryText = string.Empty;

        #region :: MenuLog 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT A.SEQ
	                         , A.TYPE
	                         , A.SUBJECT
	                         , A.CONTENTS
	                         , A.PRIORITY
	                         , A.EXPIREDATE
	                         , NVL(A.UPDTTM, A.INITDTTM) CHANGEDTTM 
	                         , fnGetUserName(NVL(A.UPBY, A.INITBY)) CHANGEBY 
                        FROM Notify A 
                        WHERE EXPIREDATE > SYSDATE
                        ORDER BY A.""EXPIREDATE"" DESC, A.SEQ DESC
                        ";

            return queryText;
        }

        #endregion

        /// <summary>
        /// NOTIFY 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = $@"MERGE INTO KFMETA.NOTIFY d
                        USING (
                          Select
                            :SEQ         as SEQ,
                            :TYPE        as TYPE,
                            :SUBJECT     as SUBJECT,
                            :CONTENTS    as CONTENTS,
                            :PRIORITY    as PRIORITY,
                            :EXPIREDATE  as EXPIREDATE,
                            :INITDTTM    as INITDTTM,
                            :INITBY      as INITBY,
                            :UPDTTM      as UPDTTM,
                            :UPBY        as UPBY
                          From Dual) s
                        ON
                          (d.SEQ = s.SEQ )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.TYPE = s.TYPE,
                          d.SUBJECT = s.SUBJECT,
                          d.CONTENTS = s.CONTENTS,
                          d.PRIORITY = s.PRIORITY,
                          d.EXPIREDATE = s.EXPIREDATE,
                          d.INITDTTM = s.INITDTTM,
                          d.INITBY = s.INITBY,
                          d.UPDTTM = s.UPDTTM,
                          d.UPBY = s.UPBY
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          SEQ, TYPE, SUBJECT,
                          CONTENTS, PRIORITY, EXPIREDATE,
                          INITDTTM, INITBY, UPDTTM,
                          UPBY)
                        VALUES (
                          s.SEQ, s.TYPE, s.SUBJECT,
                          s.CONTENTS, s.PRIORITY, s.EXPIREDATE,
                          s.INITDTTM, s.INITBY, s.UPDTTM,
                          s.UPBY);
                        ";

            return queryText;
        }

        /// <summary>
        /// NOTIFY 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = $@"DELETE FROM NOTIFY
                        WHERE SEQ = :SEQ
                        ";

            return queryText;
        }
    }
}
