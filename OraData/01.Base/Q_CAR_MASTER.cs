namespace I2S.SQL.COMMON.DATA.OraData.Base
{
    #region :: I2S.SQL.COMMON.DATA.OraData.Base.Q_CAR_MASTER ::


    /// <summary>
    /// CAR_MASTER 테이블 쿼리
    /// </summary>
    public static class Q_CAR_MASTER
    {
        static string queryText = string.Empty;

        #region :: CAR_MASTER 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
            SELECT CAR_CODE
                , CAR_NO_REAL
                , CAR_TON_CODE
                , CAR_GROUP
                , DELIVERY_EMP
                , DELIVERTY_TEL
                , ISREQUIRESEAL
                , USE_YN
                , U_USER
                , U_DTTM
                , I_DTTM
                , I_USER
            FROM CAR_MASTER
            WHERE CAR_NO_REAL = :CAR_NO_REAL
            ";

            return queryText;
        }

        /// <summary>
        /// 자동차 번호 콤보
        /// </summary>
        /// <returns></returns>
        public static string CAR_MASTERCombo()
        {
            queryText = string.Empty;

            queryText = @" /* CAR_MASTERCombo() */
                        SELECT a.RESOURCE_NO AS CODEVALUE
                            , a.RESOURCE_NO || ': ' || a.RESOURCE_NAME AS DISPLAYVALUE
                        FROM CAR_MASTER a
                        ORDER BY a.RESOURCE_NAME
                        ";

            return queryText;
        }
        #endregion

        #region :: CAR_MASTER 테이블 Merge 쿼리
        /// <summary>
        /// CAR_MASTER 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"
            MERGE INTO CAR_MASTER d
            USING (
                SELECT
                      :CAR_CODE         AS CAR_CODE
                    , :CAR_NO_REAL      AS CAR_NO_REAL
                    , :CAR_TON_CODE     AS CAR_TON_CODE
                    , :CAR_GROUP        AS CAR_GROUP
                    , :DELIVERY_EMP     AS DELIVERY_EMP
                    , :DELIVERTY_TEL    AS DELIVERTY_TEL
                    , :ISREQUIRESEAL    AS ISREQUIRESEAL
                    , :USE_YN           AS USE_YN
                    , SYSDATE           AS CHANGEDTTM
                    , :USER_ID          AS CHANGEBY
                FROM DUAL
            ) s
            ON (
                d.CAR_CODE = s.CAR_CODE
            )

            WHEN MATCHED THEN
            UPDATE SET
                  d.CAR_NO_REAL     = s.CAR_NO_REAL
                , d.CAR_TON_CODE    = s.CAR_TON_CODE
                , d.CAR_GROUP       = s.CAR_GROUP
                , d.DELIVERY_EMP    = s.DELIVERY_EMP
                , d.DELIVERTY_TEL   = s.DELIVERTY_TEL
                , d.ISREQUIRESEAL   = s.ISREQUIRESEAL
                , d.USE_YN          = s.USE_YN
                , d.U_USER          = s.CHANGEBY
                , d.U_DTTM          = s.CHANGEDTTM

            WHEN NOT MATCHED THEN
            INSERT (
                  CAR_CODE
                , CAR_NO_REAL
                , CAR_TON_CODE
                , CAR_GROUP
                , DELIVERY_EMP
                , DELIVERTY_TEL
                , ISREQUIRESEAL
                , USE_YN
                , I_USER
                , I_DTTM
            )
            VALUES (
                  s.CAR_CODE
                , s.CAR_NO_REAL
                , s.CAR_TON_CODE
                , s.CAR_GROUP
                , s.DELIVERY_EMP
                , s.DELIVERTY_TEL
                , s.ISREQUIRESEAL
                , s.USE_YN
                , s.CHANGEBY
                , s.CHANGEDTTM
            )
            ";

            return queryText;
        }
        #endregion

        #region :: CAR_MASTER 테이블 Delete 쿼리
        /// <summary>
        /// CAR_MASTER 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM CAR_MASTER
                            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
