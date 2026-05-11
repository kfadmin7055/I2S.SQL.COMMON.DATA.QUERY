namespace I2S.SQL.COMMON.DATA.OraData.Dosing
{
    #region :: I2S.SQL.COMMON.DATA.OraData.Dosing.Q_MIX_RESULT ::


    /// <summary>
    /// MIX_RESULT 테이블 쿼리
    /// </summary>
    public static class Q_MIX_RESULT
    {
        static string queryText = string.Empty;

        #region :: MIX_RESULT 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
            SELECT mr.PLANT_CODE
                , mr.PROCESS_CODE
                , mr.LINE_CODE
                , mr.WORK_DATE
                , mr.WORK_NUM
                , mr.BATCH_NUM
                , mr.SEQ
                , mr.MATERIAL_CODE
                , mr.RESULT_QTY_ACT
                , mr.RESULT_QTY_SET
                , CASE WHEN b.SET_ACT_VALUE_TYPE = 'A' THEN mr.RESULT_QTY_ACT ELSE mr.RESULT_QTY_SET END MIX_VALUE
                , mr.RESULT_QTY_SET - mr.RESULT_QTY_ACT AS MIX_DIFF
                , mr.MIX_TIME
                , mr.U_USER
                , mr.U_DTTM
                , mr.I_DTTM
                , mr.I_USER
            FROM MIX_RESULT mr
                JOIN MATERIAL m ON m.MATERIAL_CODE = mr.MATERIAL_CODE
            WHERE (:PLANT_CODE IS NULL OR mr.PLANT_CODE = :PLANT_CODE)
                AND (:PROCESS_CODE IS NULL OR mr.PROCESS_CODE = :PROCESS_CODE)
                AND (:LINE_CODE IS NULL OR mr.LINE_CODE = :LINE_CODE)
                AND (:WORK_DATE IS NULL OR mr.WORK_DATE = :WORK_DATE)
                AND (:WORK_NUM IS NULL OR mr.WORK_NUM = :WORK_NUM)
            ORDER BY mr.BATCH_NUM, mr.SEQ
            ";

            return queryText;
        }

        public static string SelectBatchQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
            SELECT DISTINCT mr.PLANT_CODE
                , mr.PROCESS_CODE
                , mr.LINE_CODE
                , mr.WORK_DATE
                , mr.WORK_NUM
                , mr.BATCH_NUM
            FROM MIX_RESULT mr
                JOIN MATERIAL m ON m.MATERIAL_CODE = mr.MATERIAL_CODE
            WHERE (:PLANT_CODE IS NULL OR mr.PLANT_CODE = :PLANT_CODE)
                AND (:PROCESS_CODE IS NULL OR mr.PROCESS_CODE = :PROCESS_CODE)
                AND (:LINE_CODE IS NULL OR mr.LINE_CODE = :LINE_CODE)
                AND (:WORK_DATE IS NULL OR mr.WORK_DATE = :WORK_DATE)
                AND (:WORK_NUM IS NULL OR mr.WORK_NUM = :WORK_NUM)
            ORDER BY mr.BATCH_NUM
            ";

            return queryText;
        }

        #endregion

        #region :: MIX_RESULT 테이블 Merge 쿼리
        /// <summary>
        /// MIX_RESULT 테이블 머지 쿼리
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

        #region :: MIX_RESULT 테이블 Delete 쿼리
        /// <summary>
        /// MIX_RESULT 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM MIX_RESULT
                            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
