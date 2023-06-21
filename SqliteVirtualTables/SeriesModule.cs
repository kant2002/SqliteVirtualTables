using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SqliteVirtualTables;

unsafe class SeriesModule : IDisposable
{
    /*
    ** CAPI3REF: Result Codes
    ** KEYWORDS: {result code definitions}
    **
    ** Many SQLite functions return an integer result code from the set shown
    ** here in order to indicate success or failure.
    **
    ** New error codes may be added in future versions of SQLite.
    **
    ** See also: [extended result code definitions]
    */
    const int SQLITE_OK = 0;   /* Successful result */
    /* beginning-of-error-codes */
    const int SQLITE_ERROR = 1;   /* Generic error */
    const int SQLITE_INTERNAL = 2;   /* Internal logic error in SQLite */
    const int SQLITE_PERM = 3;   /* Access permission denied */
    const int SQLITE_ABORT = 4;   /* Callback routine requested an abort */
    const int SQLITE_BUSY = 5;   /* The database file is locked */
    const int SQLITE_LOCKED = 6;   /* A table in the database is locked */
    const int SQLITE_NOMEM = 7;   /* A malloc() failed */
    const int SQLITE_READONLY = 8;   /* Attempt to write a readonly database */
    const int SQLITE_INTERRUPT = 9;   /* Operation terminated by sqlite3_interrupt()*/
    const int SQLITE_IOERR = 10;   /* Some kind of disk I/O error occurred */
    const int SQLITE_CORRUPT = 11;   /* The database disk image is malformed */
    const int SQLITE_NOTFOUND = 12;   /* Unknown opcode in sqlite3_file_control() */
    const int SQLITE_FULL = 13;   /* Insertion failed because database is full */
    const int SQLITE_CANTOPEN = 14;   /* Unable to open the database file */
    const int SQLITE_PROTOCOL = 15;   /* Database lock protocol error */
    const int SQLITE_EMPTY = 16;   /* Internal use only */
    const int SQLITE_SCHEMA = 17;   /* The database schema changed */
    const int SQLITE_TOOBIG = 18;   /* String or BLOB exceeds size limit */
    const int SQLITE_CONSTRAINT = 19;   /* Abort due to constraint violation */
    const int SQLITE_MISMATCH = 20;   /* Data type mismatch */
    const int SQLITE_MISUSE = 21;   /* Library used incorrectly */
    const int SQLITE_NOLFS = 22;   /* Uses OS features not supported on host */
    const int SQLITE_AUTH = 23;   /* Authorization denied */
    const int SQLITE_FORMAT = 24;   /* Not used */
    const int SQLITE_RANGE = 25;   /* 2nd parameter to sqlite3_bind out of range */
    const int SQLITE_NOTADB = 26;   /* File opened that is not a database file */
    const int SQLITE_NOTICE = 27;   /* Notifications from sqlite3_log() */
    const int SQLITE_WARNING = 28;   /* Warnings from sqlite3_log() */
    const int SQLITE_ROW = 100;  /* sqlite3_step() has another row ready */
    const int SQLITE_DONE = 101;  /* sqlite3_step() has finished executing */
    /* end-of-error-codes */


    const int SQLITE_VTAB_CONSTRAINT_SUPPORT = 1;
    const int SQLITE_VTAB_INNOCUOUS = 2;
    const int SQLITE_VTAB_DIRECTONLY = 3;
    const int SQLITE_VTAB_USES_ALL_SCHEMAS = 4;

    /* Column numbers */
    const int SERIES_COLUMN_VALUE = 0;
    const int SERIES_COLUMN_START = 1;
    const int SERIES_COLUMN_STOP = 2;
    const int SERIES_COLUMN_STEP = 3;
    /*
    ** CAPI3REF: Fundamental Datatypes
    ** KEYWORDS: SQLITE_TEXT
    **
    ** ^(Every value in SQLite has one of five fundamental datatypes:
    **
    ** <ul>
    ** <li> 64-bit signed integer
    ** <li> 64-bit IEEE floating point number
    ** <li> string
    ** <li> BLOB
    ** <li> NULL
    ** </ul>)^
    **
    ** These constants are codes for each of those types.
    **
    ** Note that the SQLITE_TEXT constant was also used in SQLite version 2
    ** for a completely different meaning.  Software that links against both
    ** SQLite version 2 and SQLite version 3 should use SQLITE3_TEXT, not
    ** SQLITE_TEXT.
    */
    const int SQLITE_INTEGER = 1;
    const int SQLITE_FLOAT = 2;
    const int SQLITE_BLOB = 4;
    const int SQLITE_NULL = 5;
    const int SQLITE_TEXT = 3;

