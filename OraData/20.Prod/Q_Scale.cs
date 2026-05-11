namespace I2S.SQL.COMMON.DATA.OraData.Dosing
{
    #region :: I2S.SQL.COMMON.DATA.OraData.Dosing.Q_Scale ::


    /// <summary>
    /// Scale 테이블 쿼리
    /// </summary>
    public static class Q_Scale
    {
        static string queryText = string.Empty;

        #region :: Scale 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = @"
                        ";

            return queryText;
        }

        public static string ScaleCombo()
        {
            queryText = string.Empty;

            queryText = @" /* ScaleCombo() */
                        SELECT SCALE_CODE AS CODEVALUE
                            , SCALE_CODE || ': ' || SCALE_NAME AS DISPLAYVALUE
                        FROM SCALE
                        ORDER BY SCALE_CODE
                        ";

            return queryText;
        }

        #endregion

        #region :: Scale 테이블 Merge 쿼리
        /// <summary>
        /// Scale 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"MERGE INTO KFATOCHANG.SCALE d
                        USING (
                          Select
                            :SCALE_CODE         as SCALE_CODE,
                            :SCALE_NAME         as SCALE_NAME,
                            :MAX_Q              as MAX_Q,
                            :ER_Q               as ER_Q,
                            :W_WAIT             as W_WAIT,
                            :W_STP              as W_STP,
                            :R_WAIT             as R_WAIT,
                            :R_STP              as R_STP,
                            :MAX_HZ             as MAX_HZ,
                            :IN_SCALE           as IN_SCALE,
                            :SCALE_NO           as SCALE_NO,
                            :PLC_ADDRESS        as PLC_ADDRESS,
                            SYSDATE             as CHANGEDTTM,
                            :CHANGEBY           as CHANGEBY
                          From Dual) s
                        ON
                          (d.SCALE_CODE = s.SCALE_CODE )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.SCALE_NAME = s.SCALE_NAME,
                          d.MAX_Q = s.MAX_Q,
                          d.ER_Q = s.ER_Q,
                          d.W_WAIT = s.W_WAIT,
                          d.W_STP = s.W_STP,
                          d.R_WAIT = s.R_WAIT,
                          d.R_STP = s.R_STP,
                          d.MAX_HZ = s.MAX_HZ,
                          d.IN_SCALE = s.IN_SCALE,
                          d.SCALE_NO = s.SCALE_NO,
                          d.PLC_ADDRESS = s.PLC_ADDRESS,
                          d.UPDTTM = s.CHANGEDTTM,
                          d.UPBY = s.CHANGEBY
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          SCALE_CODE, SCALE_NAME, MAX_Q,
                          ER_Q, W_WAIT, W_STP,
                          R_WAIT, R_STP, MAX_HZ,
                          IN_SCALE, SCALE_NO, PLC_ADDRESS,
                          INITDTTM, INITBY, UPDTTM,
                          UPBY)
                        VALUES (
                          s.SCALE_CODE, s.SCALE_NAME, s.MAX_Q,
                          s.ER_Q, s.W_WAIT, s.W_STP,
                          s.R_WAIT, s.R_STP, s.MAX_HZ,
                          s.IN_SCALE, s.SCALE_NO, s.PLC_ADDRESS,
                          s.CHANGEDTTM, s.CHANGEBY)
                        ";

            return queryText;
        }
        #endregion

        #region :: Scale 테이블 Delete 쿼리
        /// <summary>
        /// Scale 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM Scale
                            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
