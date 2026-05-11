namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    #region :: EBAP.UI.ADM._0.Query.Q_PlantAuth ::


    /// <summary>
    /// PlantAuth 테이블 쿼리
    /// </summary>
    public static class Q_PlantAuth
    {
        static string queryText = string.Empty;

        #region :: PlantAuth 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT A.PLANTCODE
	                         , A.USERID
	                         , B.USERNAME
	                         , B.DEPTNAME
	                         , A.EXPIREDATE
	                         , A.ISDELEGATE
	                         , NVL(A.UPDTTM, A.INITDTTM) CHANGEDTTM 
	                         , NVL(A.UPBY, A.INITBY) CHANGEBY 
                        FROM PlantAuth A 
                            JOIN UserInfo B ON A.USERID = B.USERID
                        WHERE A.PLANTCODE = :PLANTCODE
                        AND B.USERNAME LIKE :USERNAME || '%'
                        ";

            return queryText;
        }

        #endregion

        #region :: PlantAuth 테이블 Usert 쿼리
        /// <summary>
        /// PlantAuth 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"MERGE INTO KFMETA.PLANTAUTH d
                        USING (
                          Select
                            :PLANTCODE       as PLANTCODE,
                            :USERID          as USERID,
                            :EXPIREDATE      as EXPIREDATE,
                            :ISDELEGATE      as ISDELEGATE,
                            :INITDTTM        as INITDTTM,
                            :INITBY          as INITBY,
                            :UPDTTM          as UPDTTM,
                            :UPBY            as UPBY
                          From Dual) s
                        ON
                          (d.PLANTCODE = s.PLANTCODE and 
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
                          PLANTCODE, USERID, EXPIREDATE,
                          ISDELEGATE, INITDTTM, INITBY,
                          UPDTTM, UPBY)
                        VALUES (
                          s.PLANTCODE, s.USERID, s.EXPIREDATE,
                          s.ISDELEGATE, s.INITDTTM, s.INITBY,
                          s.UPDTTM, s.UPBY)
                        ";

            return queryText;
        }
        #endregion

        #region :: PlantAuth 테이블 Delete 쿼리
        /// <summary>
        /// PlantAuth 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM dbo.PlantAuth
		                WHERE PLANTCODE = :PLANTCODE AND USERID = :USERID
                            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
