namespace I2S.SQL.COMMON.DATA.OraData.Item
{
    #region :: I2S.SQL.COMMON.DATA.OraData.Base.Q_PRODUCT ::


    /// <summary>
    /// PRODUCT 테이블 쿼리
    /// </summary>
    public static class Q_Product
    {
        static string queryText = string.Empty;

        #region :: PRODUCT 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
            SELECT PLANT_CODE
                , PRODUCT_TYPE
                , PRODUCT_CODE
                , PRODUCT_NAME
                , UOM
                , PRODUCT_GROUP
                , FEED_FORM
                , MIX_TIME
                , DRY_TIME
                , FINAL_TIME
                , ERP_ITEM_CODE
                , REMARKS
                , U_USER
                , U_DTTM
                , SORTER_YN
                , I_DTTM
                , I_USER
                , USE_YN
            FROM PRODUCT
            ";

            return queryText;
        }

        #endregion

        #region :: PRODUCT 테이블 Merge 쿼리
        /// <summary>
        /// PRODUCT 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"
            MERGE INTO PRODUCT d
            USING (
                SELECT
                      :PLANT_CODE        AS PLANT_CODE
                    , :PRODUCT_TYPE      AS PRODUCT_TYPE
                    , :PRODUCT_CODE      AS PRODUCT_CODE
                    , :PRODUCT_NAME      AS PRODUCT_NAME
                    , :UOM               AS UOM
                    , :PRODUCT_GROUP     AS PRODUCT_GROUP
                    , :FEED_FORM         AS FEED_FORM
                    , :MIX_TIME          AS MIX_TIME
                    , :DRY_TIME          AS DRY_TIME
                    , :FINAL_TIME        AS FINAL_TIME
                    , :ERP_ITEM_CODE     AS ERP_ITEM_CODE
                    , :REMARKS           AS REMARKS
                    , :SORTER_YN         AS SORTER_YN
                    , :USE_YN            AS USE_YN
                    , SYSDATE            AS CHANGEDTTM
                    , :USER_ID           AS CHANGEBY
                FROM DUAL
            ) s
            ON (
                   d.PLANT_CODE   = s.PLANT_CODE
               AND d.PRODUCT_TYPE = s.PRODUCT_TYPE
               AND d.PRODUCT_CODE = s.PRODUCT_CODE
            )

            WHEN MATCHED THEN
            UPDATE SET
                  d.PRODUCT_NAME    = s.PRODUCT_NAME
                , d.UOM             = s.UOM
                , d.PRODUCT_GROUP   = s.PRODUCT_GROUP
                , d.FEED_FORM       = s.FEED_FORM
                , d.MIX_TIME        = s.MIX_TIME
                , d.DRY_TIME        = s.DRY_TIME
                , d.FINAL_TIME      = s.FINAL_TIME
                , d.ERP_ITEM_CODE   = s.ERP_ITEM_CODE
                , d.REMARKS         = s.REMARKS
                , d.SORTER_YN       = s.SORTER_YN
                , d.USE_YN          = s.USE_YN
                , d.U_USER          = s.CHANGEBY
                , d.U_DTTM          = s.CHANGEDTTM

            WHEN NOT MATCHED THEN
            INSERT (
                  PLANT_CODE
                , PRODUCT_TYPE
                , PRODUCT_CODE
                , PRODUCT_NAME
                , UOM
                , PRODUCT_GROUP
                , FEED_FORM
                , MIX_TIME
                , DRY_TIME
                , FINAL_TIME
                , ERP_ITEM_CODE
                , REMARKS
                , SORTER_YN
                , USE_YN
                , I_USER
                , I_DTTM
            )
            VALUES (
                  s.PLANT_CODE
                , s.PRODUCT_TYPE
                , s.PRODUCT_CODE
                , s.PRODUCT_NAME
                , s.UOM
                , s.PRODUCT_GROUP
                , s.FEED_FORM
                , s.MIX_TIME
                , s.DRY_TIME
                , s.FINAL_TIME
                , s.ERP_ITEM_CODE
                , s.REMARKS
                , s.SORTER_YN
                , s.USE_YN
                , s.CHANGEBY
                , s.CHANGEDTTM
            )
            ";

            return queryText;
        }
        #endregion

        #region :: PRODUCT 테이블 Delete 쿼리
        /// <summary>
        /// PRODUCT 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM PRODUCT
                            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