    /*
    ** CAPI3REF: Virtual Table Constraint Operator Codes
    **
    ** These macros define the allowed values for the
    ** [sqlite3_index_info].aConstraint[].op field.  Each value represents
    ** an operator that is part of a constraint term in the WHERE clause of
    ** a query that uses a [virtual table].
    **
    ** ^The left-hand operand of the operator is given by the corresponding
    ** aConstraint[].iColumn field.  ^An iColumn of -1 indicates the left-hand
    ** operand is the rowid.
    ** The SQLITE_INDEX_CONSTRAINT_LIMIT and SQLITE_INDEX_CONSTRAINT_OFFSET
    ** operators have no left-hand operand, and so for those operators the
    ** corresponding aConstraint[].iColumn is meaningless and should not be
    ** used.
    **
    ** All operator values from SQLITE_INDEX_CONSTRAINT_FUNCTION through
    ** value 255 are reserved to represent functions that are overloaded
    ** by the [xFindFunction|xFindFunction method] of the virtual table
    ** implementation.
    **
    ** The right-hand operands for each constraint might be accessible using
    ** the [sqlite3_vtab_rhs_value()] interface.  Usually the right-hand
    ** operand is only available if it appears as a single constant literal
    ** in the input SQL.  If the right-hand operand is another column or an
    ** expression (even a constant expression) or a parameter, then the
    ** sqlite3_vtab_rhs_value() probably will not be able to extract it.
    ** ^The SQLITE_INDEX_CONSTRAINT_ISNULL and
    ** SQLITE_INDEX_CONSTRAINT_ISNOTNULL operators have no right-hand operand
    ** and hence calls to sqlite3_vtab_rhs_value() for those operators will
    ** always return SQLITE_NOTFOUND.
    **
    ** The collating sequence to be used for comparison can be found using
    ** the [sqlite3_vtab_collation()] interface.  For most real-world virtual
    ** tables, the collating sequence of constraints does not matter (for example
    ** because the constraints are numeric) and so the sqlite3_vtab_collation()
    ** interface is not commonly needed.
    */
    const int SQLITE_INDEX_CONSTRAINT_EQ        =   2;
    const int SQLITE_INDEX_CONSTRAINT_GT        =   4;
    const int SQLITE_INDEX_CONSTRAINT_LE        =   8;
    const int SQLITE_INDEX_CONSTRAINT_LT        =  16;
    const int SQLITE_INDEX_CONSTRAINT_GE        =  32;
    const int SQLITE_INDEX_CONSTRAINT_MATCH     =  64;
    const int SQLITE_INDEX_CONSTRAINT_LIKE      =  65;
    const int SQLITE_INDEX_CONSTRAINT_GLOB      =  66;
    const int SQLITE_INDEX_CONSTRAINT_REGEXP    =  67;
    const int SQLITE_INDEX_CONSTRAINT_NE        =  68;
    const int SQLITE_INDEX_CONSTRAINT_ISNOT     =  69;
    const int SQLITE_INDEX_CONSTRAINT_ISNOTNULL =  70;
    const int SQLITE_INDEX_CONSTRAINT_ISNULL    =  71;
    const int SQLITE_INDEX_CONSTRAINT_IS        =  72;
    const int SQLITE_INDEX_CONSTRAINT_LIMIT     =  73;
    const int SQLITE_INDEX_CONSTRAINT_OFFSET    =  74;
    const int SQLITE_INDEX_CONSTRAINT_FUNCTION  = 150;
    private readonly SqliteConnection connection;
    static sqlite3_module seriesModule = new sqlite3_module()
    {
        iVersion = 0,
        xCreate = 0,
        xConnect = &seriesConnect,
        xBestIndex = &seriesBestIndex,
        xDisconnect = &seriesDisconnect,
        xDestroy = 0,
        xOpen = &seriesOpen,
        xClose = &seriesClose,
        xFilter = &seriesFilter,
        xNext = &seriesNext,
        xEof = &seriesEof,
        xColumn = &seriesColumn,
        xRowid = &seriesRowid,
        xUpdate = 0,
        xBegin = 0,
        xSync = 0,
        xCommit = 0,
        xRollback = 0,
        xFindFunction = 0,
        xRename = 0,
        xSavepoint = 0,
        xRelease = 0,
        xRollbackTo = 0,
        xShadowName = 0
    };

