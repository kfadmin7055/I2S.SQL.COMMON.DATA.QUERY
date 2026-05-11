using System.Data;

namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    #region :: EBAP.UI.ADM._0.Query.Q_CurrentUserInfo ::

    #region :: TEMPTABLE 테이블 Select 쿼리
    /// <summary>
    /// TEMPTABLE 테이블 쿼리
    /// </summary>
    public static class Q_CurrentUserInfo
    {
        static string queryText = string.Empty;

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT , NVL(A.UPDTTM, A.INITDTTM) CHANGEDTTM
	                             , NVL(A.UPBY, A.INITBY) CHANGEBY
                        FROM TEMPTABLE A 
                            ";

            return queryText;
        }

        /// <summary>
        /// 테이블1
        /// </summary>
        /// <returns></returns>
        public static string SignLog()
        {
            queryText = @"/* SignLog() */
                        SELECT * FROM (SELECT VENDORCODE, IPADDRESS INTO :LASTVENDOR, :LASTIP 
                                        FROM SignLog
                                        WHERE USERID = :USERID
                                        ORDER BY SIGNINTIME DESC) WHERE ROWNUM <= 1
                        ";

            return queryText;
        }

        /// <summary>
        /// 테이블2
        /// </summary>
        /// <returns></returns>
        public static string UserToken()
        {
            queryText = @"/* UserToken() */
                        SELECT TOKENID INTO :MSGKEY 
                        FROM UserToken
                        WHERE USERID = :USERID
                        ";

            return queryText;
        }

        /// <summary>
        /// 테이블3
        /// </summary>
        /// <returns></returns>
        public static string UserInfo()
        {
            queryText = @"/* UserInfo() */
                        WITH Token AS (
                            SELECT TOKENID AS MSGKEY
                            FROM UserToken
                            WHERE USERID = :USERID    
                                AND ROWNUM = 1
                        )

                        SELECT USERID, USERNAME, PWD, DEPTCODE, DEPTNAME, WORK, 
                            PHONE, OFFICEPHONE, CELLPHONE, EMAILADDRESS, 
                            USEFLAG, LOCKFLAG, ADMINFLAG, NVL(SSOFLAG, 0) SSOFLAG, (SELECT MSGKEY FROM Token) MSGKEY,
                            NVL(UPDTTM, INITDTTM) CHANGEDTTM,
                            NVL(UPBY, INITBY) CHANGEBY
                        FROM UserInfo 
                        WHERE USERID = :USERID
                        ";

            return queryText;
        }

        /// <summary>
        /// 테이블4
        /// </summary>
        /// <returns></returns>
        public static string AuthGroupUser()
        {
            queryText = @"/* AuthGroupUser() */
                        SELECT A.GROUPCODE, B.GROUPNAME, B.RESTRICTIONFLAG, B.DESCRIPTION, A.EXPIREDATE, A.ISDELEGATE
                        FROM AuthGroupUser A 
                            JOIN AuthGroup B ON A.GROUPCODE = B.GROUPCODE
                        WHERE USERID = :USERID
                        ORDER BY B.RESTRICTIONFLAG
                        ";

            return queryText;
        }

        /// <summary>
        /// 테이블5
        /// </summary>
        /// <returns></returns>
        public static string MyMenuS()
        {
            queryText = @"/* MyMenuS() */
                        SELECT DISTINCT A.MENUID AS CODEVALUE
                            , A.MENUID || ' : ' || B.MENUNAME || '(' || A.IPADDRESS || ')' AS DISPLAYVALUE 
                        FROM MyMenu A
                            JOIN MenuMaster B ON A.MENUID = B.MENUID
                        WHERE 1 = 1
                            AND A.USERID = :USERID
                            AND A.GUBUN = 'S'
                        ";

            return queryText;
        }

        /// <summary>
        /// 테이블6
        /// </summary>
        /// <returns></returns>
        public static string MyMenuF()
        {
            queryText = @"/* MyMenuF() */
                        SELECT DISTINCT A.MENUID AS CODEVALUE
                            , A.MENUID || ' : ' || B.MENUNAME || '(' || A.IPADDRESS || ')' AS DISPLAYVALUE 
                        FROM MyMenu A
                            JOIN MenuMaster B ON A.MENUID = B.MENUID
                        WHERE 1 = 1
                            AND A.USERID = :USERID
                            AND A.GUBUN = 'F'
                        ";

            return queryText;
        }

        /// <summary>
        /// 테이블7
        /// </summary>
        /// <returns></returns>
        public static string MyMenuT()
        {
            queryText = @"/* MyMenuT() */
                        SELECT DISTINCT A.MENUID AS CODEVALUE
                            , A.MENUID || ' : ' || B.MENUNAME || '(' || A.IPADDRESS || ')' AS DISPLAYVALUE 
                        FROM MyMenu A
                            JOIN MenuMaster B ON A.MENUID = B.MENUID
                        WHERE 1 = 1
                            AND A.USERID = :USERID
                            AND A.GUBUN = 'T'
                        ";

            return queryText;
        }

        /// <summary>
        /// 테이블8
        /// </summary>
        /// <returns></returns>
        public static string VendorAuth()
        {
            queryText = @"/* VendorAuth() */
                        SELECT VENDORCODE, B.DISPLAYVALUE VENDORNAME, A.EXPIREDATE, ISDELEGATE
                        FROM VendorAuth A
                            JOIN CodeMaster B ON B.PCODEVALUE = 'C$VENDORCODE' AND A.VENDORCODE = B.CODEVALUE
                        WHERE A.USERID = :USERID
                        ";

            return queryText;
        }

        #endregion

        /// <summary>
        /// TEMPTABLE 테이블 머지 쿼리
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static string Merge(string[] paramList, DataRow dr)
        {
            queryText = string.Empty;

            queryText = @"
                        ";

            return queryText;
        }

        /// <summary>
        /// TEMPTABLE 테이블 삭제 쿼리
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string Delete(string[] paramList, DataTable dt)
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM TEMPTABLE
                            ";
            return queryText;
        }
    }

    #endregion
}
