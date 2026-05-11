namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    #region :: EBAP.Exe.MainUI.Query.Q_LocaleMaster ::


    /// <summary>
    /// LocaleMaster 테이블 쿼리
    /// </summary>
    public static class Q_LocaleMaster
    {
        static string queryText = string.Empty;


        #region :: LocaleMaster 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT  STRINGID, 
		                        KOKR AS ""ko-KR"", 
                               ENUS AS ""en-US"",
		                        ZHCHS, 
		                        ENUMCLASS, 
		                        ISEXPORT, 
		                        NVL(UPDTTM, INITDTTM) CHANGEDTTM,
	                            fnGetUserName(NVL(UPBY, INITBY)) CHANGEBY 
                        FROM LocaleMaster
                        WHERE KOKR LIKE '%' || :KOKR || '%'
                        ";

            return queryText;
        }

        /// <summary>
        /// LocaleMaster 테이블 조회 쿼리
        /// 특정 조인 쿼리는 Select_[화면명] 등으로 새로 만든다.
        /// </summary>
        /// <returns></returns>
        public static string LanguageMaster()
        {
            queryText = string.Empty;

            queryText = @"/* LanguageMaster() */
                        SELECT STRINGID, 
                               ENUMCLASS, 
                               KOKR AS ""ko-KR"", 
                               ENUS AS ""en-US""
                        FROM LocaleMaster
                        WHERE ISEXPORT = 1
                        ";

            return queryText;
        }

        /// <summary>
        /// LocaleMaster 테이블 조회 쿼리
        /// 특정 조인 쿼리는 Select_[화면명] 등으로 새로 만든다.
        /// </summary>
        /// <returns></returns>
        public static string PopLanguageMaster_Select()
        {
            queryText = string.Empty;

            queryText = @"SELECT  STRINGID, 
		                        KOKR AS ""ko-KR"", 
                               ENUS AS ""en-US"",
		                        ZHCHS, 
		                        ENUMCLASS
                        FROM LocaleMaster 
                        WHERE (KOKR LIKE '%' ||  :CAPTION || '%' OR ENUS LIKE '%' ||  :CAPTION || '%')
                        AND ENUMCLASS LIKE :ID || '%'";

            return queryText;
        }

        #endregion

        #region :: LocaleMaster 테이블 Usert 쿼리
        /// <summary>
        /// LocaleMaster 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"MERGE INTO KFMETA.LOCALEMASTER d
                        USING (
                          Select
                            :STRINGID        as STRINGID,
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
                          (d.STRINGID = s.STRINGID )
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
                          STRINGID, KOKR, ENUS,
                          ZHCHS, ENUMCLASS, ISEXPORT,
                          INITDTTM, INITBY, UPDTTM,
                          UPBY)
                        VALUES (
                          s.STRINGID, s.KOKR, s.ENUS,
                          s.ZHCHS, s.ENUMCLASS, s.ISEXPORT,
                          s.INITDTTM, s.INITBY, s.UPDTTM,
                          s.UPBY);
                        ";

            return queryText;
        }
        #endregion

        #region :: LocaleMaster 테이블 Delete 쿼리
        /// <summary>
        /// LocaleMaster 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM LocaleMaster
		                WHERE STRINGID = :STRINGID
                            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
