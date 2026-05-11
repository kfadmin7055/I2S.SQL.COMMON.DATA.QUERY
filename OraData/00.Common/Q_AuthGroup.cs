namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    /// <summary>
    /// AuthGroup 테이블 쿼리
    /// </summary>
    public static class Q_AuthGroup
    {
        static string queryText = string.Empty;

        #region :: MenuLog 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT A.AUTHTYPE,
	                           A.GROUPCODE, 
                               A.GROUPNAME, 
	                           A.CHECKPROCESS, 
	                           A.RESTRICTIONFLAG, 
	                           A.DESCRIPTION, 
	                           NVL(A.UPDTTM, A.INITDTTM) CHANGEDTTM,
	                           B.UserName
                        FROM AuthGroup A
                            INNER JOIN USERINFO B ON B.USERID = NVL(A.UPBY, A.INITBY)
                        WHERE 1 = 1
                            AND A.GROUPNAME LIKE :GROUPNAME || '%'
                            AND A.GROUPCODE IN (SELECT GROUPCODE FROM AuthGroupUser WHERE USERID = :USERID AND ISDELEGATE = 'Y')
                            AND A.AUTHTYPE LIKE :AUTHTYPE || '%'
                            AND ((:RESTRICTIONFLAG IS NULL) OR (A.RESTRICTIONFLAG = :RESTRICTIONFLAG))
                        ORDER BY CASE WHEN A.GROUPCODE = 'ADMIN' THEN 1 ELSE 2 END, A.GROUPCODE, A.RESTRICTIONFLAG, A.GROUPNAME
                        ";

            return queryText;
        }

        #endregion

        /// <summary>
        /// AuthGroup 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = $@"MERGE INTO AUTHGROUP D
                        USING (
                          SELECT
                            :AUTHTYPE            AS AUTHTYPE,
                            :GROUPCODE           AS GROUPCODE,
                            :GROUPNAME           AS GROUPNAME,
                            :CHECKPROCESS        AS CHECKPROCESS,
                            :RESTRICTIONFLAG     AS RESTRICTIONFLAG,
                            :DESCRIPTION         AS DESCRIPTION,
                            SYSDATE            AS INITDTTM,
                            :CHANGEBY              AS INITBY,
                            SYSDATE              AS UPDTTM,
                            :CHANGEBY                AS UPBY
                          FROM Dual) s
                        ON
                          (d.GROUPCODE = s.GROUPCODE )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.AUTHTYPE = s.AUTHTYPE,
                          d.GROUPNAME = s.GROUPNAME,
                          d.CHECKPROCESS = s.CHECKPROCESS,
                          d.RESTRICTIONFLAG = s.RESTRICTIONFLAG,
                          d.DESCRIPTION = s.DESCRIPTION,
                          d.INITDTTM = s.INITDTTM,
                          d.INITBY = s.INITBY,
                          d.UPDTTM = s.UPDTTM,
                          d.UPBY = s.UPBY
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          AUTHTYPE, GROUPCODE, GROUPNAME,
                          CHECKPROCESS, RESTRICTIONFLAG, DESCRIPTION,
                          INITDTTM, INITBY, UPDTTM,
                          UPBY)
                        VALUES (
                          s.AUTHTYPE, s.GROUPCODE, s.GROUPNAME,
                          s.CHECKPROCESS, s.RESTRICTIONFLAG, s.DESCRIPTION,
                          s.INITDTTM, s.INITBY, s.UPDTTM,
                          s.UPBY);
                        ";

            return queryText;
        }

        /// <summary>
        /// AuthGroup 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = $@"DELETE FROM AuthGroup
                            WHERE GROUPCODE = :GROUPCODE
                            ";

            //for (int i = 0; i < dt.Rows.Count; i++)
            //    queryText = SelectQuery(string reference).GetWhereText(paramList, valueList, dt.GetRowAsArray(i));

            return queryText;
        }
    }
}
