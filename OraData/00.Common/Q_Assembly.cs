using System.Data;

namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    /// <summary>
    /// Assembly 테이블 쿼리
    /// </summary>
    public static class Q_Assembly
    {
        static string queryText = string.Empty;

        static string userId = string.Empty;

        #region :: MenuLog 테이블 Select 쿼리

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sUIserId"></param>
        /// <returns></returns>
        public static string SelectQuery(string reference, string sUIserId)
        {
            queryText = string.Empty;

            if (userId != null && userId.Equals(""))
                queryText = $@"/* {reference} */
                            SELECT DISTINCT A.FILEID ASSEMBLYID
				                , B.FILENAME ASSEMBLYNAME
				                , A.VERSION ASSEMBLYVERSION
				                , B.DEPLOYCOUNT
				                , A.FILESIZE ASSEMBLYSIZE
				                , A.ISESSENTIAL
				                , A.REASON
	                        FROM KFDeployHistory@KFFILE A
	                        JOIN KFFileMaster@KFFILE B
	                        ON A.FILEID = B.FILEID
	                        JOIN FileContent@KFFILE D
	                        ON A.FILEID = D.FILEID
	                        WHERE A.CHANGEDTTM = (
							                        SELECT MAX(CHANGEDTTM) FROM KFDeployHistory@KFFILE
							                        WHERE A.FILEID = FILEID
						                            )
                            ";
            else
                queryText = $@"SELECT DISTINCT A.FILEID ASSEMBLYID
				                    , B.FILENAME ASSEMBLYNAME
				                    , A.VERSION ASSEMBLYVERSION
				                    , B.DEPLOYCOUNT
				                    , A.FILESIZE ASSEMBLYSIZE
				                    , A.ISESSENTIAL
				                    , A.REASON
	                            FROM KFDeployHistory@KFFILE A
	                            JOIN KFFileMaster@KFFILE B
	                            ON A.FILEID = B.FILEID
	                            LEFT JOIN ScreenMaster C
	                            ON B.FILENAME = C.DLLNAME
	                            LEFT JOIN MenuMaster M
	                            ON C.SCREENID = M.SCREENID
	                            JOIN FileContent@KFFILE D
	                            ON B.FILEID = D.FILEID
	                            WHERE 1 = 1
	                            AND A.CHANGEDTTM = (SELECT MAX(CHANGEDTTM) 
                                                    FROM KFDeployHistory@KFFILE
						                            WHERE A.FILEID = FILEID) AND (M.MENUID IN (SELECT MENUID 
																				                FROM AuthGroupMenu A
																				                JOIN AuthGroupUser B
																				                ON A.GROUPCODE = B.GROUPCODE
																				                WHERE 1 = 1
																				                AND B.USERID = :USERID
																				                AND B.EXPIREDATE >= SYSTIMESTAMP
																				                )
																				                OR B.ISCOMMON = 1)
                            ";

            return queryText;
        }

        #endregion

        /// <summary>
        /// Assembly 테이블 머지 쿼리
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static string Merge(string[] paramList, DataRow dr)
        {
            queryText = string.Empty;

            queryText = $@"
                        ";

            return queryText;
        }

        /// <summary>
        /// Assembly 테이블 삭제 쿼리
        /// </summary>
        /// <param name="paramList"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string Delete(string[] paramList, DataTable dt)
        {
            queryText = string.Empty;

            queryText = $@"DELETE FROM Assembly
                            ";
            return queryText;
        }
    }
}
