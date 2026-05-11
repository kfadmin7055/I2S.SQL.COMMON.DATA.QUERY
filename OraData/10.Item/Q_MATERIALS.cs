namespace I2S.SQL.COMMON.DATA.OraData.Item
{
    #region :: I2S.SQL.COMMON.DATA.OraData.Base.Q_MATERIALS ::


    /// <summary>
    /// MATERIALS 테이블 쿼리
    /// </summary>
    public static class Q_MATERIALS
    {
        static string queryText = string.Empty;

        #region :: MATERIALS 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
            SELECT PLANT_CODE
                , MATERIAL_TYPE
                , MATERIAL_CODE
                , MATERIAL_NAME
                , UOM
                , MATERIALS
                , FEED_FORM
                , MIX_TIME
                , DRY_TIME
                , FINAL_TIME
                , SORTER_YN
                , REMARKS
                , USE_YN
                , I_DTTM
                , U_DTTM
                , I_USER
                , U_USER
            FROM MATERIALS
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
            MERGE INTO MATERIALS d
            USING (SELECT        :PLANT_CODE as PLANT_CODE
                        ,        :MATERIAL_TYPE as MATERIAL_TYPE
                        ,        :MATERIAL_CODE as MATERIAL_CODE
                        ,        :MATERIAL_NAME as MATERIAL_NAME
                        ,        :UOM as UOM
                        ,        :PRODUCT as PRODUCT
                        ,        :FEED_FORM as FEED_FORM
                        ,        :MIX_TIME as MIX_TIME
                        ,        :DRY_TIME as DRY_TIME
                        ,        :FINAL_TIME as FINAL_TIME
                        ,        :SORTER_YN as SORTER_YN
                        ,        :REMARKS as REMARKS
                        ,        :USE_YN as USE_YN
                        ,        :CHANGEDTTM as CHANGEDTTM
                        ,        :CHANGEUSER as CHANGEUSER
                          FROM DUAL) s
                    ON (d.PLANT_CODE = s.PLANT_CODE
                        AND d.MATERIAL_TYPE = s.MATERIAL_TYPE
                        AND d.MATERIAL_CODE = s.MATERIAL_CODE)
            WHEN MATCHED
            THEN
                UPDATE SET d.MATERIAL_NAME = s.MATERIAL_NAME
                         , d.UOM = s.UOM
                         , d.PRODUCT = s.PRODUCT
                         , d.FEED_FORM = s.FEED_FORM
                         , d.MIX_TIME = s.MIX_TIME
                         , d.DRY_TIME = s.DRY_TIME
                         , d.FINAL_TIME = s.FINAL_TIME
                         , d.SORTER_YN = s.SORTER_YN
                         , d.REMARKS = s.REMARKS
                         , d.USE_YN = s.USE_YN
                         , d.I_USER = s.CHANGEDTTM
                         , d.U_USER = s.CHANGEUSER
            WHEN NOT MATCHED
            THEN
                INSERT     (PLANT_CODE
                          , MATERIAL_TYPE
                          , MATERIAL_CODE
                          , MATERIAL_NAME
                          , UOM
                          , PRODUCT
                          , FEED_FORM
                          , MIX_TIME
                          , DRY_TIME
                          , FINAL_TIME
                          , SORTER_YN
                          , REMARKS
                          , USE_YN
                          , I_DTTM
                          , I_USER)
                    VALUES (
                            s.PLANT_CODE
                          , s.MATERIAL_TYPE
                          , s.MATERIAL_CODE
                          , s.MATERIAL_NAME
                          , s.UOM
                          , s.PRODUCT
                          , s.FEED_FORM
                          , s.MIX_TIME
                          , s.DRY_TIME
                          , s.FINAL_TIME
                          , s.SORTER_YN
                          , s.REMARKS
                          , s.USE_YN
                          , s.CHANGEDTTM
                          , s.CHANGEUSER
                           )
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

            queryText = @"DELETE FROM MATERIALS
                            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
