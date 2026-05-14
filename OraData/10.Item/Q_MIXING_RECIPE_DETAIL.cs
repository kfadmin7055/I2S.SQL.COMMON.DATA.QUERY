using System.Data;
using System.Xml;

namespace I2S.SQL.COMMON.DATA.OraData.Item
{
    #region :: I2S.SQL.COMMON.DATA.OraData.Item.Q_MIXING_RECIPE_DETAIL ::


    /// <summary>
    /// MIXING_RECIPE_DETAIL 테이블 쿼리
    /// </summary>
    public static class Q_MIXING_RECIPE_DETAIL
    {
        static string queryText = string.Empty;

        #region :: MIXING_RECIPE_DETAIL 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
            SELECT PLANT_CODE
                , PRODUCT_CODE
                , RECIPE_VER
                , MATERIAL_CODE
                , MIX_MATERIAL_UOM
                , VALID_FROM_DATE
                , VALIE_TO_DATE
                , RATIO
                , SORT_SEQ
                , INITBY
                , UPBY
                , UPDTTM
                , INITDTTM
            FROM MIXING_RECIPE_DETAIL
            WHERE PLANT_CODE = :PLANT_CODE
                AND PRODUCT_CODE = :PRODUCT_CODE
                AND RECIPE_VER = :RECIPE_VER
            ";

            return queryText;
        }

        #endregion

        #region :: MIXING_RECIPE_DETAIL 테이블 Merge 쿼리
        /// <summary>
        /// MIXING_RECIPE_DETAIL 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
            MERGE INTO MIXING_RECIPE_DETAIL d
                    USING (SELECT        :PLANT_CODE AS PLANT_CODE
                                ,        :PRODUCT_CODE AS PRODUCT_CODE
                                ,        :RECIPE_VER AS RECIPE_VER
                                ,        :MATERIAL_CODE AS MATERIAL_CODE
                                ,        :MIX_MATERIAL_UOM AS MIX_MATERIAL_UOM
                                ,        :VALID_FROM_DATE AS VALID_FROM_DATE
                                ,        :VALIE_TO_DATE AS VALIE_TO_DATE
                                ,        :RATIO AS RATIO
                                ,        :SORT_SEQ AS SORT_SEQ
                                ,        :CHANGEDTTM AS CHANGEDTTM
                                ,        :CHANGEBY AS CHANGEBY
                            FROM DUAL) s
                    ON (d.PLANT_CODE = s.PLANT_CODE
                        AND d.PRODUCT_CODE = s.PRODUCT_CODE
                        AND d.RECIPE_VER = s.RECIPE_VER
                        AND d.MATERIAL_CODE = s.MATERIAL_CODE)
            WHEN MATCHED
            THEN
                UPDATE SET d.MIX_MATERIAL_UOM = s.MIX_MATERIAL_UOM
                            , d.VALID_FROM_DATE = s.VALID_FROM_DATE
                            , d.VALIE_TO_DATE = s.VALIE_TO_DATE
                            , d.RATIO = s.RATIO
                            , d.SORT_SEQ = s.SORT_SEQ
                            , d.UPDTTM = s.CHANGEDTTM
                            , d.UPBY = s.CHANGEBY
            WHEN NOT MATCHED
            THEN
                INSERT     (PLANT_CODE
                            , PRODUCT_CODE
                            , RECIPE_VER
                            , MATERIAL_CODE
                            , MIX_MATERIAL_UOM
                            , VALID_FROM_DATE
                            , VALIE_TO_DATE
                            , RATIO
                            , SORT_SEQ
                            , INITDTTM
                            , INITBY)
                    VALUES (s.PLANT_CODE
                            , s.PRODUCT_CODE
                            , s.RECIPE_VER
                            , s.MATERIAL_CODE
                            , s.MIX_MATERIAL_UOM
                            , s.VALID_FROM_DATE
                            , s.VALIE_TO_DATE
                            , s.RATIO
                            , s.SORT_SEQ
                            , s.CHANGEDTTM
                            , s.CHANGEBY)
            ";

            return queryText;
        }
        #endregion

        #region :: MIXING_RECIPE_DETAIL 테이블 Delete 쿼리
        /// <summary>
        /// MIXING_RECIPE_DETAIL 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
            DELETE FROM MIXING_RECIPE_DETAIL
            WHERE PLANT_CODE = :PLANT_CODE
                AND PRODUCT_CODE = :PRODUCT_CODE
                AND RECIPE_VER = :RECIPE_VER
                AND (:MATERIAL_CODE IS NULL OR MATERIAL_CODE = :MATERIAL_CODE)
            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
