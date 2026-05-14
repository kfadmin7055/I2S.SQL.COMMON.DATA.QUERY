using System.Data;
using System.Xml;

namespace I2S.SQL.COMMON.DATA.OraData.Item
{
    #region :: I2S.SQL.COMMON.DATA.OraData.Item.Q_MIXING_RECIPE ::


    /// <summary>
    /// MIXING_RECIPE 테이블 쿼리
    /// </summary>
    public static class Q_MIXING_RECIPE
    {
        static string queryText = string.Empty;

        #region :: MIXING_RECIPE 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
            SELECT mr.PLANT_CODE
                , mr.PRODUCT_CODE
                , p.PRODUCT_NAME      
                , mr.RECIPE_VER
                , mr.VALID_FROM_DATE
                , mr.VALIE_TO_DATE
                , mr.MIX_STAGE
                , mr.CONFIRM_YN
                , mr.INITBY
                , mr.UPBY
                , mr.UPDTTM
                , mr.INITDTTM
            FROM MIXING_RECIPE mr
                JOIN PRODUCT p ON p.PLANT_CODE = mr.PLANT_CODE
                                AND p.PRODUCT_CODE = mr.PRODUCT_CODE
            WHERE mr.PLANT_CODE = :PLANT_CODE
                AND (:PRODUCT_CODE IS NULL OR mr.PRODUCT_CODE = :PRODUCT_CODE)
                AND (:RECIPE_VER IS NULL OR mr.RECIPE_VER = :RECIPE_VER)
                AND (:CONFIRM_YN IS NULL OR mr.CONFIRM_YN = :CONFIRM_YN)
            ";

            return queryText;
        }

        #endregion

        #region :: MIXING_RECIPE 테이블 Merge 쿼리
        /// <summary>
        /// MIXING_RECIPE 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
            MERGE INTO MIXING_RECIPE d
                    USING (SELECT        :PLANT_CODE AS PLANT_CODE
                                ,        :PRODUCT_CODE AS PRODUCT_CODE
                                ,        :RECIPE_VER AS RECIPE_VER
                                ,        :VALID_FROM_DATE AS VALID_FROM_DATE
                                ,        :VALIE_TO_DATE AS VALIE_TO_DATE
                                ,        :MIX_STAGE AS MIX_STAGE
                                ,        :CONFIRM_YN AS CONFIRM_YN
                                ,        :CHANGEDTTM AS CHANGEDTTM
                                ,        :CHANGEBY AS CHANGEBY
                            FROM DUAL) s
                    ON (d.PLANT_CODE = s.PLANT_CODE
                        AND d.PRODUCT_CODE = s.PRODUCT_CODE
                        AND d.RECIPE_VER = s.RECIPE_VER)
            WHEN MATCHED
            THEN
                UPDATE SET d.VALID_FROM_DATE = s.VALID_FROM_DATE
                            , d.VALIE_TO_DATE = s.VALIE_TO_DATE
                            , d.MIX_STAGE = s.MIX_STAGE
                            , d.CONFIRM_YN = s.CONFIRM_YN
                            , d.UPDTTM = s.CHANGEDTTM
                            , d.UPBY = s.CHANGEBY
            WHEN NOT MATCHED
            THEN
                INSERT     (PLANT_CODE
                            , PRODUCT_CODE
                            , RECIPE_VER
                            , VALID_FROM_DATE
                            , VALIE_TO_DATE
                            , MIX_STAGE
                            , CONFIRM_YN
                            , INITDTTM
                            , INITBY)
                    VALUES (s.PLANT_CODE
                            , s.PRODUCT_CODE
                            , s.RECIPE_VER
                            , s.VALID_FROM_DATE
                            , s.VALIE_TO_DATE
                            , s.MIX_STAGE
                            , s.CONFIRM_YN
                            , s.CHANGEDTTM
                            , s.CHANGEBY)
            ";

            return queryText;
        }
        #endregion

        #region :: MIXING_RECIPE 테이블 Delete 쿼리
        /// <summary>
        /// MIXING_RECIPE 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
            DELETE FROM MIXING_RECIPE
            WHERE PLANT_CODE = :PLANT_CODE
                AND PRODUCT_CODE = :PRODUCT_CODE
            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
