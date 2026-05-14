using System.Data;
using System.Xml;

namespace I2S.SQL.COMMON.DATA.OraData.Item
{
    #region :: I2S.SQL.COMMON.DATA.OraData.Base.Q_MATERIAL ::


    /// <summary>
    /// MATERIALS 테이블 쿼리
    /// </summary>
    public static class Q_MATERIAL
    {
        static string queryText = string.Empty;

        #region :: MATERIALS 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"
            /* {reference} */
            SELECT PLANT_CODE
                , MATERIAL_CODE
                , MATERIAL_NAME
                , WEIGHING_TYPE
                , MATERIAL_TYPE
                , INITBY
                , UPBY
                , UPDTTM
                , INITDTTM
                , USEFLAG
                , REMARKS
            FROM MATERIAL
            WHERE PLANT_CODE = :PLANT_CODE
                AND (:MATERIAL_TYPE IS NULL OR MATERIAL_TYPE = :MATERIAL_TYPE)
                AND MATERIAL_NAME LIKE '%' || :MATERIAL_NAME || '%'
                AND MATERIAL_CODE LIKE '%' || :MATERIAL_CODE || '%'
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
            MERGE INTO MATERIAL d
                    USING (SELECT        :PLANT_CODE AS PLANT_CODE
                                ,        :MATERIAL_CODE AS MATERIAL_CODE
                                ,        :MATERIAL_NAME AS MATERIAL_NAME
                                ,        :WEIGHING_TYPE AS WEIGHING_TYPE
                                ,        :MATERIAL_TYPE AS MATERIAL_TYPE
                                ,        SYSDATE AS CHANGEDTTM
                                ,        :CHANGEBY AS CHANGEBY
                                ,        :USEFLAG AS USEFLAG
                                ,        :REMARKS AS REMARKS
                            FROM DUAL) s
                    ON (d.MATERIAL_CODE = s.MATERIAL_CODE)
            WHEN MATCHED
            THEN
                UPDATE SET d.PLANT_CODE = s.PLANT_CODE
                            , d.MATERIAL_NAME = s.MATERIAL_NAME
                            , d.WEIGHING_TYPE = s.WEIGHING_TYPE
                            , d.MATERIAL_TYPE = s.MATERIAL_TYPE
                            , d.UPDTTM = s.CHANGEDTTM
                            , d.UPBY = s.CHANGEBY
                            , d.USEFLAG = s.USEFLAG
                            , d.REMARKS = s.REMARKS
            WHEN NOT MATCHED
            THEN
                INSERT     (PLANT_CODE
                            , MATERIAL_CODE
                            , MATERIAL_NAME
                            , WEIGHING_TYPE
                            , MATERIAL_TYPE
                            , INITDTTM
                            , INITBY
                            , USEFLAG
                            , REMARKS)
                    VALUES (s.PLANT_CODE
                            , s.MATERIAL_CODE
                            , s.MATERIAL_NAME
                            , s.WEIGHING_TYPE
                            , s.MATERIAL_TYPE
                            , s.CHANGEDTTM
                            , s.CHANGEBY
                            , s.USEFLAG
                            , s.REMARKS)
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
            DELETE FROM MATERIAL
            WHERE PLANT_CODE = :PLANT_CODE
                AND MATERIAL_CODE = :MATERIAL_CODE
            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
