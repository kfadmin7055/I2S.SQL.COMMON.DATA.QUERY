using System.Data;
using System.Xml;

namespace I2S.SQL.COMMON.DATA.OraData.Item
{
    #region :: I2S.SQL.COMMON.DATA.OraData.Base.Q_PRODUCT ::


    /// <summary>
    /// MATERIALS 테이블 쿼리
    /// </summary>
    public static class Q_PRODUCT
    {
        static string queryText = string.Empty;

        #region :: MATERIALS 테이블 Select 쿼리

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
                , SORTER_YN
                , REMARKS
                , USEFLAG
                , INITBY
                , NVL(UPBY, INITBY) AS CHANGEBY
                , NVL(UPDTTM, INITDTTM) AS INITDTTM
            FROM PRODUCT
            WHERE PLANT_CODE = :PLANT_CODE
                AND (:PRODUCT_TYPE IS NULL OR PRODUCT_TYPE = :PRODUCT_TYPE)
                AND PRODUCT_NAME LIKE '%' || :PRODUCT_NAME || '%'
                AND PRODUCT_CODE LIKE '%' || :PRODUCT_CODE || '%'
            ";

            return queryText;
        }

        #endregion

        #region :: MATERIALS 테이블 Merge 쿼리
        /// <summary>
        /// MATERIALS 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"
            MERGE INTO PRODUCT d
                    USING (SELECT        :PLANT_CODE AS PLANT_CODE
                                ,        :PRODUCT_TYPE AS PRODUCT_TYPE
                                ,        :PRODUCT_CODE AS PRODUCT_CODE
                                ,        :PRODUCT_NAME AS PRODUCT_NAME
                                ,        :UOM AS UOM
                                ,        :PRODUCT_GROUP AS PRODUCT_GROUP
                                ,        :FEED_FORM AS FEED_FORM
                                ,        :MIX_TIME AS MIX_TIME
                                ,        :DRY_TIME AS DRY_TIME
                                ,        :FINAL_TIME AS FINAL_TIME
                                ,        :SORTER_YN AS SORTER_YN
                                ,        :REMARKS AS REMARKS
                                ,        :USEFLAG AS USEFLAG
                                ,        SYSDATE AS CHANGEDTTM
                                ,        :CHANGEBY AS CHANGEBY
                            FROM DUAL) s
                    ON (d.PLANT_CODE = s.PLANT_CODE
                        AND d.PRODUCT_TYPE = s.PRODUCT_TYPE
                        AND d.PRODUCT_CODE = s.PRODUCT_CODE)
            WHEN MATCHED
            THEN
                UPDATE SET d.PRODUCT_NAME = s.PRODUCT_NAME
                            , d.UOM = s.UOM
                            , d.PRODUCT_GROUP = s.PRODUCT_GROUP
                            , d.FEED_FORM = s.FEED_FORM
                            , d.MIX_TIME = s.MIX_TIME
                            , d.DRY_TIME = s.DRY_TIME
                            , d.FINAL_TIME = s.FINAL_TIME
                            , d.SORTER_YN = s.SORTER_YN
                            , d.REMARKS = s.REMARKS
                            , d.USEFLAG = s.USEFLAG
                            , d.UPDTTM = s.CHANGEDTTM
                            , d.UPBY = s.CHANGEBY
            WHEN NOT MATCHED
            THEN
                INSERT     (PLANT_CODE
                            , PRODUCT_TYPE
                            , PRODUCT_CODE
                            , PRODUCT_NAME
                            , UOM
                            , PRODUCT_GROUP
                            , FEED_FORM
                            , MIX_TIME
                            , DRY_TIME
                            , FINAL_TIME
                            , SORTER_YN
                            , REMARKS
                            , USEFLAG
                            , INITDTTM
                            , INITBY)
                    VALUES (s.PLANT_CODE
                            , s.PRODUCT_TYPE
                            , s.PRODUCT_CODE
                            , s.PRODUCT_NAME
                            , s.UOM
                            , s.PRODUCT_GROUP
                            , s.FEED_FORM
                            , s.MIX_TIME
                            , s.DRY_TIME
                            , s.FINAL_TIME
                            , s.SORTER_YN
                            , s.REMARKS
                            , s.USEFLAG
                            , s.CHANGEDTTM
                            , s.CHANGEBY)
            ";

            return queryText;
        }
        #endregion

        #region :: MATERIALS 테이블 Delete 쿼리
        /// <summary>
        /// MATERIALS 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"
            DELETE FROM PRODUCT
            WHERE PLANT_CODE = :PLANT_CODE
                AND PRODUCT_CODE = :PRODUCT_CODE
            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
