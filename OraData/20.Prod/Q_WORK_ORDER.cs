namespace I2S.SQL.COMMON.DATA.OraData.Dosing
{
    #region :: I2S.SQL.COMMON.DATA.OraData.Dosing.Q_WORK_ORDER ::


    /// <summary>
    /// WORK_ORDER 테이블 쿼리
    /// </summary>
    public static class Q_WORK_ORDER
    {
        static string queryText = string.Empty;

        #region :: WORK_ORDER 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
            SELECT PLANT_CODE
                , PROCESS_CODE
                , LINE_CODE
                , WORK_DATE
                , WORK_NUM
                , MATERIAL_CODE
                , RECIPE_VER
                , BATCH_QTY
                , BATCH_CNT
                , REPROCESS_YN
                , TAGET_BIN
                , TAGET_BIN2
                , RUN_BATCH
                , SHIFT_CODE
                , WORKER
                , ORDER_QTY
                , PROD_QTY
                , WORK_STATE
                , END_TIME
                , REMARKS
                , DEL_YN
                , START_TIME
                , I_DTTM
                , U_DTTM
                , I_USER
                , U_USER
            FROM WORK_ORDER
            ";

            return queryText;
        }

        public static string GetWorkSend(string reference)
        {
            queryText = string.Empty;

            queryText = $@" /* {reference} */
                        SELECT WORKDATE, NUM, BATCH_Q, BATCH, LOCATION_ED
                            , ISNULL(LOCATION_ED2, 0) as LOCATION_ED2
                        FROM WORK_ORDER 
                        WHERE PROCESS_KEY = :PROCESS_KEY AND WORKDATE = :WORKDATE AND NUM = :NUM
                        ";

            return queryText;
        }

        public static string GetWorkOrderDataRow(string reference)
        {
            queryText = string.Empty;

            queryText = $@" /* {reference} */
                        SELECT 1
                        FROM WORK_ORDER 
                        WHERE PROCESS_KEY = 'P04' AND WORKDATE = :WORKDATE AND NUM = :NUM
                        ";

            return queryText;
        }

        #endregion

        #region :: WORK_ORDER 테이블 Merge 쿼리
        /// <summary>
        /// WORK_ORDER 테이블 머지 쿼리
        /// </summary>/// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"
                        ";

            return queryText;
        }

        /// <summary>
        /// WORK_ORDER 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string UpdateProgress()
        {
            queryText = string.Empty;

            queryText = @"UPDATE WORK_ORDER SET C_CONDITION = '031003', R_BATCH = '1', START_TIME = GETDATE() WHERE PROCESS_KEY = :PROCESS_KEY AND WORKDATE = :WORKDATE AND NUM = :NUM
                            ";
            return queryText;
        }
        #endregion

        #region :: WORK_ORDER 테이블 Delete 쿼리
        /// <summary>
        /// WORK_ORDER 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM WORK_ORDER
                            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
