namespace I2S.SQL.COMMON.DATA.OraData.Dosing
{
    #region :: I2S.SQL.COMMON.DATA.OraData.Dosing.Q_INGRED ::


    /// <summary>
    /// INGRED 테이블 쿼리
    /// </summary>
    public static class Q_CAR_MASTER
    {
        static string queryText = string.Empty;

        #region :: INGRED 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT a.RESOURCE_NO, a.RESOURCE_NAME
                            , a.H_ERROR, a.L_ERROR, a.SM_INPUT
                            , a.B_W, a.MI_USE, a.MR_USE, a.USEYN
                            , a.M_INGRED_CODE, a.PAY_YN, a.GRI_YN
                            , a.WEIGHT_TYPE
                        FROM INGRED a
                        WHERE ((:RESOURCE_NAME IS NULL) OR (a.RESOURCE_NAME = :RESOURCE_NAME))
                        ORDER BY a.RESOURCE_NAME
                        ";

            return queryText;
        }

        /// <summary>
        /// 원료 콤보
        /// </summary>
        /// <returns></returns>
        public static string IngredCombo()
        {
            queryText = string.Empty;

            queryText = @" /* IngredCombo() */
                        SELECT a.RESOURCE_NO AS CODEVALUE
                            , a.RESOURCE_NO || ': ' || a.RESOURCE_NAME AS DISPLAYVALUE
                        FROM INGRED a
                        ORDER BY a.RESOURCE_NAME
                        ";

            return queryText;
        }
        #endregion

        #region :: INGRED 테이블 Merge 쿼리
        /// <summary>
        /// INGRED 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"MERGE INTO KFATOCHANG.INGRED D
                        USING (
                          SELECT
                            :RESOURCE_NO,     AS RESOURCE_NO,
                            :RESOURCE_NAME    AS RESOURCE_NAME,
                            :H_ERROR,         AS H_ERROR,
                            :L_ERROR,         AS L_ERROR,
                            :SM_INPUT,        AS SM_INPUT,
                            :B_W,             AS B_W,
                            :MI_USE,          AS MI_USE,
                            :MR_USE,          AS MR_USE,
                            :USEYN,           AS USEYN,
                            :M_INGRED_CODE    AS M_INGRED_CODE,
                            :PAY_YN,          AS PAY_YN,
                            :GRI_YN,          AS GRI_YN,
                            :ST_STOCK,        AS ST_STOCK,
                            :PR_DATE,         AS PR_DATE,
                            :WEIGHT_TYPE,     AS WEIGHT_TYPE,
                            SYSDATE,          AS CHANGEDTTM,
                            :CHANGEBY,        AS CHANGEBY
                          FROM Dual) s
                        ON
                          (d.RESOURCE_NO = s.RESOURCE_NO )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.H_ERROR = s.H_ERROR,
                          d.L_ERROR = s.L_ERROR,
                          d.SM_INPUT = s.SM_INPUT,
                          d.B_W = s.B_W,
                          d.MI_USE = s.MI_USE,
                          d.MR_USE = s.MR_USE,
                          d.USEYN = s.USEYN,
                          d.M_INGRED_CODE = s.M_INGRED_CODE,
                          d.PAY_YN = s.PAY_YN,
                          d.GRI_YN = s.GRI_YN,
                          d.ST_STOCK = s.ST_STOCK,
                          d.PR_DATE = s.PR_DATE,
                          d.WEIGHT_TYPE = s.WEIGHT_TYPE,
                          d.UPDTTM = s.UPDTTM,
                          d.UPBY = s.CHANGEBY,
                          d.RESOURCE_NAME = s.RESOURCE_NAME
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          RESOURCE_NO, H_ERROR, L_ERROR,
                          SM_INPUT, B_W, MI_USE,
                          MR_USE, USEYN, M_INGRED_CODE,
                          PAY_YN, GRI_YN, ST_STOCK,
                          PR_DATE, WEIGHT_TYPE, INITDTTM,
                          INITBY, UPDTTM, UPBY,
                          RESOURCE_NAME)
                        VALUES (
                          s.RESOURCE_NO, s.H_ERROR, s.L_ERROR,
                          s.SM_INPUT, s.B_W, s.MI_USE,
                          s.MR_USE, s.USEYN, s.M_INGRED_CODE,
                          s.PAY_YN, s.GRI_YN, s.ST_STOCK,
                          s.PR_DATE, s.WEIGHT_TYPE, s.INITDTTM,
                          s.INITBY, s.RESOURCE_NAME)
                        ";

            return queryText;
        }
        #endregion

        #region :: INGRED 테이블 Delete 쿼리
        /// <summary>
        /// INGRED 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM INGRED
                            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
