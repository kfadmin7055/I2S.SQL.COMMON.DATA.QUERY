namespace I2S.SQL.COMMON.DATA.OraData.Dosing
{
    #region :: I2S.SQL.COMMON.DATA.OraData.Dosing.Q_WORK_DETAIL ::


    /// <summary>
    /// WORK_DETAIL 테이블 쿼리
    /// </summary>
    public static class Q_WORK_DETAIL
    {
        static string queryText = string.Empty;

        #region :: WORK_DETAIL 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@" /* {reference} */
                        ";

            return queryText;
        }

        public static string GetScaleSv(string reference)
        {
            queryText = string.Empty;

            queryText = $@" /* {reference} */
                        SELECT A.SCALE_CODE, A.SV, A.MAX_Q
                        FROM (
                                SELECT W.SCALE_CODE, SUM(W.SET_VAL) AS SV, MAX(SC.MAX_Q) AS MAX_Q 
                                FROM WORK_DETAIL W 
                                    LEFT OUTER JOIN SCALE SC ON W.SCALE_CODE = SC.SCALE_CODE
                                WHERE W.PROCESS_KEY = 'P04' AND W.WORKDATE = :WORKDATE AND W.NUM = :NUM
                                GROUP BY W.SCALE_CODE
                        ) A WHERE A.SV > MAX_Q
                        ";

            return queryText;
        }

        public static string resultWorkSendIndex(string reference)
        {
            queryText = string.Empty;

            queryText = $@" /* {reference} */
                        SELECT workd.WORKDATE, workd.NUM, workd.SCALE_CODE, sc.SCALE_NO, b.BIN_SERIAL
                            , workd.LOCATION, workd.INGRED_CODE as RESOURCE_USED
                            , workd.QTY_PCT, workd.SET_VAL as MIX_RESULT, b.FAIL, sc.IN_SCALE 
                        FROM WORK_DETAIL workd
                            LEFT OUTER JOIN BIN b ON workd.LOCATION = b.LOCATION
                            LEFT OUTER JOIN SCALE sc ON workd.SCALE_CODE = sc.SCALE_CODE
                            LEFT OUTER JOIN INGRED ing ON workd.INGRED_CODE = ing.RESOURCE_NO
                        WHERE workd.PROCESS_KEY = :PROCESS_KEY AND workd.WORKDATE = :WORKDATE AND workd.NUM = :NUM
                            AND sc.SCALE_CODE <> 'LIQUID' AND ISNULL(sc.SCALE_CODE,'NO') <> 'NO'
                        ORDER BY workd.SEQ
                        ";

            return queryText;
        }

        public static string GetWorkDetailValue(string reference)
        {
            queryText = string.Empty;

            queryText = $@" /* {reference} */
                        SELECT SET_VAL, QTY_PCT
                        FROM WORK_DETAIL
                        WHERE INGRED_CODE = '962118' AND PROCESS_KEY = :PROCESS_KEY AND WORKDATE = :WORKDATE AND NUM = :NUM
                        ";

            return queryText;
        }

        public static string GetLiquid(string reference)
        {
            queryText = string.Empty;

            queryText = $@" /* {reference} */
                        SELECT  sc.SCALE_NAME, workd.SET_VAL, workd.QTY_PCT
                        FROM WORK_DETAIL workd LEFT OUTER JOIN BIN b ON workd.LOCATION = b.LOCATION
                            LEFT OUTER JOIN SCALE sc ON workd.SCALE_CODE = sc.SCALE_CODE
                            LEFT OUTER JOIN INGRED ing ON workd.INGRED_CODE = ing.RESOURCE_NO
                        WHERE workd.PROCESS_KEY = :PROCESS_KEY AND workd.WORKDATE = :WORKDATE AND workd.NUM = :NUM 
                            AND sc.SCALE_CODE = 'LIQUID'
                        ";

            return queryText;
        }

        public static string GetScaleRecipeSumary(string reference)
        {
            queryText = string.Empty;

            queryText = $@" /* {reference} */
                        SELECT  sc.SCALE_NO, SUM(workd.SET_VAL * sc.IN_SCALE) as TOTAL
                        FROM WORK_DETAIL workd LEFT OUTER JOIN BIN b ON workd.LOCATION = b.LOCATION 
                            LEFT OUTER JOIN SCALE sc ON workd.SCALE_CODE = sc.SCALE_CODE 
                            LEFT OUTER JOIN INGRED ing ON workd.INGRED_CODE = ing.RESOURCE_NO 
                        WHERE workd.PROCESS_KEY = :PROCESS_KEY AND workd.WORKDATE = :WORKDATE AND workd.NUM = :NUM 
                            AND sc.SCALE_CODE <> 'LIQUID' AND ISNULL(sc.SCALE_CODE,'NO') <> 'NO'
                        GROUP BY sc.SCALE_NO
                        ORDER BY sc.SCALE_NO 
                        ";

            return queryText;
        }

        #endregion

        #region :: WORK_DETAIL 테이블 Merge 쿼리
        /// <summary>
        /// WORK_DETAIL 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"
                        ";

            return queryText;
        }
        #endregion

        #region :: WORK_DETAIL 테이블 Delete 쿼리
        /// <summary>
        /// WORK_DETAIL 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM WORK_DETAIL
                            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
