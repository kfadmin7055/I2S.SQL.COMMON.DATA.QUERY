namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    #region :: EBAP.Exe.MainUI.Query.Q_LocaleMessageMaster ::


    /// <summary>
    /// LocaleMessageMaster 테이블 쿼리
    /// </summary>
    public static class Q_LocaleMessageMaster
    {
        static string queryText = string.Empty;

        #region :: LocaleMessageMaster 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT MESSAGEID, ENUMCLASS, KOKR AS ""ko-KR"", ENUS AS ""en-US""
                        FROM LocaleMessageMaster 
                        WHERE ISEXPORT = 1
                        ";

            return queryText;
        }

        #endregion

        #region :: LocaleMessageMaster 테이블 Usert 쿼리
        /// <summary>
        /// LocaleMessageMaster 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"MERGE INTO KFMETA.LOCALEMESSAGEMASTER d
                        USING (
                          Select
                            :MESSAGEID       as MESSAGEID,
                            :KOKR            as KOKR,
                            :ENUS            as ENUS,
                            :ZHCHS           as ZHCHS,
                            :ENUMCLASS       as ENUMCLASS,
                            :ISEXPORT        as ISEXPORT,
                            :INITDTTM        as INITDTTM,
                            :INITBY          as INITBY,
                            :UPDTTM          as UPDTTM,
                            :UPBY            as UPBY
                          From Dual) s
                        ON
                          (d.MESSAGEID = s.MESSAGEID )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.KOKR = s.KOKR,
                          d.ENUS = s.ENUS,
                          d.ZHCHS = s.ZHCHS,
                          d.ENUMCLASS = s.ENUMCLASS,
                          d.ISEXPORT = s.ISEXPORT,
                          d.INITDTTM = s.INITDTTM,
                          d.INITBY = s.INITBY,
                          d.UPDTTM = s.UPDTTM,
                          d.UPBY = s.UPBY
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          MESSAGEID, KOKR, ENUS,
                          ZHCHS, ENUMCLASS, ISEXPORT,
                          INITDTTM, INITBY, UPDTTM,
                          UPBY)
                        VALUES (
                          s.MESSAGEID, s.KOKR, s.ENUS,
                          s.ZHCHS, s.ENUMCLASS, s.ISEXPORT,
                          s.INITDTTM, s.INITBY, s.UPDTTM,
                          s.UPBY);
                        ";

            return queryText;
        }
        #endregion

        #region :: LocaleMessageMaster 테이블 Delete 쿼리
        /// <summary>
        /// LocaleMessageMaster 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM LocaleMessageMaster
		                WHERE MessageID = :MessageID
                        ";

            return queryText;
        }
        #endregion
    }

    #endregion
}