    public unsafe SeriesModule(SqliteConnection connection)
    {
        this.connection = connection;

        var rc = VirtualModule.sqlite3_create_module_v2(connection.Handle!, "generate_series", in seriesModule, IntPtr.Zero, null);
        Debug.Assert(rc == 0);
    }

    public void Dispose()
    {
        VirtualModule.sqlite3_drop_modules(connection.Handle!, null);
    }
    /*
    ** Return that member of a generate_series(...) sequence whose 0-based
    ** index is ix. The 0th member is given by smBase. The sequence members
    ** progress per ix increment by smStep.
    */
    static long genSeqMember(long smBase,
                            long smStep,
                            ulong ix)
    {
        if (ix >= (ulong)long.MaxValue)
        {
            /* Get ix into signed i64 range. */
            ix -= (ulong)long.MaxValue;
            /* With 2's complement ALU, this next can be 1 step, but is split into
             * 2 for UBSAN's satisfaction (and hypothetical 1's complement ALUs.) */
            smBase += (long.MaxValue / 2) * smStep;
            smBase += (long.MaxValue - long.MaxValue / 2) * smStep;
        }
        /* Under UBSAN (or on 1's complement machines), must do this last term
         * in steps to avoid the dreaded (and harmless) signed multiply overlow. */
        if (ix >= 2)
        {
            long ix2 = (long)ix / 2;
            smBase += ix2 * smStep;
            ix -= (ulong)ix2;
        }
        return smBase + ((long)ix) * smStep;
    }

    /*
    ** Prepare a SequenceSpec for use in generating an integer series
    ** given initialized iBase, iTerm and iStep values. Sequence is
    ** initialized per given isReversing. Other members are computed.
    */
    static void setupSequence(SequenceSpec* pss)
    {
        bool bSameSigns;
        pss->uSeqIndexMax = 0;
        pss->isNotEOF = 0;
        bSameSigns = (pss->iBase < 0) == (pss->iTerm < 0);
        if (pss->iTerm < pss->iBase)
        {
            ulong nuspan = 0;
            if (bSameSigns)
            {
                nuspan = (ulong)(pss->iBase - pss->iTerm);
            }
            else
            {
                /* Under UBSAN (or on 1's complement machines), must do this in steps.
                 * In this clause, iBase>=0 and iTerm<0 . */
                nuspan = 1;
                nuspan += (ulong)pss->iBase;
                nuspan += (ulong)-(pss->iTerm + 1);
            }
            if (pss->iStep < 0)
            {
                pss->isNotEOF = 1;
                if (nuspan == ulong.MaxValue)
                {
                    pss->uSeqIndexMax = (pss->iStep > long.MinValue) ? (ulong)((long)nuspan / -pss->iStep) : 1;
                }
                else if (pss->iStep > long.MinValue)
                {
                    pss->uSeqIndexMax = (ulong)((long)nuspan / -pss->iStep);
                }
            }
        }
        else if (pss->iTerm > pss->iBase)
        {
            ulong puspan = 0;
            if (bSameSigns)
            {
                puspan = (ulong)(pss->iTerm - pss->iBase);
            }
            else
            {
                /* Under UBSAN (or on 1's complement machines), must do this in steps.
                 * In this clause, iTerm>=0 and iBase<0 . */
                puspan = 1;
                puspan += (ulong)pss->iTerm;
                puspan += (ulong)-(pss->iBase + 1);
            }
            if (pss->iStep > 0)
            {
                pss->isNotEOF = 1;
                pss->uSeqIndexMax = puspan / (ulong)pss->iStep;
            }
        }
        else if (pss->iTerm == pss->iBase)
        {
            pss->isNotEOF = 1;
            pss->uSeqIndexMax = 0;
        }
        pss->uSeqIndexNow = (pss->isReversing != 0) ? pss->uSeqIndexMax : 0;
        pss->iValueNow = (pss->isReversing != 0)
          ? genSeqMember(pss->iBase, pss->iStep, pss->uSeqIndexMax)
          : pss->iBase;
    }

