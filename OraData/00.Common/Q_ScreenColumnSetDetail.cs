using I2S.SQL.COMMON.DATA.OraData.Collections;
using System.Data;

namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    /// <summary>
    /// ScreenColumnSetDetail 테이블 쿼리
    /// </summary>
    public static class Q_ScreenColumnSetDetail
    {
        static string queryText = string.Empty;

        #region :: MenuLog 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT A.SCREENID
                            , A.GRIDNAME
	                        , A.USESELECTCOLUMN
	                        , A.KEYCOLUMNS
	                        , A.MANDATORYCOLUMNS
	                        , A.NEWROWENABLECOLUMNS
	                        , A.NEWROWCOPYCOLUMNS
	                        , NVL(A.UPDTTM, A.INITDTTM) CHANGEDTTM
	                        , NVL(A.UPBY, A.INITBY) CHANGEBY
                        FROM ScreenColumnSetDetail A
                        WHERE A.SCREENID = NVL(:SCREENID, A.SCREENID)
                            AND A.GRIDNAME = NVL(:GRIDNAME, A.GRIDNAME)
                        ORDER BY A.SCREENID
                        ";

            return queryText;
        }

        #endregion

        /// <summary>
        /// ScreenColumnSetDetail 테이블 업서트 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Upsert(ParamCollection param)
        {
            queryText = string.Empty;

            //queryText = CommonQuery.GetSetText("ScreenColumnSetDetail", param);

            return queryText;
        }

        /// <summary>
        /// ScreenColumnSetDetail 테이블 업서트 쿼리
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static string Upsert(string[] paramList, DataRow dr)
        {
            queryText = string.Empty;

            //queryText = CommonQuery.GetSetText("ScreenColumnSetDetail", paramList, dr);

            return queryText;
        }

        /// <summary>
        /// ScreenColumnSetDetail 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = $@"MERGE INTO SCREENCOLUMNSETDETAIL D
                            USING (
                              SELECT
                               :SCREENID             AS SCREENID,
                               :GRIDNAME             AS GRIDNAME,
                               :USESELECTCOLUMN      AS USESELECTCOLUMN,
                               :KEYCOLUMNS           AS KEYCOLUMNS,
                               :MANDATORYCOLUMNS     AS MANDATORYCOLUMNS,
                               :NEWROWENABLECOLUMNS  AS NEWROWENABLECOLUMNS,
                               :NEWROWCOPYCOLUMNS    AS NEWROWCOPYCOLUMNS,
                               SYSDATE             AS INITDTTM,
                               :CHANGEBY               AS INITBY,
                               SYSDATE               AS UPDTTM,
                               :CHANGEBY                 AS UPBY
                              FROM Dual) s
                            ON
                              (d.SCREENID = s.SCREENID AND 
                              d.GRIDNAME = s.GRIDNAME )
                            WHEN MATCHED
                            THEN
                            UPDATE SET
                              d.USESELECTCOLUMN = s.USESELECTCOLUMN,
                              d.KEYCOLUMNS = s.KEYCOLUMNS,
                              d.MANDATORYCOLUMNS = s.MANDATORYCOLUMNS,
                              d.NEWROWENABLECOLUMNS = s.NEWROWENABLECOLUMNS,
                              d.NEWROWCOPYCOLUMNS = s.NEWROWCOPYCOLUMNS,
                              d.INITDTTM = s.INITDTTM,
                              d.INITBY = s.INITBY,
                              d.UPDTTM = s.UPDTTM,
                              d.UPBY = s.UPBY
                            WHEN NOT MATCHED
                            THEN
                            INSERT (
                              SCREENID, GRIDNAME, USESELECTCOLUMN,
                              KEYCOLUMNS, MANDATORYCOLUMNS, NEWROWENABLECOLUMNS,
                              NEWROWCOPYCOLUMNS, INITDTTM, INITBY,
                              UPDTTM, UPBY)
                            VALUES (
                              s.SCREENID, s.GRIDNAME, s.USESELECTCOLUMN,
                              s.KEYCOLUMNS, s.MANDATORYCOLUMNS, s.NEWROWENABLECOLUMNS,
                              s.NEWROWCOPYCOLUMNS, s.INITDTTM, s.INITBY,
                              s.UPDTTM, s.UPBY)
                        ";

            return queryText;
        }

        /// <summary>
        /// ScreenColumnSetDetail 테이블 삭제 쿼리
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string Delete(string[] paramList, DataTable dt)
        {
            queryText = string.Empty;

            queryText = $@"DELETE FROM ScreenColumnSetDetail
                        WHERE SCREENID = :SCREENID
                            ";

            return queryText;
        }
    }
}
