namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    /// <summary>
    /// MyMenu 테이블 쿼리
    /// </summary>
    public static class Q_MyMenu
    {
        static string queryText = string.Empty;

        #region :: MenuLog 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT DISTINCT A.USERID
	                            , A.MENUID
	                            , B.MENUNAME
	                            , C.DLLNAME
	                            , C.CLASSNAME
	                            , A.GUBUN
	                            , CASE WHEN :GUBUN = 'F' THEN '' ELSE RTRIM(A.IPADDRESS) END IPADDRESS
	                            --, ISNULL(A.UPDTTM, A.INITDTTM) CHANGEDTTM 
	                            --, ISNULL(A.UPBY, A.INITBY) CHANGEBY 
                        FROM MyMenu A
                            INNER JOIN MenuMaster B ON A.MENUID = B.MENUID
                            INNER JOIN ScreenMaster C ON B.SCREENID = C.SCREENID
                        WHERE A.USERID = :USERID AND A.GUBUN = :GUBUN
                        ";

            return queryText;
        }

        #endregion

        /// <summary>
        /// MyMenu 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            //string SCREENID = dr.CheckDataRowByColumnName("SCREENID") == true ? dr["SCREENID"].ToString() : "";

            queryText = string.Empty;

            queryText = $@"
                        MERGE INTO KFMETA.MYMENU d
                        USING (
                            Select
                            :USERID,         as USERID,
                            :MENUID,         as MENUID,
                            :GUBUN,         as GUBUN,
                            :IPADDRESS         as IPADDRESS,
                            :INITDTTM,         as INITDTTM,
                            :INITBY,         as INITBY,
                            :UPDTTM,         as UPDTTM,
                            :UPBY         as UPBY
                            From Dual) s
                        ON
                            (d.USERID = s.USERID and 
                            d.MENUID = s.MENUID and 
                            d.GUBUN = s.GUBUN and 
                            d.IPADDRESS = s.IPADDRESS )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                            d.INITDTTM = s.INITDTTM,
                            d.INITBY = s.INITBY,
                            d.UPDTTM = s.UPDTTM,
                            d.UPBY = s.UPBY
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                            USERID, MENUID, GUBUN,
                            IPADDRESS, INITDTTM, INITBY,
                            UPDTTM, UPBY)
                        VALUES (
                            s.USERID, s.MENUID, s.GUBUN,
                            s.IPADDRESS, s.INITDTTM, s.INITBY,
                            s.UPDTTM, s.UPBY)
                        ";

            return queryText;
        }

        /// <summary>
        /// MyMenu 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = $@"DELETE FROM MyMenu
		                WHERE MENUID NOT IN (SELECT MENUID FROM MenuMaster)
                            ";

            //for (int i = 0; i < dt.Rows.Count; i++)
                //queryText = SelectQuery(string reference).GetWhereText(paramList, valueList, dt.GetRowAsArray(i));

            return queryText;
        }
    }
}
