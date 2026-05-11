
using I2S.SQL.COMMON.DATA.OraData;
using I2S.SQL.COMMON.DATA.OraData.Common;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace I2S.SQL.COMMON.DATA.OraData.Dosing
{
    #region :: I2S.SQL.COMMON.DATA.OraData.Dosing.Q_Bin ::


    /// <summary>
    /// BIN 테이블 쿼리
    /// </summary>
    public static class Q_Bin
    {
        static string queryText;

        #region :: BIN 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                           SELECT b.PROCESS_NAME,
                           a.BIN_CODE, a.BIN_NAME, a.BIN_SERIAL, 
                           a.RESOURCE_NO, a.SCALE_CODE, a.HI_Q, 
                           a.LO_Q, a.MAX_CAPA, a.FAIL, 
                           a.HZ_01, a.HZ_02, a.M_RATE, 
                           a.M_ON, a.M_OFF, a.ROW_RATE, 
                           a.S_ON, a.S_OFF, a.HZ_03, 
                           a.HL_ERROR, a.B_STOCK, a.STOCK, 
                           a.JOG_ON_TIME, a.DROP_SAFE_T, a.BAG_FAIL, 
                           a.BAG_ROW_RATE, a.BAG_S_ON, a.BAG_S_OFF, 
                           a.PROCESS_KEY, a.SEQ, a.BIN_GUBUN, 
                           a.PLC_ADDRESS, a.ERP_BIN_CODE, a.USE_YN
                            , NVL(A.UPDTTM, A.INITDTTM) CHANGEDTTM
	                        , NVL(A.UPBY, A.INITBY) CHANGEBY
                        FROM BIN a
                            INNER JOIN PROCESS_DIVISION b ON b.PROCESS_CODE = a.PROCESS_KEY
                        WHERE (( :PROCESS_KEY IS NULL) OR (a.PROCESS_KEY = :PROCESS_KEY ))
                            --AND (( :BIN_CODE IS NULL) OR (a.BIN_CODE = :BIN_CODE ))
                        ORDER BY a.SCALE_CODE, a.BIN_SERIAL
                        ";

            return queryText;
        }

        /// <summary>
        /// 빈 우선 순위 변경 조회
        /// </summary>
        /// <returns></returns>
        public static string Sel_BinPriorityChange()
        {
            queryText = string.Empty;

            queryText = @"/* Sel_BinPriorityChange() */
                        WITH RESOURCE AS 
                            SELECT A.RESOURCE_NO
                                 FROM BIN A, SCALE B
                                 WHERE A.SCALE_CODE = a.SCALE_CODE
                                 AND A.PROCESS_KEY = :PROCESS_KEY
                             GROUP BY A.RESOURCE_NO
                             HAVING COUNT(A.RESOURCE_NO) >= 2
                            )

                         SELECT a.SCALE_CODE, b.RESOURCE_NO, b.DESCRIPTION ,a.BIN_CODE, a.SEQ
                         FROM BIN a
                            JOIN ERP_DBLINK.{clsCommon.erp_dosing_db_name}.dbo.V_MES_ATG_101_1 b ON a.RESOURCE_NO = b.RESOURCE_NO
                         WHERE a.RESOURCE_NO IN (SELECT RESOURCE_NO FROM RESOURCE)
                         AND  a.PROCESS_KEY = :PROCESS_KEY
                         ORDER BY a.SCALE_CODE,  erp_ing.RESOURCE_NO, a.BIN_CODE, a.SEQ
                            ";

            return queryText;
        }

        public static string GetLOCATION_List()
        {
            queryText = string.Empty;

            queryText = $@"/* GetLOCATIONList() */
                        SELECT a.LOCATION
                        FROM BIN a
                        WHERE a.PROCESS_KEY IN ({Q_CodeMaster.CodeMaster_GetRef("REF1", "AT$EXCEPTIONPROCESS", "LOCATION")})
                        ORDER BY a.LOCATION
                        ";

            return queryText;
        }

        public static string GetLOCATION()
        {
            queryText = string.Empty;

            queryText = $@"/* GetLOCATION() */
                        SELECT a.LOCATION
                        FROM BIN a
                        WHERE a.LOCATION = :LOCATION
                        ORDER BY a.LOCATION
                        ";

            return queryText;
        }
        #endregion

        #region :: BIN 테이블 Merge 쿼리
        /// <summary>
        /// BIN 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"MERGE INTO KFATOCHANG.BIN d
                        USING (
                          Select
                            :BIN_CODE       AS BIN_CODE,
                            :BIN_NAME       AS BIN_NAME,
                            :BIN_SERIAL     AS BIN_SERIAL,
                            :RESOURCE_NO    AS RESOURCE_NO,
                            :SCALE_CODE     AS SCALE_CODE,
                            :HI_Q           AS HI_Q,
                            :LO_Q           AS LO_Q,
                            :MAX_CAPA       AS MAX_CAPA,
                            :FAIL           AS FAIL,
                            :HZ_01          AS HZ_01,
                            :HZ_02          AS HZ_02,
                            :M_RATE         AS M_RATE,
                            :M_ON           AS M_ON,
                            :M_OFF          AS M_OFF,
                            :ROW_RATE       AS ROW_RATE,
                            :S_ON           AS S_ON,
                            :S_OFF          AS S_OFF,
                            :HZ_03          AS HZ_03,
                            :HL_ERROR       AS HL_ERROR,
                            :B_STOCK        AS B_STOCK,
                            (SELECT NVL(BIN_CODE, :STOCK) FROM BIN WHERE BIN_CODE = :BIN_CODE) AS STOCK,
                            :JOG_ON_TIME    AS JOG_ON_TIME,
                            :DROP_SAFE_T    AS DROP_SAFE_T,
                            :BAG_FAIL       AS BAG_FAIL,
                            :BAG_ROW_RATE   AS BAG_ROW_RATE,
                            :BAG_S_ON       AS BAG_S_ON,
                            :BAG_S_OFF      AS BAG_S_OFF,
                            :PROCESS_KEY    AS PROCESS_KEY,
                            :SEQ            AS SEQ,
                            :BIN_GUBUN      AS BIN_GUBUN,
                            :PLC_ADDRESS    AS PLC_ADDRESS,
                            :ERP_BIN_CODE   AS ERP_BIN_CODE,
                            :USE_YN         AS USE_YN,
                            SYSDATE         as CHANGEDTTM,
                            :CHANGEBY       as CHANGEBY
                          From Dual) s
                        ON
                          (d.BIN_CODE = s.BIN_CODE )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.BIN_NAME = s.BIN_NAME,
                          d.BIN_SERIAL = s.BIN_SERIAL,
                          d.RESOURCE_NO = s.RESOURCE_NO,
                          d.SCALE_CODE = s.SCALE_CODE,
                          d.HI_Q = s.HI_Q,
                          d.LO_Q = s.LO_Q,
                          d.MAX_CAPA = s.MAX_CAPA,
                          d.FAIL = s.FAIL,
                          d.HZ_01 = s.HZ_01,
                          d.HZ_02 = s.HZ_02,
                          d.M_RATE = s.M_RATE,
                          d.M_ON = s.M_ON,
                          d.M_OFF = s.M_OFF,
                          d.ROW_RATE = s.ROW_RATE,
                          d.S_ON = s.S_ON,
                          d.S_OFF = s.S_OFF,
                          d.HZ_03 = s.HZ_03,
                          d.HL_ERROR = s.HL_ERROR,
                          d.B_STOCK = s.B_STOCK,
                          d.STOCK = s.STOCK,
                          d.JOG_ON_TIME = s.JOG_ON_TIME,
                          d.DROP_SAFE_T = s.DROP_SAFE_T,
                          d.BAG_FAIL = s.BAG_FAIL,
                          d.BAG_ROW_RATE = s.BAG_ROW_RATE,
                          d.BAG_S_ON = s.BAG_S_ON,
                          d.BAG_S_OFF = s.BAG_S_OFF,
                          d.PROCESS_KEY = s.PROCESS_KEY,
                          d.SEQ = s.SEQ,
                          d.BIN_GUBUN = s.BIN_GUBUN,
                          d.PLC_ADDRESS = s.PLC_ADDRESS,
                          d.ERP_BIN_CODE = s.ERP_BIN_CODE,
                          d.USE_YN = s.USE_YN,
                          d.UPDTTM = s.CHANGEDTTM,
                          d.UPBY = s.CHANGEBY
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          BIN_CODE, BIN_NAME, BIN_SERIAL,
                          RESOURCE_NO, SCALE_CODE, HI_Q,
                          LO_Q, MAX_CAPA, FAIL,
                          HZ_01, HZ_02, M_RATE,
                          M_ON, M_OFF, ROW_RATE,
                          S_ON, S_OFF, HZ_03,
                          HL_ERROR, B_STOCK, STOCK,
                          JOG_ON_TIME, DROP_SAFE_T, BAG_FAIL,
                          BAG_ROW_RATE, BAG_S_ON, BAG_S_OFF,
                          PROCESS_KEY, SEQ, BIN_GUBUN,
                          PLC_ADDRESS, ERP_BIN_CODE, USE_YN,
                          INITDTTM, INITBY)
                        VALUES (
                          s.BIN_CODE, s.BIN_NAME, s.BIN_SERIAL,
                          s.RESOURCE_NO, s.SCALE_CODE, s.HI_Q,
                          s.LO_Q, s.MAX_CAPA, s.FAIL,
                          s.HZ_01, s.HZ_02, s.M_RATE,
                          s.M_ON, s.M_OFF, s.ROW_RATE,
                          s.S_ON, s.S_OFF, s.HZ_03,
                          s.HL_ERROR, s.B_STOCK, s.STOCK,
                          s.JOG_ON_TIME, s.DROP_SAFE_T, s.BAG_FAIL,
                          s.BAG_ROW_RATE, s.BAG_S_ON, s.BAG_S_OFF,
                          s.PROCESS_KEY, s.SEQ, s.BIN_GUBUN,
                          s.PLC_ADDRESS, s.ERP_BIN_CODE, s.USE_YN,
                          s.CHANGETTM, s.CHANGEBY)
                        ";

            return queryText;
        }

        /// <summary>
        /// BIN 테이블의 SEQ 컬럼을 업데이트 한다
        /// </summary>
        /// <returns></returns>
        public static string UpdateBinSEQ(string PROCESS_KEY)
        {
            queryText = string.Empty;

            queryText = $@"UPDATE BIN SET
                            SEQ = (SELECT NVL(SEQ, 0) 
                                FROM BIN 
                                WHERE PROCESS_KEY = '{PROCESS_KEY}' AND SCALE_CODE = :SCALE_CODE 
                                    AND RESOURCE_NO = :RESOURCE_NO
                                )
                        WHERE BIN_CODE = :BIN_CODE";

            return queryText;
        }

        /// <summary>
        /// 스케일에 원료가 1개인 빈은 순번 1로 조정
        /// </summary>
        /// <returns></returns>
        public static string UpdateFirstBin()
        {
            queryText = string.Empty;

            queryText = $@"UPDATE BIN SET SEQ = 1 WHERE RESOURCE_NO IN
                        (
                        SELECT a.RESOURCE_NO 
                        FROM (
                                SELECT  RESOURCE_NO FROM BIN
                                WHERE SCALE_CODE IS NOT NULL
                                    AND PROCESS_KEY IN (({Q_CodeMaster.GetRefValue("REF1", "ExceptionProcess", "Dosing")})) AND BIN_CODE NOT IN (({Q_CodeMaster.GetCodeValue("ExceptionBin")}))
                                GROUP BY RESOURCE_NO
                                HAVING  COUNT(BIN_CODE) = 1 AND MAX(SEQ) > 1
                            ) a
                        ) ";

            return queryText;
        }
        #endregion

        #region :: BIN 테이블 Delete 쿼리
        /// <summary>
        /// BIN 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM BIN
                            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
