namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    /// <summary>
    /// AuthGroupUser 테이블 쿼리
    /// </summary>
    public static class Q_AuthGroupUser
    {
        static string queryText = string.Empty;

        #region :: MenuLog 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT NVL(B.GROUPCODE, '') AS GROUPCODE,
		                        RTRIM(A.USERID) AS USERID,
		                        RTRIM(A.USERNAME) AS USERNAME,
		                        RTRIM(A.DEPTNAME) AS DEPTNAME,
		                        B.EXPIREDATE,
		                        B.ISDELEGATE,
		                        NVL(B.UPDTTM, B.INITDTTM) CHANGEDTTM,
	                            UI.USERNAME AS CHANGEBY
                        FROM UserInfo A
                            JOIN AuthGroupUser B ON A.USERID = B.USERID
                            JOIN UserInfo UI ON UI.USERID = NVL(B.UPBY, B.INITBY)
                        WHERE B.GROUPCODE = :GROUPCODE
                            AND A.USERNAME LIKE :USERNAME || '%'
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
                                   RTRIM(XMLAGG(XMLELEMENT(E, A.GROUPCODE || ',').EXTRACT('//text()') ORDER BY A.GROUPCODE).GETCLOBVAL(), ','),
                                   ''
                               ) AS RESTRICTIONS
                        FROM AuthGroupUser A
                        JOIN AuthGroup B
                        ON A.GROUPCODE = B.GROUPCODE
                        WHERE A.USERID = :USERID
                        AND B.RESTRICTIONFLAG = 'Y'
                        ";

            return queryText;
        }

        #endregion

        /// <summary>
        /// AuthGroupUser 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = $@"MERGE INTO KFMETA.AUTHGROUPUSER D
                        USING (
                          SELECT
                            :GROUPCODE       AS GROUPCODE,
                            :USERID          AS USERID,
                            :EXPIREDATE      AS EXPIREDATE,
                            :ISDELEGATE      AS ISDELEGATE,
                            :INITDTTM        AS INITDTTM,
                            :INITBY          AS INITBY,
                            :UPDTTM          AS UPDTTM,
                            :UPBY            AS UPBY
                          FROM Dual) s
                        ON
                          (d.GROUPCODE = s.GROUPCODE AND 
                          d.USERID = s.USERID )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.EXPIREDATE = s.EXPIREDATE,
                          d.ISDELEGATE = s.ISDELEGATE,
                          d.INITDTTM = s.INITDTTM,
                          d.INITBY = s.INITBY,
                          d.UPDTTM = s.UPDTTM,
                          d.UPBY = s.UPBY
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          GROUPCODE, USERID, EXPIREDATE,
                          ISDELEGATE, INITDTTM, INITBY,
                          UPDTTM, UPBY)
                        VALUES (
                          s.GROUPCODE, s.USERID, s.EXPIREDATE,
                          s.ISDELEGATE, s.INITDTTM, s.INITBY,
                          s.UPDTTM, s.UPBY);
                        ";

            return queryText;
        }

        /// <summary>
        /// AuthGroupUser 테이블 삭제 쿼리
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = $@"DELETE FROM AuthGroupUser
		                WHERE GROUPCODE = :GROUPCODE AND USERID = :USERID
                            ";

            //for (int i = 0; i < dt.Rows.Count; i++)
            //    queryText = SelectQuery(string reference).GetWhereText(paramList, valueList, dt.GetRowAsArray(i));

            return queryText;
        }
    }
}
