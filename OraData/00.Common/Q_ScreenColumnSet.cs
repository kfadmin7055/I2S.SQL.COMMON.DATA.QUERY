namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    /// <summary>
    /// ScreenColumnSet 테이블 쿼리
    /// </summary>
    public static class Q_ScreenColumnSet
    {
        static string queryText = string.Empty;

        #region :: MenuLog 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT A.SCREENID
                                , B.SCREENNAME
	                            , A.GRIDNAME
	                            , A.IDX
	                            , A.FIELDNAME
	                            , A.CAPTION
	                            , A.WIDTH
	                            , A.MAXLENGTH
	                            , A.DECIMALPLACE
	                            , A.ALLOWEDIT
	                            , A.VISIBLE
	                            , A.DATATYPE
	                            , A.HORIZONALIGN
	                            , NVL(A.UPDTTM, A.INITDTTM) CHANGEDTTM
	                            , NVL(A.UPBY, A.INITBY) CHANGEBY
                        FROM SCREENCOLUMNSET A
                            INNER JOIN ScreenMaster B ON A.SCREENID = B.SCREENID
                        WHERE ((:SCREENID IS NULL) OR (A.SCREENID = :SCREENID))
                        ORDER BY A.SCREENID, A.IDX
                        ";

            return queryText;
        }

        #endregion

        /// <summary>
        /// ScreenColumnSet 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = $@"MERGE INTO KFMETA.SCREENCOLUMNSET D
                        USING (
                            SELECT
                            :SCREENID       AS SCREENID,
                            :GRIDNAME       AS GRIDNAME,
                            :IDX            AS IDX,
                            :FIELDNAME      AS FIELDNAME,
                            :CAPTION        AS CAPTION,
                            :WIDTH          AS WIDTH,
                            :MAXLENGTH      AS MAXLENGTH,
                            :DECIMALPLACE   AS DECIMALPLACE,
                            :ALLOWEDIT      AS ALLOWEDIT,
                            :VISIBLE        AS VISIBLE,
                            :DATATYPE       AS DATATYPE,
                            :HORIZONALIGN   AS HORIZONALIGN,
                            SYSDATE         AS INITDTTM,
                            :CHANGEBY       AS INITBY,
                            SYSDATE         AS UPDTTM,
                            :CHANGEBY       AS UPBY
                            FROM Dual) s
                        ON
                            (d.SCREENID = s.SCREENID AND 
                            d.GRIDNAME = s.GRIDNAME AND 
                            d.FIELDNAME = s.FIELDNAME )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                            d.IDX = s.IDX,
                            d.CAPTION = s.CAPTION,
                            d.WIDTH = s.WIDTH,
                            d.MAXLENGTH = s.MAXLENGTH,
                            d.DECIMALPLACE = s.DECIMALPLACE,
                            d.ALLOWEDIT = s.ALLOWEDIT,
                            d.VISIBLE = s.VISIBLE,
                            d.DATATYPE = s.DATATYPE,
                            d.HORIZONALIGN = s.HORIZONALIGN,
                            d.UPDTTM = s.UPDTTM,
                            d.UPBY = s.UPBY
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                            SCREENID, GRIDNAME, IDX,
                            FIELDNAME, CAPTION, WIDTH,
                            MAXLENGTH, DECIMALPLACE, ALLOWEDIT,
                            VISIBLE, DATATYPE, HORIZONALIGN,
                            INITDTTM, INITBY)
                        VALUES (
                            s.SCREENID, s.GRIDNAME, s.IDX,
                            s.FIELDNAME, s.CAPTION, s.WIDTH,
                            s.MAXLENGTH, s.DECIMALPLACE, s.ALLOWEDIT,
                            s.VISIBLE, s.DATATYPE, s.HORIZONALIGN,
                            s.INITDTTM, s.INITBY)
                        ";

            return queryText;
        }

        /// <summary>
        /// ScreenColumnSet 테이블 삭제 쿼리
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = $@"DELETE FROM dbo.ScreenColumnSet
		                WHERE SCREENID = :SCREENID AND GRIDNAME = :GRIDNAME AND FIELDNAME = :FIELDNAME
                            ";

            return queryText;
        }
    }
}
