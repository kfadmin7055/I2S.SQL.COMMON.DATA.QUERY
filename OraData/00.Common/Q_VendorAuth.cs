namespace I2S.SQL.COMMON.DATA.OraData.Common
{
    #region :: EBAP.UI.ADM._0.Query.Q_VendorAuth ::


    /// <summary>
    /// TEMPTABLE 테이블 쿼리
    /// </summary>
    public static class Q_VendorAuth
    {
        static string queryText = string.Empty;

        #region :: TEMPTABLE 테이블 Select 쿼리

        public static string SelectQuery(string reference)
        {
            queryText = string.Empty;

            queryText = $@"/* {reference} */
                        SELECT A.VENDORCODE
	                         , A.USERID
	                         , B.USERNAME
	                         , B.DEPTNAME
	                         , A.EXPIREDATE
	                         , A.ISDELEGATE
	                         , NVL(A.UPDTTM, A.INITDTTM) CHANGEDTTM 
	                         , NVL(A.UPBY, A.INITBY) CHANGEBY 
                        FROM VendorAuth A 
                        JOIN UserInfo B
                        ON A.USERID = B.USERID
                        WHERE A.VENDORCODE = :VENDORCODE
                        AND B.USERNAME LIKE :USERNAME || '%'
                        ";

            return queryText;
        }

        public static string VendorList_Select()
        {
            queryText = string.Empty;

            queryText = @"/* VendorList_Select() */
                        SELECT A.CODEVALUE
	                         , A.DISPLAYVALUE
	                         , NVL(A.UPDTTM, A.INITDTTM) CHANGEDTTM 
	                         , NVL(A.UPBY, A.INITBY) CHANGEBY 
                        FROM CodeMaster A 
                        WHERE 1 = 1
                            AND A.USEFLAG = 'Y'
                            AND A.PCODEVALUE = 'C$VENDORCODE'
                            AND A.CODEVALUE LIKE :CODEVALUE || '%'
                            AND A.DISPLAYVALUE LIKE  :DISPLAYVALUE || '%'
                            AND A.CODEVALUE IN ( SELECT VENDORCODE FROM VendorAuth WHERE USERID = :USERID AND ISDELEGATE = 'Y' )
                        ORDER BY A.IDX
                        ";

            return queryText;
        }

        #endregion

        #region :: TEMPTABLE 테이블 Usert 쿼리
        /// <summary>
        /// TEMPTABLE 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Merge()
        {
            queryText = string.Empty;

            queryText = @"MERGE INTO KFMETA.VENDORAUTH d
                        USING (
                          Select
                            :VENDORCODE          as VENDORCODE,
                            :USERID              as USERID,
                            :EXPIREDATE          as EXPIREDATE,
                            :ISDELEGATE          as ISDELEGATE,
                            SYSDATE              as CHANGEDTTM,
                            :CHANGEBY            as CHANGEBY
                          From Dual) s
                        ON
                          (d.VENDORCODE = s.VENDORCODE and 
                          d.USERID = s.USERID )
                        WHEN MATCHED
                        THEN
                        UPDATE SET
                          d.EXPIREDATE = s.EXPIREDATE,
                          d.ISDELEGATE = s.ISDELEGATE,
                          d.UPDTTM = s.CHANGEDTTM,
                          d.UPBY = s.CHANGEBY
                        WHEN NOT MATCHED
                        THEN
                        INSERT (
                          VENDORCODE, USERID, EXPIREDATE,
                          ISDELEGATE, INITDTTM, INITBY)
                        VALUES (
                          s.VENDORCODE, s.USERID, s.EXPIREDATE,
                          s.ISDELEGATE, s.CHANGEDTTM, s.CHANGEBY)
                        ";

            return queryText;
        }

        /// <summary>
        /// TEMPTABLE 테이블 머지 쿼리
        /// </summary>
        /// <returns></returns>
        public static string AuthGroupUserInsert()
        {
            queryText = string.Empty;

            queryText = @"
                        DECLARE
                            v_count NUMBER;
                        BEGIN
                            -- @ISDELEGATE가 'Y'인지 확인
                            IF :ISDELEGATE = 'Y' THEN
                                -- AuthGroupUser 테이블에 조건에 맞는 레코드가 존재하는지 확인
                                SELECT COUNT(1)
                                INTO v_count
                                FROM AuthGroupUser
                                WHERE GROUPCODE = 'AUTH001' AND USERID = :USERID;
        
                                -- 레코드가 존재하지 않으면 새 레코드 삽입
                                IF v_count = 0 THEN
                                    INSERT INTO AuthGroupUser (GROUPCODE, USERID, EXPIREDATE, ISDELEGATE, INITDTTM, INITBY)
                                    VALUES (
                                        'AUTH001',
                                        :USERID,
                                        TO_DATE(:EXPIREDATE, 'YYYY-MM-DD'),
                                        CASE WHEN :USERID = 'system' THEN 'Y' ELSE 'N' END,
                                        SYSDATE,
                                        :CHANGEBY
                                    );
                                END IF;
                            END IF;
                        END;
                        ";

            return queryText;
        }
        #endregion

        #region :: TEMPTABLE 테이블 Delete 쿼리
        /// <summary>
        /// TEMPTABLE 테이블 삭제 쿼리
        /// </summary>
        /// <returns></returns>
        public static string Delete()
        {
            queryText = string.Empty;

            queryText = @"DELETE FROM dbo.VendorAuth
		                WHERE VENDORCODE = :VENDORCODE AND USERID = :USERID
                            ";
            return queryText;
        }
        #endregion
    }

    #endregion
}
