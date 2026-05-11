namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    #region :: EBAP.UI.ADM._0.Query.Q_LinkMenu ::


    /// <summary>
    /// LinkMenu 테이블 쿼리
    /// </summary>
    public static class Q_LinkMenu
    {
        static string queryText = string.Empty;

        #region :: LinkMenu 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT A.MENUID
	                         , A.MENUNAME
	                         , A.LVL
	                         , A.IDX
	                         , A.LINKURL
	                         , A.DESCRIPTION
	                         , A.USEFLAG
	                         , A.LINKTYPE
	                         , A.ISBEGINGROUP
	                         , NVL(A.UPDTTM, A.INITDTTM) CHANGEDTTM 
	                         , NVL(A.UPBY, A.INITBY) CHANGEBY 
                        FROM LinkMenu A 
                        WHERE 1 = 1 
                            AND MENUNAME LIKE '%' || :MENUNAME || '%'
                        ";

            return queryText;
        }
        
        #endregion

        #region :: LinkMenu 테이블 Usert 쿼리
        /// <summary>
        /// LinkMenu 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"MERGE INTO KFMETA.LINKMENU d
                        USING (
                          Select
                            :MENUID         as MENUID,
                            :MENUNAME       as MENUNAME,
                            :LVL            as LVL,
                            :IDX            as IDX,
                            :LINKURL        as LINKURL,
                            :DESCRIPTION    as DESCRIPTION,
                            :USEFLAG        as USEFLAG,
                            :LINKTYPE       as LINKTYPE,
                            :ISBEGINGROUP   as ISBEGINGROUP,
                            :INITDTTM       as INITDTTM,
                            :INITBY         as INITBY,
                            :UPDTTM         as UPDTTM,
                            :UPBY           as UPBY
                          From Dual) s
                        ON
                          (d.MENUID = s.MENUID )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.MENUNAME = s.MENUNAME,
                          d.LVL = s.LVL,
                          d.IDX = s.IDX,
                          d.LINKURL = s.LINKURL,
                          d.DESCRIPTION = s.DESCRIPTION,
                          d.USEFLAG = s.USEFLAG,
                          d.LINKTYPE = s.LINKTYPE,
                          d.ISBEGINGROUP = s.ISBEGINGROUP,
                          d.INITDTTM = s.INITDTTM,
                          d.INITBY = s.INITBY,
                          d.UPDTTM = s.UPDTTM,
                          d.UPBY = s.UPBY
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          MENUID, MENUNAME, LVL,
                          IDX, LINKURL, DESCRIPTION,
                          USEFLAG, LINKTYPE, ISBEGINGROUP,
                          INITDTTM, INITBY, UPDTTM,
                          UPBY)
                        VALUES (
                          s.MENUID, s.MENUNAME, s.LVL,
                          s.IDX, s.LINKURL, s.DESCRIPTION,
                          s.USEFLAG, s.LINKTYPE, s.ISBEGINGROUP,
                          s.INITDTTM, s.INITBY, s.UPDTTM,
                          s.UPBY);
                        ";

            return queryText;
        }
        #endregion

        #region :: LinkMenu 테이블 Delete 쿼리
        /// <summary>
        /// LinkMenu 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"DELETE FROMm LinkMenu
		                WHERE MENUID = :MENUID
                            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
