namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    #region :: EBAP.UI.ADM._0.Query.Q_MenuLog ::


    /// <summary>
    /// MenuLog 테이블 쿼리
    /// </summary>
    public static class Q_MenuLog
    {
        static string queryText = string.Empty;

        #region :: MenuLog 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        WITH MenuLog AS (
                            SELECT A.SEQ,
                                   A.VENDORCODE,
                                   A.PLANTCODE,
                                   A.MENUID,
                                   A.EXETIME,
                                   NVL((SELECT MIN(EXETIME)
                                        FROM MenuExecuteLog@KFLOG 
                                        WHERE MENUID = A.MENUID 
                                          AND SEQ > A.SEQ 
                                          AND SIGNINSEQ = A.SIGNINSEQ), SYSTIMESTAMP) AS ENDTIME,
                                   A.SIGNINSEQ
                            FROM MenuExecuteLog@KFLOG A
                            WHERE SIGNINSEQ = :SIGNINSEQ
                        )

                        SELECT A.SIGNINSEQ,
                               A.VENDORCODE,
                               A.PLANTCODE,
                               A.MENUID,
                               B.MENUNAME,
                               D.POPNAME,
                               A.EXETIME,
                               D.OPERATION,
                               CASE 
                                   WHEN D.OPERATION IN ('SAVE', 'DELETE', 'UPLOAD', 'INSPECT', 'INSPECTDELETE', 'DEFECT') THEN 1 
                                   ELSE 0 
                               END AS OPERATIONTYPE,
                               ROUND(
                                   EXTRACT(SECOND FROM (CAST(NVL(D.ENDDTTM, SYSTIMESTAMP) AS TIMESTAMP) - CAST(NVL(D.STARTDTTM, A.EXETIME) AS TIMESTAMP))) 
                                   + EXTRACT(MINUTE FROM (CAST(NVL(D.ENDDTTM, SYSTIMESTAMP) AS TIMESTAMP) - CAST(NVL(D.STARTDTTM, A.EXETIME) AS TIMESTAMP))) * 60
                                   + EXTRACT(HOUR FROM (CAST(NVL(D.ENDDTTM, SYSTIMESTAMP) AS TIMESTAMP) - CAST(NVL(D.STARTDTTM, A.EXETIME) AS TIMESTAMP))) * 3600, 
                                   3
                               ) AS NUMTODSINTERVAL,
                               NVL(D.STARTDTTM, A.EXETIME) AS STARTDTTM,
                               NVL(D.ENDDTTM, SYSTIMESTAMP) AS ENDDTTM,
                               DENSE_RANK() OVER (ORDER BY NVL(D.STARTDTTM, A.EXETIME)) AS USESEQ
                        FROM MenuLog A
                        LEFT JOIN MenuMaster B ON A.MENUID = B.MENUID
                        LEFT JOIN OperationLog@KFLOG D ON A.SIGNINSEQ = D.SIGNINSEQ
                           AND A.MENUID = D.MENUID AND D.STARTDTTM >= A.EXETIME AND D.ENDDTTM <= A.ENDTIME
                        WHERE D.OPERATION IS NOT NULL
                            AND D.OPERATION IN (:OPERATION)
                        ORDER BY A.EXETIME DESC, D.STARTDTTM DESC
                        ";


            return queryText;
        }

        public static string MenuOprationLog_Select()
        {
            queryText = @"/* MenuOprationLog_Select() */ 
                        WITH MenuLog AS (
                            SELECT A.SEQ,
                                A.VENDORCODE,
                                A.PLANTCODE,
                                A.MENUID,
                                A.EXETIME,
                                NVL((SELECT MIN(EXETIME) 
                                     FROM MenuExecuteLog 
                                     WHERE MENUID = A.MENUID 
                                       AND SEQ > A.SEQ 
                                       AND SIGNINSEQ = A.SIGNINSEQ), SYSDATE) AS ENDTIME,
                                A.SIGNINSEQ
                            FROM MenuExecuteLog A
                            WHERE A.EXETIME BETWEEN TO_CHAR(TO_DATE(:STARTTIME, 'YYYY-MM-DD ""오전"" HH:MI:SS AM'), 'YYYY-MM-DD HH24:MI:SS')
                                              AND TO_CHAR(TO_DATE(:ENDTIME, 'YYYY-MM-DD ""오전"" HH:MI:SS AM'), 'YYYY-MM-DD HH24:MI:SS')
                        )
                        , LASTUPDATE AS (
                            SELECT MAX(CHANGEDTTM) AS UPDATEDTTM 
                            FROM KFDeployHistory@KFFILE
                        )

                        SELECT 
                            A.SIGNINSEQ,
                            A.VENDORCODE,
                            A.PLANTCODE,
                            E.USERID,
                            F.USERNAME,
                            F.DEPTNAME,
                            A.MENUID,
                            B.MENUNAME,
                            D.POPNAME,
                            D.OPERATION,
                            CASE 
                                WHEN D.OPERATION IN ('SAVE', 'DELETE', 'UPLOAD', 'INSPECT', 'INSPECTDELETE', 'DEFECT', 'CONFIRM') 
                                THEN 1 
                                ELSE 0 
                            END AS OPERATIONTYPE,
                            ROUND(EXTRACT(SECOND FROM (D.ENDDTTM - D.STARTDTTM)) + 
                                EXTRACT(MINUTE FROM (D.ENDDTTM - D.STARTDTTM)) * 60 + 
                                EXTRACT(HOUR FROM (D.ENDDTTM - D.STARTDTTM)) * 3600 + 
                                EXTRACT(DAY FROM (D.ENDDTTM - D.STARTDTTM)) * 86400, 3) AS INTERVAL,
                            NVL(D.STARTDTTM, A.EXETIME) AS STARTDTTM,
                            NVL(D.ENDDTTM, SYSDATE) AS ENDDTTM,
                            DENSE_RANK() OVER (PARTITION BY A.SIGNINSEQ 
                                               ORDER BY A.SIGNINSEQ, NVL(D.STARTDTTM, A.EXETIME)) AS USESEQ,
                            E.SIGNINTIME,
                            E.SIGNOFFTIME,
                            E.HOSTNAME,
                            E.IPADDRESS,
                            CASE 
                                WHEN E.SIGNINTIME > (SELECT UPDATEDTTM FROM LASTUPDATE) 
                                THEN 'Y' 
                                ELSE '' 
                            END AS LASTFLAG
                        FROM MenuLog A
                        LEFT JOIN MenuMaster@KFMETA B ON A.MENUID = B.MENUID
                        LEFT JOIN OperationLog D ON A.SIGNINSEQ = D.SIGNINSEQ AND A.MENUID = D.MENUID
                                                                    AND D.STARTDTTM >= A.EXETIME AND D.ENDDTTM <= A.ENDTIME
                        LEFT JOIN SignLog E ON A.SIGNINSEQ = E.SIGNINSEQ
                        LEFT JOIN UserInfo F ON E.USERID = F.USERID
                        WHERE D.OPERATION IS NOT NULL 
                            AND A.VENDORCODE = :VENDORCODE
                            AND E.USERID LIKE  :USERID || '%'
                            AND F.USERNAME LIKE :USERNAME || '%'
                            AND A.MENUID LIKE :MENUID || '%'
                            AND B.MENUNAME LIKE :MENUNAME || '%'
                            AND D.OPERATION IN (:OPERATION)
                        ";

            return queryText;
        }

        #endregion

        #region :: MenuLog 테이블 Usert 쿼리
        /// <summary>
        /// MenuLog 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"
                        ";

            return queryText;
        }
        #endregion

        #region :: MenuLog 테이블 Delete 쿼리
        /// <summary>
        /// MenuLog 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM MenuLog
                            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
