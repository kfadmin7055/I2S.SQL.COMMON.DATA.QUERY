namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    /// <summary>
    /// ScreenMaster 테이블 쿼리
    /// </summary>
    public static class Q_ScreenMaster
    {
        static string queryText = string.Empty;

        #region :: LocaleMaster 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT SCREENID, 
		                        SCREENNAME, 
		                        DLLNAME, 
		                        CLASSNAME, 
		                        HELPURL,
		                        DESCRIPTION, 
		                        NVL(UPDTTM, INITDTTM) CHANGEDTTM, 
		                        NVL(UPBY, INITBY) CHANGEBY 
                        FROM ScreenMaster
                        WHERE 1 = 1
                            AND SCREENID LIKE :SCREENID || '%'
                            AND SCREENNAME LIKE :SCREENNAME || '%'
                            AND DLLNAME LIKE '%' ||:DLLNAME || '%'
                            AND CLASSNAME LIKE '%' ||:CLASSNAME || '%'
                            AND ('' IS NULL OR DESCRIPTION LIKE '%' || :DESCRIPTION || '%')
                        ";

            return queryText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ScreenColumnSetMenu_Select()
        {
            queryText = string.Empty;

            queryText = @"/* ScreenColumnSetMenu_Select() */
                        SELECT DISTINCT A.MENUID,
                                         A.MENUNAME,
                                         A.LVL,
                                         A.IDX,
                                         A.PARENTMENUID,
                                         B.SCREENID,
                                         B.SCREENNAME,
                                         B.CLASSNAME,
                                         CASE WHEN C.SCREENID IS NULL THEN 0 ELSE 1 END AS USESET
                        FROM MenuMaster A
                            LEFT JOIN ScreenMaster B ON A.SCREENID = B.SCREENID
                            LEFT JOIN ScreenColumnSet C ON A.SCREENID = C.SCREENID
                        WHERE 1 = 1
                            AND B.SCREENID LIKE :SCREENID || '%'
                            AND B.SCREENNAME LIKE :SCREENNAME || '%'
                        ORDER BY A.LVL, A.IDX
                        ";

            return queryText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string Sel_MaxId()
        {
            queryText = string.Empty;

            queryText = @"/* Sel_MaxId() */
                        SELECT TO_NUMBER(SUBSTR(NVL(MAX(SCREENID), '0'), -5)) + 1 AS MAXID FROM ScreenMaster
                        ";

            return queryText;
        }

        #endregion

        public static string Merge()
        {
            queryText = string.Empty;

            queryText = $@"MERGE INTO KFMETA.SCREENMASTER D
                        USING (
                          SELECT
                            :SCREENID           AS SCREENID,
                            :SCREENNAME         AS SCREENNAME,
                            :DLLNAME            AS DLLNAME,
                            :CLASSNAME          AS CLASSNAME,
                            :HELPURL            AS HELPURL,
                            :DESCRIPTION        AS DESCRIPTION,
                            SYSDATE           AS INITDTTM,
                            :CHANGEBY             AS INITBY,
                            SYSDATE             AS UPDTTM,
                            :CHANGEBY               AS UPBY
                          FROM Dual) s
                        ON
                          (d.SCREENID = s.SCREENID )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.SCREENNAME = s.SCREENNAME,
                          d.DLLNAME = s.DLLNAME,
                          d.CLASSNAME = s.CLASSNAME,
                          d.HELPURL = s.HELPURL,
                          d.DESCRIPTION = s.DESCRIPTION,
                          d.INITDTTM = s.INITDTTM,
                          d.INITBY = s.INITBY,
                          d.UPDTTM = s.UPDTTM,
                          d.UPBY = s.UPBY
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          SCREENID, SCREENNAME, DLLNAME,
                          CLASSNAME, HELPURL, DESCRIPTION,
                          INITDTTM, INITBY, UPDTTM,
                          UPBY)
                        VALUES (
                          s.SCREENID, s.SCREENNAME, s.DLLNAME,
                          s.CLASSNAME, s.HELPURL, s.DESCRIPTION,
                          s.INITDTTM, s.INITBY, s.UPDTTM,
                          s.UPBY)
                        ";

            return queryText;
        }

        /// <summary>
        /// ScreenMaster 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = $@"
                            DELETE FROM ScreenMaster
                            WHERE SCREENID = :SCREENID
                            ";

            return queryText;
        }
    }
}
