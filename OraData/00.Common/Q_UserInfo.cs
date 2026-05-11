using System.Data;

namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    /// <summary>
    /// UserInfo 테이블 쿼리
    /// </summary>
    public static class Q_UserInfo
    {
        static string queryText = string.Empty;

        /// <summary>
        /// UserInfo 테이블 조회 쿼리
        /// 특정 조인 쿼리는 Select_[화면명] 등으로 새로 만든다.
        /// </summary>
        /// <returns></returns>
        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        WITH LASTLOGINTIME AS (
                            SELECT 
                                USERID, 
                                MAX(SIGNINTIME) AS LASTTIME, 
                                COUNT(USERID) AS CNT
                            FROM SignLog@KFLOG
                            GROUP BY USERID
                        )

                        SELECT 
                            A.EPID,
                            A.USERID,
                            A.USERNAME,
                            A.EMPNO,
                            A.PWD,
                            A.GRADECODE,
                            A.GRADENAME,
                            A.DEPTCODE,
                            A.DEPTNAME,
                            A.WORK,
                            A.PHONE,
                            A.OFFICEPHONE,
                            A.CELLPHONE,
                            A.EMAILADDRESS,
                            A.USEFLAG,
                            NVL(A.LOCKFLAG, 0) AS LOCKFLAG,
                            NVL(A.ADMINFLAG, 0) AS ADMINFLAG,
                            A.SSOFLAG,
                            NVL(A.EXTFLAG, 0) AS EXTFLAG,
                            C.TOKENID AS ENUSERID,
                            B.LASTTIME,
                            B.CNT,
                            NVL(A.UPDTTM, A.INITDTTM) AS CHANGEDTTM,
                            NVL(A.UPBY, A.INITBY) AS CHANGEBY
                        FROM UserInfo A
                        LEFT JOIN LASTLOGINTIME B
                            ON A.USERID = B.USERID
                        LEFT JOIN UserToken C
                            ON A.USERID = C.USERID
                        WHERE 1 = 1
                            AND ((:USERID IS NULL) OR (A.USERID LIKE :USERID || '%'))
                            AND ((:USERNAME IS NULL) OR (A.USERNAME LIKE :USERNAME || '%'))
                            AND ((:SSOFLAG IS NULL) OR (A.SSOFLAG LIKE :SSOFLAG || '%'))
                            ";

            return queryText;
        }

        /// <summary>
        /// UserInfo 테이블 조회 쿼리
        /// 특정 조인 쿼리는 Select_[화면명] 등으로 새로 만든다.
        /// </summary>
        /// <returns></returns>
        public static string UserSignOn()
        {
            queryText = string.Empty;

            queryText = @"/* UserSignOn() */
                        WITH LASTLOG AS (
                            SELECT 
                                L.USERID,
                                L.SIGNINTIME AS LASTSIGNINTIME,
                                L.SIGNOFFTIME AS LASTSIGNOFFTIME,
                                L.IPADDRESS AS LASTSIGNIP
                            FROM SignLog@KFLOG L
                            WHERE L.USERID = :USERID
                                AND L.SIGNOFFTIME IS NOT NULL
                                AND L.SIGNINTIME = (
                                    SELECT MAX(SIGNINTIME)
                                    FROM SignLog@KFLOG
                                    WHERE USERID = L.USERID 
                                    AND SIGNOFFTIME IS NOT NULL
                                )
                        )
                        SELECT 
                            C.TOKENID AS EPID,
                            A.LOGINID,
                            A.USERID,
                            A.USERNAME,
                            A.EMPNO,
                            A.PWD,
                            A.GRADECODE,
                            A.GRADENAME,
                            A.DEPTCODE,
                            A.DEPTNAME,
                            :VENDORCODE AS VENDORCODE,
                            fnGetCodeMasterValue('C$VENDORCODE', :VENDORCODE) AS VENDORNAME,
                            :PLANTCODE AS PLANTCODE,
                            NVL((
                                SELECT DESCRIPTION 
                                FROM Plants 
                                WHERE VENDORCODE = :VENDORCODE 
                                    AND PLANTCODE = :PLANTCODE
                            ), '') AS PLANTNAME,
                            A.PHONE,
                            A.OFFICEPHONE,
                            A.CELLPHONE,
                            A.EMAILADDRESS,
                            A.USEFLAG,
                            A.LOCKFLAG,
                            A.ADMINFLAG,
                            A.SSOFLAG,
                            A.EXTFLAG,
                            A.GTPSID,
                            NVL(B.LASTSIGNINTIME, SYSDATE) AS LASTSIGNINTIME,
                            B.LASTSIGNOFFTIME,
                            B.LASTSIGNIP
                        FROM UserInfo A
                            LEFT JOIN LASTLOG B ON A.USERID = B.USERID
                            LEFT JOIN UserToken C ON A.USERID = C.USERID
                        WHERE A.USEFLAG = 'Y'
                            AND ((:USERID IS NULL) OR (A.USERID LIKE :USERID || '%'))
                        ";

            return queryText;
        }

        /// <summary>
        /// UserInfo 테이블 조회 쿼리
        /// 특정 조인 쿼리는 Select_[화면명] 등으로 새로 만든다.
        /// </summary>
        /// <returns></returns>
        public static string UserInfo_Get()
        {
            queryText = string.Empty;

            queryText = @"/* UserInfo_Get() */
                        SELECT 
                            A.USERID AS USERID,
                            A.USERNAME AS USERNAME,
                            A.LOGINID,
                            NVL(A.GRADECODE, '') AS GRADECODE,
                            NVL(A.GRADENAME, '') AS GRADENAME,
                            NVL(A.DEPTCODE, '') AS DEPTCODE,
                            NVL(A.DEPTNAME, '') AS DEPTNAME,
                            A.EMAILADDRESS
                        FROM UserInfo A
                        WHERE 1 = 1
                            AND A.USEFLAG = 'Y'
                            AND ((:USERID IS NULL) OR (A.USERID LIKE :USERID || '%'))
                            AND ((:USERNAME IS NULL) OR (A.USERNAME LIKE :USERNAME || '%'))
                            ";

            return queryText;
        }

        /// <summary>
        /// UserInfo 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"
                        MERGE INTO KFMETA.USERINFO d
                        USING (
                          Select
                            :USERID         as USERID,
                            :USERNAME       as USERNAME,
                            :EMPNO          as EMPNO,
                            :GRADECODE      as GRADECODE,
                            :GRADENAME      as GRADENAME,
                            :PWD            as PWD,
                            :DEPTCODE       as DEPTCODE,
                            :DEPTNAME       as DEPTNAME,
                            :WORK           as WORK,
                            :PHONE          as PHONE,
                            :OFFICEPHONE    as OFFICEPHONE,
                            :CELLPHONE      as CELLPHONE,
                            :EMAILADDRESS    as EMAILADDRESS,
                            :USEFLAG        as USEFLAG,
                            :LOCKFLAG       as LOCKFLAG,
                            :ADMINFLAG      as ADMINFLAG,
                            :SSOFLAG        as SSOFLAG,
                            :EXTFLAG        as EXTFLAG,
                            :EPID           as EPID,
                            :GTPSID         as GTPSID,
                            :LOGINID        as LOGINID,
                            SYSDATE         as CHANGEDTTM,
                            :CHANGEBY       as CHANGEBY
                          From Dual) s
                        ON
                          (d.USERID = s.USERID )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.USERNAME = s.USERNAME,
                          d.DEPTCODE = s.DEPTCODE,
                          d.DEPTNAME = s.DEPTNAME,
                          d.WORK = s.WORK,
                          d.PHONE = s.PHONE,
                          d.OFFICEPHONE = s.OFFICEPHONE,
                          d.CELLPHONE = s.CELLPHONE,
                          d.EMAILADDRESS = s.EMAILADDRESS,
                          d.UPDTTM = s.CHANGEDTTM,
                          d.UPBY = s.CHANGEBY
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          USERID, USERNAME, EMPNO,
                          GRADECODE, GRADENAME, PWD,
                          DEPTCODE, DEPTNAME, WORK,
                          PHONE, OFFICEPHONE, CELLPHONE,
                          EMAILADDRESS, USEFLAG, LOCKFLAG,
                          ADMINFLAG, SSOFLAG, EXTFLAG,
                          EPID, GTPSID, LOGINID,
                          INITDTTM, INITBY)
                        VALUES (
                          s.USERID, s.USERNAME, s.EMPNO,
                          s.GRADECODE, s.GRADENAME, s.PWD,
                          s.DEPTCODE, s.DEPTNAME, s.WORK,
                          s.PHONE, s.OFFICEPHONE, s.CELLPHONE,
                          s.EMAILADDRESS, s.USEFLAG, s.LOCKFLAG,
                          s.ADMINFLAG, s.SSOFLAG, s.EXTFLAG,
                          s.EPID, s.GTPSID, s.LOGINID,
                          s.CHANGEDTTM, s.CHANGEBY)
                        ";

            return queryText;
        }

        public static string ChangePassword()
        {
            queryText = string.Empty;

            queryText = @"
                        UpdateUserPassword
                        ";

            return queryText;
        }

        /// <summary>
        /// UserInfo 테이블 삭제 쿼리
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string Delete(string[] paramList, DataTable dt)
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM UserInfo
                            ";

            //for (int i = 0; i < dt.Rows.Count; i++)
            //    queryText = SelectQuery(string reference).GetWhereText(paramList, valueList, dt.GetRowAsArray(i));

            return queryText;
        }
    }
}
