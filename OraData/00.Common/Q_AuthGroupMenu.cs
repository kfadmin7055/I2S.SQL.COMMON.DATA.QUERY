using System.Data;

namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    /// <summary>
    /// TEMPTABLE 테이블 쿼리
    /// </summary>
    public static class Q_AuthGroupMenu
    {
        static string queryText = string.Empty;
        static string USERID = string.Empty;

        #region :: MenuLog 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            DataSet ds = new DataSet();
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT DISTINCT 
                            1 AS FAVORITES,
                            TO_CHAR(A.MENUID) AS MENUID,
                            TO_CHAR(B.MENUNAME) AS MENUNAME,
                            B.PARENTMENUID,
                            C.DLLNAME,
                            C.CLASSNAME,
                            C.HELPURL,
                            B.IMAGEIDX,
                            B.SELECTIMAGEIDX,
                            B.LVL,
                            B.IDX,
                            TO_CHAR(B.ISCOMMON) AS ISCOMMON,
                            TO_CHAR(B.ISMULTIEXECUTE) AS ISMULTIEXECUTE,
                            TO_CHAR(B.ISBEGINGROUP) AS ISBEGINGROUP,
                            B.SCREENID
                        FROM AuthGroupMenu A
                        JOIN MenuMaster B ON A.MENUID = B.MENUID AND B.USEFLAG = 'Y'
                        LEFT JOIN ScreenMaster C ON B.SCREENID = C.SCREENID
                        WHERE A.GROUPCODE IN (
                            SELECT GROUPCODE 
                            FROM AuthGroupUser 
                            WHERE USERID = :USERID AND EXPIREDATE > SYSDATE
                        )
                        AND (INSTR(B.USEVENDORCODE, :USEVENDORCODE) > 0 OR B.USEVENDORCODE IS NULL)
                        AND (INSTR(B.USEPLANTCODE, :USEPLANTCODE) > 0 OR B.USEPLANTCODE IS NULL)

                        UNION ALL

                        SELECT 
                            0 AS FAVORITES, 
                            TO_CHAR('FAVORITES') AS MENUID, 
                            TO_CHAR('즐겨찾기 메뉴') AS MENUNAME, 
                            NULL AS PARENTMENUID, 
                            NULL AS DLLNAME, 
                            NULL AS CLASSNAME, 
                            NULL AS HELPURL, 
                            4 AS IMAGEIDX, 
                            4 AS SELECTIMAGEIDX, 
                            0 AS LVL, 
                            0 AS IDX,
                            'N' AS ISCOMMON, 
                            'N' AS ISMULTIEXECUTE, 
                            'N' AS ISBEGINGROUP, 
                            NULL AS SCREENID
                        FROM DUAL

                        UNION ALL
                        
                        SELECT DISTINCT 
                            0 AS FAVORITES, 
                            TO_CHAR(A.MENUID || '_FA') AS MENUID, 
                            TO_CHAR(B.MENUNAME), 
                            'FAVORITES' AS PARENTMENUID, 
                            D.DLLNAME, 
                            D.CLASSNAME,
                            D.HELPURL, 
                            4 AS IMAGEIDX, 
                            4 AS SELECTIMAGEIDX, 
                            1 AS LVL, 
                            1 AS IDX,
                            TO_CHAR(B.ISCOMMON) AS ISCOMMON,
                            TO_CHAR(B.ISMULTIEXECUTE) AS ISMULTIEXECUTE,
                            TO_CHAR(B.ISBEGINGROUP) AS ISBEGINGROUP,
                            B.SCREENID
                        FROM MyMenu A
                        JOIN MenuMaster B ON A.GUBUN = 'F' AND A.MENUID = B.MENUID AND B.USEFLAG = 'Y'
                        LEFT JOIN ScreenMaster D ON B.SCREENID = D.SCREENID
                        JOIN AuthGroupMenu C ON B.MENUID = C.MENUID
                        WHERE C.GROUPCODE IN (
                            SELECT GROUPCODE 
                            FROM AuthGroupUser 
                            WHERE EXPIREDATE > SYSDATE
                                AND EXISTS (SELECT 1 FROM MyMenu A WHERE A.USERID = :USERID AND A.GUBUN = 'F')
                        )
                        AND A.USERID = :USERID
                        AND (INSTR(B.USEVENDORCODE, :USEVENDORCODE) > 0 OR B.USEVENDORCODE IS NULL)
                        AND (INSTR(B.USEPLANTCODE, :USEPLANTCODE) > 0 OR B.USEPLANTCODE IS NULL)
                        ORDER BY FAVORITES, LVL, IDX
                        ";


            return queryText;
        }

        public static string AuthGroupMenu_Select()
        {
            queryText = string.Empty;

            queryText = $@"/* AuthGroupMenu_Select() */
                        SELECT 
                               CASE WHEN B.AUTHORITY IS NULL THEN 'N' ELSE 'Y' END AS ISAUTH,
                               :GROUPCODE AS GROUPCODE,
                               A.MENUID,
                               A.MENUNAME,
                               A.PARENTMENUID,
                               CASE WHEN NVL(SUBSTR(B.AUTHORITY, 1, 1), '0') = '1' THEN 'Y' ELSE 'N' END AS SELECTAUTH,
                               CASE WHEN NVL(SUBSTR(B.AUTHORITY, 3, 1), '0') = '1' THEN 'Y' ELSE 'N' END AS NEWAUTH,
                               CASE WHEN NVL(SUBSTR(B.AUTHORITY, 5, 1), '0') = '1' THEN 'Y' ELSE 'N' END AS SAVEAUTH,
                               CASE WHEN NVL(SUBSTR(B.AUTHORITY, 7, 1), '0') = '1' THEN 'Y' ELSE 'N' END AS DELAUTH,
                               NVL(B.UPDTTM, B.INITDTTM) AS CHANGEDTTM,
                               NVL(B.UPBY, B.INITBY) AS CHANGEBY
                        FROM MenuMaster A
                        LEFT JOIN (
                            SELECT GROUPCODE, MENUID, AUTHORITY, INITDTTM, UPDTTM, INITBY, UPBY  
                            FROM AuthGroupMenu
                            WHERE GROUPCODE = :GROUPCODE
                        ) B
                        ON A.MENUID = B.MENUID
                        ORDER BY A.LVL, A.IDX
                            ";

            //string[] likeColumn = { "ColumnName", "ColumnName" };

            //queryText = SelectQuery(string reference).GetWhereText(dbParams, null);

            //queryText += @"
            //              ORDER BY
            //            ";

            return queryText;
        }

        #endregion

        /// <summary>
        /// TEMPTABLE 테이블 조회 쿼리
        /// 특정 조인 쿼리는 Select_[화면명] 등으로 새로 만든다.
        /// </summary>
        /// <returns></returns>
        public static string CheckUserMenuAuth()
        {
            queryText = string.Empty;

            queryText = @"/* CheckUserMenuAuth() */
                        SELECT A.MENUID
	                             , MAX(cast(SUBSTR(A.AUTHORITY, 1, 1) as number(10))) AS SELECTAUTH
	                             , MAX(cast(SUBSTR(A.AUTHORITY, 3, 1) as number(10))) AS NEWAUTH
	                             , MAX(cast(SUBSTR(A.AUTHORITY, 5, 1) as number(10))) AS SAVEAUTH
	                             , MAX(cast(SUBSTR(A.AUTHORITY, 7, 1) as number(10))) AS DELAUTH
                            FROM AuthGroupMenu A 
                            WHERE A.GROUPCODE IN (SELECT GROUPCODE 
                                                    FROM AuthGroupUser 
                                                    WHERE USERID = :USERID  
                                                    AND EXPIREDATE > SYSDATE ) 
                            AND A.MENUID = :MENUID
                            GROUP BY A.MENUID
                            ";

            return queryText;
        }

        /// <summary>
        /// TEMPTABLE 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = $@"MERGE INTO KFMETA.AUTHGROUPMENU d
                        USING (
                          Select
                            :GROUPCODE       as GROUPCODE,
                            :MENUID          as MENUID,
                            CASE 
                                       WHEN :ISAUTH = 'Y' THEN 
                                           CASE WHEN :SELECTAUTH = 'Y'  THEN '1' ELSE '0' END || '/' ||
                                           CASE WHEN :NEWAUTH = 'Y'     THEN '1' ELSE '0' END || '/' ||
                                           CASE WHEN :SAVEAUTH = 'Y'    THEN '1' ELSE '0' END || '/' ||
                                           CASE WHEN :DELAUTH = 'Y'     THEN '1' ELSE '0' END
                                       ELSE NULL
                            END AS AUTHORITY,
                            SYSTIMESTAMP     as CHANGEDTTM,
                            :CHANGEBY        as CHANGEBY
                          From Dual) s
                        ON (
                            d.GROUPCODE = s.GROUPCODE AND d.MENUID = s.MENUID )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.AUTHORITY = s.AUTHORITY,
                          d.UPDTTM = s.CHANGEDTTM,
                          d.UPBY = s.CHANGEBY
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          GROUPCODE, MENUID, AUTHORITY,
                          INITDTTM, INITBY)
                        VALUES (
                          s.GROUPCODE, s.MENUID, s.AUTHORITY,
                          s.CHANGEDTTM, s.CHANGEBY)
                        ";

            return queryText;
        }

        /// <summary>
        /// 그룹별 메뉴 권한 관리
        /// </summary>
        /// <returns></returns>
        public static string AuthGroupMenu_Save()
        {
            queryText = string.Empty;

            queryText = $@"MERGE INTO AuthGroupMenu a
                        USING (
                            SELECT :GROUPCODE AS GROUPCODE,
                               :MENUID AS MENUID,
                               CASE 
                                   WHEN :ISAUTH = 1 THEN 
                                       CASE WHEN :SELECTAUTH = 1 THEN '1' ELSE '0' END || '/' ||
                                       CASE WHEN :NEWAUTH = 1 THEN '1' ELSE '0' END || '/' ||
                                       CASE WHEN :SAVEAUTH = 1 THEN '1' ELSE '0' END || '/' ||
                                       CASE WHEN :DELAUTH = 1 THEN '1' ELSE '0' END
                                   ELSE NULL
                               END AS AUTHORITY,
                               :CHANGEBY AS CHANGEBY
                        FROM DUAL
                        ) b
                        ON (a.groupcode = b.groupcode AND a.menuid = b.menuid)
                        WHEN MATCHED THEN
                            UPDATE SET 
                                a.authority = b.authority,
                                a.updttm = SYSDATE,
                                a.upby = b.changeby
                        WHEN NOT MATCHED THEN
                            INSERT (groupcode, menuid, authority, initdttm, initby)
                            VALUES (b.groupcode, b.menuid, b.authority, SYSDATE, b.changeby)
                        ";

            return queryText;
        }

        /// <summary>
        /// TEMPTABLE 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string AdminInsert()
        {
            queryText = string.Empty;

            queryText = $@"MERGE INTO AuthGroupMenu T
                        USING (
                            SELECT GROUPCODE, MENUID, '1/1/1/1' AS AUTHORITY, SYSDATE AS INITDTTM, :CHANGEBY AS INITBY
                            FROM AuthGroupMenu
                            WHERE MENUID = :MENUID AND GROUPCODE = 'ADMIN'
                        ) S
                        ON (T.MENUID = S.MENUID AND T.GROUPCODE = S.GROUPCODE)
                        WHEN NOT MATCHED THEN
                        INSERT (GROUPCODE, MENUID, AUTHORITY, INITDTTM, INITBY)
                        VALUES (S.GROUPCODE, S.MENUID, S.AUTHORITY, S.INITDTTM, S.INITBY)
                            ";

            return queryText;
        }

        /// <summary>
        /// TEMPTABLE 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string ManagerInsert()
        {
            queryText = string.Empty;

            queryText = $@"MERGE INTO AuthGroupMenu T
                        USING (
                            SELECT GROUPCODE, MENUID, '1/0/0/0' AS AUTHORITY, SYSDATE AS INITDTTM, :CHANGEBY AS INITBY
                            FROM AuthGroupMenu
                            WHERE MENUID = :MENUID AND GROUPCODE = 'Z00000001'
                        ) S
                        ON (T.MENUID = S.MENUID AND T.GROUPCODE = S.GROUPCODE)
                        WHEN NOT MATCHED THEN
                        INSERT (GROUPCODE, MENUID, AUTHORITY, INITDTTM, INITBY)
                        VALUES (S.GROUPCODE, S.MENUID, S.AUTHORITY, S.INITDTTM, S.INITBY)
                            ";

            return queryText;
        }

        /// <summary>
        /// TEMPTABLE 테이블 삭제 쿼리
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = $@"DELETE FROM AuthGroupMenu
		                WHERE MENUID NOT IN (SELECT MENUID FROM MenuMaster)
                            ";

            return queryText;
        }
    }
}