    /*
    ** Progress sequence generator to yield next value, if any.
    ** Leave its state to either yield next value or be at EOF.
    ** Return whether there is a next value, or 0 at EOF.
    */
    static int progressSequence(SequenceSpec* pss)
    {
        if (pss->isNotEOF == 0)
        {
            return 0;
        }

        if (pss->isReversing != 0)
        {
            if (pss->uSeqIndexNow > 0)
            {
                pss->uSeqIndexNow--;
                pss->iValueNow -= pss->iStep;
            }
            else
            {
                pss->isNotEOF = 0;
            }
        }
        else
        {
            if (pss->uSeqIndexNow < pss->uSeqIndexMax)
            {
                pss->uSeqIndexNow++;
                pss->iValueNow += pss->iStep;
            }
            else
            {
                pss->isNotEOF = 0;
            }
        }
        return pss->isNotEOF;
    }

    /*
    ** The seriesConnect() method is invoked to create a new
    ** series_vtab that describes the generate_series virtual table.
    **
    ** Think of this routine as the constructor for series_vtab objects.
    **
    ** All this routine needs to do is:
    **
    **    (1) Allocate the series_vtab object and initialize all fields.
    **
    **    (2) Tell SQLite (via the sqlite3_declare_vtab() interface) what the
    **        result set of queries against generate_series will look like.
    */
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    static int seriesConnect(
        nint db,
        IntPtr pUnused,
        int argcUnused, IntPtr argvUnused,
        sqlite3_vtab** ppVtab,
        IntPtr pzErrUnused
    )
    {
        sqlite3_vtab* pNew;
        int rc;

        rc = VirtualModule.sqlite3_declare_vtab(db, "CREATE TABLE x(value,start hidden,stop hidden,step hidden)");
        if (rc == SQLITE_OK)
        {
            pNew = *ppVtab = (sqlite3_vtab*)VirtualModule.sqlite3_malloc(sizeof(sqlite3_vtab));
            if (pNew == null)
            {
                return SQLITE_NOMEM;
            }

            NativeMemory.Fill(pNew, (nuint)sizeof(sqlite3_vtab), value: 0);
            VirtualModule.sqlite3_vtab_config(db, SQLITE_VTAB_INNOCUOUS);
        }
        return rc;
    }

    /*
    ** This method is the destructor for series_cursor objects.
    */
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    static int seriesDisconnect(sqlite3_vtab* pVtab)
    {
        VirtualModule.sqlite3_free(pVtab);
        return SQLITE_OK;
    }
    /*
    ** Constructor for a new series_cursor object.
    */
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    static int seriesOpen(sqlite3_vtab* pUnused, sqlite3_vtab_cursor** ppCursor)
    {
        series_cursor* pCur;
        pCur = (series_cursor*)VirtualModule.sqlite3_malloc(sizeof(series_cursor));
        if (pCur == null)
        {
            return SQLITE_NOMEM;
        }

        NativeMemory.Fill(pCur, (nuint)sizeof(series_cursor), 0);
        *ppCursor = &pCur->base_cursor;
        return SQLITE_OK;
    }

    /*
    ** Destructor for a series_cursor.
    */
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    static int seriesClose(sqlite3_vtab_cursor* cur)
    {
        VirtualModule.sqlite3_free(cur);
        return SQLITE_OK;
    }


    /*
    ** Advance a series_cursor to its next row of output.
    */
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    static int seriesNext(sqlite3_vtab_cursor* cur)
    {
        series_cursor* pCur = (series_cursor*)cur;
        progressSequence(&pCur->ss);
        return SQLITE_OK;
    }

    /*
    ** Return values of columns for the row at which the series_cursor
    ** is currently pointing.
    */
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    static int seriesColumn(
      sqlite3_vtab_cursor* cur,   /* The cursor */
      sqlite3_context* ctx,       /* First argument to sqlite3_result_...() */
      int i                       /* Which column to return */
    )
    {
        series_cursor* pCur = (series_cursor*)cur;
        long x = 0;
        switch (i)
        {
            case SERIES_COLUMN_START: x = pCur->ss.iBase; break;
            case SERIES_COLUMN_STOP: x = pCur->ss.iTerm; break;
            case SERIES_COLUMN_STEP: x = pCur->ss.iStep; break;
            default: x = pCur->ss.iValueNow; break;
        }

        VirtualModule.sqlite3_result_int64(ctx, x);
        return SQLITE_OK;
    }

