CREATE OR REPLACE FUNCTION SJ_GET_RC_ROUTE_PROCESS (
    V_ROUTE_ID IN VARCHAR2
) RETURN RC_ROUTE_PROCESS
    PIPELINED
AS

    V_TAB              RC_ROUTE_PROCESS := RC_ROUTE_PROCESS();
    C_SORTINDEX        NUMBER;

    CURSOR ROUTE_DETAILS IS
    WITH ROUTE_NODES AS (
        SELECT
            ROWNUM IDX,
            ROUTE_ID,
            NODE_CONTENT,
            NODE_ID
        FROM
            SAJET.SYS_RC_ROUTE_DETAIL
        START WITH ROUTE_ID = V_ROUTE_ID
                   AND NODE_CONTENT = 'START' CONNECT BY PRIOR NEXT_NODE_ID = NODE_ID
                                                         OR PRIOR NEXT_NODE_ID = GROUP_ID
    )
    SELECT
        NVL(TRIM(B.ROUTE_NAME), '0')    ROUTE_NAME,
        B.ROUTE_ID,
        NVL(TRIM(A.PROCESS_CODE), '0')  PROCESS_CODE,
        TRIM(A.PROCESS_NAME)            PROCESS_NAME,
        A.PROCESS_ID
    FROM
        SAJET.SYS_PROCESS    A,
        SAJET.SYS_RC_ROUTE   B,
        ROUTE_NODES          C
    WHERE
        B.ROUTE_ID = C.ROUTE_ID
        AND TO_CHAR(A.PROCESS_ID) = C.NODE_CONTENT
        AND A.ENABLED = 'Y'
        AND B.ENABLED = 'Y'
    ORDER BY
        C.IDX;

    ROUTE_DETAIL_ROW   ROUTE_DETAILS%ROWTYPE;

BEGIN
    C_SORTINDEX := 0;

    OPEN ROUTE_DETAILS;

    LOOP
        FETCH ROUTE_DETAILS INTO ROUTE_DETAIL_ROW;

        EXIT WHEN ROUTE_DETAILS%NOTFOUND;

        PIPE ROW ( TYPE_RC_ROUTE_PROCESS(ROUTE_DETAIL_ROW.ROUTE_ID,
                                         ROUTE_DETAIL_ROW.PROCESS_ID,
                                         ROUTE_DETAIL_ROW.PROCESS_NAME,
                                         C_SORTINDEX,
                                         ROUTE_DETAIL_ROW.PROCESS_CODE) );

        C_SORTINDEX := C_SORTINDEX + 1;

    END LOOP;

    RETURN;

END;