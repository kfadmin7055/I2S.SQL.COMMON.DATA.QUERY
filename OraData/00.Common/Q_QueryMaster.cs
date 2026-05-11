namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    #region :: EBAP.UI.ADM._0.Query.Q_QueryMaster ::


    /// <summary>
    /// QueryMaster 테이블 쿼리
    /// </summary>
    public static class Q_QueryMaster
    {
        static string queryText = string.Empty;

        #region :: QueryMaster 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT SQLGROUP, 
	                    QUERYID, 
	                    SUBJECT, 
	                    QUERYTYPE, 
	                    QUERYTEXT,
	                    DBID,
	                    IDX, 
	                    NVL(UPDTTM, INITDTTM) CHANGEDTTM, 
	                    NVL(UPBY, INITBY) CHANGEBY 
	                    FROM QueryMaster 
	                    WHERE 1 = 1
                            AND QUERYID LIKE '%' || :QUERYID || '%'
                            AND SQLGROUP LIKE '%' || :SQLGROUP || '%'
                            AND SUBJECT LIKE '%' || :SUBJECT || '%'
                        ";

            return queryText;
        }

        public static string QueryMaster_CopySource_Select()
        {
            queryText = string.Empty;

            queryText = @"/* QueryMaster_CopySource_Select() */
                        SELECT SQLGROUP, 
                            QUERYID, 
                            SUBJECT, 
                            QUERYTYPE, 
                            QUERYTEXT,
                            DBID,
                            IDX, 
                            NVL(UPDTTM, INITDTTM) CHANGEDTTM, 
                            NVL(UPBY, INITBY) CHANGEBY 
                        FROM QueryMaster 
                        WHERE 1 = 1 
                        AND QUERYID LIKE :SQLGROUP + '.' + :UINAME || '%'
                        ";

            return queryText;
        }

        public static string QueryMaster_CopyData_Select()
        {
            queryText = string.Empty;

            queryText = @"/* QueryMaster_CopyData_Select() */
                        SELECT REPLACE(REPLACE(A.QUERYID, :SQLGROUP, :CHGSQLGROUP), :UINAME, :CHGUINAME) QUERYID
	                        , REPLACE(A.SUBJECT, :SUBJECT, :CHGSUBJECT)  DESCRIPTION
	                        , A.QUERYTYPE
	                        , REPLACE(A.QUERYTEXT, :UINAME, :CHGUINAME) QUERYTEXT
	                        , A.IDX
	                        , CASE WHEN B.IDX IS NULL THEN 0 ELSE 1 END CHECKDUP
	                        , SYSTIMESTAMP CHANGEDTTM
	                        , A.INITBY CHANGEBY
	                        , :SQLGROUP SQLGROUP
	                        , :UINAME UINAME
	                        , :SUBJECT SUBJECT
	                        , :CHGSQLGROUP CHGSQLGROUP
	                        , :CHGUINAME CHGUINAME
	                        , :CHGSUBJECT CHGSUBJECT
	                        , :DBID DBID
                        FROM QueryMaster A
                        LEFT JOIN QueryMaster B
                        ON B.QUERYID = REPLACE(REPLACE(A.QUERYID, :SQLGROUP, :CHGSQLGROUP), :UINAME, :CHGUINAME)
                        WHERE 1 = 1 
                        AND A.QUERYID LIKE :SQLGROUP + '.' + :UINAME || '%'
                        ";

            return queryText;
        }

        #endregion

        #region :: QueryMaster 테이블 Usert 쿼리
        /// <summary>
        /// QueryMaster 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"MERGE INTO KFMETA.QUERYMASTER d
                        USING (
                          Select
                            SQLGROUP        as SQLGROUP,
                            QUERYID         as QUERYID,
                            SUBJECT         as SUBJECT,
                            QUERYTYPE       as QUERYTYPE,
                            QUERYTEXT       as QUERYTEXT,
                            DBID            as DBID,
                            IDX             as IDX,
                            INITDTTM        as INITDTTM,
                            INITBY          as INITBY,
                            UPDTTM          as UPDTTM,
                            UPBY            as UPBY
                          From Dual) s
                        ON
                          (d.QUERYID = s.QUERYID and 
                          d.IDX = s.IDX )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.SQLGROUP = s.SQLGROUP,
                          d.SUBJECT = s.SUBJECT,
                          d.QUERYTYPE = s.QUERYTYPE,
                          d.QUERYTEXT = s.QUERYTEXT,
                          d.DBID = s.DBID,
                          d.INITDTTM = s.INITDTTM,
                          d.INITBY = s.INITBY,
                          d.UPDTTM = s.UPDTTM,
                          d.UPBY = s.UPBY
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          SQLGROUP, QUERYID, SUBJECT,
                          QUERYTYPE, QUERYTEXT, DBID,
                          IDX, INITDTTM, INITBY,
                          UPDTTM, UPBY)
                        VALUES (
                          s.SQLGROUP, s.QUERYID, s.SUBJECT,
                          s.QUERYTYPE, s.QUERYTEXT, s.DBID,
                          s.IDX, s.INITDTTM, s.INITBY,
                          s.UPDTTM, s.UPBY)
                        ";

            return queryText;
        }

        /// <summary>
        /// QueryMaster 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Insert()
        {
            queryText = string.Empty;

            queryText = @"INSERT INTO QueryMaster (SQLGROUP, QUERYID, SUBJECT, QUERYTYPE, QUERYTEXT, DBID, IDX, INITDTTM, INITBY)
                        SELECT REPLACE(SQLGROUP, :SQLGROUP, :CHGSQLGROUP),
                               REPLACE(REPLACE(QUERYID, :SQLGROUP, :CHGSQLGROUP), :UINAME, :CHGUINAME),
                               REPLACE(SUBJECT, :SUBJECT, :CHGSUBJECT),
                               QUERYTYPE,
                               REPLACE(QUERYTEXT, :UINAME, :CHGUINAME),
                               :DBID,
                               IDX,
                               SYSDATE,
                               INITBY
                        FROM QueryMaster
                        WHERE QUERYID LIKE :SQLGROUP || '.' || :UINAME || '%'
                        ";

            return queryText;
        }
        #endregion

        #region :: QueryMaster 테이블 Delete 쿼리
        /// <summary>
        /// QueryMaster 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM dbo.QueryMaster
		                WHERE QUERYID = :QUERYID AND IDX = :IDX
                            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
