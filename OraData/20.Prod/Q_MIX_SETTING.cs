using System.Data;
using System.Text;

namespace I2S.SQL.COMMON.DATA.OraData.Dosing
{
    #region :: I2S.SQL.COMMON.DATA.OraData.Dosing.Q_MIX_SETTING ::


    /// <summary>
    /// TEMPTABLE 테이블 쿼리
    /// </summary>
    public static class Q_MIX_SETTING
    {
        static string queryText = string.Empty;

        #region :: TEMPTABLE 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
            SELECT ms.PLANT_CODE
                , ms.PROCESS_CODE
                , ms.LINE_CODE
                , ms.WORK_DATE
                , ms.WORK_NUM
                , ms.SCALE_CODE
                , ms.BIN_CODE
                , ms.MIX_MATERIAL_CODE
                , ms.MIX_QTY
                , ms.MIX_RATIO
                , ms.I_DTTM
                , ms.U_DTTM
                , ms.I_USER
                , ms.U_USER
            FROM MIX_SETTING ms
            WHERE (:PLANT_CODE IS NULL OR PLANT_CODE = :PLANT_CODE)
                AND (:PROCESS_CODE IS NULL OR PROCESS_CODE = :PROCESS_CODE)
                AND (:LINE_CODE IS NULL OR LINE_CODE = :LINE_CODE)
                AND (:WORK_DATE IS NULL OR WORK_DATE = :WORK_DATE)
                AND (:WORK_NUM IS NULL OR WORK_NUM = :WORK_NUM)
            ";

            return queryText;
        }

        #endregion

        #region :: TEMPTABLE 테이블 Merge 쿼리
        /// <summary>
        /// TEMPTABLE 테이블 머지 쿼리
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

        #region :: TEMPTABLE 테이블 Delete 쿼리
        /// <summary>
        /// TEMPTABLE 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM TEMPTABLE
                            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