    /*
    ** Return the rowid for the current row, logically equivalent to n+1 where
    ** "n" is the ascending integer in the aforesaid production definition.
    */
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    static int seriesRowid(sqlite3_vtab_cursor* cur, long* pRowid)
    {
        series_cursor* pCur = (series_cursor*)cur;
        ulong n = pCur->ss.uSeqIndexNow;
        *pRowid = (long)((n < ulong.MaxValue) ? n + 1 : 0);
        return SQLITE_OK;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    static int seriesEof(sqlite3_vtab_cursor* cur)
    {
        series_cursor* pCur = (series_cursor*)cur;
        return pCur->ss.isNotEOF == 0 ? 1 : 0;
    }

    /*
    ** This method is called to "rewind" the series_cursor object back
    ** to the first row of output.  This method is always called at least
    ** once prior to any call to seriesColumn() or seriesRowid() or
    ** seriesEof().
    **
    ** The query plan selected by seriesBestIndex is passed in the idxNum
    ** parameter.  (idxStr is not used in this implementation.)  idxNum
    ** is a bitmask showing which constraints are available:
    **
    **    1:    start=VALUE
    **    2:    stop=VALUE
    **    4:    step=VALUE
    **
    ** Also, if bit 8 is set, that means that the series should be output
    ** in descending order rather than in ascending order.  If bit 16 is
    ** set, then output must appear in ascending order.
    **
    ** This routine should initialize the cursor and position it so that it
    ** is pointing at the first row, or pointing off the end of the table
    ** (so that seriesEof() will return true) if the table is empty.
    */
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    static int seriesFilter(
      sqlite3_vtab_cursor* pVtabCursor,
      int idxNum, IntPtr idxStrUnused,
      int argc, nint* argv
    )
    {
        series_cursor* pCur = (series_cursor*)pVtabCursor;
        int i = 0;

        if ((idxNum & 1) != 0)
        {
            pCur->ss.iBase = VirtualModule.sqlite3_value_int64(argv[i++]);
        }
        else
        {
            pCur->ss.iBase = 0;
        }
        if ((idxNum & 2) != 0)
        {
            pCur->ss.iTerm = VirtualModule.sqlite3_value_int64(argv[i++]);
        }
        else
        {
            pCur->ss.iTerm = 0xffffffff;
        }
        if ((idxNum & 4) != 0)
        {
            pCur->ss.iStep = VirtualModule.sqlite3_value_int64(argv[i++]);
            if (pCur->ss.iStep == 0)
            {
                pCur->ss.iStep = 1;
            }
            else if (pCur->ss.iStep < 0)
            {
                if ((idxNum & 16) == 0) idxNum |= 8;
            }
        }
        else
        {
            pCur->ss.iStep = 1;
        }
        for (i = 0; i < argc; i++)
        {
            if (VirtualModule.sqlite3_value_type(argv[i]) == SQLITE_NULL)
            {
                /* If any of the constraints have a NULL value, then return no rows.
                ** See ticket https://www.sqlite.org/src/info/fac496b61722daf2 */
                pCur->ss.iBase = 1;
                pCur->ss.iTerm = 0;
                pCur->ss.iStep = 1;
                break;
            }
        }
        if ((idxNum & 8) != 0)
        {
            pCur->ss.isReversing = pCur->ss.iStep > 0 ? (byte)1 : (byte)0;
        }
        else
        {
            pCur->ss.isReversing = pCur->ss.iStep < 0 ? (byte)1 : (byte)0;
        }
        setupSequence(&pCur->ss);
        return SQLITE_OK;
    }

    /*
    ** SQLite will invoke this method one or more times while planning a query
    ** that uses the generate_series virtual table.  This routine needs to create
    ** a query plan for each invocation and compute an estimated cost for that
    ** plan.
    **
    ** In this implementation idxNum is used to represent the
    ** query plan.  idxStr is unused.
    **
    ** The query plan is represented by bits in idxNum:
    **
    **  (1)  start = $value  -- constraint exists
    **  (2)  stop = $value   -- constraint exists
    **  (4)  step = $value   -- constraint exists
    **  (8)  output in descending order
    */
    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    static int seriesBestIndex(
      sqlite3_vtab* pVTab,
      sqlite3_index_info* pIdxInfo
    )
    {
        int i, j;              /* Loop over constraints */
        int idxNum = 0;        /* The query plan bitmask */
        int bStartSeen = 0;    /* EQ constraint seen on the START column */
        int unusableMask = 0;  /* Mask of unusable constraints */
        int nArg = 0;          /* Number of arguments that seriesFilter() expects */
        int* aIdx = stackalloc int[3];           /* Constraints on start, stop, and step */
        sqlite3_index_constraint* pConstraint;

        /* This implementation assumes that the start, stop, and step columns
        ** are the last three columns in the virtual table. */
        Debug.Assert(SERIES_COLUMN_STOP == SERIES_COLUMN_START + 1);
        Debug.Assert(SERIES_COLUMN_STEP == SERIES_COLUMN_START + 2);

        aIdx[0] = aIdx[1] = aIdx[2] = -1;
        pConstraint = pIdxInfo->aConstraint;
        for (i = 0; i < pIdxInfo->nConstraint; i++, pConstraint++)
        {
            int iCol;    /* 0 for start, 1 for stop, 2 for step */
            int iMask;   /* bitmask for those column */
            if (pConstraint->iColumn < SERIES_COLUMN_START) continue;
            iCol = pConstraint->iColumn - SERIES_COLUMN_START;
            Debug.Assert(iCol >= 0 && iCol <= 2);
            iMask = 1 << iCol;
            if (iCol == 0) bStartSeen = 1;
            if (pConstraint->usable == 0)
            {
                unusableMask |= iMask;
                continue;
            }
            else if (pConstraint->op == SQLITE_INDEX_CONSTRAINT_EQ)
            {
                idxNum |= iMask;
                aIdx[iCol] = i;
            }
        }
        for (i = 0; i < 3; i++)
        {
            if ((j = aIdx[i]) >= 0)
            {
                pIdxInfo->aConstraintUsage[j].argvIndex = ++nArg;
                const int SQLITE_SERIES_CONSTRAINT_VERIFY = 0;
                pIdxInfo->aConstraintUsage[j].omit = SQLITE_SERIES_CONSTRAINT_VERIFY == 0 ? 1 : 0;
            }
        }
        /* The current generate_column() implementation requires at least one
        ** argument (the START value).  Legacy versions assumed START=0 if the
        ** first argument was omitted.  Compile with -DZERO_ARGUMENT_GENERATE_SERIES
        ** to obtain the legacy behavior */
#if !ZERO_ARGUMENT_GENERATE_SERIES
        if (bStartSeen == 0)
        {
            VirtualModule.sqlite3_free(pVTab->zErrMsg);
            pVTab->zErrMsg = (byte*)VirtualModule.sqlite3_mprintf(
                "first argument to \"generate_series()\" missing or unusable"u8);
            return SQLITE_ERROR;
        }
#endif
        if ((unusableMask & ~idxNum) != 0)
        {
            /* The start, stop, and step columns are inputs.  Therefore if there
            ** are unusable constraints on any of start, stop, or step then
            ** this plan is unusable */
            return SQLITE_CONSTRAINT;
        }
        if ((idxNum & 3) == 3)
        {
            /* Both start= and stop= boundaries are available.  This is the 
            ** the preferred case */
            pIdxInfo->estimatedCost = (double)(2 - ((idxNum & 4) != 0 ? 1 : 0));
            pIdxInfo->estimatedRows = 1000;
            if (pIdxInfo->nOrderBy >= 1 && pIdxInfo->aOrderBy[0].iColumn == 0)
            {
                if (pIdxInfo->aOrderBy[0].desc != 0)
                {
                    idxNum |= 8;
                }
                else
                {
                    idxNum |= 16;
                }
                pIdxInfo->orderByConsumed = 1;
            }
        }
        else
        {
            /* If either boundary is missing, we have to generate a huge span
            ** of numbers.  Make this case very expensive so that the query
            ** planner will work hard to avoid it. */
            pIdxInfo->estimatedRows = 2147483647;
        }
        pIdxInfo->idxNum = idxNum;
        return SQLITE_OK;
    }

    struct SequenceSpec
    {
        public long iBase;         /* Starting value ("start") */
        public long iTerm;         /* Given terminal value ("stop") */
        public long iStep;         /* Increment ("step") */
        public ulong uSeqIndexMax; /* maximum sequence index (aka "n") */
        public ulong uSeqIndexNow; /* Current index during generation */
        public long iValueNow;     /* Current value during generation */
        public byte isNotEOF;                 /* Sequence generation not exhausted */
        public byte isReversing;              /* Sequence is being reverse generated */
    }

    struct series_cursor
    {
        public sqlite3_vtab_cursor base_cursor;  /* Base class - must be first */
        public SequenceSpec ss;                  /* (this) Derived class data */
    };
}
