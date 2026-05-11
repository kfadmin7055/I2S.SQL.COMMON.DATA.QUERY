using System.Data;

namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    #region :: EBAP.UI.ADM._0.Query.Q_UserCell ::

    #region :: UserCell 테이블 Select 쿼리
    /// <summary>
    /// UserCell 테이블 쿼리
    /// </summary>
    public static class Q_UserCell
    {
        static string queryText = string.Empty;

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT A.USERID
                            , A.VENDORCODE
                            , A.PLANTCODE
                            , A.LINE
                            , A.CELL
                            , A.STATIONGROUP
                            , NVL(A.UPDTTM, A.INITDTTM) CHANGEDTTM
	                        , NVL(A.UPBY, A.INITBY) CHANGEBY
                        FROM UserCell A 
                        WHERE A.USERID = :USERID 
                            AND IPADDRESS LIKE :IPADDRESS || '%'
                        ";

            return queryText;
        }

        #endregion

        /// <summary>
        /// UserCell 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"MERGE INTO KFMETA.USERCELL d
                        USING (
                          Select
                            USERID          as USERID,
                            VENDORCODE      as VENDORCODE,
                            PLANTCODE       as PLANTCODE,
                            LINE            as LINE,
                            CELL            as CELL,
                            STATIONGROUP    as STATIONGROUP,
                            IPADDRESS       as IPADDRESS,
                            INITDTTM        as INITDTTM,
                            INITBY          as INITBY,
                            UPDTTM          as UPDTTM,
                            UPBY            as UPBY
                          From Dual) s
                        ON
                          (d.USERID = s.USERID and 
                          d.IPADDRESS = s.IPADDRESS )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.VENDORCODE = s.VENDORCODE,
                          d.PLANTCODE = s.PLANTCODE,
                          d.LINE = s.LINE,
                          d.CELL = s.CELL,
                          d.STATIONGROUP = s.STATIONGROUP,
                          d.INITDTTM = s.INITDTTM,
                          d.INITBY = s.INITBY,
                          d.UPDTTM = s.UPDTTM,
                          d.UPBY = s.UPBY
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          USERID, VENDORCODE, PLANTCODE,
                          LINE, CELL, STATIONGROUP,
                          IPADDRESS, INITDTTM, INITBY,
                          UPDTTM, UPBY)
                        VALUES (
                          s.USERID, s.VENDORCODE, s.PLANTCODE,
                          s.LINE, s.CELL, s.STATIONGROUP,
                          s.IPADDRESS, s.INITDTTM, s.INITBY,
                          s.UPDTTM, s.UPBY);
                        ";

            return queryText;
        }

        /// <summary>
        /// UserCell 테이블 삭제 쿼리
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string Delete(string[] paramList, DataTable dt)
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM UserCell
                            ";
            return queryText;
        }
    }

    #endregion
}
