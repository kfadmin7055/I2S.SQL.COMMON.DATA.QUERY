namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    /// <summary>
    /// TEMPTABLE 테이블 쿼리
    /// </summary>
    public static class Q_MenuMaster
    {
        static string queryText = string.Empty;

        #region :: MenuLog 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        WITH MENU AS (
                            SELECT MENUID,
                                    MENUNAME,
                                    PARENTMENUID,
                                    LVL,
                                    IDX
                            FROM MenuMaster
                            WHERE PARENTMENUID IS NOT NULL
                                AND MENUNAME LIKE :MENUNAME || '%'
                            UNION ALL
                            SELECT A.MENUID,
                                    A.MENUNAME,
                                    A.PARENTMENUID,
                                    A.LVL,
                                    A.IDX
                            FROM MenuMaster A
                        )

                        SELECT A.MENUID, 
                                A.MENUNAME_LANG,
                                CASE WHEN MENUNAME_LANG IS NULL THEN A.MENUNAME 
                                    ELSE fnGetLocale(:LANGUAGE, A.MENUNAME_LANG) 
                                END AS MENUNAME,
                                A.LVL, 
                                A.IDX, 
                                A.PARENTMENUID,
                                NVL(A.SCREENID, '') AS SCREENID,
                                NVL(B.SCREENNAME, '') AS SCREENNAME,
                                NVL(B.DLLNAME, '') AS DLLNAME,
                                NVL(B.CLASSNAME, '') AS CLASSNAME, 
                                NVL(B.HELPURL, '') AS HELPURL,
                                A.IMAGEIDX, 
                                A.SELECTIMAGEIDX,
                                A.USEVENDORCODE,
                                A.USEPLANTCODE,
                                A.DESCRIPTION,
                                NVL(A.ISCOMMON, 'Y') AS ISCOMMON, 
                                NVL(A.ISMULTIEXECUTE, 'N') AS ISMULTIEXECUTE, 
                                NVL(A.ISBEGINGROUP, 'N') AS ISBEGINGROUP,                                 
                                NVL(A.USEFLAG, 'N') AS USEFLAG, 
                                NVL(A.UPDTTM, A.INITDTTM) AS CHANGEDTTM,
                                NVL(A.UPBY, A.INITBY) AS CHANGEBY
                        FROM MenuMaster A
                            INNER JOIN (SELECT DISTINCT MENUID FROM MENU) C ON C.MENUID = A.MENUID
                            LEFT JOIN ScreenMaster B ON A.SCREENID = B.SCREENID
                        ORDER BY A.LVL, A.IDX
                            ";

            return queryText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string MenuAuth_Get()
        {
            queryText = string.Empty;

            queryText = @"/* MenuAuth_Get() */
                        SELECT NVL(
                                   RTRIM(
                                       XMLAGG(XMLELEMENT(E, A.MENUID || ',').EXTRACT('//text()') ORDER BY A.MENUID).GETCLOBVAL(),
                                       ','
                                   ),
                                   ''
                               ) AS SHORTCUTMENUS
                        FROM MyMenu A
                            JOIN MenuMaster B ON A.MENUID = B.MENUID
                        WHERE A.USERID = :USERID
                            AND A.GUBUN = 'T'
                            AND (INSTR(B.USEPLANTCODE, :USEPLANTCODE) > 0 OR B.USEPLANTCODE IS NULL OR B.USEPLANTCODE = '')
                        ";

            return queryText;
        }

        #endregion

        /// <summary>
        /// TEMPTABLE 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = $@"
                        MERGE INTO KFMETA.MenuMaster d
                        USING (
                          Select
                            :MENUID              as MENUID,
                            :MENUNAME_LANG       as MENUNAME_LANG,
                            :MENUNAME            as MENUNAME,
                            :LVL                 as LVL,
                            :IDX                 as IDX,
                            :PARENTMENUID        as PARENTMENUID,
                            :SCREENID            as SCREENID,
                            :IMAGEIDX            as IMAGEIDX,
                            :SELECTIMAGEIDX      as SELECTIMAGEIDX,
                            :USEVENDORCODE       as USEVENDORCODE,
                            :USEPLANTCODE        as USEPLANTCODE,
                            :DESCRIPTION         as DESCRIPTION,
                            :ISCOMMON            as ISCOMMON,
                            :ISMULTIEXECUTE      as ISMULTIEXECUTE,
                            :ISBEGINGROUP        as ISBEGINGROUP,
                            :USEFLAG             as USEFLAG,
                            SYSDATE             as CHANGEDTTM,
                            :CHANGEBY            as CHANGEBY
                          From Dual) s
                        ON
                          (d.MENUID = s.MENUID)
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.MENUNAME_LANG = s.MENUNAME_LANG,
                          d.MENUNAME = s.MENUNAME,
                          d.LVL = s.LVL,
                          d.IDX = s.IDX,
                          d.PARENTMENUID = s.PARENTMENUID,
                          d.SCREENID = s.SCREENID,
                          d.IMAGEIDX = s.IMAGEIDX,
                          d.SELECTIMAGEIDX = s.SELECTIMAGEIDX,
                          d.USEVENDORCODE = s.USEVENDORCODE,
                          d.USEPLANTCODE = s.USEPLANTCODE,
                          d.DESCRIPTION = s.DESCRIPTION,
                          d.ISCOMMON = s.ISCOMMON,
                          d.ISMULTIEXECUTE = s.ISMULTIEXECUTE,
                          d.ISBEGINGROUP = s.ISBEGINGROUP,
                          d.USEFLAG = s.USEFLAG,
                          d.UPDTTM = s.CHANGEDTTM,
                          d.UPBY = s.CHANGEBY
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          MENUID, MENUNAME_LANG, MENUNAME,
                          LVL, IDX, PARENTMENUID,
                          SCREENID, IMAGEIDX, SELECTIMAGEIDX,
                          USEVENDORCODE, USEPLANTCODE, DESCRIPTION,
                          ISCOMMON, ISMULTIEXECUTE, ISBEGINGROUP,
                          USEFLAG, INITDTTM, INITBY)
                        VALUES (
                          s.MENUID, s.MENUNAME_LANG, s.MENUNAME,
                          s.LVL, s.IDX, s.PARENTMENUID,
                          s.SCREENID, s.IMAGEIDX, s.SELECTIMAGEIDX,
                          s.USEVENDORCODE, s.USEPLANTCODE, s.DESCRIPTION,
                          s.ISCOMMON, s.ISMULTIEXECUTE, s.ISBEGINGROUP,
                          s.USEFLAG, s.CHANGEDTTM, s.CHANGEBY)
                        ";

            return queryText;
        }

        public static string Update()
        {
            queryText = string.Empty;

            queryText = @"
                        MERGE INTO KFMETA.MenuMaster d
                        USING (
                          Select
                            :MENUID              as MENUID,
                            :USEVENDORCODE       as USEVENDORCODE,
                            :USEPLANTCODE        as USEPLANTCODE,
                            :DESCRIPTION         as DESCRIPTION,
                            :ISCOMMON            as ISCOMMON,
                            :ISMULTIEXECUTE      as ISMULTIEXECUTE,
                            :ISBEGINGROUP        as ISBEGINGROUP
                          From Dual) s
                        ON
                          (d.MENUID = s.MENUID)
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.USEVENDORCODE = s.USEVENDORCODE,
                          d.USEPLANTCODE = s.USEPLANTCODE,
                          d.DESCRIPTION = s.DESCRIPTION,
                          d.ISCOMMON = s.ISCOMMON,
                          d.ISMULTIEXECUTE = s.ISMULTIEXECUTE,
                          d.ISBEGINGROUP = s.ISBEGINGROUP
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          USEVENDORCODE, USEPLANTCODE, DESCRIPTION,
                          ISCOMMON, ISMULTIEXECUTE, ISBEGINGROUP)
                        VALUES (
                          s.USEVENDORCODE, s.USEPLANTCODE, s.DESCRIPTION,
                          s.ISCOMMON, s.ISMULTIEXECUTE, s.ISBEGINGROUP)
                        ";

            return queryText;
        }

        /// <summary>
        /// TEMPTABLE 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = $@"
                        DELETE FROM MenuMaster WHERE MENUID = :MENUID	
		                OR MENUID IN (SELECT MENUID
                                    FROM MenuMaster
                                    WHERE PARENTMENUID IS NOT NULL
                                        AND MENUNAME LIKE :MENUNAME || '%'
                                    UNION ALL
                                    SELECT A.MENUID
                                    FROM MenuMaster A
                                    WHERE MENUID = :MENUID)
                        ";

            return queryText;
        }
    }
}
